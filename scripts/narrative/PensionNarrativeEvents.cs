namespace BREU.Scripts.Narrative;

using BREU.Scripts.Lighting;

/// <summary>
/// Sprint 15 — one-shot narrative events for PensaoVerticalBlockout01.
/// No enemy, combat, chase, or layout changes. Audio optional (not used without assets).
/// </summary>
public partial class PensionNarrativeEvents : Node
{
    public const string EventPensionEntry = "pension_entry_first_time";
    public const string EventKeyTension = "key_pickup_tension";
    public const string EventFuseFootsteps = "fuse_pickup_footsteps";
    public const string EventStairArrival = "stair_first_arrival";
    public const string EventUpperPresence = "upper_corridor_presence";
    public const string EventLockedDoorHint = "upper_locked_door_hint";

    private readonly HashSet<string> _fired = new(StringComparer.Ordinal);
    private readonly List<NarrativeTrigger3D> _triggers = new();

    private LightFlickerOneShot? _flicker;
    private PensaoPuzzleState? _puzzle;
    private Node3D? _lighting;
    private bool _messageBusy;
    private readonly Queue<(string Text, float Duration)> _messageQueue = new();

    public static PensionNarrativeEvents? Find(SceneTree tree) =>
        tree.GetFirstNodeInGroup("pension_narrative_events") as PensionNarrativeEvents;

    public override void _Ready()
    {
        AddToGroup("pension_narrative_events");

        _flicker = new LightFlickerOneShot { Name = "LightFlickerOneShot" };
        AddChild(_flicker);

        CallDeferred(nameof(DeferredSetup));
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey { Pressed: true, Keycode: Key.F8, Echo: false })
        {
            ResetAllEvents();
            HUDController.FindActive(GetTree())?.ShowMessage("Eventos narrativos resetados (F8).", 2.5f);
            GetViewport().SetInputAsHandled();
        }
    }

    public bool HasFired(string eventId) => _fired.Contains(eventId);

    public bool TryTrigger(string eventId)
    {
        if (string.IsNullOrWhiteSpace(eventId) || _fired.Contains(eventId))
        {
            return false;
        }

        _fired.Add(eventId);
        GD.Print($"[Narrative] Triggered: {eventId}");
        Execute(eventId);
        return true;
    }

    public void ResetAllEvents()
    {
        _fired.Clear();
        _messageQueue.Clear();
        _messageBusy = false;

        foreach (var trigger in _triggers)
        {
            if (GodotObject.IsInstanceValid(trigger))
            {
                trigger.ResetTrigger();
            }
        }

        GD.Print("[Narrative] Events reset.");
    }

    private void DeferredSetup()
    {
        _puzzle = GetTree().Root.FindChild("PuzzleState", recursive: true, owned: false) as PensaoPuzzleState
            ?? GetParent()?.GetNodeOrNull<PensaoPuzzleState>("PuzzleState");
        _lighting = GetParent()?.GetNodeOrNull<Node3D>("Lighting");

        if (_puzzle != null)
        {
            _puzzle.DepositKeyPickedUp += OnDepositKeyPickedUp;
            _puzzle.OldFusePickedUp += OnOldFusePickedUp;
        }

        CreateSpatialTriggers();
    }

    private void OnDepositKeyPickedUp()
    {
        // Let pickup HUD message show first, then tension beat.
        _ = DelayedTriggerAsync(EventKeyTension, 3.2f);
    }

    private void OnOldFusePickedUp()
    {
        _ = DelayedTriggerAsync(EventFuseFootsteps, 3.2f);
    }

    private async System.Threading.Tasks.Task DelayedTriggerAsync(string eventId, float delaySeconds)
    {
        await ToSignal(GetTree().CreateTimer(delaySeconds), SceneTreeTimer.SignalName.Timeout);
        TryTrigger(eventId);
    }

    private void CreateSpatialTriggers()
    {
        var host = new Node3D { Name = "NarrativeTriggers" };
        AddChild(host);

        // Reception first entry (after porch / into reception).
        AddTrigger(host, "Trigger_PensionEntry_FirstTime", EventPensionEntry,
            new Vector3(0f, 1.1f, -1.5f), new Vector3(4.5f, 2.2f, 3.0f));

        // Upper landing at stair top.
        AddTrigger(host, "Trigger_UpperLanding_FirstTime", EventStairArrival,
            new Vector3(-2.2f, 4.0f, -21.0f), new Vector3(3.2f, 2.4f, 2.8f));

        // Mid upper corridor presence.
        AddTrigger(host, "Trigger_UpperCorridor_Presence", EventUpperPresence,
            new Vector3(0f, 4.0f, -13.5f), new Vector3(2.2f, 2.4f, 2.4f));
    }

    private void AddTrigger(Node3D parent, string name, string eventId, Vector3 position, Vector3 size)
    {
        var trigger = new NarrativeTrigger3D
        {
            Name = name,
            EventId = eventId,
            Position = position
        };
        trigger.AddChild(new CollisionShape3D
        {
            Shape = new BoxShape3D { Size = size }
        });
        parent.AddChild(trigger);
        trigger.Bind(this);
        _triggers.Add(trigger);
    }

    private void Execute(string eventId)
    {
        switch (eventId)
        {
            case EventPensionEntry:
                QueueMessage("A recepção está vazia, mas a casa parece ter ouvido minha chegada.", 3.5f);
                _flicker?.Flicker(GetLight("ReceptionLight"), 0.55f, 0.45f, 2);
                break;

            case EventKeyTension:
                QueueMessage("Um rangido atravessa o corredor.", 3.0f);
                _flicker?.FlickerMany(
                    new[] { GetLight("CorridorLight"), GetLight("CorridorDeepLight") },
                    1.1f, 0.42f, 3);
                break;

            case EventFuseFootsteps:
                QueueMessage("Passos lentos ecoam acima de mim.", 3.5f);
                _flicker?.FlickerMany(
                    new[] { GetLight("CorridorLight"), GetLight("StairWellLight"), GetLight("DepositLight") },
                    1.6f, 0.38f, 4);
                break;

            case EventStairArrival:
                QueueMessage("O ar aqui em cima é mais frio.", 3.0f);
                _flicker?.FlickerMany(
                    new[] { GetLight("UpperLandingLight"), GetLight("UpperCorridorLight") },
                    1.2f, 0.4f, 3);
                break;

            case EventUpperPresence:
                QueueMessage("Por um instante, achei ter visto alguém no fim do corredor.", 3.5f);
                _flicker?.Flicker(GetLight("UpperCorridorFarLight"), 1.0f, 0.35f, 3);
                break;

            case EventLockedDoorHint:
                QueueMessage("Do outro lado, algo arranha a madeira.", 3.0f);
                _flicker?.Flicker(GetLight("UpperBlockedDoorLight"), 0.9f, 0.4f, 2);
                break;
        }
    }

    private Light3D? GetLight(string name) =>
        _lighting?.GetNodeOrNull<Light3D>(name);

    private void QueueMessage(string text, float duration)
    {
        _messageQueue.Enqueue((text, duration));
        if (!_messageBusy)
        {
            _ = DrainMessageQueueAsync();
        }
    }

    private async System.Threading.Tasks.Task DrainMessageQueueAsync()
    {
        _messageBusy = true;
        while (_messageQueue.Count > 0)
        {
            var (text, duration) = _messageQueue.Dequeue();
            HUDController.FindActive(GetTree())?.ShowMessage(text, duration);
            await ToSignal(GetTree().CreateTimer(duration + 0.15f), SceneTreeTimer.SignalName.Timeout);
        }

        _messageBusy = false;
    }
}
