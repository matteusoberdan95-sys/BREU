namespace BREU.Scripts.Interaction;

public partial class InteractableNote : Area3D, IInteractable
{
    [Export] public string InteractionText { get; set; } = "Ler bilhete";
    [Export] public string NoteTitle { get; set; } = "Bilhete do Quarto 07";
    [Export(PropertyHint.MultilineText)] public string NoteText { get; set; } = "Quarto 07 ocupado. N\u00e3o abrir depois das 22h. Se a luz apagar, n\u00e3o responda \u00e0s batidas.";
    [Export] public NodePath NoteReaderPath { get; set; } = "../../UI/NoteReaderUI";
    [Export] public NodePath SequenceControllerPath { get; set; } = "../../DemoRoomSequenceController";

    public string GetInteractionText()
    {
        if (NoteReaderUI.Find(this) is { IsOpen: true })
        {
            return "";
        }

        return InteractionText;
    }

    public void Interact(PlayerController player)
    {
        GD.Print("Bilhete encontrado:");
        GD.Print(NoteText);

        var reader = GetNodeOrNull<NoteReaderUI>(NoteReaderPath) ?? NoteReaderUI.Find(this);
        if (reader != null)
        {
            reader.ShowNote(NoteTitle, NoteText);
        }
        else
        {
            GD.Print("NoteReaderUI nao encontrado — usando fallback no console.");
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
