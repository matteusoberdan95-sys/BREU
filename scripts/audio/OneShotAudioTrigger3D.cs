namespace BREU.Scripts.Audio;

/// <summary>
/// Sprint 16 — optional Area3D one-shot trigger (safe if asset missing).
/// </summary>
public partial class OneShotAudioTrigger3D : Area3D
{
    [Export] public string SoundId { get; set; } = string.Empty;
    [Export] public bool OneShot { get; set; } = true;
    [Export] public float VolumeDb { get; set; } = -14f;

    private bool _consumed;
    private PensionAudioManager? _manager;

    public override void _Ready()
    {
        Monitoring = true;
        Monitorable = false;
        CollisionLayer = 0;
        CollisionMask = 16;
        BodyEntered += OnBodyEntered;
    }

    public void Bind(PensionAudioManager manager) => _manager = manager;

    private void OnBodyEntered(Node3D body)
    {
        if (_consumed || body is not CharacterBody3D || string.IsNullOrWhiteSpace(SoundId))
        {
            return;
        }

        _manager ??= PensionAudioManager.Find(GetTree());
        if (_manager == null)
        {
            return;
        }

        _manager.PlayOneShot(SoundId, VolumeDb);
        if (OneShot)
        {
            _consumed = true;
            Monitoring = false;
        }
    }
}
