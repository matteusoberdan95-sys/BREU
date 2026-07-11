namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>Interacao narrativa isolada para o playtest da pensao integrada.</summary>
public partial class MessageInteractable : Area3D, IInteractable
{
    [Export] public string InteractionText { get; set; } = "Examinar";
    [Export(PropertyHint.MultilineText)] public string Message { get; set; } = "";
    [Export] public float Duration { get; set; } = 3.5f;

    public string GetInteractionText() => InteractionText;

    public void Interact(PlayerController player)
    {
        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.ShowTemporaryMessage(Message, Duration);
        }
        else
        {
            GD.Print($"Pensao: {Message}");
        }
    }
}
