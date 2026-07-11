namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>Adaptador de IInteractable para a pensao blockout limpa.</summary>
public partial class PensaoBlockoutInteractable : Area3D, IInteractable
{
    [Export] public NodePath ControllerPath { get; set; } = "../PensaoBlockoutCleanController";
    [Export] public PensaoBlockoutInteractionKind Kind { get; set; }
    [Export] public string InteractionText { get; set; } = "Examinar";
    [Export] public float Duration { get; set; } = 4.0f;

    public string GetInteractionText() => InteractionText;

    public void Interact(PlayerController player)
    {
        var message = GetNodeOrNull<PensaoBlockoutCleanController>(ControllerPath)?.Interact(Kind)
            ?? "Interacao indisponivel.";
        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.ShowTemporaryMessage(message, Duration);
        }
        else
        {
            GD.Print($"PensaoBlockout: {message}");
        }
    }
}
