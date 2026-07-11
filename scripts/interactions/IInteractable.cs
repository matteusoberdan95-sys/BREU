namespace BREU.Scripts.Interaction;

/// <summary>
/// Contract for first-person interactable objects (Sprint 04+).
/// </summary>
public interface IInteractable
{
    string GetPromptText();

    void Interact(Node interactor);
}
