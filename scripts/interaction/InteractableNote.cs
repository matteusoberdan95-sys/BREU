using Godot;
using BREU.Scripts.Player;

namespace BREU.Scripts.Interaction;

public partial class InteractableNote : Area3D, IInteractable
{
    [Export] public string InteractionText { get; set; } = "Ler bilhete";
    [Export(PropertyHint.MultilineText)] public string NoteText { get; set; } = "Quarto 07 ocupado. N\u00e3o abrir depois das 22h. Se a luz apagar, n\u00e3o responda \u00e0s batidas.";

    public string GetInteractionText()
    {
        return InteractionText;
    }

    public void Interact(PlayerController player)
    {
        GD.Print("Bilhete encontrado:");
        GD.Print(NoteText);
    }
}
