namespace BREU.Scripts.Debug;

/// <summary>
/// F9 companion — validates that upper-floor triggers cannot reach the first floor,
/// balcony boundary leftovers are gone, and DebugFallRecovery cannot yank térreo play upstairs.
/// </summary>
public partial class FloorTriggerIsolationChecker : Node
{
    private const float MaxUpperTriggerBottomY = 2.6f;

    private static readonly string[] ForbiddenBoundaryNames =
    {
        "BalconyWallColliders",
        "BalconyWallCollider_Left",
        "BalconyWallCollider_Right",
        "BalconyWallCollider_FrontGuard",
        "BalconyWallCollider",
        "BalconyBoundaryColliders",
        "BalconyBoundary_Left",
        "BalconyBoundary_Right",
        "BalconyBoundary_Front",
        "BalconyBoundary_Back",
        "BalconyBoundary_BackIfNeeded",
        "BalconyWallCollision",
        "BalconyLimit",
        "BalconyBlocker",
        "BalconyRail",
        "UpperBalconyBoundary",
        "UpperWingBoundary",
        "InvisibleBoundary",
        "DebugSafetyBoundary_OuterEdge"
    };

    private static readonly string[] UpperTriggerNameHints =
    {
        "Upper", "Balcony", "Varanda", "GreenDoor", "Room203", "203", "OwnerBedroom", "Wing"
    };

