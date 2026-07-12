namespace BREU.Scripts.Audio;

/// <summary>
/// Sprint 16B — audible player footsteps. Reads movement state only; never writes Velocity/Position.
/// </summary>
public partial class PlayerFootstepAudio : Node
{
    private const string AssetRoot = "res://assets/audio/pensao/";
    private const float MinSpeed = 0.2f;

    [Export] public float WalkStepInterval { get; set; } = 0.48f;
    [Export] public float RunStepInterval { get; set; } = 0.30f;
    [Export] public float CrouchStepInterval { get; set; } = 0.70f;
    [Export] public float WalkWoodVolumeDb { get; set; } = -12f;
    [Export] public float WalkDirtVolumeDb { get; set; } = -10f;
    [Export] public float RunVolumeDb { get; set; } = -8f;
    [Export] public float CrouchVolumeDb { get; set; } = -18f;

    private CharacterBody3D? _body;
    private PlayerController? _controller;
    private AudioStreamPlayer? _player;
    private readonly List<AudioStream> _wood = new();
    private readonly List<AudioStream> _dirt = new();
    private readonly List<AudioStream> _run = new();
    private readonly HashSet<SurfaceAudioZone3D> _surfaces = new();
    private readonly RandomNumberGenerator _rng = new();
    private readonly HashSet<string> _warned = new(StringComparer.Ordinal);

    private float _timer;
    private int _lastWood = -1;
    private int _lastDirt = -1;
    private int _lastRun = -1;

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

    /// <summary>Debug helper — play one sample from a bank (forces play).</summary>
    public void DebugPlayBank(string bank)
    {
        if (_player != null && _player.Playing)
        {
            _player.Stop();
        }

        switch (bank)
        {
            case "wood":
                PlayFromBank(_wood, ref _lastWood, WalkWoodVolumeDb, force: true);
                break;
            case "dirt":
                PlayFromBank(_dirt, ref _lastDirt, WalkDirtVolumeDb, force: true);
                break;
            case "run":
                PlayFromBank(_run, ref _lastRun, RunVolumeDb, force: true);
                break;
        }
    }

    private void PlayStep(bool isSprinting, bool isCrouching)
    {
        if (isSprinting)
        {
            PlayFromBank(_run, ref _lastRun, RunVolumeDb);
            return;
        }

        var surface = ResolveSurface();
        if (surface == FootstepSurfaceType.DirtGravel)
        {
            var vol = isCrouching ? CrouchVolumeDb : WalkDirtVolumeDb;
            PlayFromBank(_dirt, ref _lastDirt, vol);
            return;
        }

        var woodVol = isCrouching ? CrouchVolumeDb : WalkWoodVolumeDb;
        PlayFromBank(_wood, ref _lastWood, woodVol);
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

    private void PlayFromBank(List<AudioStream> bank, ref int lastIndex, float volumeDb, bool force = false)
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
        _player.Stream = bank[index];
        _player.VolumeDb = volumeDb;
        _player.PitchScale = _rng.RandfRange(0.94f, 1.06f);
        _player.Play();
    }

    private void LoadBanks()
    {
        for (var i = 1; i <= 8; i++)
        {
            TryAdd(AssetRoot + $"player_footstep_wood_{i:D2}.ogg", _wood);
        }

        for (var i = 1; i <= 12; i++)
        {
            TryAdd(AssetRoot + $"player_footstep_dirt_gravel_{i:D2}.ogg", _dirt);
            TryAdd(AssetRoot + $"player_run_step_{i:D2}.ogg", _run);
        }

        GD.Print($"[Audio] Footsteps loaded: wood={_wood.Count}, dirt={_dirt.Count}, run={_run.Count}");
    }

    private void TryAdd(string path, List<AudioStream> bank)
    {
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
            bank.Add(stream);
        }
    }
}
