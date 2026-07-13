namespace BREU.Scripts.Debug;

/// <summary>F8 wall/floor probe for the clean upper-wing rebuild.</summary>
public partial class WallCollisionProbe : Node
{
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey { Pressed: true, Echo: false, Keycode: Key.F8, CtrlPressed: false }) RunProbe();
    }

    public void RunProbe()
    {
        var scene = GetTree().CurrentScene;
        var player = scene?.FindChild("Player", true, false) as CharacterBody3D;
        var camera = player?.GetNodeOrNull<Camera3D>("HeadBase/BodyMotionPivot/LeanPivot/LookBackPivot/Camera3D");
        if (scene == null || player == null || camera == null)
        {
            GD.PrintErr("[WallProbe] ERROR: scene/player/camera missing");
            return;
        }

        var forward = -camera.GlobalTransform.Basis.Z.Normalized();
        PrintHit("Forward", camera.GlobalPosition, camera.GlobalPosition + forward * 5f);
        PrintHit("Down", player.GlobalPosition + Vector3.Up * 0.2f, player.GlobalPosition + Vector3.Down * 8f);

        var nearby = Enumerate(scene)
            .OfType<Node3D>()
            .Where(n => n.Name.ToString().StartsWith("Room") ||
                n.Name.ToString() is "SharedBathroom" or "LaundryStorage" or "TechnicalRoom")
            .Where(n => n.GlobalPosition.DistanceTo(player.GlobalPosition) < 8f)
            .Select(n => n.GetPath().ToString());
        GD.Print($"[WallProbe] Nearby rooms/triggers: {string.Join(", ", nearby)}");
    }

    private void PrintHit(string label, Vector3 from, Vector3 to)
    {
        var hit = GetTree().Root.World3D.DirectSpaceState.IntersectRay(PhysicsRayQueryParameters3D.Create(from, to, 1));
        var collider = hit.Count > 0 ? hit["collider"].AsGodotObject() as Node : null;
        GD.Print($"[WallProbe] {label} hit: {collider?.GetPath().ToString() ?? "(none)"}");
        if (label != "Forward" || collider == null) return;
        var wall = collider.GetParent();
        if (wall?.Name == "StaticBody3D") wall = wall.GetParent();
        if (wall?.Name.ToString().StartsWith("Wall_") == true &&
            wall.GetNodeOrNull<CollisionShape3D>("StaticBody3D/CollisionShape3D") == null)
            GD.PrintErr($"[WallProbe] WARNING: visual wall without collider: {wall.GetPath()}");
    }

    private static IEnumerable<Node> Enumerate(Node root)
    {
        yield return root;
        foreach (var child in root.GetChildren()) foreach (var node in Enumerate(child)) yield return node;
    }
}
