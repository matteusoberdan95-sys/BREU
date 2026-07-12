namespace BREU.Scripts.Audio;

/// <summary>
/// Sprint 16E — first-person breathing (audio-only).
/// Reads sprint/stamina/speed; never writes PlayerController, stamina, or movement.
/// </summary>
public partial class PlayerBreathingAudio : Node
{
    private const string AssetRoot = "res://assets/audio/pensao/";
    private const float MinMoveSpeed = 0.2f;
    private const float SprintGateSeconds = 2.0f;
    private const float LowStaminaRatio = 0.35f;

    private enum BreathState
    {
        Idle,
        Walking,
        Running,
        Recovering
    }

    [Export] public float NormalBreathVolumeDb { get; set; } = -32f;
    [Export] public float WalkBreathVolumeDb { get; set; } = -30f;
    [Export] public float PantingMinVolumeDb { get; set; } = -80f;
    [Export] public float PantingQuietVolumeDb { get; set; } = -30f;
    [Export] public float PantingActiveVolumeDb { get; set; } = -18f;
    [Export] public float PantingLowStaminaVolumeDb { get; set; } = -14f;
    [Export] public float PantingFadeInSeconds { get; set; } = 1.2f;
    [Export] public float PantingFadeOutSeconds { get; set; } = 3.0f;
    [Export] public float RecoverMinSeconds { get; set; } = 2.0f;
    [Export] public float RecoverMaxSeconds { get; set; } = 5.0f;
    [Export] public float OneShotVolumeDb { get; set; } = -16f;
    [Export] public float OneShotCooldownSeconds { get; set; } = 8.0f;

    public bool DebugLoggingEnabled { get; set; }

    /// <summary>Debug: force panting target audible regardless of sprint state.</summary>
    public bool DebugForcePanting { get; set; }

    private CharacterBody3D? _body;
    private PlayerController? _controller;
    private PlayerStamina? _stamina;
    private PlayerHealth? _health;
    private AudioStreamPlayer? _normalPlayer;
    private AudioStreamPlayer? _pantingPlayer;
    private AudioStreamPlayer? _oneShotPlayer;
    private readonly List<NamedStream> _heavyOneShots = new();
    private readonly RandomNumberGenerator _rng = new();
    private readonly HashSet<string> _warned = new(StringComparer.Ordinal);

    private float _sprintTime;
    private float _recoverTimer;
    private float _oneShotCooldown;
    private float _pantingVolumeDb = -80f;
    private float _logTimer;
    private BreathState _state = BreathState.Idle;
    private bool _wasSprinting;
    private bool _lowStaminaOneShotArmed = true;
    private int _lastOneShotIndex = -1;

    private readonly struct NamedStream
    {
        public readonly string Id;
        public readonly AudioStream Stream;

        public NamedStream(string id, AudioStream stream)
        {
            Id = id;
            Stream = stream;
        }
    }

    public static PlayerBreathingAudio? Find(SceneTree tree) =>
        tree.GetFirstNodeInGroup("player_breathing_audio") as PlayerBreathingAudio;

    public override void _Ready()
    {
        AddToGroup("player_breathing_audio");
        _rng.Randomize();
        _body = GetParent() as CharacterBody3D;
        _controller = _body as PlayerController;
        _stamina = _body?.GetNodeOrNull<PlayerStamina>("PlayerStamina");
        _health = _body?.GetNodeOrNull<PlayerHealth>("PlayerHealth");

        _normalPlayer = CreateLoopPlayer("BreathNormalPlayer", TryLoadLoop("player_breath_heavy_loop"), NormalBreathVolumeDb);
        _pantingPlayer = CreateLoopPlayer("BreathPantingPlayer", TryLoadLoop("player_panting_loop"), PantingMinVolumeDb);
        _oneShotPlayer = new AudioStreamPlayer
        {
            Name = "BreathOneShotPlayer",
            Bus = "SFX",
            VolumeDb = OneShotVolumeDb
        };
        AddChild(_oneShotPlayer);

        for (var i = 1; i <= 4; i++)
        {
            TryAddOneShot($"player_breath_heavy_{i:D2}");
        }

        if (_normalPlayer.Stream != null)
        {
            _normalPlayer.Play();
        }

        if (_pantingPlayer.Stream != null)
        {
            _pantingPlayer.VolumeDb = PantingMinVolumeDb;
            _pantingPlayer.Play();
        }

        GD.Print(
            $"[Audio] Breathing loaded: normal={_normalPlayer.Stream != null}, " +
            $"panting={_pantingPlayer.Stream != null}, oneShots={_heavyOneShots.Count}");
    }

