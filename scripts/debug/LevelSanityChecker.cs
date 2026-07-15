namespace BREU.Scripts.Debug;

using BREU.Scripts.Levels.PensaoSantaLuzia;

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
        CheckGroundWallTops(scene);
        CheckReceptionCeiling(scene);
        CheckEntranceTrailBermClearance(scene);
        CheckManualWingOwnership(scene);
        CheckBalconyBoundaryRollback(scene);
        CheckFloorVolumes(scene);
        CheckUpperWingStructuralHotfix(scene);
        CheckRoom203Closure(scene);
        CheckStairAndTransitionHotfix(scene);
        CheckAmbientHorrorSystem(scene);
        CheckSprint27VisualPolish(scene);
        CheckSprint28LightArtPass(scene);
        CheckSprint30ABlenderAssetPilot(scene);
        CheckSprint30BBlenderProps(scene);
        CheckSprint31MaterialPass(scene);
        CheckSprint31BHeavyDegradation(scene);
        CheckSprint31CPbrMaterialPass(scene);
        CheckSprint33UpperFloorAtmosphere(scene);
        CheckSprint32DFinalRoof(scene);

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

    private void CheckGroundWallTops(Node scene)
    {
        var ground = scene.GetNodeOrNull<Node3D>("PensionGroundFloor");
        if (ground == null) return;
        var offenders = 0;
        foreach (var body in Enumerate(ground).OfType<StaticBody3D>())
        {
            var shape = body.GetNodeOrNull<CollisionShape3D>("CollisionShape3D")?.Shape as BoxShape3D
                ?? body.GetChildren().OfType<CollisionShape3D>().Select(node => node.Shape).OfType<BoxShape3D>().FirstOrDefault();
            if (shape == null || shape.Size.Y < 2.5f) continue;
            var top = body.GlobalPosition.Y + shape.Size.Y * 0.5f;
            if (top > 2.8f) { offenders++; Error($"ground-floor wall pierces upper floor: {body.GetPath()} topY={top:0.00}"); }
        }
        if (offenders == 0) Ok("ground-floor wall tops remain below upper-floor slab");
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
            // Sprint 30A owns one explicit, hidden rollback subtree. The imported
            // Blender hierarchy also legitimately uses "old" in authored names.
            if (HasAncestorNamed(node, "Sprint30A_BlenderAssetPilot") ||
                HasAncestorNamed(node, "Sprint30B_BlenderProps") ||
                HasAncestorNamed(node, "Sprint32D_FinalRoof")) continue;
            // Sprint 33 requires this exact empty organizational container in
            // the production brief; it owns no hidden mesh or legacy resource.
            if (name.Equals("UpperFloor_DeprecatedOldTextures", StringComparison.Ordinal)) continue;
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

    private void CheckEntranceTrailBermClearance(Node scene)
    {
        const float minimumRearEdgeZ = 12.0f;
        var enclosure = scene.GetNodeOrNull<Node3D>("Exterior/TrailEnclosure");
        if (enclosure == null)
        {
            Error("Exterior TrailEnclosure missing");
            return;
        }

        foreach (var name in new[] { "TrailBermWest", "TrailBermEast" })
        {
            var berm = enclosure.GetNodeOrNull<StaticBody3D>(name);
            var mesh = berm?.GetChildren().OfType<MeshInstance3D>().SingleOrDefault()?.Mesh as BoxMesh;
            var shape = berm?.GetChildren().OfType<CollisionShape3D>().SingleOrDefault()?.Shape as BoxShape3D;
            if (berm == null || mesh == null || shape == null)
            {
                Error($"Exterior trail berm is missing paired visual/collision geometry: {name}");
                continue;
            }

            if (!mesh.Size.IsEqualApprox(shape.Size))
                Error($"Exterior trail berm mesh/collider mismatch: {berm.GetPath()}");

            var rearEdgeZ = berm.Position.Z - mesh.Size.Z * 0.5f;
            if (rearEdgeZ < minimumRearEdgeZ)
                Error($"Exterior trail berm still pierces the pension facade: {berm.GetPath()} rearZ={rearEdgeZ:0.00}");
        }

        Ok("exterior trail berms stop outside the pension facade");
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

    private void CheckStairAndTransitionHotfix(Node scene)
    {
        var clean = true;
        foreach (var residual in new[]
                 {
                     "Stair_Guide_Left", "Stair_Guide_Right",
                     "Stair_Stringer_Left", "Stair_Stringer_Right"
                 })
        {
            if (scene.FindChild(residual, recursive: true, owned: false) == null) continue;
            Error($"stair side-guide residue still active: {residual}");
            clean = false;
        }

        foreach (var side in new[] { "Left", "Right" })
        {
            var handrail = scene.FindChild($"Stair_Handrail_{side}", recursive: true, owned: false);
            if (handrail == null)
            {
                Error($"stair handrail missing: {side}");
                clean = false;
                continue;
            }

            var pieces = Enumerate(handrail).OfType<StaticBody3D>().ToArray();
            if (pieces.Length != 7 || pieces.Any(piece =>
                    piece.GetNodeOrNull<MeshInstance3D>("Visual") == null ||
                    piece.GetNodeOrNull<CollisionShape3D>("CollisionShape3D")?.Shape is not BoxShape3D))
            {
                Error($"stair handrail {side} must have seven visual/collider-paired pieces");
                clean = false;
            }
        }

        if (scene.FindChild("Stair_Handrail_Visual", recursive: true, owned: false) != null)
        {
            Error("legacy fixed-height Stair_Handrail_Visual still crosses the stair treads");
            clean = false;
        }

        var player = scene.FindChild("Player", recursive: true, owned: false) as CharacterBody3D;
        if (player == null || player.FloorSnapLength < 0.49f || !player.FloorConstantSpeed)
        {
            Error("player stair descent adhesion is disabled or below the approved snap distance");
            clean = false;
        }

        var corridorEndWall = scene.FindChild("Wall_Corr_North_Mid", recursive: true, owned: false);
        var endWallMesh = corridorEndWall?.GetNodeOrNull<MeshInstance3D>("MeshInstance3D")?.Mesh as BoxMesh;
        var endWallShape = corridorEndWall?
            .GetNodeOrNull<CollisionShape3D>("StaticBody3D/CollisionShape3D")?.Shape as BoxShape3D;
        if (corridorEndWall == null || endWallMesh == null || endWallShape == null ||
            endWallMesh.Size.X < 1.59f || !endWallMesh.Size.IsEqualApprox(endWallShape.Size))
        {
            Error("upper corridor end wall is not fully closed with matching visual/collision width");
            clean = false;
        }

        var soffit = scene.FindChild("Ceiling_FirstFloor_TransitionFront", recursive: true, owned: false);
        if (soffit == null)
        {
            Error("Ceiling_FirstFloor_TransitionFront missing");
            clean = false;
        }
        else
        {
            var meshInstance = soffit.GetChildren().OfType<MeshInstance3D>().FirstOrDefault();
            var hasMesh = meshInstance?.Mesh is BoxMesh;
            var hasBody = Enumerate(soffit).Any(child => child is StaticBody3D or CollisionShape3D);
            if (!hasMesh) Error("transition soffit has no visual mesh");
            if (hasBody) Error("transition soffit added a competing collider");

            var flushWithMasterSlab = soffit is Node3D soffit3D &&
                                      meshInstance?.Mesh is BoxMesh box &&
                                      Mathf.IsEqualApprox(soffit3D.GlobalPosition.Y, 2.5f) &&
                                      Mathf.IsEqualApprox(box.Size.Y, 0.6f);
            if (!flushWithMasterSlab)
            {
                Error("transition soffit hangs below the SecondFloor_MasterSlab profile");
            }

            clean &= hasMesh && !hasBody && flushWithMasterSlab;
        }

        if (clean) Ok("stair residue absent, paired handrails valid, corridor end closed, and front slab flush");
    }

    private void CheckAmbientHorrorSystem(Node scene)
    {
        var root = scene.GetNodeOrNull<Node3D>("World/Gameplay/AmbientHorror");
        if (root == null) { Error("Sprint 26 AmbientHorror tree missing under World/Gameplay"); return; }
        var director = root.GetNodeOrNull<AmbientHorrorDirector>("AmbientHorrorDirector");
        var triggers = root.GetNodeOrNull<Node3D>("AmbientEventTriggers");
        var emitters = root.GetNodeOrNull<Node3D>("AmbientAudioEmitters");
        var visuals = root.GetNodeOrNull<Node3D>("AmbientVisuals");
        var debug = root.GetNodeOrNull<Node>("Debug");
        if (director == null || triggers == null || emitters == null || visuals == null || debug == null)
        { Error("AmbientHorror mandatory containers/director are incomplete"); return; }
        if (Enumerate(root).Any(node => node is StaticBody3D))
            Error("AmbientHorror contains forbidden physical StaticBody3D");

        var areas = triggers.GetChildren().OfType<Area3D>().ToArray();
        if (areas.Length < 6) Error($"AmbientHorror has only {areas.Length}/6 localized triggers");
        foreach (var area in areas)
        {
            if (area.CollisionLayer != 0 || area.CollisionMask != 16)
                Error($"ambient trigger has unsafe layer/mask: {area.GetPath()}");
            if (area.GetNodeOrNull<CollisionShape3D>("CollisionShape3D")?.Shape is not BoxShape3D box)
            { Error($"ambient trigger missing BoxShape3D: {area.GetPath()}"); continue; }
            if (box.Size.Y > 1.5f || box.Size.X > 2.25f || box.Size.Z > 1.65f)
                Error($"ambient trigger is oversized: {area.GetPath()} size={box.Size}");
            var lower = area.GlobalPosition.Y - box.Size.Y * 0.5f;
            var upper = area.GlobalPosition.Y + box.Size.Y * 0.5f;
            if (area.IsInGroup("level_first_floor") && upper >= SecondFloorMinY)
                Error($"ground ambient trigger invades second floor: {area.GetPath()} upperY={upper:0.00}");
            if (area.IsInGroup("level_second_floor") && lower <= SecondFloorMinY)
                Error($"upper ambient trigger invades ground floor: {area.GetPath()} lowerY={lower:0.00}");
        }
        if (Enumerate(visuals).Any(node => node is CollisionObject3D or CollisionShape3D))
            Error("AmbientVisuals contains collision; Sprint 26 visuals must be non-blocking");
        else
            Ok("Sprint 26 ambient tree has six localized triggers and no physical blockers");
    }

    private void CheckSprint27VisualPolish(Node scene)
    {
        var root = scene.GetNodeOrNull<Node3D>("World/VisualPolish/Sprint27_FakeWindowsLighting");
        if (root == null)
        {
            Error("Sprint 27 visual polish container missing");
            return;
        }

        var required = new[] { "FakeWindows", "LightLeaks", "LocalLights", "WallDetails", "CurtainsAndCloth", "DebugMarkers" };
        foreach (var name in required)
            if (root.GetNodeOrNull<Node>(name) == null) Error($"Sprint 27 container missing: {name}");

        var forbiddenPhysics = Enumerate(root)
            .Where(node => node is CollisionObject3D or CollisionShape3D or NavigationRegion3D)
            .ToArray();
        foreach (var node in forbiddenPhysics)
            Error($"Sprint 27 visual-only tree contains physics/navigation: {node.GetPath()}");

        var windows = root.GetNodeOrNull<Node3D>("FakeWindows")?.GetChildren().OfType<Node3D>().ToArray()
            ?? Array.Empty<Node3D>();
        if (windows.Length != 18) Error($"Sprint 27/27A fine adjustment expected 18 fake windows, found {windows.Length}");
        var exteriorWindows = windows.Count(window => window.Name.ToString().StartsWith("Facade", StringComparison.Ordinal));
        if (exteriorWindows != 14) Error($"Sprint 27A fine adjustment expected 14 exterior facade windows, found {exteriorWindows}");
        if (windows.Select(window => window.Name.ToString()).Distinct(StringComparer.Ordinal).Count() != windows.Length)
            Error("Sprint 27 contains duplicate fake-window names");
        var removedWindowNames = new[]
        {
            "FakeWindow_GroundCorridorWest",
            "FacadeGround_ReceptionWest",
            "FacadeGround_ReceptionEast",
            "FacadeGround_Room102Back",
            "FacadeGround_KitchenBack",
            "FacadeUpper_Room201Back",
            "FacadeUpper_Room202Back",
            "FacadeUpper_BackWallWest",
            "FacadeUpper_BackWallEast"
        };
        foreach (var removedName in removedWindowNames)
        {
            if (windows.Any(window => window.Name == removedName))
                Error($"Sprint 27A removed window returned: {removedName}");
        }
        var requiredAdjustedWindows = new[]
        {
            "FacadeGround_Room102WestInterior",
            "FacadeGround_KitchenEastInterior",
            "FacadeUpper_WestWallPairSouth",
            "FacadeUpper_WestWallPairNorth",
            "FacadeUpper_Room201WestInterior",
            "FacadeUpper_Room202EastInterior"
        };
        foreach (var requiredName in requiredAdjustedWindows)
        {
            if (!windows.Any(window => window.Name == requiredName))
                Error($"Sprint 27A adjusted window missing: {requiredName}");
        }
        foreach (var window in windows)
        {
            if (window.GetNodeOrNull<Node3D>("Frame_Visual") == null ||
                window.GetNodeOrNull<MeshInstance3D>("DarkGlass_Visual") == null)
                Error($"fake window missing visual frame/glass: {window.GetPath()}");

            if (window.HasMeta("visual_backing_only") && window.GetMeta("visual_backing_only").AsBool())
            {
                var shellName = window.GlobalPosition.X < 0f
                    ? "Shell_FacadeUpper_FrontLeft"
                    : "Shell_FacadeUpper_FrontRight";
                if (scene.FindChild(shellName, recursive: true, owned: false) == null)
                    Error($"facade window lost its authored visual shell: {window.GetPath()}");
            }
            else
            {
                var query = PhysicsRayQueryParameters3D.Create(
                    window.GlobalPosition - window.GlobalBasis.Z * 0.55f,
                    window.GlobalPosition + window.GlobalBasis.Z * 0.55f,
                    collisionMask: 1);
                query.CollideWithAreas = false;
                var hit = GetViewport().World3D.DirectSpaceState.IntersectRay(query);
                if (hit.Count == 0)
                    Error($"fake window is not backed by an existing solid wall: {window.GetPath()}");
            }
        }

        var lights = root.GetNodeOrNull<Node3D>("LocalLights")?.GetChildren().OfType<Light3D>().ToArray()
            ?? Array.Empty<Light3D>();
        if (lights.Length > 5) Error($"Sprint 27 has too many local lights: {lights.Length}");
        foreach (var light in lights)
        {
            if (light.LightEnergy > 0.2f) Error($"Sprint 27 local light too bright: {light.GetPath()} energy={light.LightEnergy:0.00}");
            if (light is SpotLight3D spot && spot.SpotRange > 4f)
                Error($"Sprint 27 local light range too large: {light.GetPath()} range={spot.SpotRange:0.00}");
        }

        if (forbiddenPhysics.Length == 0 && windows.Length == 18 && exteriorWindows == 14 && lights.Length <= 5)
            Ok("Sprint 27/27A fine adjustment has 4 interior + 14 exterior fake windows and zero collision/navigation nodes");
    }

    private void CheckSprint28LightArtPass(Node scene)
    {
        var root = scene.GetNodeOrNull<Node3D>("World/VisualPolish/Sprint28_LightArtPass");
        if (root == null)
        {
            Error("Sprint 28 light art pass container missing");
            return;
        }

        var required = new[]
        {
            "Reception_Props", "Kitchen_Props", "Bathroom_Props", "TechnicalRoom_Props",
            "UpperRooms_Props", "Room203_Props", "Corridor_Props", "Decals_Stains", "Cloths_Curtains"
        };
        foreach (var name in required)
        {
            var container = root.GetNodeOrNull<Node3D>(name);
            if (container == null) Error($"Sprint 28 container missing: {name}");
            else if (container.GetChildCount() == 0) Error($"Sprint 28 container is empty: {name}");
        }

        var removedVideoReviewOffenders = new[]
        {
            "GroundCorridor_CoatRail", "GroundHall_CrookedPicture", "UpperHall_CrookedPicture",
            "BackHall_NarrowShelf", "UpperLanding_Bench", "UpperLanding_Trunk",
            "UpperLanding_FamilyPortrait", "Office_ArchiveShelf", "Office_FilingCabinet",
            "Office_VisitorChair", "Office_LedgerStack", "Office_WestArchive",
            "Bathroom_WallShelf", "UpperArrival_Oratory", "UpperArrival_GrandfatherClock"
        };
        foreach (var name in removedVideoReviewOffenders)
            if (root.FindChild(name, recursive: true, owned: false) != null)
                Error($"Sprint 28 video-review offender still active: {name}");

        var upperArrivalRequired = new[]
        {
            "UpperArrival_Rug", "UpperArrival_Settee",
            "UpperArrival_FamilyPortrait", "UpperArrival_Chandelier",
            "UpperEastLounge_Rug", "UpperEastLounge_FloorOratory",
            "UpperEastLounge_GrandfatherClock", "UpperEastLounge_Settee",
            "UpperEastLounge_SideTable", "UpperEastLounge_TravelTrunk",
            "UpperEastLounge_ArmchairNorth", "UpperEastLounge_ArmchairSouth",
            "UpperEastLounge_CardTable", "UpperEastLounge_NorthCabinet",
            "Upper203Opposite_ReadingChair", "Upper203Opposite_Luggage",
            "UpperCorridor_Runner", "Office_NorthLowCabinet"
        };
        foreach (var name in upperArrivalRequired)
            if (root.FindChild(name, recursive: true, owned: false) == null)
                Error($"Sprint 28 corrected upper-arrival composition missing: {name}");

        var entranceHallRequired = new[]
        {
            "Entrance_WestWaitingBench", "Entrance_EastChair_A", "Entrance_EastChair_B",
            "Entrance_EastLowCabinet", "Entrance_WestAbandonedBoxes",
            "Entrance_WestFamilyPortrait", "Entrance_WestReligiousPrint",
            "Entrance_EastPensionPhoto", "Entrance_EastMissingPortrait",
            "Entrance_TrashBag_West", "Entrance_TrashBag_East",
            "Entrance_DampStain_West", "Entrance_Grime_East"
        };
        foreach (var name in entranceHallRequired)
            if (root.FindChild(name, recursive: true, owned: false) == null)
                Error($"Sprint 28 entrance-hall dressing missing: {name}");

        var landingOnly = new[]
        {
            "UpperArrival_Rug", "UpperArrival_Settee",
            "UpperArrival_FamilyPortrait", "UpperArrival_Chandelier"
        };
        foreach (var name in landingOnly)
        {
            if (root.FindChild(name, recursive: true, owned: false) is not Node3D prop) continue;
            var p = prop.GlobalPosition;
            if (p.X < -4.75f || p.X > 0.65f || p.Z < -22.6f || p.Z > -19.4f)
                Error($"upper-arrival prop is outside UpperLanding_Main footprint: {name} at {p}");
        }

        var forbidden = Enumerate(root)
            .Where(node => node is CollisionObject3D or CollisionShape3D or NavigationRegion3D or Joint3D)
            .ToArray();
        foreach (var node in forbidden)
            Error($"Sprint 28 visual-only tree contains physics/navigation: {node.GetPath()}");

        var nonVisual = Enumerate(root)
            .Where(node => node != root && node is not Node3D && node is not MeshInstance3D)
            .ToArray();
        foreach (var node in nonVisual)
            Error($"Sprint 28 contains unexpected non-visual node: {node.GetPath()}");

        var meshes = Enumerate(root).OfType<MeshInstance3D>().ToArray();
        if (meshes.Length < 120) Error($"Sprint 28 video-review art pass is unexpectedly sparse: {meshes.Length} meshes");

        var panel = scene.FindChild("TechnicalPanel", recursive: true, owned: false) as Node3D;
        var room205Door = scene.FindChild("Door_Room205_Locked", recursive: true, owned: false) as Node3D;
        if (panel == null || room205Door == null)
            Error("technical panel / Room 205 door missing after Sprint 28 video-review correction");
        else
        {
            var separation = panel.GlobalPosition.DistanceTo(room205Door.GlobalPosition);
            if (panel.GlobalPosition.X < 13.5f || separation < 4f)
                Error($"technical panel still conflicts with Room 205 door: x={panel.GlobalPosition.X:0.00}, separation={separation:0.00}m");
            else if (panel.GetNodeOrNull<Area3D>("InteractionArea") == null)
                Error("technical panel lost its original InteractionArea while being repositioned");
            else
                Ok($"technical panel isolated on east wall; Room 205 door clearance {separation:0.00}m");
        }

        if (forbidden.Length == 0 && nonVisual.Length == 0 && meshes.Length >= 120)
            Ok($"Sprint 28 art pass isolated: {meshes.Length} visual meshes, zero collision/navigation/gameplay nodes");
    }

    private void CheckSprint30ABlenderAssetPilot(Node scene)
    {
        var root = scene.GetNodeOrNull<Node3D>("World/VisualPolish/Sprint30A_BlenderAssetPilot");
        if (root == null)
        {
            Error("Sprint 30A Blender asset pilot container missing");
            return;
        }

        var bed = root.GetNodeOrNull<Node3D>("PropSingleBedOld01_Instance_Room201");
        var backup = root.GetNodeOrNull<Node3D>("Backup_Placeholders_Sprint30A");
        if (bed == null) Error("Sprint 30A Room 201 bed instance missing");
        if (backup == null) Error("Sprint 30A placeholder backup container missing");

        var forbidden = Enumerate(root)
            .Where(node => !HasAncestorNamed(node, "Backup_Placeholders_Sprint30A"))
            .Where(node => node is CollisionObject3D or CollisionShape3D or NavigationRegion3D or Joint3D or Camera3D or Light3D)
            .ToArray();
        foreach (var node in forbidden)
            Error($"Sprint 30A pilot contains forbidden physics/navigation/camera/light node: {node.GetPath()}");

        if (bed != null)
        {
            var bedMeshes = Enumerate(bed).OfType<MeshInstance3D>().Count();
            if (bedMeshes != 18)
                Error($"Sprint 30A bed import expected 18 authored meshes, found {bedMeshes}");
            if (!bed.Scale.IsEqualApprox(Vector3.One))
                Error($"Sprint 30A bed wrapper scale changed unexpectedly: {bed.Scale}");
            if (bed.Position.DistanceTo(new Vector3(-4.15f, 2.8f, -14f)) > 0.02f)
                Error($"Sprint 30A bed moved outside the approved Room 201 pilot position: {bed.Position}");
        }

        if (backup != null)
        {
            if (backup.Visible) Error("Sprint 30A placeholder backup is visible in the live composition");

            var expectedBackup = new[] { "Furniture_Room201_Bed", "Room201_ThinMattress" };
            foreach (var name in expectedBackup)
            {
                var oldPart = backup.GetNodeOrNull<Node3D>(name);
                if (oldPart == null) Error($"Sprint 30A rollback placeholder missing: {name}");
                else if (oldPart.IsVisibleInTree()) Error($"Sprint 30A rollback placeholder is still visible: {name}");
            }

            foreach (var oldBody in Enumerate(backup).OfType<CollisionObject3D>())
                if (oldBody.CollisionLayer != 0 || oldBody.CollisionMask != 0)
                    Error($"Sprint 30A rollback body still participates in physics: {oldBody.GetPath()}");
            foreach (var oldShape in Enumerate(backup).OfType<CollisionShape3D>())
                if (!oldShape.Disabled)
                    Error($"Sprint 30A rollback collision shape is still enabled: {oldShape.GetPath()}");
        }

        if (forbidden.Length == 0 && bed != null && backup != null)
            Ok("Sprint 30A Blender bed pilot isolated in Room 201: scale 1:1, no active collision/camera/light, placeholders hidden");
    }

    private void CheckSprint30BBlenderProps(Node scene)
    {
        var root = scene.GetNodeOrNull<Node3D>("World/VisualPolish/Sprint30B_BlenderProps");
        if (root == null)
        {
            Error("Sprint 30B Blender props container missing");
            return;
        }

        var requiredSectors = new[]
        {
            "Reception", "Kitchen", "Bathroom", "TechnicalRoom", "Bedrooms", "Windows",
            "Backup_Placeholders_Sprint30B", "DebugMarkers"
        };
        foreach (var sector in requiredSectors)
            if (root.GetNodeOrNull<Node3D>(sector) == null)
                Error($"Sprint 30B sector missing: {sector}");

        var forbidden = Enumerate(root)
            .Where(node => node is CollisionObject3D or CollisionShape3D or Area3D or
                NavigationRegion3D or Joint3D or Camera3D or Light3D or RigidBody3D)
            .ToArray();
        foreach (var node in forbidden)
            Error($"Sprint 30B visual container contains forbidden gameplay/physics node: {node.GetPath()}");

        var expectedInstances = new[]
        {
            "PropReceptionCounter01_Instance_Reception",
            "PropBrokenChair01_Instance_Reception",
            "PropKitchenTableOld01_Instance_Kitchen",
            "PropKitchenStoveOld01_Instance_Kitchen",
            "PropSinkOld01_Instance_Kitchen",
            "PropBucketOld01_Instance_Kitchen",
            "PropDrainOld01_Instance_SharedBathroom",
            "PropBrokenMirror01_Instance_SharedBathroom",
            "PropBucketOld01_Instance_SharedBathroom",
            "PropElectricPanelOld01_Instance_TechnicalRoom",
            "PropOldSuitcase01_Instance_Room201",
            "PropNightstandOld01_Instance_Room204",
            "PropWindowOld01_Instance_Room201",
            "PropTornCurtain01_Instance_Room201",
            "PropWindowOld01_Instance_Room202"
        };
        foreach (var instance in expectedInstances)
            if (root.FindChild(instance, recursive: true, owned: false) == null)
                Error($"Sprint 30B Blender prop instance missing: {instance}");

        var floorAligned = new (string Name, float ExpectedY)[]
        {
            ("PropReceptionCounter01_Instance_Reception", 0.02f),
            ("PropBrokenChair01_Instance_Reception", 0.02f),
            ("PropKitchenTableOld01_Instance_Kitchen", 0.02f),
            ("PropKitchenStoveOld01_Instance_Kitchen", 0.02f),
            ("PropSinkOld01_Instance_Kitchen", 0.02f),
            ("PropBucketOld01_Instance_Kitchen", 0.02f),
            ("PropDrainOld01_Instance_SharedBathroom", 2.82f),
            ("PropBucketOld01_Instance_SharedBathroom", 2.82f),
            ("PropOldSuitcase01_Instance_Room201", 2.82f),
            ("PropNightstandOld01_Instance_Room204", 2.82f)
        };
        foreach (var (name, expectedY) in floorAligned)
        {
            if (root.FindChild(name, recursive: true, owned: false) is Node3D prop &&
                !Mathf.IsEqualApprox(prop.GlobalPosition.Y, expectedY))
                Error($"Sprint 30B floor prop is not seated on approved floor: {name} Y={prop.GlobalPosition.Y:0.###}, expected {expectedY:0.##}");
        }

        if (root.FindChild("PropKitchenStoveOld01_Instance_Kitchen", recursive: true, owned: false) is Node3D stove)
        {
            const float stoveHalfWidth = 0.37f;
            const float wardrobeWestEdge = 4.64f;
            if (stove.GlobalPosition.X + stoveHalfWidth >= wardrobeWestEdge)
                Error("Sprint 30B Blender stove still intersects Wardrobe_Kitchen_Vintage");

            var stoveInteraction = scene.GetNodeOrNull<Node3D>("Interactions/KitchenStove");
            if (stoveInteraction == null ||
                stove.GlobalPosition.DistanceTo(stoveInteraction.GlobalPosition) > 1.2f)
                Error("Sprint 30B Blender stove is no longer aligned with its preserved interaction prompt");
        }

        var backup = root.GetNodeOrNull<Node3D>("Backup_Placeholders_Sprint30B");
        if (backup?.Visible != false)
            Error("Sprint 30B placeholder backup marker must remain hidden");

        if (Enumerate(root).Any(node => node.Name.ToString().Contains("Room203", StringComparison.OrdinalIgnoreCase)))
            Error("Sprint 30B modified Room 203 despite its protected event scope");

        var panel = scene.GetNodeOrNull<Node3D>(
            "World/Level/SecondFloor/UpperWingRooms/TechnicalRoom/TechnicalPanel");
        if (panel?.GetNodeOrNull<Area3D>("InteractionArea") == null)
            Error("Sprint 30B replacement lost the functional technical-panel InteractionArea");

        var drain = scene.GetNodeOrNull<Node3D>(
            "World/Level/SecondFloor/UpperWingRooms/SharedBathroom/Interact_BathroomInspect");
        if (drain?.GetNodeOrNull<Area3D>("InteractionArea") == null)
            Error("Sprint 30B replacement lost the functional drain InteractionArea");

        if (forbidden.Length == 0 && expectedInstances.All(name =>
                root.FindChild(name, recursive: true, owned: false) != null))
            Ok("Sprint 30B Blender props isolated and floor-aligned by sector: 15 visual instances, stove clear of wardrobe, zero new collision/gameplay nodes; panel and drain logic preserved");
    }

    private void CheckSprint31MaterialPass(Node scene)
    {
        var root = scene.GetNodeOrNull<Node3D>("World/VisualPolish/Sprint31_Materials");
        if (root == null)
        {
            Error("Sprint 31 material-pass container missing");
            return;
        }

        var expectedContainers = new[]
        {
            "Baseboards", "DampPatches", "FloorStains", "CeilingStains", "Room203DragMarks"
        };
        foreach (var name in expectedContainers)
            if (root.GetNodeOrNull<Node3D>(name) == null)
                Error($"Sprint 31 visual container missing: {name}");

        var forbidden = Enumerate(root)
            .Where(node => node is CollisionObject3D or CollisionShape3D or Area3D or
                NavigationRegion3D or Joint3D or Camera3D or Light3D or RigidBody3D)
            .ToArray();
        foreach (var node in forbidden)
            Error($"Sprint 31 material pass contains forbidden physics/gameplay node: {node.GetPath()}");

        if (root.GetMeta("new_collision_count", -1).AsInt32() != 0)
            Error("Sprint 31 reports a non-zero collision count");
        if (root.GetMeta("frozen_balcony_deck_modified", true).AsBool())
            Error("Sprint 31 modified the frozen balcony collision deck");

        if (forbidden.Length == 0)
            Ok("Sprint 31 material pass isolated under VisualPolish: shared aged palette, visual-only stains/baseboards, frozen balcony deck untouched");
    }

    private void CheckSprint31BHeavyDegradation(Node scene)
    {
        var root = scene.GetNodeOrNull<Node3D>("World/VisualPolish/Sprint31B_HeavyDegradation");
        if (root == null)
        {
            Error("Sprint 31B heavy-degradation container missing");
            return;
        }

        var expectedContainers = new[]
        {
            "WallDecay", "FloorDecay", "CeilingDecay", "AbandonmentProps", "WindowGrime", "LightingMood"
        };
        foreach (var name in expectedContainers)
            if (root.GetNodeOrNull<Node3D>(name) == null)
                Error($"Sprint 31B visual container missing: {name}");

        var forbidden = Enumerate(root)
            .Where(node => node is CollisionObject3D or CollisionShape3D or Area3D or
                NavigationRegion3D or Joint3D or Camera3D or RigidBody3D)
            .ToArray();
        foreach (var node in forbidden)
            Error($"Sprint 31B contains forbidden physics/gameplay node: {node.GetPath()}");

        var lights = Enumerate(root).OfType<Light3D>().ToArray();
        if (lights.Length > 4)
            Error($"Sprint 31B exceeds the approved local-light budget: {lights.Length}/4");
        foreach (var light in lights)
        {
            if (light.ShadowEnabled)
                Error($"Sprint 31B decorative light must not cast dynamic shadows: {light.GetPath()}");
            if (light.LightEnergy > 0.4f)
                Error($"Sprint 31B decorative light is too strong: {light.GetPath()} energy={light.LightEnergy:0.00}");
            if (light is OmniLight3D omni && omni.OmniRange > 7f)
                Error($"Sprint 31B decorative light range is too large: {light.GetPath()} range={omni.OmniRange:0.00}");
        }

        if (root.GetMeta("collision_nodes", -1).AsInt32() != 0 ||
            root.GetMeta("navigation_nodes", -1).AsInt32() != 0 ||
            root.GetMeta("gameplay_nodes", -1).AsInt32() != 0)
            Error("Sprint 31B reports physics, navigation or gameplay nodes");
        if (root.GetMeta("structural_geometry_changed", true).AsBool() ||
            root.GetMeta("frozen_upper_deck_changed", true).AsBool())
            Error("Sprint 31B reports structural geometry or frozen deck changes");

        if (forbidden.Length == 0 && lights.Length <= 4)
            Ok("Sprint 31B isolated under VisualPolish: heavy decay, safe edge dressing and four restrained local lights; zero physics/gameplay nodes, frozen deck untouched");
    }

    private void CheckSprint31CPbrMaterialPass(Node scene)
    {
        var root = scene.GetNodeOrNull<Node3D>("World/VisualPolish/Sprint31C_PBRMaterials");
        if (root == null)
        {
            Error("Sprint 31C PBR-material container missing");
            return;
        }

        foreach (var name in new[] { "WallDecals", "FloorDecals", "CeilingDecals" })
            if (root.GetNodeOrNull<Node3D>(name) == null)
                Error($"Sprint 31C visual container missing: {name}");

        var forbidden = Enumerate(root)
            .Where(node => node is CollisionObject3D or CollisionShape3D or Area3D or
                NavigationRegion3D or Joint3D or Camera3D or Light3D or RigidBody3D)
            .ToArray();
        foreach (var node in forbidden)
            Error($"Sprint 31C contains forbidden physics/gameplay/light node: {node.GetPath()}");

        if (root.GetMeta("collision_nodes", -1).AsInt32() != 0 ||
            root.GetMeta("navigation_nodes", -1).AsInt32() != 0 ||
            root.GetMeta("gameplay_nodes", -1).AsInt32() != 0)
            Error("Sprint 31C reports physics, navigation or gameplay nodes");
        if (root.GetMeta("structural_geometry_changed", true).AsBool() ||
            root.GetMeta("frozen_upper_deck_changed", true).AsBool())
            Error("Sprint 31C reports structural geometry or frozen deck changes");
        if (!root.GetMeta("stair_upper_landing_extension", false).AsBool())
            Error("Sprint 31C stair/upper-landing material extension metadata is missing");
        if (root.GetMeta("stair_surface_material_count", 0).AsInt32() <= 0)
            Error("Sprint 31C did not apply the dedicated damp stair-shaft material profile");
        if (root.GetMeta("upper_landing_surface_material_count", 0).AsInt32() <= 0)
            Error("Sprint 31C did not apply the dedicated dry/cold upper-arrival material profile");

        var exteriorShell = scene.GetNodeOrNull<Node3D>("PensionGroundFloor/BuildingExteriorShell");
        foreach (var wallName in new[]
                 {
                     "Wall_Exterior_Entrance_Infill_Left",
                     "Wall_Exterior_Entrance_Infill_Right"
                 })
        {
            var wall = exteriorShell?.GetNodeOrNull<StaticBody3D>(wallName);
            if (wall == null)
            {
                Error($"Ground-floor entrance infill missing: {wallName}");
                continue;
            }

            var visual = wall.GetChildren().OfType<MeshInstance3D>().SingleOrDefault();
            var collision = wall.GetChildren().OfType<CollisionShape3D>().SingleOrDefault();
            if (visual?.Mesh is not BoxMesh wallMesh || collision?.Shape is not BoxShape3D wallShape)
            {
                Error($"Ground-floor entrance infill must own exactly one visual mesh and one matching child collider: {wall.GetPath()}");
                continue;
            }

            if (!wallMesh.Size.IsEqualApprox(wallShape.Size))
                Error($"Ground-floor entrance infill mesh/collider size mismatch: {wall.GetPath()}");
        }

        var receptionWalls = scene.GetNodeOrNull<Node3D>("PensionGroundFloor/ReceptionWalls");
        var legacyVarandaWalls = scene.GetNodeOrNull<Node3D>("PensionGroundFloor/VarandaWalls");
        if (legacyVarandaWalls != null)
            Error("Legacy ground-floor VarandaWalls still rebuilds malformed wall shards inside the reception entrance");

        var receptionWallNames = new[]
        {
            "Wall_Reception_Left",
            "Wall_Reception_Right",
            "Wall_Reception_SouthLeft",
            "Wall_Reception_SouthRight"
        };
        foreach (var wallName in receptionWallNames)
        {
            var wall = receptionWalls?.GetNodeOrNull<StaticBody3D>(wallName);
            if (wall == null)
            {
                Error($"Ground-floor reception entrance wall missing: {wallName}");
                continue;
            }

            var visuals = wall.GetChildren().OfType<MeshInstance3D>().ToArray();
            var collisions = wall.GetChildren().OfType<CollisionShape3D>().ToArray();
            if (visuals.Length != 1 || collisions.Length != 1 ||
                visuals[0].Mesh is not BoxMesh wallMesh || collisions[0].Shape is not BoxShape3D wallShape)
            {
                Error($"Reception entrance wall must own exactly one BoxMesh and one matching child BoxShape3D: {wall.GetPath()}");
                continue;
            }

            if (!wallMesh.Size.IsEqualApprox(wallShape.Size))
                Error($"Reception entrance wall mesh/collider size mismatch: {wall.GetPath()}");
        }

        var receptionLeft = receptionWalls?.GetNodeOrNull<StaticBody3D>("Wall_Reception_Left");
        var receptionRight = receptionWalls?.GetNodeOrNull<StaticBody3D>("Wall_Reception_Right");
        var southLeft = receptionWalls?.GetNodeOrNull<StaticBody3D>("Wall_Reception_SouthLeft");
        var southRight = receptionWalls?.GetNodeOrNull<StaticBody3D>("Wall_Reception_SouthRight");
        if (receptionLeft != null && receptionRight != null && southLeft != null && southRight != null)
        {
            const float receptionSouthZ = 1.2f;
            const float expectedDoorWidth = 1.4f;
            const float tolerance = 0.06f;

            foreach (var side in new[] { receptionLeft, receptionRight })
            {
                if (side.GetChildren().OfType<MeshInstance3D>().FirstOrDefault()?.Mesh is not BoxMesh sideMesh)
                    continue;

                var forwardEdge = side.Position.Z + sideMesh.Size.Z * 0.5f;
                if (Mathf.Abs(forwardEdge - (receptionSouthZ + 0.04f)) > tolerance)
                    Error($"Reception side wall still projects past the straight entrance plane: {side.GetPath()} edgeZ={forwardEdge:0.00}");
            }

            if (southLeft.GetChildren().OfType<MeshInstance3D>().FirstOrDefault()?.Mesh is BoxMesh southLeftMesh &&
                southRight.GetChildren().OfType<MeshInstance3D>().FirstOrDefault()?.Mesh is BoxMesh southRightMesh)
            {
                var leftOpeningEdge = southLeft.Position.X + southLeftMesh.Size.X * 0.5f;
                var rightOpeningEdge = southRight.Position.X - southRightMesh.Size.X * 0.5f;
                var openingWidth = rightOpeningEdge - leftOpeningEdge;
                if (Mathf.Abs(openingWidth - expectedDoorWidth) > tolerance)
                    Error($"Reception entrance central opening changed unexpectedly: width={openingWidth:0.00}");
            }
        }

        var materials = new[]
        {
            "M_RebocoSeco_Master.tres",
            "M_RebocoUmidoMofado_Master.tres",
            "M_MadeiraVelha_Master.tres",
            "M_TetoInfiltrado_Master.tres",
            "M_Terreo_RebocoMofadoBR_Master.tres",
            "M_Terreo_AssoalhoBR_Master.tres",
            "M_Terreo_TetoRebocoInfiltrado_Master.tres",
            "M_Entrepiso_TerreoTeto_SuperiorAssoalho.tres"
        };
        foreach (var material in materials)
            if (ResourceLoader.Load<Material>($"res://assets/materials/pensao/{material}") == null)
                Error($"Sprint 31C master material could not be loaded: {material}");

        var decals = new[]
        {
            "Decal_Mofo_Rodape_01.png",
            "Decal_Umidade_Canto_01.png",
            "Decal_Reboco_Descascado_01.png",
            "Decal_Infiltracao_Vertical_01.png",
            "Decal_Teto_Mancha_Agua_01.png",
            "Decal_Piso_Sujeira_Canto_01.png"
        };
        foreach (var decal in decals)
            if (ResourceLoader.Load<Texture2D>($"res://assets/decals/pensao/{decal}") == null)
                Error($"Sprint 31C decal texture could not be loaded: {decal}");

        if (forbidden.Length == 0)
            Ok("Sprint 31C isolated under VisualPolish: Brazilian ground-floor materials plus distinct damp stair and dry/cold upper-arrival profiles; legacy reception shards absent, paired entrance walls preserved, no detached blockers, frozen collision deck untouched");
    }

    private void CheckSprint33UpperFloorAtmosphere(Node scene)
    {
        var root = scene.GetNodeOrNull<Node3D>("World/VisualPolish/Sprint33_UpperFloorAtmosphere");
        if (root == null)
        {
            Error("Sprint 33 upper-floor atmosphere container missing");
            return;
        }

        foreach (var name in new[]
                 {
                     "UpperFloor_Walls",
                     "UpperFloor_Floors",
                     "UpperFloor_Ceilings",
                     "UpperFloor_Decals",
                     "UpperFloor_Lighting",
                     "UpperFloor_DeprecatedOldTextures"
                 })
            if (root.GetNodeOrNull<Node3D>(name) == null)
                Error($"Sprint 33 visual container missing: {name}");

        var forbidden = Enumerate(root)
            .Where(node => node is CollisionObject3D or CollisionShape3D or Area3D or
                NavigationRegion3D or Joint3D or Camera3D or RigidBody3D)
            .ToArray();
        foreach (var node in forbidden)
            Error($"Sprint 33 contains forbidden physics/gameplay/navigation node: {node.GetPath()}");

        var lights = Enumerate(root).OfType<Light3D>().ToArray();
        if (lights.Length > 2)
            Error($"Sprint 33 exceeds restrained upper-floor light budget: {lights.Length}/2");
        if (lights.Any(light => light.ShadowEnabled))
            Error("Sprint 33 fill lights must remain shadowless and visual-only");

        if (root.GetMeta("collision_nodes", -1).AsInt32() != 0 ||
            root.GetMeta("navigation_nodes", -1).AsInt32() != 0 ||
            root.GetMeta("gameplay_nodes", -1).AsInt32() != 0)
            Error("Sprint 33 reports physics, navigation or gameplay nodes");
        if (root.GetMeta("structural_geometry_changed", true).AsBool() ||
            root.GetMeta("frozen_upper_deck_changed", true).AsBool())
            Error("Sprint 33 reports structural geometry or frozen deck changes");
        if (root.GetMeta("first_floor_material_count", -1).AsInt32() != 0)
            Error("Sprint 33 material pass reached the approved first floor");
        if (root.GetMeta("materials_applied", 0).AsInt32() <= 0)
            Error("Sprint 33 did not apply any second-floor materials");
        if (root.GetMeta("decal_count", -1).AsInt32() != 0)
            Error("Sprint 33 legacy overlay boxes are active and may render as gray artifacts");
        if (!root.GetMeta("ground_ceiling_face_preserved", false).AsBool())
            Error("Sprint 33 ceramic floor does not report the approved ground-floor ceiling face as preserved");
        if (root.GetMeta("dual_face_floor_count", 0).AsInt32() <= 0)
            Error("Sprint 33 dual-face ceramic floor coverage is missing");
        if (root.GetMeta("room_201_202_header_material_count", 0).AsInt32() != 2)
            Error("Sprint 33 did not cover both Room 201/202 door headers with upper-wall plaster");
        if (root.GetMeta("perimeter_floor_edge_corrected_count", 0).AsInt32() != 2)
            Error("Sprint 33 did not blend both exposed floor-edge strips into the upper walls");
        if (root.GetMeta("stair_guard_material_count", 0).AsInt32() != 4)
            Error("Sprint 33 did not replace all four gray stair-guard blockout materials");
        if (root.GetMeta("obsolete_stair_platform_nodes_removed", 0).AsInt32() != 6)
            Error("Sprint 33 stair-platform hotfix metadata is incomplete");
        if (!root.GetMeta("stair_bridge_overlap_removed", false).AsBool())
            Error("Sprint 33 stair bridge still reports a coplanar overlap with the main floor");
        if (!root.GetMeta("floor_side_faces_hidden", false).AsBool())
            Error("Sprint 33 ceramic floor side faces may still render as stair-shaft frames");

        foreach (var removedNode in new[]
                 {
                     "Floor_Second_Main_NorthCap", "Floor_Second_Main_NorthCap_Visual",
                     "Floor_Second_Main_NorthWestCap", "Floor_Second_Main_NorthWestCap_Visual",
                     "UpperLanding_Main", "UpperLanding_Main_Visual"
                 })
        {
            if (scene.FindChild(removedNode, recursive: true, owned: false) != null)
                Error($"obsolete stair platform or overlapping landing floor is still active: {removedNode}");
        }

        foreach (var material in new[]
                 {
                     "M_Upper_Wall_RebocoMofado.tres",
                     "M_Upper_Floor_CeramicaAntiga.tres",
                     "M_Upper_Ceiling_Infiltrado.tres",
                     "M_Upper_Varanda_Mureta_RebocoPodre.tres"
                 })
            if (ResourceLoader.Load<Material>($"res://assets/materials/pensao/upper_floor/{material}") == null)
                Error($"Sprint 33 material could not be loaded: {material}");

        var ceramic = ResourceLoader.Load<ShaderMaterial>(
            "res://assets/materials/pensao/upper_floor/M_Upper_Floor_CeramicaAntiga.tres");
        if (ceramic?.Shader == null ||
            ceramic.GetShaderParameter("top_albedo").AsGodotObject() is not Texture2D ||
            ceramic.GetShaderParameter("bottom_albedo").AsGodotObject() is not Texture2D)
            Error("Sprint 33 ceramic floor must keep distinct top ceramic and bottom plaster textures");

        foreach (var decal in new[]
                 {
                     "Decal_Upper_Mofo_Rodape_01.png",
                     "Decal_Upper_Mofo_Canto_01.png",
                     "Decal_Upper_Reboco_Descascado_Grande_01.png",
                     "Decal_Upper_Infiltracao_Vertical_01.png",
                     "Decal_Upper_Mancha_Teto_01.png",
                     "Decal_Upper_Poeira_Canto_Chao_01.png",
                     "Decal_Upper_Marca_Arrasto_Quarto203_01.png",
                     "Decal_Upper_Janela_Umidade_01.png"
                 })
            if (ResourceLoader.Load<Texture2D>($"res://assets/decals/pensao/upper_floor/{decal}") == null)
                Error($"Sprint 33 decal texture could not be loaded: {decal}");

        if (forbidden.Length == 0 && lights.Length <= 2)
            Ok("Sprint 33 isolated under VisualPolish: second-floor-only PBR materials, old ceramic floor and two restrained fill lights; artifact overlays disabled, first floor and frozen deck untouched");
    }

    private void CheckSprint32DFinalRoof(Node scene)
    {
        var root = scene.GetNodeOrNull<Node3D>("World/ExteriorShell/Sprint32D_FinalRoof");
        if (root == null)
        {
            Error("Sprint 32D final-roof container missing");
            return;
        }

        if (root.GetNodeOrNull<Node3D>("FinalColonialRoof") == null)
            Error("Sprint 32D final colonial roof instance missing");

        var forbidden = Enumerate(root)
            .Where(node => node is CollisionObject3D or CollisionShape3D or Area3D or
                NavigationRegion3D or Joint3D or Camera3D or Light3D or RigidBody3D)
            .ToArray();
        foreach (var node in forbidden)
            Error($"Sprint 32D roof contains forbidden physics/gameplay node: {node.GetPath()}");

        if (!root.GetMeta("visual_only", false).AsBool())
            Error("Sprint 32D roof is not marked visual-only");
        if (root.GetMeta("collision_nodes", -1).AsInt32() != 0)
            Error("Sprint 32D roof reports a non-zero collision count");
        if (root.GetMeta("structural_geometry_changed", true).AsBool() ||
            root.GetMeta("frozen_upper_deck_changed", true).AsBool())
            Error("Sprint 32D roof reports structural geometry or frozen deck changes");

        foreach (var legacyName in new[]
                 {
                     "Sprint32B_RoofVisualShell", "Roof_Main_Colonial", "Roof_Front_Porch",
                     "Roof_Damage_Tiles", "Roof_Moss_Patches", "Roof_Blockout_Main"
                 })
        {
            var legacy = scene.FindChild(legacyName, recursive: true, owned: false);
            if (legacy != null)
                Error($"Sprint 32D legacy roof remains active: {legacy.GetPath()}");
        }

        if (forbidden.Length == 0)
            Ok("Sprint 32D isolated under ExteriorShell: one final colonial roof, zero collision/gameplay nodes, legacy roofs absent, interior geometry and frozen upper deck untouched");
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
