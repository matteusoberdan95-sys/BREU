namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Sprint 14Z — simple opaque door blockers without frames or prefabs.
/// Sprint 17A — balcony door uses floor-local panel Y (never double-add second-floor height).
/// </summary>
public partial class PensaoTerreoBlockout01Builder
{
    private const float BlockerDepth = 0.2f;
    private const float BlockerStandoffZ = -0.08f;

    protected BlockoutLockedDoor AddLockedDoorBlocker(
        Node3D parent,
        string rootName,
        string blockerName,
        Vector3 worldPosition,
        float openingWidth,
        StandardMaterial3D panelMaterial,
        string promptText,
        string lockedMessage,
        float floorTopY = 0f,
        float panelOffsetX = 0f,
        float rotationY = 0f)
    {
        var root = new BlockoutLockedDoor
        {
            Name = rootName,
            Position = worldPosition,
            Rotation = new Vector3(0f, rotationY, 0f),
            PromptText = promptText,
            LockedMessage = lockedMessage
        };
        parent.AddChild(root);

        // floorTopY is relative to the door root (deposit: root≈0, floorTopY=0).
        // Do NOT pass SecondFloorTopY here if the root is already at second-floor height.
        var panelCenterY = floorTopY + DoorHeight * 0.5f - WallEmbedBelowFloor;
        var panelPos = new Vector3(panelOffsetX, panelCenterY, BlockerStandoffZ);

        var panel = new MeshInstance3D
        {
            Name = blockerName,
            Mesh = new BoxMesh { Size = new Vector3(openingWidth, DoorHeight, BlockerDepth) },
            MaterialOverride = panelMaterial,
            Position = panelPos
        };
        root.AddChild(panel);

        var body = new StaticBody3D
        {
            Name = "BlockingBody",
            CollisionLayer = WorldLayer,
            CollisionMask = 0
        };
        var blockShape = new CollisionShape3D
        {
            Name = "BlockingShape",
            Position = panelPos,
            Shape = new BoxShape3D { Size = new Vector3(openingWidth, DoorHeight, BlockerDepth) }
        };
        body.AddChild(blockShape);
        root.AddChild(body);

        var area = new Area3D
        {
            Name = "InteractionArea",
            Position = new Vector3(panelOffsetX, 1.45f, BlockerStandoffZ - 0.35f),
            CollisionLayer = InteractableLayer,
            CollisionMask = 0,
            Monitoring = false
        };
        var interactShape = new CollisionShape3D
        {
            Shape = new BoxShape3D
            {
                Size = new Vector3(
                    Mathf.Max(0.5f, openingWidth - 0.15f),
                    1.2f,
                    0.6f)
            }
        };
        area.AddChild(interactShape);
        root.AddChild(area);

        return root;
    }

    protected BlockoutUnlockHideDoor AddDepositDoorBlocker(
        Node3D parent,
        string rootName,
        string blockerName,
        Vector3 worldPosition,
        float openingWidth,
        StandardMaterial3D panelMaterial,
        float floorTopY = 0f)
    {
        var root = new BlockoutUnlockHideDoor
        {
            Name = rootName,
            Position = worldPosition
        };
        parent.AddChild(root);

        var panelCenterY = floorTopY + DoorHeight * 0.5f - WallEmbedBelowFloor;
        var panelPos = new Vector3(0f, panelCenterY, BlockerStandoffZ);

        var panel = new MeshInstance3D
        {
            Name = blockerName,
            Mesh = new BoxMesh { Size = new Vector3(openingWidth, DoorHeight, BlockerDepth) },
            MaterialOverride = panelMaterial,
            Position = panelPos
        };
        root.AddChild(panel);

        var body = new StaticBody3D
        {
            Name = "BlockingBody",
            CollisionLayer = WorldLayer,
            CollisionMask = 0
        };
        var blockShape = new CollisionShape3D
        {
            Name = "BlockingShape",
            Position = panelPos,
            Shape = new BoxShape3D { Size = new Vector3(openingWidth, DoorHeight, BlockerDepth) }
        };
        body.AddChild(blockShape);
        root.AddChild(body);

        var area = new Area3D
        {
            Name = "InteractionArea",
            Position = panelPos + new Vector3(0f, 0f, -0.26f),
            CollisionLayer = InteractableLayer,
            CollisionMask = 0,
            Monitoring = false
        };
        var interactShape = new CollisionShape3D
        {
            Shape = new BoxShape3D { Size = new Vector3(1.2f, 2.0f, 0.55f) }
        };
        area.AddChild(interactShape);
        root.AddChild(area);

        return root;
    }

