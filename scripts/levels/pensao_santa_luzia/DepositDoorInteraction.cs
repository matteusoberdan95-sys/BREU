namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Interaction;

/// <summary>
/// Stateful deposit door — locked until key is used (Sprint 07).
/// </summary>
public partial class DepositDoorInteraction : Node, IInteractable
{
    private const string LockedPrompt = "Tentar abrir depósito";
    private const string LockedMessage = "Está trancado. Preciso encontrar uma chave.";
    private const string UnlockPrompt = "Usar chave velha";
    private const string UnlockMessage = "A porta do depósito destrancou.";

    private PensaoPuzzleState? _state;
    private StaticBody3D? _doorBody;
    private CollisionShape3D? _doorCollision;

    public void Initialize(PensaoPuzzleState state, StaticBody3D doorBody)
    {
        _state = state;
        _doorBody = doorBody;
        _doorCollision = doorBody.GetNodeOrNull<CollisionShape3D>("Door_Deposit_Blocking_Collision");

        if (!doorBody.IsInGroup("interactable"))
        {
            doorBody.AddToGroup("interactable");
        }
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
        DisableDoorBlocking();
        hud?.ShowMessage(UnlockMessage, 3.0f);
    }

    private void DisableDoorBlocking()
    {
        if (_doorBody != null)
        {
            _doorBody.Visible = false;
        }

        if (_doorCollision != null)
        {
            _doorCollision.Disabled = true;
        }
    }
}
