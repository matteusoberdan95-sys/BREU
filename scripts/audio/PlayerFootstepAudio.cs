namespace BREU.Scripts.Audio;

/// <summary>
/// Sprint 16B/16C — audible player footsteps. Reads movement state only; never writes Velocity/Position.
/// Run uses the same surface bank as walk (player_run_step_* reserved for a future sprint).
/// </summary>
public partial class PlayerFootstepAudio : Node
{
    private const string AssetRoot = "res://assets/audio/pensao/";
    private const float MinSpeed = 0.2f;

    [Export] public float WalkStepInterval { get; set; } = 0.55f;
    [Export] public float RunStepInterval { get; set; } = 0.36f;
    [Export] public float CrouchStepInterval { get; set; } = 0.78f;
    [Export] public float WalkWoodVolumeDb { get; set; } = -12f;
    [Export] public float WalkDirtVolumeDb { get; set; } = -11f;
    [Export] public float RunWoodVolumeDb { get; set; } = -9f;
    [Export] public float RunDirtVolumeDb { get; set; } = -8f;
    [Export] public float CrouchVolumeDb { get; set; } = -18f;

    /// <summary>When true, logs each footstep (surface/state/sample). Set by Audio Debug F7.</summary>
    public bool DebugLoggingEnabled { get; set; }

    private CharacterBody3D? _body;
    private PlayerController? _controller;
    private AudioStreamPlayer? _player;
    private readonly List<NamedStream> _wood = new();
    private readonly List<NamedStream> _dirt = new();
    private readonly HashSet<SurfaceAudioZone3D> _surfaces = new();
    private readonly RandomNumberGenerator _rng = new();
    private readonly HashSet<string> _warned = new(StringComparer.Ordinal);

    private float _timer;
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

        if (!_body.IsOnFloor())
        {
            _timer = 0f;
            return;
        }

        var horizontal = new Vector2(_body.Velocity.X, _body.Velocity.Z).Length();
        if (horizontal < MinSpeed)
        {
            _timer = 0f;
            return;
        }

        var isCrouching = _controller?.IsCrouching == true;
        var isSprinting = _controller?.IsSprinting == true && !isCrouching;
        var interval = isCrouching ? CrouchStepInterval : isSprinting ? RunStepInterval : WalkStepInterval;

        _timer += (float)delta;
        if (_timer < interval)
        {
            return;
        }

        _timer = 0f;
        PlayStep(isSprinting, isCrouching);
    }

    /// <summary>
    /// Debug helper. "run" previews the current surface bank at run volume/pitch (not player_run_step_*).
    /// </summary>
    public void DebugPlayBank(string bank)
    {
        if (_player != null && _player.Playing)
        {
            _player.Stop();
        }

        switch (bank)
        {
            case "wood":
                PlayFromBank(_wood, ref _lastWood, WalkWoodVolumeDb, "Walk", FootstepSurfaceType.Wood,
                    pitchMin: 0.97f, pitchMax: 1.03f, force: true);
                break;
            case "dirt":
                PlayFromBank(_dirt, ref _lastDirt, WalkDirtVolumeDb, "Walk", FootstepSurfaceType.DirtGravel,
                    pitchMin: 0.97f, pitchMax: 1.03f, force: true);
                break;
            case "run":
            {
                var surface = ResolveSurface();
                if (surface == FootstepSurfaceType.DirtGravel)
                {
                    PlayFromBank(_dirt, ref _lastDirt, RunDirtVolumeDb, "Run", surface,
                        pitchMin: 0.98f, pitchMax: 1.04f, force: true);
                }
                else
                {
                    PlayFromBank(_wood, ref _lastWood, RunWoodVolumeDb, "Run", FootstepSurfaceType.Wood,
                        pitchMin: 0.98f, pitchMax: 1.04f, force: true);
                }

                break;
            }
        }
    }

    private void PlayStep(bool isSprinting, bool isCrouching)
    {
        var surface = ResolveSurface();
        var state = isCrouching ? "Crouch" : isSprinting ? "Run" : "Walk";

        float volumeDb;
        float pitchMin;
        float pitchMax;

        if (isCrouching)
        {
            volumeDb = CrouchVolumeDb;
            pitchMin = 0.96f;
            pitchMax = 1.02f;
        }
        else if (isSprinting)
        {
            volumeDb = surface == FootstepSurfaceType.DirtGravel ? RunDirtVolumeDb : RunWoodVolumeDb;
            pitchMin = 0.98f;
            pitchMax = 1.04f;
        }
        else
        {
            volumeDb = surface == FootstepSurfaceType.DirtGravel ? WalkDirtVolumeDb : WalkWoodVolumeDb;
            pitchMin = 0.97f;
            pitchMax = 1.03f;
        }

        if (surface == FootstepSurfaceType.DirtGravel)
        {
            PlayFromBank(_dirt, ref _lastDirt, volumeDb, state, surface, pitchMin, pitchMax);
            return;
        }

        PlayFromBank(_wood, ref _lastWood, volumeDb, state, FootstepSurfaceType.Wood, pitchMin, pitchMax);
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
        string state,
        FootstepSurfaceType surface,
        float pitchMin,
        float pitchMax,
        bool force = false)
    {
        if (_player == null || bank.Count == 0)
        {
            return;
        }

        if (!force && _player.Playing)
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
        _player.Stream = sample.Stream;
        _player.VolumeDb = volumeDb;
        _player.PitchScale = _rng.RandfRange(pitchMin, pitchMax);
        _player.Play();

        if (DebugLoggingEnabled || force)
        {
            GD.Print($"[Footstep] surface={surface} state={state} sample={sample.Id}");
        }
    }

    private void LoadBanks()
    {
        for (var i = 1; i <= 8; i++)
        {
            TryAdd($"player_footstep_wood_{i:D2}", _wood);
        }

        for (var i = 1; i <= 12; i++)
        {
            TryAdd($"player_footstep_dirt_gravel_{i:D2}", _dirt);
        }

        // player_run_step_01..12 remain on disk for a future chase/panic sprint — not loaded here.
        GD.Print($"[Audio] Footsteps loaded: wood={_wood.Count}, dirt={_dirt.Count} (run bank deferred)");
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
