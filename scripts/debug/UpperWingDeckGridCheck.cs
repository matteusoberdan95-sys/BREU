namespace BREU.Scripts.Debug;

/// <summary>F9 7x7 coverage audit for the authoritative upper-wing collision deck.</summary>
public partial class UpperWingDeckGridCheck : Node
{
    private const int GridSize = 7;
    private static readonly Vector2 DeckMin = new(-20f, -10.8f);
    private static readonly Vector2 DeckMax = new(30f, 20f);

    public override void _Ready() => CallDeferred(nameof(RunGridCheck));

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventKey { Pressed: true, Echo: false, Keycode: Key.F9 }) return;
        RunGridCheck();
    }

    public void RunGridCheck()
    {
        var scene = GetTree().CurrentScene;
        var deck = scene?.GetNodeOrNull<Node3D>("World/Level/SecondFloor/Floors/UpperWing_CollisionDeck");
        var expected = deck?.GetNodeOrNull<StaticBody3D>("StaticBody3D");
        if (expected == null)
        {
            GD.PrintErr("[DeckGrid] ERROR UpperWing_CollisionDeck missing");
            return;
        }

        var failures = 0;
        var excluded = new Godot.Collections.Array<Rid>();
        CollectOtherBodies(scene!, expected, excluded);
        for (var zIndex = 0; zIndex < GridSize; zIndex++)
        for (var xIndex = 0; xIndex < GridSize; xIndex++)
        {
            var x = Mathf.Lerp(DeckMin.X + 0.4f, DeckMax.X - 0.4f, xIndex / (GridSize - 1f));
            var z = Mathf.Lerp(DeckMin.Y + 0.4f, DeckMax.Y - 0.4f, zIndex / (GridSize - 1f));
            var from = new Vector3(x, 3.05f, z);
            var query = PhysicsRayQueryParameters3D.Create(from, new Vector3(x, 1.8f, z), 1);
            query.Exclude = excluded;
            var hit = GetTree().Root.World3D.DirectSpaceState.IntersectRay(query);
            var collider = hit.Count > 0 ? hit["collider"].AsGodotObject() as Node : null;
            if (collider == expected)
                GD.Print($"[DeckGrid] OK point {x:0.00},{z:0.00} hit UpperWing_CollisionDeck");
            else
            {
                failures++;
                if (collider == null) GD.PrintErr($"[DeckGrid] ERROR hole at {x:0.00},{z:0.00} — no collision below");
                else GD.PrintErr($"[DeckGrid] ERROR hit wrong collider at {x:0.00},{z:0.00}: {collider.GetPath()}");
            }
        }

        GD.Print(failures == 0
            ? "[DeckGrid] PASS 49/49 points hit UpperWing_CollisionDeck"
            : $"[DeckGrid] FAIL {failures}/49 points did not hit UpperWing_CollisionDeck");
    }

    private static void CollectOtherBodies(Node node, StaticBody3D expected, Godot.Collections.Array<Rid> excluded)
    {
        if (node is CollisionObject3D body && body != expected) excluded.Add(body.GetRid());
        foreach (var child in node.GetChildren()) CollectOtherBodies(child, expected, excluded);
    }
}
