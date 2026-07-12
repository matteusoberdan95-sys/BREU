namespace BREU.Scripts.Debug;

/// <summary>F8 diagnostics for the single authoritative upper-wing collision deck.</summary>
public partial class UpperFloorCollisionProbe : Node
{
    private static readonly string[] FloorMarkers =
    {
        "Marker_MasterSlab_Start", "Marker_MasterSlab_Center", "Marker_MasterSlab_Right",
        "Marker_MasterSlab_FarRight", "Marker_MasterSlab_Left", "Marker_MasterSlab_Back",
        "Marker_MasterSlab_Front", "Marker_MasterSlab_Room203Path",
        "Marker_MasterSlab_GreenDoorPath"
    };

    private static readonly string[] CeilingMarkers =
    {
        "Marker_Reception_CeilingJumpTest", "Marker_Entry_CeilingJumpTest"
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
        var deck = FindDeck(scene);
        if (scene == null || player == null || deck == null)
        {
            GD.PrintErr("[CollisionDeckProbe] ERROR: scene, player or UpperWing_CollisionDeck missing");
            return;
        }

        PrintDeckData(deck);
        GD.Print($"[CollisionDeckProbe] Player position: {player.GlobalPosition}");
        PrintRay("Down", player.GlobalPosition + Vector3.Up, player.GlobalPosition + Vector3.Down * 10f, "no floor below player");
        PrintRay("Up", player.GlobalPosition + Vector3.Up * 0.1f, player.GlobalPosition + Vector3.Up * 10f, "no ceiling above reception jump area");

        var camera = player.GetNodeOrNull<Camera3D>("HeadBase/BodyMotionPivot/LeanPivot/LookBackPivot/Camera3D");
        if (camera == null) GD.PrintErr("[Probe] ERROR: Camera3D missing");
        else
        {
            var from = camera.GlobalPosition;
            PrintRay("Forward", from, from - camera.GlobalTransform.Basis.Z.Normalized() * 12f, "no forward collider");
        }

        RunMarkerProbe();
    }

    private void RunMarkerProbe()
    {
        var scene = GetTree().CurrentScene;
        var deck = FindDeck(scene);
        var expected = deck?.GetNodeOrNull<StaticBody3D>("StaticBody3D");
        if (scene == null || deck == null || expected == null)
        {
            GD.PrintErr("[CollisionDeckProbe] ERROR: marker probe could not find UpperWing_CollisionDeck");
            return;
        }

        foreach (var markerName in FloorMarkers)
        {
            var marker = scene.FindChild(markerName, recursive: true, owned: false) as Marker3D;
            ProbeMarker(markerName, marker, Vector3.Down, expected);
        }

        foreach (var markerName in CeilingMarkers)
        {
            var marker = scene.FindChild(markerName, recursive: true, owned: false) as Marker3D;
            ProbeMarker(markerName, marker, Vector3.Up, expected);
        }
    }

    private void ProbeMarker(string name, Marker3D? marker, Vector3 direction, StaticBody3D expected)
    {
        if (marker == null)
        {
            GD.PrintErr($"[Probe] ERROR: marker missing: {name}");
            return;
        }

        var query = PhysicsRayQueryParameters3D.Create(marker.GlobalPosition, marker.GlobalPosition + direction * 12f, 1);
        var hit = GetTree().Root.World3D.DirectSpaceState.IntersectRay(query);
        var collider = hit.Count > 0 ? hit["collider"].AsGodotObject() as Node : null;
        if (collider == expected)
            GD.Print($"[CollisionDeckProbe] OK {name} hit UpperWing_CollisionDeck at {hit["position"].AsVector3()}");
        else
            GD.PrintErr($"[Probe] ERROR {name}: hit {collider?.GetPath().ToString() ?? "nothing"}");
    }

    private void PrintRay(string label, Vector3 from, Vector3 to, string error)
    {
        var query = PhysicsRayQueryParameters3D.Create(from, to, 1);
        var hit = GetTree().Root.World3D.DirectSpaceState.IntersectRay(query);
        if (hit.Count == 0)
        {
            GD.PrintErr($"[Probe] ERROR: {error}");
            return;
        }

        var collider = hit["collider"].AsGodotObject() as Node;
        GD.Print($"[CollisionDeckProbe] {label} hit: {collider?.GetParent()?.Name ?? collider?.Name} ({collider?.GetPath()}) at {hit["position"].AsVector3()}");
    }

    private static Node3D? FindDeck(Node? scene) =>
        scene?.GetNodeOrNull<Node3D>("World/Level/SecondFloor/Floors/UpperWing_CollisionDeck");

    private static void PrintDeckData(Node3D deck)
    {
        var body = deck.GetNodeOrNull<StaticBody3D>("StaticBody3D");
        var shape = deck.GetNodeOrNull<CollisionShape3D>("StaticBody3D/CollisionShape3D")?.Shape as BoxShape3D;
        GD.Print($"[CollisionDeckProbe] Deck shape size: {shape?.Size}");
        GD.Print($"[CollisionDeckProbe] Deck layer/mask: {body?.CollisionLayer}/{body?.CollisionMask}");
        if (shape == null) return;
        var half = shape.Size * 0.5f;
        GD.Print($"[CollisionDeckProbe] Deck AABB min/max: {deck.GlobalPosition - half} / {deck.GlobalPosition + half}");
    }
}
