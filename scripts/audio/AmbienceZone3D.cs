namespace BREU.Scripts.Audio;

/// <summary>
/// Sprint 16 — Area3D ambience zone. No physics collision; notifies PensionAudioManager.
/// </summary>
public partial class AmbienceZone3D : Area3D
{
    [Export] public string AmbienceId { get; set; } = string.Empty;
    [Export] public int ZonePriority { get; set; } = 0;
    [Export] public string SecondaryLoopId { get; set; } = string.Empty;
    [Export] public string TertiaryLoopId { get; set; } = string.Empty;

    private PensionAudioManager? _manager;
    private bool _playerInside;

    public override void _Ready()
    {
        Monitoring = true;
        Monitorable = false;
        CollisionLayer = 0;
        CollisionMask = 16;
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    public void Bind(PensionAudioManager manager) => _manager = manager;

    public bool IsPlayerInside => _playerInside;

    private void OnBodyEntered(Node3D body)
    {
        if (body is not CharacterBody3D || string.IsNullOrWhiteSpace(AmbienceId))
        {
            return;
        }

        _playerInside = true;
        _manager ??= PensionAudioManager.Find(GetTree());
        _manager?.NotifyZoneEntered(this);
    }

    private void OnBodyExited(Node3D body)
    {
        if (body is not CharacterBody3D)
        {
            return;
        }

        _playerInside = false;
        _manager ??= PensionAudioManager.Find(GetTree());
        _manager?.NotifyZoneExited(this);
    }
}