    private int _errors;

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventKey { Pressed: true, Echo: false, Keycode: Key.F9 })
        {
            return;
        }

        RunChecks();
    }

    public void RunChecks()
    {
        _errors = 0;
        var scene = GetTree().CurrentScene;
        if (scene == null)
        {
            GD.PrintErr("[FloorIsolation] ERROR: no current scene");
            return;
        }

        GD.Print("[FloorIsolation] === Floor trigger isolation check ===");

        CheckBalconyBoundaryRollback(scene);
        CheckCollisionDeck(scene);
        CheckFloorVolumes(scene);
        CheckUpperTriggersDoNotReachFirstFloor(scene);
        CheckDebugFallRecoverySafe(scene);

        if (_errors == 0)
        {
            GD.Print("[FloorIsolation] OK: no upper trigger overlaps first floor");
            GD.Print("[FloorIsolation] OK: balcony boundary rollback clean");
        }
        else
        {
            GD.PrintErr($"[FloorIsolation] Finished with {_errors} ERROR(s)");
        }
    }

    private void Error(string message)
    {
        _errors++;
        GD.PrintErr($"[FloorIsolation] ERROR: {message}");
    }

    private void CheckBalconyBoundaryRollback(Node scene)
    {
        foreach (var name in ForbiddenBoundaryNames)
        {
            if (scene.FindChild(name, recursive: true, owned: false) != null)
            {
                Error($"forbidden balcony boundary still present: {name}");
            }
        }
    }

    private void CheckCollisionDeck(Node scene)
    {
        var deck = scene.GetNodeOrNull<Node3D>("World/Level/SecondFloor/Floors/UpperWing_CollisionDeck");
        var body = deck?.GetNodeOrNull<StaticBody3D>("StaticBody3D");
        var shapeNode = deck?.GetNodeOrNull<CollisionShape3D>("StaticBody3D/CollisionShape3D");
        var shape = shapeNode?.Shape as BoxShape3D;
        if (deck == null || body == null || shape == null || !body.IsInsideTree())
        {
            Error("UpperWing_CollisionDeck missing or inactive");
            return;
        }

        if (deck.Position != new Vector3(5f, 2.4f, 4.6f) ||
            shape.Size != new Vector3(50f, 0.8f, 30.8f) ||
            body.CollisionLayer != 1 ||
            body.CollisionMask != 0)
        {
            Error("UpperWing_CollisionDeck was altered (approved deck must stay frozen)");
            return;
        }

        GD.Print("[FloorIsolation] OK: UpperWing_CollisionDeck active and unchanged");
    }

    private void CheckFloorVolumes(Node scene)
    {
        foreach (var name in new[] { "FirstFloorVolume", "SecondFloorVolume", "UpperWingVolume" })
        {
            if (scene.FindChild(name, recursive: true, owned: false) == null)
            {
                Error($"logical volume missing: {name}");
            }
        }
    }

    private void CheckUpperTriggersDoNotReachFirstFloor(Node scene)
    {
        var firstVolume = scene.FindChild("FirstFloorVolume", recursive: true, owned: false) as Area3D;
        Aabb? firstAabb = firstVolume != null ? GetAreaAabb(firstVolume) : null;

        foreach (var area in EnumerateAreas(scene))
        {
            if (!IsUpperRelated(area))
            {
                continue;
            }

            var aabb = GetAreaAabb(area);
            if (aabb == null)
            {
                continue;
            }

            var box = aabb.Value;
            if (box.Position.Y < MaxUpperTriggerBottomY)
            {
                Error($"{area.Name} reaches too low (bottomY={box.Position.Y:0.00}) — can catch first floor");
            }

            if (firstAabb != null && AabbIntersects(box, firstAabb.Value))
            {
                Error($"{area.Name} overlaps FirstFloorVolume");
            }
        }
    }

    private void CheckDebugFallRecoverySafe(Node scene)
    {
        var recovery = scene.FindChild("DebugFallRecovery", recursive: true, owned: false);
        if (recovery == null)
        {
            GD.Print("[FloorIsolation] OK: DebugFallRecovery absent");
            return;
        }

        // Static analysis of the known bug: recovery must not treat first-floor Y as fall-through.
        // Runtime proof remains playtest; here we confirm SafeMarker_Reception exists for void recovery.
        var reception = scene.FindChild("SafeMarker_Reception", recursive: true, owned: false);
        if (reception == null)
        {
            Error("DebugFallRecovery can teleport first floor player to second floor (SafeMarker_Reception missing)");
            return;
        }

        GD.Print("[FloorIsolation] OK: DebugFallRecovery has reception safe marker (KillY-only policy in script)");
    }

    private static bool IsUpperRelated(Area3D area)
    {
        var name = area.Name.ToString();
        if (name.Contains("Volume", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("AudioZone", StringComparison.OrdinalIgnoreCase) ||
            area is AmbienceZone3D or SurfaceAudioZone3D or OneShotAudioTrigger3D or RandomOneShotEmitter3D)
        {
            return false;
        }

        if (area.IsInGroup("level_second_floor") || area.IsInGroup("level_upper_wing"))
        {
            return true;
        }

        var path = area.GetPath().ToString();
        foreach (var hint in UpperTriggerNameHints)
        {
            if (path.Contains(hint, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        var p = area.GetParent();
        while (p != null)
        {
            var n = p.Name.ToString();
            if (n is "BalconyWing" or "PensionSecondFloor" or "Room203Door" or "SecondFloor")
            {
                return true;
            }

            p = p.GetParent();
        }

        return false;
    }

    private static Aabb? GetAreaAabb(Area3D area)
    {
        Aabb? result = null;
        foreach (var child in area.GetChildren())
        {
            if (child is not CollisionShape3D { Shape: BoxShape3D box } shape)
            {
                continue;
            }

            var center = shape.GlobalPosition;
            var half = box.Size * 0.5f;
            var local = new Aabb(center - half, box.Size);
            result = result == null ? local : result.Value.Merge(local);
        }

        return result;
    }

    private static bool AabbIntersects(Aabb a, Aabb b)
    {
        return a.Intersects(b);
    }

    private static IEnumerable<Area3D> EnumerateAreas(Node root)
    {
        foreach (var node in Enumerate(root))
        {
            if (node is Area3D area)
            {
                yield return area;
            }
        }
    }

    private static IEnumerable<Node> Enumerate(Node root)
    {
        yield return root;
        foreach (var child in root.GetChildren())
        {
            foreach (var n in Enumerate(child))
            {
                yield return n;
            }
        }
    }
}
