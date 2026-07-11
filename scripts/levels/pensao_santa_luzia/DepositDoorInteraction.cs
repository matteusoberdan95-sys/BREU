namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Sprint 14B — deposit door unlock: hide panel + disable collision only (no scale/animation).
/// </summary>
public partial class DepositDoorInteraction : Node, IInteractable
{
    private const string LockedPrompt = "Tentar abrir depósito";
    private const string LockedMessage = "Está trancado. Preciso encontrar a chave.";
    private const string UnlockPrompt = "Usar chave velha";
    private const string UnlockMessage = "A chave gira com dificuldade. A porta destranca.";

    private PensaoPuzzleState? _state;
    private Node3D? _doorPanel;
    private CollisionShape3D? _doorCollision;
    private Area3D? _interactArea;

    public void Initialize(PensaoPuzzleState state, Node3D doorRoot)
    {
        _state = state;
        _doorPanel = doorRoot.GetNodeOrNull<Node3D>("Door_Deposit_Panel");
        _doorCollision = doorRoot.GetNodeOrNull<CollisionShape3D>("Door_Deposit_Blocking/Door_Deposit_Collision");
        _interactArea = doorRoot.GetNodeOrNull<Area3D>("Door_Deposit_InteractArea");
    }

    public string GetPromptText()
    {
        if (_state == null || _state.IsDepositUnlocked)
        {
            return string.Empty;
        }

        return _state.HasDepositKey ? UnlockPrompt : LockedPrompt;
    }

    public void Interact(Node interactor)
    {
        if (_state == null || _state.IsDepositUnlocked)
        {
            return;
        }

        var hud = HUDController.FindActive(GetTree());

        if (!_state.HasDepositKey)
        {
            hud?.ShowMessage(LockedMessage, 3.0f);
            return;
        }

        _state.UnlockDeposit();
        OpenDoor();
        hud?.ShowMessage(UnlockMessage, 3.0f);
    }

    private void OpenDoor()
    {
        if (_doorPanel != null)
        {
            _doorPanel.Visible = false;
        }

        if (_doorCollision != null)
        {
            _doorCollision.Disabled = true;
        }

        if (_interactArea != null)
        {
            _interactArea.Monitorable = false;
        }
    }
}
