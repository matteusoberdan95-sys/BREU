namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Builds ground-floor blockout geometry under World/Exterior, PensionGroundFloor and Interactions.
/// Sprint 05 — continuous floor collision (hotfix: no gaps).
/// </summary>
public partial class PensaoTerreoBlockout01Builder : Node3D
{
    private const uint WorldLayer = 1;
    private const uint InteractableLayer = 2;
    private const uint WorldInteractableLayer = WorldLayer | InteractableLayer;

    private const float WallHeight = 3.0f;
    private const float WallThickness = 0.2f;
    private const float DoorWidth = 1.4f;
    private const float DoorHeight = 2.3f;
    private const float CorridorWidth = 2.4f;

    private const float FloorThickness = 0.25f;
    private const float FloorTopY = 0.0f;
    private const float FloorCenterY = FloorTopY - FloorThickness * 0.5f;
    private const float FloorOverlap = 0.08f;
    private const float VisualFloorHeight = 0.04f;

    private StandardMaterial3D _matExteriorGround = null!;
    private StandardMaterial3D _matTrail = null!;
    private StandardMaterial3D _matVaranda = null!;
    private StandardMaterial3D _matInteriorFloor = null!;
    private StandardMaterial3D _matExteriorWall = null!;
    private StandardMaterial3D _matInteriorWall = null!;
    private StandardMaterial3D _matCounter = null!;
    private StandardMaterial3D _matDoor = null!;
    private StandardMaterial3D _matInteractable = null!;
    private StandardMaterial3D _matBed = null!;

    private Node3D _exterior = null!;
    private Node3D _interior = null!;
    private Node3D _interactions = null!;

    public override void _Ready()
    {
        CreateMaterials();
        ResolveNodes();
        BuildContinuousFloors();
        BuildFloorVisuals();
        BuildExteriorBoundaries();
        BuildVarandaWalls();
        BuildReception();
        BuildCorridor();
        BuildRoom102();
        BuildKitchen();
        BuildStorage();
        BuildInteractions();
    }

    private void ResolveNodes()
    {
        _exterior = GetNode<Node3D>("../../Exterior");
        _interior = GetNode<Node3D>("../../PensionGroundFloor");
        _interactions = GetNode<Node3D>("../../Interactions");
    }

    private void CreateMaterials()
    {
        _matExteriorGround = Mat(new Color(0.25f, 0.18f, 0.12f));
        _matTrail = Mat(new Color(0.42f, 0.32f, 0.22f));
        _matVaranda = Mat(new Color(0.45f, 0.35f, 0.25f));
        _matInteriorFloor = Mat(new Color(0.28f, 0.22f, 0.16f));
        _matExteriorWall = Mat(new Color(0.72f, 0.7f, 0.66f));
        _matInteriorWall = Mat(new Color(0.78f, 0.74f, 0.68f));
        _matCounter = Mat(new Color(0.4f, 0.3f, 0.22f));
        _matDoor = Mat(new Color(0.22f, 0.16f, 0.12f));
        _matInteractable = Mat(new Color(0.55f, 0.62f, 0.48f));
        _matBed = Mat(new Color(0.35f, 0.32f, 0.38f));
    }

    private static StandardMaterial3D Mat(Color color)
    {
        return new StandardMaterial3D { AlbedoColor = color };
    }

    /// <summary>Three overlapping collision slabs — no walkable gaps.</summary>
    private void BuildContinuousFloors()
    {
        AddCollisionFloor(
            _exterior,
            "Exterior_MainGround",
            new Vector3(0, FloorCenterY, 29.5f),
            new Vector3(44, FloorThickness, 47 + FloorOverlap));

        AddCollisionFloor(
            _interior,
            "Porch_MainFloor",
            new Vector3(0, FloorCenterY, 8.0f),
            new Vector3(14 + FloorOverlap, FloorThickness, 9 + FloorOverlap));

        AddCollisionFloor(
            _interior,
            "PensionGroundFloor_MainFloor",
            new Vector3(0, FloorCenterY, -10.75f),
            new Vector3(14 + FloorOverlap, FloorThickness, 44.5f + FloorOverlap));
    }

