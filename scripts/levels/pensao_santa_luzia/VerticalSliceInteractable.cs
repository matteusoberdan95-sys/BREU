namespace BREU.Scripts.Levels.PensaoSantaLuzia;

public enum VerticalSliceInteractionKind
{
    GuestRegister,
    Room102Door,
    Room102Note,
    FusePickup,
    Deposit,
    UpperCorridor,
    ManagerClue,
    LockedUpperRoom,
    Bathroom,
}

/// <summary>Adaptador de IInteractable para o estado local da vertical slice.</summary>
public partial class VerticalSliceInteractable : Area3D, IInteractable
{
    [Export] public NodePath ControllerPath { get; set; } = "../../VerticalSliceController";
    [Export] public VerticalSliceInteractionKind Kind { get; set; }
    [Export] public string InteractionText { get; set; } = "Examinar";
    [Export] public float Duration { get; set; } = 4.0f;

    public string GetInteractionText() => InteractionText;

    public void Interact(PlayerController player)
    {
        var message = GetNodeOrNull<PensaoVerticalSliceController>(ControllerPath)?.Interact(Kind)
            ?? "Interacao indisponivel.";
        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.ShowTemporaryMessage(message, Duration);
        }
        else
        {
            GD.Print($"PensaoVerticalSlice: {message}");
        }
    }
}
