namespace BREU.Scripts.Narrative;

/// <summary>
/// Sprint 15 — Area3D one-shot narrative trigger. Does not block the player.
/// </summary>
public partial class NarrativeTrigger3D : Area3D
{
    [Export] public string EventId { get; set; } = string.Empty;
    [Export] public bool OneShot { get; set; } = true;

    private bool _consumed;
    private PensionNarrativeEvents? _events;

    public override void _Ready()
    {
        Monitoring = true;
        Monitorable = false;
        CollisionLayer = 0;
        // Player CharacterBody3D uses collision_layer = 16.
        CollisionMask = 16;
        BodyEntered += OnBodyEntered;
    }

    public void Bind(PensionNarrativeEvents events)
    {
        _events = events;
    }

    public void ResetTrigger()
    {
        _consumed = false;
        Monitoring = true;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (_consumed || string.IsNullOrWhiteSpace(EventId))
        {
            return;
        }

        if (body is not CharacterBody3D)
        {
            return;
        }

        _events ??= PensionNarrativeEvents.Find(GetTree());
        if (_events == null)
        {
            return;
        }

        if (!_events.TryTrigger(EventId))
        {
            return;
        }

        if (OneShot)
        {
            _consumed = true;
            SetDeferred(Area3D.PropertyName.Monitoring, false);
        }
    }
}
