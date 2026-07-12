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

        GD.Print("[ManualBalconyWing] Green-door owner initialized; legacy rooms removed.");
    }
}
