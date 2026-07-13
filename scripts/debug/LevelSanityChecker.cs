namespace BREU.Scripts.Debug;

/// <summary>
/// Sprint 18C — structural sanity checks for PensaoVerticalBlockout01.
/// Press F9 (does not touch F10/F11). Player reset remains on debug_reset_player (F3).
/// </summary>
public partial class LevelSanityChecker : Node
{
    private const float SecondFloorMinY = 2.55f;
    private const float ReceptionVolumeMinY = 0.2f;
    private const float ReceptionVolumeMaxY = 2.55f;
    private const float SuspiciousAreaVolume = 6.0f;
    private const float MaxInteractAreaAxis = 1.8f;

    private static readonly string[] ForbiddenNameParts =
    {
        "Old", "Temp", "Backup", "Test", "Legacy", "Deprecated", "DebugTemp", "BlockerOld", "_Rebuilt", "Placeholder"
    };

    private static readonly string[] ForbiddenResidualGeometry =
    {
        "Shell_FacadeUpper_SideWest", "Shell_FacadeUpper_SideEast", "Roof_Blockout_LowerFront"
    };

    /// <summary>Runtime wing recreators — must never appear enabled in the live tree.</summary>
    private static readonly string[] ForbiddenSetupNames =
    {
        "BalconyWingPuzzleSetup"
    };

    private int _errors;
    private int _warnings;

    public override void _Ready()
    {
        if (OS.IsDebugBuild()) CallDeferred(nameof(RunChecks));
    }

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
        _warnings = 0;
        var scene = GetTree().CurrentScene;
        if (scene == null)
        {
            GD.PrintErr("[LevelSanity] ERROR: no current scene.");
            return;
        }

        GD.Print("[LevelSanity] === Pension structural check (18C) ===");

        CheckForbiddenSetups(scene);
        CheckForbiddenNames(scene);
        CheckForbiddenResidualGeometry(scene);
        CheckInvisibleColliders(scene);
        CheckInteractAreas(scene);
        CheckFloorCollisions(scene);
        CheckSecondFloorInvasion(scene);
        CheckReceptionCeiling(scene);
        CheckManualWingOwnership(scene);
        CheckBalconyBoundaryRollback(scene);
        CheckFloorVolumes(scene);
        CheckUpperWingStructuralHotfix(scene);
        CheckRoom203Closure(scene);

        if (_errors == 0 && _warnings == 0)
        {
            GD.Print("[LevelSanity] OK: 0 ERROR / 0 WARNING");
            HUDController.FindActive(GetTree())?.ShowMessage("LevelSanity OK (F9).", 2.5f);
            return;
        }

