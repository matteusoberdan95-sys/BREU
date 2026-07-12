namespace BREU.Scripts.Debug;

/// <summary>
/// Sprint 18B — structural sanity checks for PensaoVerticalBlockout01.
/// Press F4 in-game (does not touch F9 player reset / F10 / F11).
/// </summary>
public partial class LevelSanityChecker : Node
{
    private const float FirstFloorPlayableMaxY = 2.45f;
    private const float SuspiciousAreaVolume = 8.0f;
    private const float MaxInteractAreaAxis = 2.2f;

    private static readonly string[] ForbiddenNameParts =
    {
        "Old", "Backup", "Temp", "Test", "DebugTemp", "BlockerOld", "_Rebuilt", "Placeholder"
    };

    private static readonly string[] ForbiddenActiveSetupNames =
    {
        "BalconyWingPuzzleSetup"
    };

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventKey { Pressed: true, Echo: false, Keycode: Key.F4 })
        {
            return;
        }

        RunChecks();
        GetViewport().SetInputAsHandled();
    }

    public void RunChecks()
    {
        var warnings = 0;
        var scene = GetTree().CurrentScene;
        if (scene == null)
        {
            GD.PrintErr("[LevelSanity] No current scene.");
            return;
        }

        GD.Print("[LevelSanity] === Pension structural check ===");

        warnings += CheckForbiddenSetups(scene);
        warnings += CheckForbiddenNames(scene);
        warnings += CheckSecondFloorInvasion(scene);
        warnings += CheckInteractAreas(scene);
        warnings += CheckFloorCollisions(scene);
        warnings += CheckReceptionCeiling(scene);
        warnings += CheckManualWingOwnership(scene);

        if (warnings == 0)
        {
            GD.Print("[LevelSanity] OK: no warnings.");
            HUDController.FindActive(GetTree())?.ShowMessage("LevelSanity OK (F4).", 2.5f);
        }
        else
        {
            GD.PrintErr($"[LevelSanity] Finished with {warnings} warning(s). See Output.");
            HUDController.FindActive(GetTree())?.ShowMessage(
                $"LevelSanity: {warnings} aviso(s) — ver Output.", 3.5f);
        }
    }

    private static int CheckForbiddenSetups(Node scene)
    {
        var warnings = 0;
        foreach (var name in ForbiddenActiveSetupNames)
        {
            if (scene.FindChild(name, recursive: true, owned: false) is Node node)
            {
                GD.PrintErr($"[LevelSanity] WARNING: forbidden setup present in tree: {name}");
                warnings++;
                _ = node;
            }
        }

        if (scene.FindChild("BalconyWing_Rebuilt", recursive: true, owned: false) != null)
        {
            GD.PrintErr("[LevelSanity] WARNING: runtime BalconyWing_Rebuilt found — builder not frozen?");
            warnings++;
        }
        else
        {
            GD.Print("[LevelSanity] OK: no active balcony wing rebuild");
        }

        return warnings;
    }

    private static int CheckForbiddenNames(Node scene)
    {
        var warnings = 0;
        foreach (var node in Enumerate(scene))
        {
            var name = node.Name.ToString();
            foreach (var part in ForbiddenNameParts)
            {
                if (!name.Contains(part, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Allow documented historical script files living outside the live tree.
                if (node is not Node3D)
                {
                    continue;
                }

                GD.PrintErr($"[LevelSanity] WARNING: deprecated-style name in live tree: {node.GetPath()}");
                warnings++;
                break;
            }
        }

        return warnings;
    }

    private static int CheckSecondFloorInvasion(Node scene)
    {
        var warnings = 0;
        var second = scene.GetNodeOrNull<Node3D>("PensionSecondFloor");
        if (second == null)
        {
            return 0;
        }

        foreach (var node in Enumerate(second))
        {
            if (node is not Node3D n3d)
            {
                continue;
            }

            // Only flag collision/mesh bodies that sit clearly inside GF playable volume.
            if (node is not (StaticBody3D or MeshInstance3D or CsgShape3D))
            {
                continue;
            }

            var y = n3d.GlobalPosition.Y;
            if (y < FirstFloorPlayableMaxY && y > 0.4f)
            {
                GD.PrintErr(
                    $"[LevelSanity] WARNING: second floor node below allowed Y ({y:0.00}): {n3d.Name}");
                warnings++;
            }
        }

        if (warnings == 0)
        {
            GD.Print("[LevelSanity] OK: second floor nodes above GF playable band");
        }

        return warnings;
    }

    private static int CheckInteractAreas(Node scene)
    {
        var warnings = 0;
        var areas = new List<Area3D>();
        foreach (var node in Enumerate(scene))
        {
            if (node is not Area3D area)
            {
                continue;
            }

            if (area.IsInGroup("interactable") || area.CollisionLayer == 2)
            {
                areas.Add(area);
            }
        }

        areas = areas.Distinct().ToList();

        foreach (var area in areas)
        {
            foreach (var child in area.GetChildren())
            {
                if (child is not CollisionShape3D { Shape: BoxShape3D box })
                {
                    continue;
                }

                var vol = box.Size.X * box.Size.Y * box.Size.Z;
                if (vol > SuspiciousAreaVolume ||
                    box.Size.X > MaxInteractAreaAxis ||
                    box.Size.Z > MaxInteractAreaAxis)
                {
                    GD.PrintErr(
                        $"[LevelSanity] WARNING: oversized interact Area3D at {area.GetPath()} size={box.Size}");
                    warnings++;
                }
            }
        }

        // Duplicate door interactors near 203
        var near203 = areas.Where(a =>
        {
            var p = a.GlobalPosition;
            return Mathf.Abs(p.X + 1.0f) < 1.2f && Mathf.Abs(p.Z + 10f) < 1.5f;
        }).ToList();
        if (near203.Count > 2)
        {
            GD.PrintErr("[LevelSanity] WARNING: duplicate interaction near Door_Room203");
            warnings++;
        }

        return warnings;
    }

    private static int CheckFloorCollisions(Node scene)
    {
        var warnings = 0;
        string[] floorNames =
        {
            "UpperWing_SolidFloor",
            "UpperWing_FreeWalkableFloor",
            "UpperWing_ConnectorFloor",
            "UpperBalcony_FrontWalkway"
        };

        foreach (var floorName in floorNames)
        {
            var meshes = 0;
            var bodies = 0;
            foreach (var node in Enumerate(scene))
            {
                if (node.Name != floorName)
                {
                    continue;
                }

                if (node is MeshInstance3D) meshes++;
                if (node is StaticBody3D) bodies++;
            }

            if (meshes > 0 && bodies == 0)
            {
                GD.PrintErr($"[LevelSanity] WARNING: floor mesh without StaticBody: {floorName}");
                warnings++;
            }
            else if (meshes > 0 && bodies > 0)
            {
                GD.Print($"[LevelSanity] OK: {floorName} has collision");
            }
        }

        return warnings;
    }

    private static int CheckReceptionCeiling(Node scene)
    {
        var ceiling = scene.GetNodeOrNull<Node3D>("PensionCeiling");
        if (ceiling == null)
        {
            GD.PrintErr("[LevelSanity] WARNING: PensionCeiling missing");
            return 1;
        }

        var seal = ceiling.FindChild("Ceiling_FirstFloor_Seal", recursive: true, owned: false);
        var soffit = ceiling.FindChild("Ceiling_Reception_Soffit", recursive: true, owned: false);
        if (seal == null)
        {
            GD.PrintErr("[LevelSanity] WARNING: Ceiling_FirstFloor_Seal missing");
            return 1;
        }

        if (soffit == null)
        {
            GD.PrintErr("[LevelSanity] WARNING: Ceiling_Reception_Soffit missing");
            return 1;
        }

        GD.Print("[LevelSanity] OK: reception ceiling clear");
        return 0;
    }

    private static int CheckManualWingOwnership(Node scene)
    {
        var warnings = 0;
        var wings = 0;
        foreach (var node in Enumerate(scene))
        {
            if (node.Name == "BalconyWing")
            {
                wings++;
            }
        }

        if (wings != 1)
        {
            GD.PrintErr($"[LevelSanity] WARNING: expected 1 BalconyWing instance, found {wings}");
            warnings++;
        }
        else
        {
            GD.Print("[LevelSanity] OK: single BalconyWing owner");
        }

        return warnings;
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
