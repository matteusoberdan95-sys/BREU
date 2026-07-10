namespace BREU.Scripts.Interaction;

/// <summary>
/// Bilhete da Sala dos Santos Secos. Usa NoteReaderUI quando disponivel.
/// </summary>
public partial class RitualNoteInteractable : Area3D, IInteractable
{
    [Export] public string InteractionText { get; set; } = "Ler bilhete";
    [Export] public string NoteTitle { get; set; } = "Bilhete da Sala dos Santos Secos";
    [Export(PropertyHint.MultilineText)]
    public string NoteText { get; set; } = "Nao mexam nos santos secos. Eles escutam quando a vela apaga. O hospede do Quarto 07 ja atravessou a porta.";
    [Export] public NodePath NoteReaderPath { get; set; } = "../../UI/NoteReaderUI";

    private bool _hasRead;

    public string GetInteractionText()
    {
        return NoteReaderUI.Find(this) is { IsOpen: true } ? "" : InteractionText;
    }

    public void Interact(PlayerController player)
    {
        GD.Print("Bilhete ritual:");
        GD.Print(NoteText);

        var reader = GetNodeOrNull<NoteReaderUI>(NoteReaderPath) ?? NoteReaderUI.Find(this);
        if (reader != null)
        {
            reader.ShowNote(NoteTitle, NoteText);
        }
        else if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.ShowTemporaryMessage(NoteText, 5.0f);
        }
        else
        {
            GD.Print("RitualNoteInteractable: NoteReaderUI/HUD nao encontrados.");
        }

        if (!_hasRead)
        {
            _hasRead = true;
            CheckpointManager.Instance?.UpdateCheckpoint("Ritual_Note_Read");
        }
    }
}