    public override void _Process(double delta)
    {
        if (_normalPlayer == null || _pantingPlayer == null)
        {
            return;
        }

        var dt = (float)delta;
        if (_oneShotCooldown > 0f)
        {
            _oneShotCooldown -= dt;
        }

        if (_health?.IsDead == true)
        {
            FadeToward(ref _pantingVolumeDb, PantingMinVolumeDb, PantingFadeOutSeconds, dt);
            ApplyVolumes(NormalBreathVolumeDb);
            return;
        }

        var horizontal = _controller?.HorizontalSpeed
            ?? (_body != null ? new Vector2(_body.Velocity.X, _body.Velocity.Z).Length() : 0f);
        var isSprinting = _controller?.IsSprinting == true;
        var isCrouching = _controller?.IsCrouching == true;
        var staminaRatio = GetStaminaRatio();
        var lowStamina = staminaRatio < LowStaminaRatio;

        UpdateSprintTracking(isSprinting, dt);
        ResolveState(isSprinting, isCrouching, horizontal);
        UpdatePantingTarget(isSprinting, lowStamina, dt);
        UpdateNormalBreath(horizontal, isCrouching, dt);
        MaybePlayOneShots(isSprinting, lowStamina);

        _wasSprinting = isSprinting;
        LogState(dt);
    }

    public void DebugPlayHeavyOneShot()
    {
        PlayHeavyOneShot(force: true);
    }

    public void DebugTogglePanting()
    {
        DebugForcePanting = !DebugForcePanting;
        GD.Print($"[Breathing] DebugForcePanting={DebugForcePanting}");
    }

    private void UpdateSprintTracking(bool isSprinting, float dt)
    {
        if (isSprinting)
        {
            _sprintTime += dt;
            _recoverTimer = 0f;
            return;
        }

        if (_wasSprinting && _sprintTime >= SprintGateSeconds)
        {
            _recoverTimer = _rng.RandfRange(RecoverMinSeconds, RecoverMaxSeconds);
            PlayHeavyOneShot(force: false);
        }

        if (!isSprinting)
        {
            _sprintTime = 0f;
        }
    }

    private void ResolveState(bool isSprinting, bool isCrouching, float horizontal)
    {
        if (_recoverTimer > 0f && !isSprinting)
        {
            _state = BreathState.Recovering;
            return;
        }

        if (isSprinting && _sprintTime >= SprintGateSeconds)
        {
            _state = BreathState.Running;
            return;
        }

        if (isCrouching || horizontal < MinMoveSpeed)
        {
            _state = BreathState.Idle;
            return;
        }

        _state = BreathState.Walking;
    }

    private void UpdatePantingTarget(bool isSprinting, bool lowStamina, float dt)
    {
        float target;

        if (DebugForcePanting)
        {
            target = PantingActiveVolumeDb;
        }
        else if (_state == BreathState.Recovering)
        {
            _recoverTimer -= dt;
            target = lowStamina ? PantingLowStaminaVolumeDb : PantingActiveVolumeDb;
            if (_recoverTimer <= 0f)
            {
                _recoverTimer = 0f;
                target = PantingMinVolumeDb;
            }
        }
        else if (isSprinting && _sprintTime >= SprintGateSeconds)
        {
            target = lowStamina ? PantingLowStaminaVolumeDb : PantingActiveVolumeDb;
        }
        else if (lowStamina && _sprintTime >= SprintGateSeconds * 0.5f)
        {
            // Soft cue while still low after recent effort (without forcing constant pant).
            target = PantingQuietVolumeDb;
        }
        else
        {
            target = PantingMinVolumeDb;
        }

        var fadeSeconds = target > _pantingVolumeDb ? PantingFadeInSeconds : PantingFadeOutSeconds;
        FadeToward(ref _pantingVolumeDb, target, fadeSeconds, dt);
        _pantingPlayer!.VolumeDb = _pantingVolumeDb;

        if (_pantingPlayer.Stream != null && !_pantingPlayer.Playing)
        {
            _pantingPlayer.Play();
        }
    }

    private void UpdateNormalBreath(float horizontal, bool isCrouching, float dt)
    {
        var target = NormalBreathVolumeDb;
        if (!isCrouching && horizontal >= MinMoveSpeed && _state != BreathState.Running)
        {
            target = WalkBreathVolumeDb;
        }

        // Duck slightly under strong panting so loops do not fight.
        if (_pantingVolumeDb > -25f)
        {
            target -= 4f;
        }

        var current = _normalPlayer!.VolumeDb;
        FadeToward(ref current, target, 1.0f, dt);
        _normalPlayer.VolumeDb = current;

        if (_normalPlayer.Stream != null && !_normalPlayer.Playing)
        {
            _normalPlayer.Play();
        }
    }

