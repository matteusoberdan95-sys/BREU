namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;

/// <summary>Sprint 19 — permanently locked door (Room 205). Never opens.</summary>
public partial class BlockoutLockedRoomDoor : Node3D, IInteractable
{
    [Export] public string Prompt { get; set; } = "Tentar abrir Quarto 205";
    [Export] public string LockedMessage { get; set; } =
        "A maçaneta gira sozinha, mas a porta não abre.";

    public override void _Ready()
    {
        if (!IsInGroup("interactable"))
        {
            AddToGroup("interactable");
        }
    }

    public string GetPromptText() => Prompt;

    public void Interact(Node interactor)
    {
        HUDController.FindActive(GetTree())?.ShowMessage(LockedMessage, 3.5f);
        PensionAudioManager.Find(GetTree())?.PlayOneShot("door_scratch_01", -18f);
    }
}