    /// <summary>Visual-only floor overlays — materials only, no collision.</summary>
    private void BuildFloorVisuals()
    {
        var visualY = FloorTopY + VisualFloorHeight * 0.5f;

        AddVisualFloor(_exterior, "Exterior_GroundVisual", new Vector3(0, visualY, 29.5f), new Vector3(44, VisualFloorHeight, 47), _matExteriorGround);
        AddVisualFloor(_exterior, "Exterior_PathFloor", new Vector3(0, visualY + 0.01f, 30), new Vector3(3, VisualFloorHeight, 38), _matTrail);
        AddVisualFloor(_interior, "Porch_VisualFloor", new Vector3(0, visualY, 7), new Vector3(12, VisualFloorHeight, 6), _matVaranda);
        AddVisualFloor(_interior, "Reception_VisualFloor", new Vector3(0, visualY, -3.5f), new Vector3(10, VisualFloorHeight, 7), _matInteriorFloor);
        AddVisualFloor(_interior, "Corridor_VisualFloor", new Vector3(0, visualY, -17.5f), new Vector3(CorridorWidth, VisualFloorHeight, 18), _matInteriorFloor);
        AddVisualFloor(_interior, "Room102_VisualFloor", new Vector3(-4.2f, visualY, -15.5f), new Vector3(4.5f, VisualFloorHeight, 4), _matInteriorFloor);
        AddVisualFloor(_interior, "Kitchen_VisualFloor", new Vector3(4.2f, visualY, -20.5f), new Vector3(4.5f, VisualFloorHeight, 4), _matInteriorFloor);
        AddVisualFloor(_interior, "Storage_VisualFloor", new Vector3(0, visualY, -29.5f), new Vector3(CorridorWidth, VisualFloorHeight, 4), _matInteriorFloor);
    }

    private void BuildExteriorBoundaries()
    {
        AddBoundaryWall(_exterior, "BoundaryTrailEnd", new Vector3(0, 1.5f, 52), new Vector3(44, 3, WallThickness));
        AddBoundaryWall(_exterior, "BoundaryBuildingFar", new Vector3(0, 1.5f, -35), new Vector3(44, 3, WallThickness));
        AddBoundaryWall(_exterior, "BoundaryEast", new Vector3(22, 1.5f, 28), new Vector3(WallThickness, 3, 52));
        AddBoundaryWall(_exterior, "BoundaryWest", new Vector3(-22, 1.5f, 28), new Vector3(WallThickness, 3, 52));
    }

    private void BuildVarandaWalls()
    {
        AddWall(_interior, "VarandaWest", new Vector3(-6.1f, 1.5f, 7), new Vector3(WallThickness, WallHeight, 6), _matExteriorWall);
        AddWall(_interior, "VarandaEast", new Vector3(6.1f, 1.5f, 7), new Vector3(WallThickness, WallHeight, 6), _matExteriorWall);
    }

    private void BuildReception()
    {
        AddWall(_interior, "ReceptionWest", new Vector3(-5.1f, 1.5f, -3.5f), new Vector3(WallThickness, WallHeight, 7), _matInteriorWall);
        AddWall(_interior, "ReceptionEast", new Vector3(5.1f, 1.5f, -3.5f), new Vector3(WallThickness, WallHeight, 7), _matInteriorWall);

        var northZ = -7.0f;
        AddWall(_interior, "ReceptionNorthLeft", new Vector3(-3.5f, 1.5f, northZ), new Vector3(4, WallHeight, WallThickness), _matInteriorWall);
        AddWall(_interior, "ReceptionNorthRight", new Vector3(3.5f, 1.5f, northZ), new Vector3(4, WallHeight, WallThickness), _matInteriorWall);
        AddWall(_interior, "ReceptionCounter", new Vector3(3.4f, 0.55f, -3.5f), new Vector3(2.4f, 1.1f, 0.6f), _matCounter);
    }

    private void BuildCorridor()
    {
        var centerZ = -17.5f;
        var length = 18.0f;
        var halfWidth = CorridorWidth * 0.5f;

        BuildWallWithDoorGap(_interior, "CorridorWest", -halfWidth - WallThickness * 0.5f, centerZ, length, -15.5f, DoorWidth, _matInteriorWall);
        BuildWallWithDoorGap(_interior, "CorridorEast", halfWidth + WallThickness * 0.5f, centerZ, length, -20.5f, DoorWidth, _matInteriorWall);
    }