    private void ApplyVolumes(float normalDb)
    {
        if (_normalPlayer != null)
        {
            _normalPlayer.VolumeDb = normalDb;
        }

        if (_pantingPlayer != null)
        {
            _pantingPlayer.VolumeDb = _pantingVolumeDb;
        }
    }

    private void MaybePlayOneShots(bool isSprinting, bool lowStamina)
    {
        if (lowStamina && _lowStaminaOneShotArmed && !isSprinting)
        {
            _lowStaminaOneShotArmed = false;
            PlayHeavyOneShot(force: false);
        }

        if (!lowStamina)
        {
            _lowStaminaOneShotArmed = true;
        }
    }

    private void PlayHeavyOneShot(bool force)
    {
        if (_oneShotPlayer == null || _heavyOneShots.Count == 0)
        {
            return;
        }

        if (!force && _oneShotCooldown > 0f)
        {
            return;
        }

        var index = _rng.RandiRange(0, _heavyOneShots.Count - 1);
        if (_heavyOneShots.Count > 1 && index == _lastOneShotIndex)
        {
            index = (index + 1) % _heavyOneShots.Count;
        }

        _lastOneShotIndex = index;
        var sample = _heavyOneShots[index];
        if (_oneShotPlayer.Playing)
        {
            _oneShotPlayer.Stop();
        }

        _oneShotPlayer.Stream = sample.Stream;
        _oneShotPlayer.VolumeDb = OneShotVolumeDb;
        _oneShotPlayer.Play();
        _oneShotCooldown = OneShotCooldownSeconds;

        if (DebugLoggingEnabled || force)
        {
            GD.Print($"[Breathing] one-shot={sample.Id}");
        }
    }

    private float GetStaminaRatio()
    {
        if (_stamina == null || _stamina.MaxStamina <= 0.01f)
        {
            return 1f;
        }

        return _stamina.Current / _stamina.MaxStamina;
    }

    private void LogState(float dt)
    {
        if (!DebugLoggingEnabled)
        {
            return;
        }

        _logTimer -= dt;
        if (_logTimer > 0f)
        {
            return;
        }

        _logTimer = 0.75f;
        switch (_state)
        {
            case BreathState.Running:
                GD.Print(
                    $"[Breathing] state=Running sprintTime={_sprintTime:0.0} volume={_pantingVolumeDb:0}");
                break;
            case BreathState.Recovering:
                GD.Print($"[Breathing] state=Recovering timer={_recoverTimer:0.0}");
                break;
            default:
                GD.Print($"[Breathing] state={_state} volume={_normalPlayer!.VolumeDb:0}");
                break;
        }
    }

    private static void FadeToward(ref float currentDb, float targetDb, float seconds, float dt)
    {
        seconds = Mathf.Max(0.05f, seconds);
        var t = Mathf.Clamp(dt / seconds, 0f, 1f);
        currentDb = Mathf.Lerp(currentDb, targetDb, t);
        if (Mathf.Abs(currentDb - targetDb) < 0.15f)
        {
            currentDb = targetDb;
        }
    }

    private AudioStreamPlayer CreateLoopPlayer(string name, AudioStream? stream, float volumeDb)
    {
        var player = new AudioStreamPlayer
        {
            Name = name,
            Bus = "SFX",
            VolumeDb = volumeDb,
            Autoplay = false
        };
        if (stream != null)
        {
            player.Stream = stream;
        }

        AddChild(player);
        return player;
    }

    private AudioStream? TryLoadLoop(string id)
    {
        var path = AssetRoot + id + ".ogg";
        if (!ResourceLoader.Exists(path))
        {
            WarnMissing(path);
            return null;
        }

        var stream = GD.Load<AudioStream>(path);
        if (stream is AudioStreamOggVorbis ogg)
        {
            ogg.Loop = true;
        }

        return stream;
    }

    private void TryAddOneShot(string id)
    {
        var path = AssetRoot + id + ".ogg";
        if (!ResourceLoader.Exists(path))
        {
            WarnMissing(path);
            return;
        }

        var stream = GD.Load<AudioStream>(path);
        if (stream != null)
        {
            _heavyOneShots.Add(new NamedStream(id, stream));
        }
    }

    private void WarnMissing(string path)
    {
        if (!_warned.Add(path))
        {
            return;
        }

        GD.PushWarning($"[Audio] Missing asset: {path}");
    }
}