    /// <summary>
    /// Sprint 17/17A — green balcony door. worldFloorTopY = second-floor slab top.
    /// Root sits on that floor; panel/Area use local Y only (no double floor height).
    /// </summary>
    protected BlockoutBalconyDoor AddBalconyDoorBlocker(
        Node3D parent,
        string rootName,
        string blockerName,
        Vector3 worldXz,
        float openingWidth,
        StandardMaterial3D panelMaterial,
        float worldFloorTopY,
        float panelOffsetX = 0f)
    {
        var root = new BlockoutBalconyDoor
        {
            Name = rootName,
            Position = new Vector3(worldXz.X, worldFloorTopY - WallEmbedBelowFloor, worldXz.Z)
        };
        parent.AddChild(root);

        // Local to floor root — same pattern as ground-floor deposit door.
        var panelCenterY = DoorHeight * 0.5f - WallEmbedBelowFloor;
        var panelPos = new Vector3(panelOffsetX, panelCenterY, BlockerStandoffZ);

        var panel = new MeshInstance3D
        {
            Name = blockerName,
            Mesh = new BoxMesh { Size = new Vector3(openingWidth, DoorHeight, BlockerDepth) },
            MaterialOverride = panelMaterial,
            Position = panelPos
        };
        root.AddChild(panel);

        var body = new StaticBody3D
        {
            Name = "BlockingBody",
            CollisionLayer = WorldLayer,
            CollisionMask = 0
        };
        var blockShape = new CollisionShape3D
        {
            Name = "BlockingShape",
            Position = panelPos,
            Shape = new BoxShape3D { Size = new Vector3(openingWidth, DoorHeight, BlockerDepth) }
        };
        body.AddChild(blockShape);
        root.AddChild(body);

        // Chest-height interact volume toward corridor (local -Z), not ceiling.
        var area = new Area3D
        {
            Name = "Interact_BalconyDoor",
            Position = new Vector3(panelOffsetX, 1.45f, BlockerStandoffZ - 0.38f),
            CollisionLayer = InteractableLayer,
            CollisionMask = 0,
            Monitoring = false,
            Monitorable = true
        };
        var interactShape = new CollisionShape3D
        {
            Shape = new BoxShape3D
            {
                Size = new Vector3(
                    Mathf.Max(0.55f, openingWidth - 0.1f),
                    1.15f,
                    0.65f)
            }
        };
        area.AddChild(interactShape);
        root.AddChild(area);

        GD.Print(
            $"[BalconyDoor] Created {rootName} root={root.Position} " +
            $"panelLocal={panelPos} areaLocal={area.Position} " +
            $"worldPanelY≈{root.Position.Y + panelPos.Y:0.00}");

        return root;
    }

    /// <summary>
    /// Sprint 17C — owner bedroom door. Same unlock-hide pattern as deposit/balcony.
    /// Root on second-floor slab; panel/Area use local Y only.
    /// </summary>
    protected BlockoutOwnerBedroomDoor AddOwnerBedroomDoorBlocker(
        Node3D parent,
        string rootName,
        string blockerName,
        Vector3 worldXz,
        float openingWidth,
        StandardMaterial3D panelMaterial,
        float worldFloorTopY,
        float panelOffsetX = 0f,
        float rotationY = 0f)
    {
        var root = new BlockoutOwnerBedroomDoor
        {
            Name = rootName,
            Position = new Vector3(worldXz.X, worldFloorTopY - WallEmbedBelowFloor, worldXz.Z),
            Rotation = new Vector3(0f, rotationY, 0f)
        };
        parent.AddChild(root);

        var panelCenterY = DoorHeight * 0.5f - WallEmbedBelowFloor;
        var panelPos = new Vector3(panelOffsetX, panelCenterY, BlockerStandoffZ);

        root.AddChild(new MeshInstance3D
        {
            Name = blockerName,
            Mesh = new BoxMesh { Size = new Vector3(openingWidth, DoorHeight, BlockerDepth) },
            MaterialOverride = panelMaterial,
            Position = panelPos
        });

        var body = new StaticBody3D
        {
            Name = "BlockingBody",
            CollisionLayer = WorldLayer,
            CollisionMask = 0
        };
        body.AddChild(new CollisionShape3D
        {
            Name = "BlockingShape",
            Position = panelPos,
            Shape = new BoxShape3D { Size = new Vector3(openingWidth, DoorHeight, BlockerDepth) }
        });
        root.AddChild(body);

        var area = new Area3D
        {
            Name = "Interact_OwnerDoor",
            Position = new Vector3(panelOffsetX, 1.45f, BlockerStandoffZ - 0.38f),
            CollisionLayer = InteractableLayer,
            CollisionMask = 0,
            Monitoring = false,
            Monitorable = true
        };
        area.AddChild(new CollisionShape3D
        {
            Shape = new BoxShape3D
            {
                Size = new Vector3(Mathf.Max(0.55f, openingWidth - 0.1f), 1.15f, 0.65f)
            }
        });
        root.AddChild(area);
        return root;
    }
}
