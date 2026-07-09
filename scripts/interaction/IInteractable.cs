namespace BREU.Scripts.Interaction;

public interface IInteractable
{
    string GetInteractionText();
    void Interact(PlayerController player);
}
