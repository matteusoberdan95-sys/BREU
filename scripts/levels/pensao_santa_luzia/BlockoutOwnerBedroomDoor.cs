namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Narrative;

/// <summary>Sprint 17C — owner bedroom unlock (panel hide + collision off).</summary>
public partial class BlockoutOwnerBedroomDoor : Node3D, IInteractable
{
    private PensaoPuzzleState? _state;
    private MeshInstance3D? _panel;
    private CollisionShape3D? _blockingShape;
    private Area3D? _area;

    public void Initialize(PensaoPuzzleState state, Node3D doorRoot)
    {
        _state = state;
        _panel = doorRoot.GetNodeOrNull<MeshInstance3D>("Door_OwnerBedroom_Blocker")
            ?? doorRoot.GetNodeOrNull<MeshInstance3D>("DoorPanel");
        _blockingShape = doorRoot.GetNodeOrNull<CollisionShape3D>("BlockingBody/BlockingShape");
        _area = doorRoot.GetNodeOrNull<Area3D>("InteractionArea");
        ApplyState();
    }

    public string GetPromptText() => _state switch
    {
        null => string.Empty,
        { IsOwnerRoomUnlocked: true } => string.Empty,
        { HasOwnerRoomKey: true } => "Destravar quarto",
        _ => "Tentar abrir quarto"
    };

    public void Interact(Node interactor)
    {
        if (_state == null || _state.IsOwnerRoomUnlocked)
        {
            return;
        }

        var hud = HUDController.FindActive(GetTree());
        if (!_state.HasOwnerRoomKey)
        {
            hud?.ShowMessage("Está trancado. A fechadura parece antiga.", 3.0f);
            return;
        }

        _state.UnlockOwnerRoom();
        ApplyState();
        hud?.HideInteractionPrompt();
        hud?.ShowMessage("A chave encaixa. A porta cede devagar.", 3.0f);
    }

    public void ApplyState()
    {
        var open = _state?.IsOwnerRoomUnlocked == true;
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
}
