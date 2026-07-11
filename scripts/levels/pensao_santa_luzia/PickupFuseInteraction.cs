namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Interaction;

/// <summary>
/// One-time fuse pickup inside deposit (Sprint 07).
/// </summary>
public partial class PickupFuseInteraction : Node, IInteractable
{
    private const string Prompt = "Pegar fusível velho";
    private const string Message = "Fusível velho adquirido.";

    private PensaoPuzzleState? _state;
    private Node3D? _pickupRoot;
    private bool _pickedUp;

    public void Initialize(PensaoPuzzleState state, Node3D pickupRoot)
    {
        _state = state;
        _pickupRoot = pickupRoot;

        if (!pickupRoot.IsInGroup("interactable"))
        {
            pickupRoot.AddToGroup("interactable");
        }
    }

    public string GetPromptText()
    {
        if (_pickedUp || _state == null || !_state.IsDepositUnlocked)
        {
            return string.Empty;
        }

        return Prompt;
    }

    public void Interact(Node interactor)
    {
        if (_pickedUp || _state == null || !_state.IsDepositUnlocked)
        {
            return;
        }

        _state.PickupOldFuse();
        _pickedUp = true;
        HUDController.FindActive(GetTree())?.ShowMessage(Message, 3.0f);
        HidePickup();
    }

    private void HidePickup()
    {
        if (_pickupRoot == null)
        {
            return;
        }

        _pickupRoot.Visible = false;

        foreach (var child in _pickupRoot.GetChildren())
        {
            if (child is not Area3D area)
            {
                continue;
            }

            area.SetDeferred("monitoring", false);
            area.SetDeferred("monitorable", false);

            foreach (var areaChild in area.GetChildren())
            {
                if (areaChild is CollisionShape3D shape)
                {
                    shape.SetDeferred(CollisionShape3D.PropertyName.Disabled, true);
                }
            }
        }
    }
}
