namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Sprint 14Z — simple opaque door blockers without frames or prefabs.
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
            Position = panelPos + new Vector3(0f, 0f, -0.26f),
            CollisionLayer = InteractableLayer,
            CollisionMask = 0,
            Monitoring = false
        };
        var interactShape = new CollisionShape3D
        {
            Shape = new BoxShape3D
            {
                Size = new Vector3(
                    Mathf.Max(0.5f, openingWidth - 0.2f),
                    Mathf.Max(1.0f, DoorHeight - 0.3f),
                    0.55f)
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
}
