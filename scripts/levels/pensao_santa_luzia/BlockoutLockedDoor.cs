namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>Stable blockout door: an opaque, immovable panel with a local interaction message.</summary>
public partial class BlockoutLockedDoor : Node3D, IInteractable
{
    [Export] public string PromptText { get; set; } = "Tentar abrir";
    [Export] public string LockedMessage { get; set; } = "Está trancada por dentro.";

    public string GetPromptText() => PromptText;

    public void Interact(Node interactor)
    {
        HUDController.FindActive(GetTree())?.ShowMessage(LockedMessage, 3.0f);
    }
}
