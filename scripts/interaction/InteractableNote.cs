namespace BREU.Scripts.Interaction;

public partial class InteractableNote : Area3D, IInteractable
{
    [Export] public string InteractionText { get; set; } = "Ler bilhete";
    [Export(PropertyHint.MultilineText)] public string NoteText { get; set; } = "Quarto 07 ocupado. N\u00e3o abrir depois das 22h. Se a luz apagar, n\u00e3o responda \u00e0s batidas.";
    [Export] public NodePath SequenceControllerPath { get; set; } = "../../DemoRoomSequenceController";

    public string GetInteractionText()
    {
        return InteractionText;
    }

    public void Interact(PlayerController player)
    {
        GD.Print("Bilhete encontrado:");
        GD.Print(NoteText);

        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.ShowTemporaryMessage("Bilhete encontrado.");
        }

        NotifyNoteRead();
    }

    private void NotifyNoteRead()
    {
        if (GetNodeOrNull(SequenceControllerPath) is DemoRoomSequenceController sequence)
        {
            sequence.OnNoteRead();
            return;
        }

        if (GetTree().GetFirstNodeInGroup("demo_sequence") is DemoRoomSequenceController fallback)
        {
            fallback.OnNoteRead();
        }
    }
}
