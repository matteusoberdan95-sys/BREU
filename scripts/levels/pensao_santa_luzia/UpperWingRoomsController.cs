namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>Sprint 19 — initializes authored UpperWingRooms; creates no geometry.</summary>
public partial class UpperWingRoomsController : Node3D
{
    public override void _Ready() => CallDeferred(nameof(Initialize));

    private void Initialize()
    {
        var doors = GetNodeOrNull<Node>("Doors");
        if (doors == null)
        {
            GD.PushWarning("[UpperWingRooms] Doors container missing.");
            return;
        }

        foreach (var child in doors.GetChildren())
        {
            if (child is BlockoutSimpleRoomDoor simple)
            {
                // Self-binds in _Ready.
                _ = simple;
            }
        }

        GD.Print("[UpperWingRooms] Static rooms initialized; deck untouched.");
    }
}