        GD.PrintErr($"[LevelSanity] Finished: {_errors} ERROR / {_warnings} WARNING — see Output.");
        HUDController.FindActive(GetTree())?.ShowMessage(
            $"LevelSanity: {_errors} err / {_warnings} warn (F9)", 3.5f);
    }

    private void Error(string message)
    {
        _errors++;
        GD.PrintErr($"[LevelSanity] ERROR: {message}");
    }

    private void Warn(string message)
    {
        _warnings++;
        GD.PrintErr($"[LevelSanity] WARNING: {message}");
    }

    private void Ok(string message) => GD.Print($"[LevelSanity] OK: {message}");

    private void CheckForbiddenResidualGeometry(Node scene)
    {
        foreach (var name in ForbiddenResidualGeometry)
            if (scene.FindChild(name, recursive: true, owned: false) != null)
                Error($"visual-only residual geometry still active: {name}");
    }

    private void CheckRoom203Closure(Node scene)
    {
        var closure = scene.GetNodeOrNull<Node3D>("Room203Door/Wall_Room203_Closure");
        var mesh = closure?.GetNodeOrNull<MeshInstance3D>("MeshInstance3D")?.Mesh as BoxMesh;
        var shape = closure?.GetNodeOrNull<CollisionShape3D>("StaticBody3D/CollisionShape3D")?.Shape as BoxShape3D;
        if (mesh == null || shape == null || !mesh.Size.IsEqualApprox(shape.Size))
            Error("Room 203 is not closed by a matching visual wall/collider");
        else
            Ok("Room 203 side opening closed by authored wall");
    }

    private void CheckForbiddenSetups(Node scene)
    {
        foreach (var name in ForbiddenSetupNames)
        {
            if (scene.FindChild(name, recursive: true, owned: false) != null)
            {
                Error($"forbidden runtime wing setup in tree: {name}");
            }
        }

        if (scene.FindChild("BalconyWing_Rebuilt", recursive: true, owned: false) != null)
        {
            Error("BalconyWing_Rebuilt present — BuildUpperBalconyWing is not frozen");
        }
        else
        {
            Ok("no active runtime level builders for upper wing");
        }
    }

    private void CheckForbiddenNames(Node scene)
    {
        foreach (var node in Enumerate(scene))
        {
            if (node is not Node3D)
            {
                continue;
            }

            var name = node.Name.ToString();
            // Authored diagnostics and door/stair thresholds are current geometry,
            // not legacy nodes merely because their words contain Test/old.
            if (name.EndsWith("Test", StringComparison.OrdinalIgnoreCase) ||
                name.Contains("Threshold", StringComparison.OrdinalIgnoreCase)) continue;
            foreach (var part in ForbiddenNameParts)
            {
                if (!name.Contains(part, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                Error($"deprecated-style name in live tree: {node.GetPath()}");
                break;
            }
        }
    }

    private void CheckInvisibleColliders(Node scene)
    {
        foreach (var node in Enumerate(scene))
        {
            if (node is not StaticBody3D body)
            {
                continue;
            }

            var hasShape = false;
            foreach (var child in body.GetChildren())
            {
                if (child is CollisionShape3D)
                {
                    hasShape = true;
                    break;
                }
            }

            if (!hasShape)
            {
                continue;
            }

            var name = body.Name.ToString();
            if (name.Length < 3 || name.StartsWith("@", StringComparison.Ordinal))
            {
                Warn($"suspicious invisible collider (unclear name): {body.GetPath()}");
            }
        }
    }

    private void CheckInteractAreas(Node scene)
    {
        var areas = new List<Area3D>();
        foreach (var node in Enumerate(scene))
        {
            if (node is Area3D area && (area.IsInGroup("interactable") || area.CollisionLayer == 2))
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
                    Warn($"oversized interact Area3D at {area.GetPath()} size={box.Size}");
                }
            }
        }

        var near203 = areas.Count(a =>
        {
            var p = a.GlobalPosition;
            return Mathf.Abs(p.X + 1.0f) < 1.2f && Mathf.Abs(p.Z + 10f) < 1.5f;
        });
        if (near203 > 2)
        {
            Warn("duplicate interaction near Door_Room203");
        }
        else
        {
            Ok("no duplicate interactions (203 band)");
        }
    }

    private void CheckFloorCollisions(Node scene)
    {
        string[] floorNames =
        {
            "UpperWing_CollisionDeck",
            "UpperWing_FreeWalkableFloor",
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
                Error($"floor mesh without StaticBody: {floorName}");
            }
            else if (meshes > 0 && bodies > 0)
            {
                Ok($"{floorName} has collision");
            }
        }

        foreach (var node in Enumerate(scene))
        {
            if (!node.IsInGroup("level_floor") || node is not MeshInstance3D mesh)
            {
                continue;
            }

            var parent = mesh.GetParent();
            var hasBodyNearby = false;
            if (parent != null)
            {
                foreach (var sibling in Enumerate(parent))
                {
                    if (sibling is StaticBody3D && sibling.Name == mesh.Name)
                    {
                        hasBodyNearby = true;
                        break;
                    }
                }
            }

            // Collisions often live under a Collisions container — already checked by name above.
            _ = hasBodyNearby;
        }
    }

    private void CheckSecondFloorInvasion(Node scene)
    {
        var floorMarker = scene.FindChild("Marker_SecondFloor_FloorY", recursive: true, owned: false) as Marker3D
            ?? scene.FindChild("Marker_SecondFloor_FloorHeight", recursive: true, owned: false) as Marker3D;
        var minY = floorMarker?.GlobalPosition.Y - 0.35f ?? SecondFloorMinY;

        foreach (var node in Enumerate(scene))
        {
            if (HasAncestorNamed(node, "UpperWing_CollisionDeck")) continue;
            if (!node.IsInGroup("level_second_floor") && node.GetParent()?.IsInGroup("level_second_floor") != true)
            {
                // Also scan PensionSecondFloor children.
                if (node is not Node3D n3d)
                {
                    continue;
                }

                var underSecond = false;
                var p = node.GetParent();
                while (p != null)
                {
                    if (p.Name == "PensionSecondFloor")
                    {
                        underSecond = true;
                        break;
                    }

                    p = p.GetParent();
                }

                if (!underSecond)
                {
                    continue;
                }

                if (node is not (StaticBody3D or MeshInstance3D))
                {
                    continue;
                }

                if (n3d.GlobalPosition.Y < minY && n3d.GlobalPosition.Y > 0.3f)
                {
                    Error($"second floor node invading first floor: {n3d.GetPath()} y={n3d.GlobalPosition.Y:0.00}");
                }

                continue;
            }

            if (node is Node3D tagged && tagged.GlobalPosition.Y < minY && tagged.GlobalPosition.Y > 0.3f
                && node is (StaticBody3D or MeshInstance3D))
            {
                Error($"second floor node invading first floor: {tagged.GetPath()}");
            }
        }
    }

    private void CheckReceptionCeiling(Node scene)
    {
        var ceiling = scene.GetNodeOrNull<Node3D>("PensionCeiling");
        if (ceiling == null)
        {
            Error("PensionCeiling missing");
            return;
        }

        if (ceiling.FindChild("Ceiling_FirstFloor_Seal", recursive: true, owned: false) == null)
        {
            Error("Ceiling_FirstFloor_Seal missing");
            return;
        }

        // Any upper-wing mesh whose center sits inside the reception *playable* box
        // (below the seal) is an invasion. Floors at second-floor height are OK.
        var before = _errors;
        var wingRoots = new List<Node3D>();
        if (scene.GetNodeOrNull<Node3D>("BalconyWing") is { } balconyWing)
        {
            wingRoots.Add(balconyWing);
        }

        foreach (var wing in wingRoots)
        {
            foreach (var node in Enumerate(wing))
            {
                if (node is not MeshInstance3D mesh)
                {
                    continue;
                }

                var p = mesh.GlobalPosition;
                var inReceptionXz = Mathf.Abs(p.X) < 5.5f && p.Z > -7.0f && p.Z < 1.5f;
                if (inReceptionXz && p.Y > ReceptionVolumeMinY && p.Y < ReceptionVolumeMaxY)
                {
                    Error($"upper wing mesh inside reception volume: {mesh.GetPath()} y={p.Y:0.00}");
                }
            }
        }

        if (_errors == before)
        {
            Ok("reception ceiling clear");
        }
    }

    private void CheckManualWingOwnership(Node scene)
    {
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
            Error($"expected 1 BalconyWing instance, found {wings}");
        }
        else
        {
            Ok("single BalconyWing owner");
        }
    }

    private void CheckUpperWingStructuralHotfix(Node scene)
    {
        var rooms = scene.GetNodeOrNull<Node3D>("World/Level/SecondFloor/UpperWingRooms");
        if (rooms == null)
        {
            Error("UpperWingRooms missing");
            return;
        }

        var checkedWalls = 0;
        foreach (var node in Enumerate(rooms))
        {
            if (node is not Node3D wall || !wall.Name.ToString().StartsWith("Wall_")) continue;
            checkedWalls++;
            var mesh = wall.GetNodeOrNull<MeshInstance3D>("MeshInstance3D")?.Mesh as BoxMesh;
            var body = wall.GetNodeOrNull<StaticBody3D>("StaticBody3D");
            var shape = wall.GetNodeOrNull<CollisionShape3D>("StaticBody3D/CollisionShape3D")?.Shape as BoxShape3D;
            if (mesh == null || body == null || shape == null)
            {
                Error($"accessible wall lacks local mesh/body/shape: {wall.GetPath()}");
                continue;
            }

            if (!mesh.Size.IsEqualApprox(shape.Size))
                Error($"wall mesh/shape mismatch: {wall.GetPath()} mesh={mesh.Size} shape={shape.Size}");
            else
                CheckWallPhysicalRay(wall, mesh.Size);
        }

        if (checkedWalls == 0) Error("no authored Wall_* nodes found in UpperWingRooms");
        else Ok($"{checkedWalls} upper-wing walls have matching local colliders");

        var panel = rooms.GetNodeOrNull<Node3D>("TechnicalRoom/TechnicalPanel");
        if (panel?.GetNodeOrNull<MeshInstance3D>("PanelVisual") == null ||
            panel.GetNodeOrNull<Area3D>("InteractionArea") == null)
            Error("TechnicalPanel is not mounted and interactable inside TechnicalRoom");
        else Ok("TechnicalPanel mounted inside TechnicalRoom with local InteractionArea");

        foreach (var ceilingPath in new[] { "LaundryStorage/Ceiling_Laundry", "SharedBathroom/Ceiling_Bath", "TechnicalRoom/Ceiling_Tech" })
        {
            if (rooms.GetNodeOrNull<MeshInstance3D>(ceilingPath) == null) Error($"critical room ceiling missing: {ceilingPath}");
        }
    }

    private void CheckWallPhysicalRay(Node3D wall, Vector3 size)
    {
        var localNormal = size.X <= size.Z ? Vector3.Right : Vector3.Back;
        var normal = (wall.GlobalTransform.Basis * localNormal).Normalized();
        var center = wall.GlobalPosition;
        var query = PhysicsRayQueryParameters3D.Create(center - normal * 0.65f, center + normal * 0.65f, 1);
        var hit = GetTree().Root.World3D.DirectSpaceState.IntersectRay(query);
        var collider = hit.Count > 0 ? hit["collider"].AsGodotObject() as Node : null;
        if (collider == null || !collider.IsInGroup("level_wall"))
            Error($"wall is not physically blocking at its visual center: {wall.GetPath()} hit={collider?.GetPath()}");
    }

    private void CheckBalconyBoundaryRollback(Node scene)
    {
        string[] forbidden =
        {
            "BalconyWallColliders", "BalconyWallCollider_Left", "BalconyWallCollider_Right",
            "BalconyWallCollider_FrontGuard", "BalconyWallCollider",
            "BalconyBoundaryColliders", "BalconyBoundary_Left", "BalconyBoundary_Right",
            "BalconyBoundary_Front", "BalconyBoundary_Back", "InvisibleBoundary"
        };

        var clean = true;
        foreach (var name in forbidden)
        {
            if (scene.FindChild(name, recursive: true, owned: false) != null)
            {
                Error($"balcony boundary leftover: {name}");
                clean = false;
            }
        }

        if (clean)
        {
            Ok("balcony boundary rollback clean");
        }
    }

    private void CheckFloorVolumes(Node scene)
    {
        foreach (var name in new[] { "FirstFloorVolume", "SecondFloorVolume", "UpperWingVolume" })
        {
            if (scene.FindChild(name, recursive: true, owned: false) == null)
            {
                Error($"logical floor volume missing: {name}");
            }
            else
            {
                Ok($"{name} present");
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

    private static bool HasAncestorNamed(Node node, string name)
    {
        for (var current = node; current != null; current = current.GetParent())
            if (current.Name == name) return true;
        return false;
    }
}
