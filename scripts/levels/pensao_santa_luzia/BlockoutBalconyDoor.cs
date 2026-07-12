namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Narrative;

/// <summary>
/// Sprint 17 — green balcony door. Unlock hides panel + disables collision (deposit pattern).
/// </summary>
public partial class BlockoutBalconyDoor : Node3D, IInteractable
{
    private PensaoPuzzleState? _state;
    private MeshInstance3D? _panel;
    private CollisionShape3D? _blockingShape;
    private Area3D? _area;
    private bool _hintFired;

    public void Initialize(PensaoPuzzleState state, Node3D doorRoot)
    {
        _state = state;
        _panel = doorRoot.GetNodeOrNull<MeshInstance3D>("BalconyDoor_Green_Panel")
            ?? doorRoot.GetNodeOrNull<MeshInstance3D>("Door_UpperBalcony_Blocker")
            ?? doorRoot.GetNodeOrNull<MeshInstance3D>("DoorPanel");
        _blockingShape = doorRoot.GetNodeOrNull<CollisionShape3D>("BlockingBody/BlockingShape");
        _area = doorRoot.GetNodeOrNull<Area3D>("Interact_BalconyDoor")
            ?? doorRoot.GetNodeOrNull<Area3D>("InteractionArea");
        ApplyState();
    }

    public string GetPromptText() => _state switch
    {
        null => string.Empty,
        { IsBalconyUnlocked: true } => string.Empty,
        { HasBalconyKey: true } => "Destravar varanda",
        _ => "Tentar abrir varanda"
    };

    public void Interact(Node interactor)
    {
        if (_state == null || _state.IsBalconyUnlocked)
        {
            return;
        }

        var hud = HUDController.FindActive(GetTree());
        if (!_state.HasBalconyKey)
        {
            hud?.ShowMessage("Está trancada.", 3.0f);
            if (!_hintFired)
            {
                _hintFired = true;
                CallDeferred(nameof(DeferredNarrativeHint));
            }

            return;
        }

        _state.UnlockBalcony();
        ApplyState();
        hud?.HideInteractionPrompt();
        hud?.ShowMessage("Ouvi o trinco ceder.", 3.0f);
        PensionNarrativeEvents.Find(GetTree())?.TryTrigger(PensionNarrativeEvents.EventBalconyOpened);
    }

    public void ApplyState()
    {
        var open = _state?.IsBalconyUnlocked == true;
        if (_panel != null)
        {
            _panel.Visible = !open;
        }

        if (_blockingShape != null)
        {
            _blockingShape.Disabled = open;
        }

        if (_area != null)
        {
            _area.Monitorable = !open;
        }
    }

    private void DeferredNarrativeHint()
    {
        _ = HintAsync();
    }

    private async System.Threading.Tasks.Task HintAsync()
    {
        await ToSignal(GetTree().CreateTimer(3.1f), SceneTreeTimer.SignalName.Timeout);
        PensionNarrativeEvents.Find(GetTree())?.TryTrigger(PensionNarrativeEvents.EventLockedDoorHint);
    }
}
