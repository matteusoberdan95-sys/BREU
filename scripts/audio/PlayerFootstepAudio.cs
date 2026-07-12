namespace BREU.Scripts.Audio;

/// <summary>
/// Sprint 16B–16D — player footsteps (audio-only). Reads movement state; never writes Velocity/Position.
/// Single timer + cooldown. Surface banks only (no sequence files, no player_run_step_*).
/// </summary>
public partial class PlayerFootstepAudio : Node
{
    private const string AssetRoot = "res://assets/audio/pensao/";
    private const float MinSpeed = 0.2f;

    private enum FootstepState
    {
        Idle,
        Walk,
        Run,
        Crouch
    }

    [Export] public float WalkStepInterval { get; set; } = 0.64f;
    [Export] public float RunStepInterval { get; set; } = 0.36f;
    [Export] public float CrouchStepInterval { get; set; } = 0.85f;
    [Export] public float MinimumStepCooldown { get; set; } = 0.28f;
    /// <summary>Horizontal speed at/above this counts as Run even if sprint flag flickers.</summary>
    [Export] public float RunSpeedThreshold { get; set; } = 5.2f;
    [Export] public float WalkWoodVolumeDb { get; set; } = -12f;
    [Export] public float WalkDirtVolumeDb { get; set; } = -11f;
    [Export] public float RunWoodVolumeDb { get; set; } = -9f;
    [Export] public float RunDirtVolumeDb { get; set; } = -8f;
    [Export] public float CrouchVolumeDb { get; set; } = -18f;

    /// <summary>When true, logs each footstep. Set by Audio Debug F7.</summary>
    public bool DebugLoggingEnabled { get; set; }

    private CharacterBody3D? _body;
    private PlayerController? _controller;
    private AudioStreamPlayer? _player;
    private readonly List<NamedStream> _wood = new();
    private readonly List<NamedStream> _dirt = new();
    private readonly HashSet<SurfaceAudioZone3D> _surfaces = new();
    private readonly RandomNumberGenerator _rng = new();
    private readonly HashSet<string> _warned = new(StringComparer.Ordinal);

    private float _stepTimer;
    private float _cooldownRemaining;
    private int _lastWood = -1;
    private int _lastDirt = -1;

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

    public static PlayerFootstepAudio? Find(SceneTree tree) =>
        tree.GetFirstNodeInGroup("player_footstep_audio") as PlayerFootstepAudio;

    public override void _Ready()
    {
        AddToGroup("player_footstep_audio");
        _rng.Randomize();
        _body = GetParent() as CharacterBody3D;
        _controller = _body as PlayerController;
        _player = new AudioStreamPlayer
        {
            Name = "FootstepPlayer",
            Bus = "SFX",
            VolumeDb = WalkWoodVolumeDb
        };
        AddChild(_player);
        LoadBanks();
    }

    public void NotifySurfaceEntered(SurfaceAudioZone3D zone) => _surfaces.Add(zone);

    public void NotifySurfaceExited(SurfaceAudioZone3D zone) => _surfaces.Remove(zone);

    public override void _PhysicsProcess(double delta)
    {
        if (_body == null || _player == null)
        {
            return;
        }

        var dt = (float)delta;
        if (_cooldownRemaining > 0f)
        {
            _cooldownRemaining -= dt;
        }

        if (!_body.IsOnFloor())
        {
            ResetStepTimer();
            return;
        }

        var horizontal = new Vector2(_body.Velocity.X, _body.Velocity.Z).Length();
        var state = ResolveState(horizontal);
        if (state == FootstepState.Idle)
        {
            ResetStepTimer();
            return;
        }

        var interval = GetInterval(state);
        _stepTimer += dt;
        if (_stepTimer < interval)
        {
            return;
        }

        if (_cooldownRemaining > 0f)
        {
            return;
        }

        _stepTimer = 0f;
        _cooldownRemaining = MinimumStepCooldown;
        PlayStep(state, interval);
    }

    /// <summary>
    /// Debug helper. "run" previews the current surface bank at run volume/pitch (not player_run_step_*).
    /// </summary>
    public void DebugPlayBank(string bank)
    {
        switch (bank)
        {
            case "wood":
                PlayFromBank(_wood, ref _lastWood, WalkWoodVolumeDb, FootstepState.Walk,
                    FootstepSurfaceType.Wood, WalkStepInterval, pitchMin: 0.98f, pitchMax: 1.03f, force: true);
                break;
            case "dirt":
                PlayFromBank(_dirt, ref _lastDirt, WalkDirtVolumeDb, FootstepState.Walk,
                    FootstepSurfaceType.DirtGravel, WalkStepInterval, pitchMin: 0.98f, pitchMax: 1.03f, force: true);
                break;
            case "run":
            {
                var surface = ResolveSurface();
                if (surface == FootstepSurfaceType.DirtGravel)
                {
                    PlayFromBank(_dirt, ref _lastDirt, RunDirtVolumeDb, FootstepState.Run, surface,
                        RunStepInterval, pitchMin: 0.99f, pitchMax: 1.04f, force: true);
                }
                else
                {
                    PlayFromBank(_wood, ref _lastWood, RunWoodVolumeDb, FootstepState.Run,
                        FootstepSurfaceType.Wood, RunStepInterval, pitchMin: 0.99f, pitchMax: 1.04f, force: true);
                }

                break;
            }
        }
    }

