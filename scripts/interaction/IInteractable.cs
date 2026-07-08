using BREU.Scripts.Player;

namespace BREU.Scripts.Interaction;

public interface IInteractable
{
    string Prompt { get; }
    void Interact(PlayerController player);
}