    private void BuildRoom102()
    {
        AddWall(_interior, "Room102West", new Vector3(-6.45f, 1.5f, -15.5f), new Vector3(WallThickness, WallHeight, 4.0f), _matInteriorWall);
        AddWall(_interior, "Room102North", new Vector3(-4.2f, 1.5f, -17.55f), new Vector3(4.5f, WallHeight, WallThickness), _matInteriorWall);
        AddWall(_interior, "Room102South", new Vector3(-4.2f, 1.5f, -13.45f), new Vector3(4.5f, WallHeight, WallThickness), _matInteriorWall);
        AddVisualOnly(_interior, "Room102Bed", new Vector3(-5.5f, 0.35f, -15.5f), new Vector3(2.0f, 0.7f, 1.8f), _matBed);
    }

    private void BuildKitchen()
    {
        AddWall(_interior, "KitchenEast", new Vector3(6.45f, 1.5f, -20.5f), new Vector3(WallThickness, WallHeight, 4.0f), _matInteriorWall);
        AddWall(_interior, "KitchenNorth", new Vector3(4.2f, 1.5f, -22.55f), new Vector3(4.5f, WallHeight, WallThickness), _matInteriorWall);
        AddWall(_interior, "KitchenSouth", new Vector3(4.2f, 1.5f, -18.45f), new Vector3(4.5f, WallHeight, WallThickness), _matInteriorWall);
        AddWall(_interior, "KitchenCounter", new Vector3(3.4f, 0.55f, -19.7f), new Vector3(1.8f, 1.1f, 0.6f), _matCounter);
    }

    private void BuildStorage()
    {
        AddWall(_interior, "StorageBack", new Vector3(0, 1.5f, -31.5f), new Vector3(CorridorWidth, WallHeight, WallThickness), _matInteriorWall);
        AddWall(_interior, "StorageWest", new Vector3(-1.3f, 1.5f, -29.5f), new Vector3(WallThickness, WallHeight, 4.0f), _matInteriorWall);
        AddWall(_interior, "StorageEast", new Vector3(1.3f, 1.5f, -29.5f), new Vector3(WallThickness, WallHeight, 4.0f), _matInteriorWall);
    }

    private void BuildInteractions()
    {
        AddInteractableBody(
            _interactions,
            "JobOfferSign",
            new Vector3(2.8f, 1.2f, 32),
            new Vector3(1.4f, 1.0f, 0.08f),
            "Ler placa",
            "OFERTA DE TRABALHO - MINERAÇÃO - PENSÃO SANTA LUZIA.",
            "job_offer_sign");

        AddInteractableArea(
            _interactions,
            "ReceptionBook",
            new Vector3(3.4f, 1.05f, -4.5f),
            new Vector3(0.5f, 0.12f, 0.35f),
            "Examinar livro",
            "Seu nome já está no registro.",
            "reception_book");

        AddInteractableArea(
            _interactions,
            "Room102Inspect",
            new Vector3(-4.2f, 1.4f, -15.5f),
            new Vector3(1.2f, 1.2f, 1.2f),
            "Examinar quarto",
            "Parece que este quarto foi preparado para mim.",
            "room_102");

        AddInteractableArea(
            _interactions,
            "KitchenInspect",
            new Vector3(4.2f, 1.4f, -20.5f),
            new Vector3(1.2f, 1.2f, 1.2f),
            "Examinar cozinha",
            "A cozinha está fria demais para uma pensão habitada.",
            "kitchen");

        AddInteractableBody(
            _interactions,
            "StorageDoorInteract",
            new Vector3(0, 1.15f, -26.5f),
            new Vector3(DoorWidth, DoorHeight, 0.12f),
            "Tentar abrir depósito",
            "Está trancado.",
            "storage_door");
    }