    private FootstepState ResolveState(float horizontalSpeed)
    {
        if (horizontalSpeed < MinSpeed)
        {
            return FootstepState.Idle;
        }

        // Exactly one active locomotion state per frame.
        if (_controller?.IsCrouching == true)
        {
            return FootstepState.Crouch;
        }

        var sprinting = _controller?.IsSprinting == true;
        if (sprinting || horizontalSpeed >= RunSpeedThreshold)
        {
            return FootstepState.Run;
        }

        return FootstepState.Walk;
    }

    private float GetInterval(FootstepState state) =>
        state switch
        {
            FootstepState.Run => RunStepInterval,
            FootstepState.Crouch => CrouchStepInterval,
            _ => WalkStepInterval
        };

    private void PlayStep(FootstepState state, float interval)
    {
        var surface = ResolveSurface();

        float volumeDb;
        float pitchMin;
        float pitchMax;

        switch (state)
        {
            case FootstepState.Crouch:
                volumeDb = CrouchVolumeDb;
                pitchMin = 0.96f;
                pitchMax = 1.01f;
                break;
            case FootstepState.Run:
                volumeDb = surface == FootstepSurfaceType.DirtGravel ? RunDirtVolumeDb : RunWoodVolumeDb;
                pitchMin = 0.99f;
                pitchMax = 1.04f;
                break;
            default:
                volumeDb = surface == FootstepSurfaceType.DirtGravel ? WalkDirtVolumeDb : WalkWoodVolumeDb;
                pitchMin = 0.98f;
                pitchMax = 1.03f;
                break;
        }

        if (surface == FootstepSurfaceType.DirtGravel)
        {
            PlayFromBank(_dirt, ref _lastDirt, volumeDb, state, surface, interval, pitchMin, pitchMax);
            return;
        }

        PlayFromBank(_wood, ref _lastWood, volumeDb, state, FootstepSurfaceType.Wood, interval, pitchMin, pitchMax);
    }

    private FootstepSurfaceType ResolveSurface()
    {
        SurfaceAudioZone3D? best = null;
        foreach (var zone in _surfaces)
        {
            if (!GodotObject.IsInstanceValid(zone))
            {
                continue;
            }

            if (best == null || zone.ZonePriority > best.ZonePriority)
            {
                best = zone;
            }
        }

        return best?.SurfaceType ?? FootstepSurfaceType.Wood;
    }

    private void PlayFromBank(
        List<NamedStream> bank,
        ref int lastIndex,
        float volumeDb,
        FootstepState state,
        FootstepSurfaceType surface,
        float interval,
        float pitchMin,
        float pitchMax,
        bool force = false)
    {
        if (_player == null || bank.Count == 0)
        {
            return;
        }

        var index = _rng.RandiRange(0, bank.Count - 1);
        if (bank.Count > 1 && index == lastIndex)
        {
            index = (index + 1) % bank.Count;
        }

        lastIndex = index;
        var sample = bank[index];

        // One player only — stop any lingering playback to avoid stacked/embolados steps.
        if (_player.Playing)
        {
            _player.Stop();
        }

        _player.Stream = sample.Stream;
        _player.VolumeDb = volumeDb;
        _player.PitchScale = _rng.RandfRange(pitchMin, pitchMax);
        _player.Play();

        if (DebugLoggingEnabled || force)
        {
            GD.Print(
                $"[Footstep] state={state} surface={surface} interval={interval:0.00} sample={sample.Id}");
        }
    }

    private void ResetStepTimer() => _stepTimer = 0f;

    private void LoadBanks()
    {
        // Individual one-shots only. Never load:
        // - player_footsteps_*_sequence / player_running_sequence
        // - player_run_step_* (reserved for future chase/panic)
        for (var i = 1; i <= 8; i++)
        {
            TryAdd($"player_footstep_wood_{i:D2}", _wood);
        }

        for (var i = 1; i <= 12; i++)
        {
            TryAdd($"player_footstep_dirt_gravel_{i:D2}", _dirt);
        }

        GD.Print(
            $"[Audio] Footsteps loaded: wood={_wood.Count}, dirt={_dirt.Count} " +
            "(sequences + run bank not used)");
    }

    private void TryAdd(string id, List<NamedStream> bank)
    {
        var path = AssetRoot + id + ".ogg";
        if (!ResourceLoader.Exists(path))
        {
            if (_warned.Add(path))
            {
                GD.PushWarning($"[Audio] Missing asset: {path}");
            }

            return;
        }

        var stream = GD.Load<AudioStream>(path);
        if (stream != null)
        {
            bank.Add(new NamedStream(id, stream));
        }
    }
}
