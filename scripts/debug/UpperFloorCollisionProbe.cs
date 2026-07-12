namespace BREU.Scripts.Debug;

/// <summary>F8 diagnostics for the authoritative upper-wing floor and test markers.</summary>
public partial class UpperFloorCollisionProbe : Node
{
    private static readonly string[] MarkerNames =
    {
        "Marker_UpperWing_Start", "Marker_UpperWing_Center",
        "Marker_UpperWing_Right", "Marker_UpperWing_FarRight",
        "Marker_UpperWing_Left", "Marker_UpperWing_End"
    };

    public override void _Ready() => CallDeferred(nameof(RunMarkerProbe));

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventKey { Pressed: true, Echo: false, Keycode: Key.F8, CtrlPressed: false }) return;
        RunProbe();
        GetViewport().SetInputAsHandled();
    }

    public void RunProbe()
    {
        var scene = GetTree().CurrentScene;
        var player = scene?.FindChild("Player", recursive: true, owned: false) as Node3D;
        var floor = scene?.FindChild("UpperWing_SolidFloor", recursive: true, owned: false) as Node3D;
        if (scene == null || player == null || floor == null)
        {
            GD.PrintErr("[UpperFloorProbe] ERROR: scene, player or UpperWing_SolidFloor missing");
            return;
        }

        var mesh = floor.GetNodeOrNull<MeshInstance3D>("MeshInstance3D");
        var body = floor.GetNodeOrNull<StaticBody3D>("StaticBody3D");
        var shapeNode = floor.GetNodeOrNull<CollisionShape3D>("StaticBody3D/CollisionShape3D");
        var boxMesh = mesh?.Mesh as BoxMesh;
        var boxShape = shapeNode?.Shape as BoxShape3D;
        GD.Print($"[UpperFloorProbe] Player global pos: {player.GlobalPosition}");
        GD.Print($"[UpperFloorProbe] Floor path: {floor.GetPath()} parent={floor.GetParent()?.GetPath()}");
        GD.Print($"[UpperFloorProbe] Floor global pos={floor.GlobalPosition} parentScale={floor.GetParent<Node3D>()?.GlobalTransform.Basis.Scale} floorScale={floor.GlobalTransform.Basis.Scale}");
        GD.Print($"[UpperFloorProbe] Floor mesh size: {boxMesh?.Size} shape size: {boxShape?.Size}");
        GD.Print($"[UpperFloorProbe] Floor layer={body?.CollisionLayer} mask={body?.CollisionMask}");
        if (boxMesh != null)
        {
            var half = boxMesh.Size * 0.5f;
            GD.Print($"[UpperFloorProbe] Floor global AABB min={floor.GlobalPosition - half} max={floor.GlobalPosition + half}");
        }

        ProbePlayerRays(player);
        foreach (var markerName in MarkerNames)
        {
            var marker = scene.FindChild(markerName, recursive: true, owned: false) as Marker3D;
            if (marker == null) GD.PrintErr($"[UpperFloorProbe] ERROR: marker missing: {markerName}");
            else ProbePoint(markerName.Replace("Marker_UpperWing_", ""), marker.GlobalPosition, body);
        }
    }

    private void ProbePlayerRays(Node3D player)
    {
        GD.Print($"[Probe] Player position: {player.GlobalPosition}");
        var camera = player.GetNodeOrNull<Camera3D>("HeadBase/BodyMotionPivot/LeanPivot/LookBackPivot/Camera3D");
        if (camera != null)
        {
            var from = camera.GlobalPosition;
            var forward = -camera.GlobalTransform.Basis.Z.Normalized();
            PrintGenericHit("Forward", from, from + forward * 12f);
        }
        else GD.PrintErr("[Probe] ERROR: Camera3D missing");

        PrintGenericHit("Down", player.GlobalPosition + Vector3.Up, player.GlobalPosition + Vector3.Down * 8f);
    }

    private void PrintGenericHit(string label, Vector3 from, Vector3 to)
    {
        var query = PhysicsRayQueryParameters3D.Create(from, to, 1);
        var hit = GetTree().Root.World3D.DirectSpaceState.IntersectRay(query);
        if (hit.Count == 0)
        {
            GD.PrintErr($"[Probe] ERROR: no {(label == "Down" ? "floor below player" : "forward collider")}");
            return;
        }
        var collider = hit["collider"].AsGodotObject() as Node;
        GD.Print($"[Probe] {label} hit: {collider?.GetPath()} at {hit["position"].AsVector3()}");
    }

    private void RunMarkerProbe()
    {
        var scene = GetTree().CurrentScene;
        var floor = scene?.FindChild("UpperWing_SolidFloor", recursive: true, owned: false) as Node3D;
        var body = floor?.GetNodeOrNull<StaticBody3D>("StaticBody3D");
        if (scene == null || floor == null || body == null)
        {
            GD.PrintErr("[UpperFloorProbe] ERROR: static marker probe could not find official floor");
            return;
        }

        foreach (var markerName in MarkerNames)
        {
            var marker = scene.FindChild(markerName, recursive: true, owned: false) as Marker3D;
            if (marker == null) GD.PrintErr($"[UpperFloorProbe] ERROR: marker missing: {markerName}");
            else ProbePoint(markerName.Replace("Marker_UpperWing_", ""), marker.GlobalPosition, body);
        }
    }

    private void ProbePoint(string label, Vector3 position, StaticBody3D? expected)
    {
        var query = PhysicsRayQueryParameters3D.Create(position + Vector3.Up * 1.0f, position + Vector3.Down * 6.0f, 1);
        var hit = GetTree().Root.World3D.DirectSpaceState.IntersectRay(query);
        if (hit.Count == 0)
        {
            GD.PrintErr($"[UpperFloorProbe] ERROR {label}: no floor collision under {position}");
            return;
        }

        var collider = hit["collider"].AsGodotObject() as Node;
        if (collider == expected)
            GD.Print($"[UpperFloorProbe] OK {label} hit UpperWing_SolidFloor at {hit["position"].AsVector3()}");
        else
            GD.PrintErr($"[UpperFloorProbe] ERROR {label}: hit {collider?.GetPath()} instead of UpperWing_SolidFloor");
    }
}
