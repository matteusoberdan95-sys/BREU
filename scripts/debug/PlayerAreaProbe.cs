namespace BREU.Scripts.Debug;

/// <summary>
/// F8 — probe which floors/triggers currently affect the player.
/// Detects upper-floor Area3Ds incorrectly overlapping a first-floor player.
/// </summary>
public partial class PlayerAreaProbe : Node
{
    private const float FirstFloorMaxY = 2.55f;
    private const float SecondFloorMinY = 2.65f;

    private static readonly string[] UpperNameHints =
    {
        "Upper", "Balcony", "Varanda", "GreenDoor", "Room203", "203", "Owner", "Wing"
    };

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventKey { Pressed: true, Echo: false, Keycode: Key.F8, CtrlPressed: false })
        {
            return;
        }

        RunProbe();
    }

    public void RunProbe()
    {
        var scene = GetTree().CurrentScene;
        var player = scene?.FindChild("Player", recursive: true, owned: false) as CharacterBody3D;
        if (scene == null || player == null)
        {
            GD.PrintErr("[PlayerAreaProbe] ERROR: scene or player missing");
            return;
        }

        var pos = player.GlobalPosition;
        var floor = EstimateFloor(pos.Y);
        GD.Print($"[PlayerAreaProbe] Player position: {pos}");
        GD.Print($"[PlayerAreaProbe] Estimated floor: {floor}");

        PrintRay("Down", pos + Vector3.Up * 0.2f, pos + Vector3.Down * 12f);
        PrintRay("Up", pos + Vector3.Up * 0.1f, pos + Vector3.Up * 12f);

        GD.Print("[PlayerAreaProbe] Overlapping areas:");
        var any = false;
        var errorCount = 0;
        foreach (var area in EnumerateAreas(scene))
        {
            if (!PointInsideArea(area, pos + Vector3.Up * 0.9f) &&
                !PointInsideArea(area, pos + Vector3.Up * 0.2f) &&
                !PointInsideArea(area, pos))
            {
                continue;
            }

            any = true;
            var path = area.GetPath().ToString();
            var isUpper = IsUpperRelated(area);
            if (floor == "FirstFloor" && isUpper)
            {
                errorCount++;
                GD.PrintErr($"[PlayerAreaProbe]  - {area.Name} ({path}) ERROR: upper trigger overlaps first floor player");
            }
            else
            {
                GD.Print($"[PlayerAreaProbe]  - {area.Name} ({path})");
            }
        }

        if (!any)
        {
            GD.Print("[PlayerAreaProbe]  (none)");
        }

        if (floor == "FirstFloor" && errorCount == 0)
        {
            GD.Print("[PlayerAreaProbe] OK: no upper Area3D overlaps first-floor player");
        }
    }

    private void PrintRay(string label, Vector3 from, Vector3 to)
    {
        var query = PhysicsRayQueryParameters3D.Create(from, to, 1);
        var hit = GetTree().Root.World3D.DirectSpaceState.IntersectRay(query);
        if (hit.Count == 0)
        {
            GD.Print($"[PlayerAreaProbe] {label} hit: (none)");
            return;
        }

        var collider = hit["collider"].AsGodotObject() as Node;
        var name = collider?.GetParent()?.Name.ToString() ?? collider?.Name.ToString() ?? "?";
        GD.Print($"[PlayerAreaProbe] {label} hit: {name} ({collider?.GetPath()})");
    }

    private static string EstimateFloor(float y)
    {
        if (y >= 3.6f) return "UpperWing";
        if (y >= SecondFloorMinY) return "SecondFloor";
        if (y < FirstFloorMaxY) return "FirstFloor";
        return "Transition";
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
        foreach (var hint in UpperNameHints)
        {
            if (path.Contains(hint, StringComparison.OrdinalIgnoreCase) ||
                name.Contains(hint, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        // Parent chain under BalconyWing / PensionSecondFloor / Room203.
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

    private static bool PointInsideArea(Area3D area, Vector3 point)
    {
        foreach (var child in area.GetChildren())
        {
            if (child is not CollisionShape3D { Shape: BoxShape3D box } shape)
            {
                continue;
            }

            var local = shape.GlobalTransform.AffineInverse() * point;
            var half = box.Size * 0.5f;
            if (Mathf.Abs(local.X) <= half.X &&
                Mathf.Abs(local.Y) <= half.Y &&
                Mathf.Abs(local.Z) <= half.Z)
            {
                return true;
            }
        }

        return false;
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
