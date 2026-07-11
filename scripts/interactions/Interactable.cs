namespace BREU.Scripts.Interaction;

/// <summary>
/// Base interactable component. Attach as child of a collider body in group "interactable".
/// </summary>
public partial class Interactable : Node, IInteractable
{
    [Export] public string PromptText { get; set; } = "Interagir";
    [Export(PropertyHint.MultilineText)] public string InteractionMessage { get; set; } = string.Empty;
    [Export] public bool OneShot { get; set; }
    [Export] public string InteractionId { get; set; } = string.Empty;

    public bool HasBeenUsed { get; private set; }

    public override void _Ready()
    {
        RegisterInteractableGroup(GetParent());

        if (GetParent() is Area3D area && area.GetParent() != null)
        {
            RegisterInteractableGroup(area.GetParent());
        }
    }

    public string GetPromptText()
    {
        if (OneShot && HasBeenUsed)
        {
            return string.Empty;
        }

        return PromptText;
    }

    public void Interact(Node interactor)
    {
        if (OneShot && HasBeenUsed)
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(InteractionMessage))
        {
            HUDController.FindActive(GetTree())?.ShowMessage(InteractionMessage, 3.0f);
        }

        if (OneShot)
        {
            HasBeenUsed = true;
        }
    }

    private static void RegisterInteractableGroup(Node? host)
    {
        if (host == null)
        {
            return;
        }

        if (!host.IsInGroup("interactable"))
        {
            host.AddToGroup("interactable");
        }
    }
}
