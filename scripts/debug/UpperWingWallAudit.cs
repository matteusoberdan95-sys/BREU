namespace BREU.Scripts.Debug;

/// <summary>F9 ownership and structural audit scoped to UpperWingRooms.</summary>
public partial class UpperWingWallAudit : Node
{
    private int _errors;
    private static readonly string[] Rooms = { "Room204_Bedroom", "Room205_Locked", "SharedBathroom", "LaundryStorage", "TechnicalRoom", "OwnersOffice" };

    public override void _Ready() => CallDeferred(nameof(RunAudit));
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey { Pressed: true, Echo: false, Keycode: Key.F9 }) RunAudit();
    }

    public void RunAudit()
    {
        _errors = 0;
        var root = GetTree().CurrentScene?.GetNodeOrNull<Node3D>("World/Level/SecondFloor/UpperWingRooms");
        if (root == null) { Error("UpperWingRooms missing"); return; }

        foreach (var wall in Enumerate(root).OfType<Node3D>().Where(n => n.Name.ToString().StartsWith("Wall_")))
        {
            var mesh = wall.GetNodeOrNull<MeshInstance3D>("MeshInstance3D")?.Mesh as BoxMesh;
            var body = wall.GetNodeOrNull<StaticBody3D>("StaticBody3D");
            var shape = wall.GetNodeOrNull<CollisionShape3D>("StaticBody3D/CollisionShape3D")?.Shape as BoxShape3D;
            if (mesh == null || body == null || shape == null || !mesh.Size.IsEqualApprox(shape.Size)) Error($"wall without matching collider: {wall.GetPath()}");
            else if (Mathf.Min(mesh.Size.X, mesh.Size.Z) < 0.3f) Error($"wall is too thin for reliable player blocking: {wall.GetPath()} size={mesh.Size}");
            else GD.Print($"[UpperWingWallAudit] OK wall: {wall.Name}");
        }

        foreach (var room in Rooms)
            if (root.FindChild($"Ceiling_{CeilingSuffix(room)}", true, false) is not MeshInstance3D) Error($"room without ceiling: {room}");

        CheckOwned(root, "TechnicalRoom/TechnicalPanel", "panel outside TechnicalRoom");
        CheckOwned(root, "LaundryStorage/Interact_LaundryWire", "ArameTorto outside LaundryStorage");
        CheckOwned(root, "LaundryStorage/Interact_LaundryFuse", "UpperFuse outside LaundryStorage");
        CheckOwned(root, "SharedBathroom/Interact_BathroomInspect", "drain outside SharedBathroom");
        CheckOwned(root, "SharedBathroom/Prop_Bath_Drain", "drain visual outside SharedBathroom");
        CheckOwned(root, "SharedBathroom/Wall_WestOuter_LaundryBathJoin", "open west seam between Laundry and Bathroom");
        CheckOwned(root, "TechnicalRoom/Wall_EastOuter_204TechJoin", "open east seam between Room 204 and TechnicalRoom");
        CheckOwned(root, "OwnersOffice/Wall_WestOuter_BathOfficeJoin", "open west seam between Bathroom and OwnersOffice");
        CheckOwned(root, "Room205_Locked/Wall_EastOuter_Tech205Join", "open east seam between TechnicalRoom and Room 205");
        CheckOwned(root, "Corridor_Main/Ceiling_Transition/StaticBody3D/CollisionShape3D", "green-door transition has no solid ceiling");
        CheckOwned(root, "SharedBathroom/Interact_BathroomInspect/InteractionArea/ManualBalconyInteraction", "bathroom drain is not wired to the key puzzle");

        foreach (var node in Enumerate(root))
        {
            var name = node.Name.ToString();
            if (name.StartsWith("FloorVis_", StringComparison.OrdinalIgnoreCase)) Error($"duplicate visual floor plate: {node.GetPath()}");
            if (new[] { "Old", "Temp", "Debug", "Backup", "Legacy", "Boundary", "Invisible" }
                .Any(part => name.Contains(part, StringComparison.OrdinalIgnoreCase))) Error($"duplicate/legacy node: {node.GetPath()}");
            if (node is Area3D area && area.GetNodeOrNull<CollisionShape3D>("CollisionShape3D")?.Shape is BoxShape3D box &&
                Mathf.Max(box.Size.X, Mathf.Max(box.Size.Y, box.Size.Z)) > 1.8f) Error($"oversized interaction: {area.GetPath()}");
        }

        if (_errors == 0) GD.Print("[UpperWingWallAudit] OK no duplicate/legacy geometry — 0 ERROR");
        else GD.PrintErr($"[UpperWingWallAudit] FAIL {_errors} ERROR(s)");
    }

    private static string CeilingSuffix(string room) => room switch
    {
        "Room204_Bedroom" => "204", "Room205_Locked" => "205", "SharedBathroom" => "Bath", "LaundryStorage" => "Laundry",
        "TechnicalRoom" => "Tech", "OwnersOffice" => "Office", _ => room
    };

    private void CheckOwned(Node root, string path, string message)
    {
        if (root.GetNodeOrNull(path) == null) Error(message);
    }

    private void Error(string message) { _errors++; GD.PrintErr($"[UpperWingWallAudit] ERROR {message}"); }
    private static IEnumerable<Node> Enumerate(Node root)
    {
        yield return root;
        foreach (var child in root.GetChildren()) foreach (var node in Enumerate(child)) yield return node;
    }
}
