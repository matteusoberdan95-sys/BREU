namespace BREU.Scripts.Audio;

/// <summary>
/// Sprint 16B — random one-shot emitter (water drops). Audio only, no spam.
/// </summary>
public partial class RandomOneShotEmitter3D : Area3D
{
    [Export] public string SoundIdFormat { get; set; } = "water_drop_{0:D2}";
    [Export] public int VariantCount { get; set; } = 3;
    [Export] public float MinIntervalSeconds { get; set; } = 6f;
    [Export] public float MaxIntervalSeconds { get; set; } = 18f;
    [Export] public float VolumeDb { get; set; } = -14f;

    private readonly RandomNumberGenerator _rng = new();
    private float _timer;
    private bool _playerInside;

    public override void _Ready()
    {
        Monitoring = true;
        Monitorable = false;
        CollisionLayer = 0;
        CollisionMask = 16;
        _rng.Randomize();
        _timer = _rng.RandfRange(MinIntervalSeconds, MaxIntervalSeconds);
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    public override void _Process(double delta)
    {
        if (!_playerInside)
        {
            return;
        }

        _timer -= (float)delta;
        if (_timer > 0f)
        {
            return;
        }

        _timer = _rng.RandfRange(MinIntervalSeconds, MaxIntervalSeconds);
        var index = _rng.RandiRange(1, Mathf.Max(1, VariantCount));
        var id = string.Format(SoundIdFormat, index);
        PensionAudioManager.Find(GetTree())?.PlayOneShot(id, VolumeDb);
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body is CharacterBody3D)
        {
            _playerInside = true;
        }
    }

    private void OnBodyExited(Node3D body)
    {
        if (body is CharacterBody3D)
        {
            _playerInside = false;
        }
    }
}