    private void BuildWallWithDoorGap(
        Node3D parent,
        string baseName,
        float wallX,
        float centerZ,
        float totalLength,
        float doorCenterZ,
        float doorWidth,
        StandardMaterial3D material)
    {
        var halfLength = totalLength * 0.5f;
        var doorHalf = doorWidth * 0.5f;
        var southEnd = centerZ + halfLength;
        var northEnd = centerZ - halfLength;
        var doorSouth = doorCenterZ + doorHalf;
        var doorNorth = doorCenterZ - doorHalf;

        var southSegmentLength = southEnd - doorSouth;
        if (southSegmentLength > 0.05f)
        {
            AddWall(
                parent,
                $"{baseName}South",
                new Vector3(wallX, 1.5f, doorSouth + southSegmentLength * 0.5f),
                new Vector3(WallThickness, WallHeight, southSegmentLength),
                material);
        }

        var northSegmentLength = doorNorth - northEnd;
        if (northSegmentLength > 0.05f)
        {
            AddWall(
                parent,
                $"{baseName}North",
                new Vector3(wallX, 1.5f, doorNorth - northSegmentLength * 0.5f),
                new Vector3(WallThickness, WallHeight, northSegmentLength),
                material);
        }
    }

    private void AddCollisionFloor(Node3D parent, string name, Vector3 center, Vector3 size)
    {
        var body = new StaticBody3D
        {
            Name = name,
            Position = center,
            CollisionLayer = WorldLayer,
            CollisionMask = 0
        };

        body.AddChild(CreateBoxCollision(size));
        parent.AddChild(body);
    }

    private void AddVisualFloor(Node3D parent, string name, Vector3 center, Vector3 size, StandardMaterial3D material)
    {
        AddVisualOnly(parent, name, center, size, material);
    }

    private void AddWall(Node3D parent, string name, Vector3 center, Vector3 size, StandardMaterial3D material)
    {
        AddSolid(parent, name, center, size, material, WorldLayer);
    }

    private void AddBoundaryWall(Node3D parent, string name, Vector3 center, Vector3 size)
    {
        AddSolid(parent, name, center, size, _matExteriorWall, WorldLayer);
    }

    private void AddSolid(Node3D parent, string name, Vector3 center, Vector3 size, StandardMaterial3D material, uint layer)
    {
        var body = new StaticBody3D
        {
            Name = name,
            Position = center,
            CollisionLayer = layer,
            CollisionMask = 0
        };

        body.AddChild(CreateBoxCollision(size));
        body.AddChild(CreateBoxMesh(size, material));
        parent.AddChild(body);
    }

    private void AddVisualOnly(Node3D parent, string name, Vector3 center, Vector3 size, StandardMaterial3D material)
    {
        var visual = new Node3D { Name = name, Position = center };
        visual.AddChild(CreateBoxMesh(size, material));
        parent.AddChild(visual);
    }

    private void AddInteractableBody(
        Node3D parent,
        string name,
        Vector3 center,
        Vector3 size,
        string prompt,
        string message,
        string id)
    {
        var body = new StaticBody3D
        {
            Name = name,
            Position = center,
            CollisionLayer = WorldInteractableLayer,
            CollisionMask = 0
        };

        body.AddChild(CreateBoxCollision(size));
        body.AddChild(CreateBoxMesh(size, _matInteractable));
        body.AddChild(CreateInteractable(prompt, message, id));
        parent.AddChild(body);
    }

    private void AddInteractableArea(
        Node3D parent,
        string name,
        Vector3 center,
        Vector3 size,
        string prompt,
        string message,
        string id)
    {
        var host = new Node3D { Name = name, Position = center };

        var area = new Area3D
        {
            Name = "InteractionArea",
            CollisionLayer = InteractableLayer,
            CollisionMask = 0,
            Monitoring = false,
            Monitorable = true
        };

        area.AddChild(CreateBoxCollision(size));
        area.AddChild(CreateInteractable(prompt, message, id));
        host.AddChild(area);
        parent.AddChild(host);
    }

    private static CollisionShape3D CreateBoxCollision(Vector3 size)
    {
        return new CollisionShape3D { Shape = new BoxShape3D { Size = size } };
    }

    private static MeshInstance3D CreateBoxMesh(Vector3 size, StandardMaterial3D material)
    {
        return new MeshInstance3D
        {
            Mesh = new BoxMesh { Size = size },
            MaterialOverride = material
        };
    }

    private static Interactable CreateInteractable(string prompt, string message, string id)
    {
        return new Interactable
        {
            PromptText = prompt,
            InteractionMessage = message,
            InteractionId = id
        };
    }
}
