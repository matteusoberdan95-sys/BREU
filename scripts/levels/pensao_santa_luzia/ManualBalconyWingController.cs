namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>Initializes pre-authored BalconyWing.tscn nodes; creates no geometry or areas.</summary>
public partial class ManualBalconyWingController : Node3D
{
    public override void _Ready() => CallDeferred(nameof(InitializeStaticInteractions));

    private void InitializeStaticInteractions()
    {
        var state = GetTree().CurrentScene.GetNodeOrNull<PensaoPuzzleState>("PuzzleState");
        if (state == null)
        {
            GD.PushError("[ManualBalconyWing] PuzzleState missing.");
            return;
        }

        var ownerDoor = GetNodeOrNull<BlockoutOwnerBedroomDoor>("Interactions/Door_OwnerBedroom_Blocker");
        ownerDoor?.Initialize(state, ownerDoor);

        GD.Print("[ManualBalconyWing] Static scene initialized; no runtime geometry created.");
    }
}
