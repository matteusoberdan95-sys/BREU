namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>Deposit door: unlocking only hides its panel and disables its blocking shape.</summary>
public partial class BlockoutUnlockHideDoor : Node3D, IInteractable
{
    private PensaoPuzzleState? _state;
    private MeshInstance3D? _panel;
    private CollisionShape3D? _blockingShape;
    private Area3D? _area;

    public void Initialize(PensaoPuzzleState state, Node3D doorRoot)
    {
        _state = state;
        _panel = doorRoot.GetNodeOrNull<MeshInstance3D>("DoorPanel");
        _blockingShape = doorRoot.GetNodeOrNull<CollisionShape3D>("BlockingBody/BlockingShape");
        _area = doorRoot.GetNodeOrNull<Area3D>("InteractionArea");
        doorRoot.GetNodeOrNull<MeshInstance3D>("Frame/OpenDoorLeafLeft")?.Hide();
        doorRoot.GetNodeOrNull<MeshInstance3D>("Frame/OpenDoorLeafRight")?.Hide();
        doorRoot.GetNodeOrNull<MeshInstance3D>("Frame/UpperWallInfill")?.Hide();
        ApplyState();
    }

    public string GetPromptText() => _state switch
    {
        null => string.Empty,
        { IsDepositUnlocked: true } => string.Empty,
        { HasDepositKey: true } => "Usar chave velha",
        _ => "Tentar abrir depósito"
    };

    public void Interact(Node interactor)
    {
        if (_state == null || _state.IsDepositUnlocked)
            return;

        var hud = HUDController.FindActive(GetTree());
        if (!_state.HasDepositKey)
        {
            hud?.ShowMessage("Está trancado. Preciso encontrar a chave.", 3.0f);
            return;
        }

        _state.UnlockDeposit();
        ApplyState();
        hud?.HideInteractionPrompt();
        hud?.ShowMessage("A chave gira com dificuldade. A porta destranca.", 3.0f);
    }

    public void ApplyState()
    {
        var open = _state?.IsDepositUnlocked == true;
        if (_panel != null) _panel.Visible = !open;
        if (_blockingShape != null) _blockingShape.Disabled = open;
        if (_area != null) _area.Monitorable = !open;
    }
}
