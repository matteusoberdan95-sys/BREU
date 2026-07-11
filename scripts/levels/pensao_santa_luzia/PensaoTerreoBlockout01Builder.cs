namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Builds ground-floor blockout geometry under World/Exterior, PensionGroundFloor and Interactions.
/// Sprint 05 blockout + Sprint 06 tuning + Sprint 09A stair integration.
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
    private const float FloorWallLip = 0.08f;
    private const float WallCornerOverlap = 0.08f;

    private const float VisualFloorThickness = 0.2f;
    private const float ExteriorFloorTopY = 0.01f;
    private const float TrailFloorTopY = 0.06f;
    private const float InteriorFloorTopY = 0.02f;
    private const float PorchFloorTopY = 0.03f;
    private const float ThresholdLiftY = 0.012f;

    private const float WallEmbedBelowFloor = 0.05f;
    private static float WallCenterY => WallHeight * 0.5f - WallEmbedBelowFloor;

    private const float BuildingHalfWidth = 7.0f;
    private const float ReceptionHalfWidth = 5.1f;
    private const float CorridorHalfWidth = CorridorWidth * 0.5f;
    private const float CorridorWallX = CorridorHalfWidth + WallThickness * 0.5f;
    private const float BuildingFrontZ = 11.6f;
    private const float BuildingBackZ = -32.6f;
    private const float MainEntryWidth = 5.2f;

    private StandardMaterial3D _matExteriorGround = null!;
    private StandardMaterial3D _matTrail = null!;
    private StandardMaterial3D _matVaranda = null!;
    private StandardMaterial3D _matInteriorFloor = null!;
    private StandardMaterial3D _matCorridorFloor = null!;
    private StandardMaterial3D _matExteriorWall = null!;
    private StandardMaterial3D _matInteriorWall = null!;
    private StandardMaterial3D _matCounter = null!;
    private StandardMaterial3D _matDoor = null!;
    private StandardMaterial3D _matInteractable = null!;
    private StandardMaterial3D _matBed = null!;
    private StandardMaterial3D _matStairStep = null!;
    private StandardMaterial3D _matStairStringer = null!;
    private StandardMaterial3D _matStairUpper = null!;
    private StandardMaterial3D _matStairRail = null!;

    private Node3D _exterior = null!;
    private Node3D _interior = null!;
    private Node3D _interactions = null!;

    public override void _Ready()
    {
        CreateMaterials();
        ResolveNodes();
        BuildContinuousFloors();
        BuildFloorVisuals();
        BuildExteriorTrailEnclosure();
        BuildBuildingExteriorShell();
        BuildVarandaWalls();
        BuildReception();
        BuildCorridor();
        BuildRoom102();
        BuildKitchen();
        BuildStorage();
        BuildStairIntegration();
        BuildFurnitureCollisions();
        BuildDoorThresholds();
        BuildExteriorBoundaries();
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
        _matCorridorFloor = Mat(new Color(0.24f, 0.19f, 0.14f));
        _matExteriorWall = Mat(new Color(0.72f, 0.7f, 0.66f));
        _matInteriorWall = Mat(new Color(0.78f, 0.74f, 0.68f));
        _matCounter = Mat(new Color(0.4f, 0.3f, 0.22f));
        _matDoor = Mat(new Color(0.22f, 0.16f, 0.12f));
        _matInteractable = Mat(new Color(0.55f, 0.62f, 0.48f));
        _matBed = Mat(new Color(0.35f, 0.32f, 0.38f));
        _matStairStep = Mat(new Color(0.58f, 0.46f, 0.34f));
        _matStairStringer = Mat(new Color(0.34f, 0.32f, 0.36f));
        _matStairUpper = Mat(new Color(0.52f, 0.54f, 0.58f));
        _matStairRail = Mat(new Color(0.38f, 0.4f, 0.44f));
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

    private void BuildFloorVisuals()
    {
        var exteriorFloors = new Node3D { Name = "FloorsVisual" };
        _exterior.AddChild(exteriorFloors);

        AddVisualFloorPlate(
            exteriorFloors,
            "Floor_Exterior_Main_Visual",
            new Vector3(0, 0, 30),
            new Vector2(52, 54),
            ExteriorFloorTopY,
            _matExteriorGround);

        AddVisualFloorPlate(
            exteriorFloors,
            "Floor_Exterior_Trail",
            new Vector3(0, 0, 33),
            new Vector2(3.4f, 38),
            TrailFloorTopY,
            _matTrail);

        var interiorFloors = new Node3D { Name = "FloorsVisual" };
        _interior.AddChild(interiorFloors);

        var interiorLength = BuildingFrontZ - BuildingBackZ + FloorWallLip * 2;
        var interiorCenterZ = (BuildingFrontZ + BuildingBackZ) * 0.5f;

        AddVisualFloorPlate(
            interiorFloors,
            "Floor_PensionGround_Main_Visual",
            new Vector3(0, 0, interiorCenterZ),
            new Vector2(BuildingHalfWidth * 2 + FloorWallLip * 2, interiorLength),
            InteriorFloorTopY,
            _matInteriorFloor);

        AddVisualFloorPlate(
            interiorFloors,
            "Floor_Porch_Main",
            new Vector3(0, 0, 8.2f),
            new Vector2(12.4f, 7.2f),
            PorchFloorTopY,
            _matVaranda);

        AddVisualFloorPlate(
            interiorFloors,
            "Floor_Corridor_Readability",
            new Vector3(0, 0, -17.5f),
            new Vector2(CorridorWidth + 0.24f, 18.4f),
            InteriorFloorTopY + 0.015f,
            _matCorridorFloor);
    }

    /// <summary>Low side berms so the trail does not expose empty space immediately.</summary>
    private void BuildExteriorTrailEnclosure()
    {
        var enclosure = new Node3D { Name = "TrailEnclosure" };
        _exterior.AddChild(enclosure);

        const float bermHeight = 2.4f;
        const float bermCenterY = bermHeight * 0.5f - WallEmbedBelowFloor;
        const float bermLength = 40f;
        const float bermCenterZ = 31f;
        const float bermOffsetX = 4.2f;

        AddBoundaryWall(
            enclosure,
            "TrailBermWest",
            new Vector3(-bermOffsetX, bermCenterY, bermCenterZ),
            new Vector3(WallThickness, bermHeight, bermLength));

        AddBoundaryWall(
            enclosure,
            "TrailBermEast",
            new Vector3(bermOffsetX, bermCenterY, bermCenterZ),
            new Vector3(WallThickness, bermHeight, bermLength));
    }

    private void BuildBuildingExteriorShell()
    {
        var shell = new Node3D { Name = "BuildingExteriorShell" };
        _interior.AddChild(shell);

        var shellDepth = BuildingFrontZ - BuildingBackZ + WallCornerOverlap;
        var shellCenterZ = (BuildingFrontZ + BuildingBackZ) * 0.5f;
        var shellSpanX = BuildingHalfWidth * 2 + WallThickness + WallCornerOverlap;

        AddWall(
            shell,
            "Wall_Exterior_Back",
            new Vector3(0, WallCenterY, BuildingBackZ - WallThickness * 0.5f),
            new Vector3(shellSpanX, WallHeight, WallThickness),
            _matExteriorWall);

        AddWall(
            shell,
            "Wall_Exterior_Left",
            new Vector3(-BuildingHalfWidth - WallThickness * 0.5f, WallCenterY, shellCenterZ),
            new Vector3(WallThickness, WallHeight, shellDepth),
            _matExteriorWall);

        AddWall(
            shell,
            "Wall_Exterior_Right",
            new Vector3(BuildingHalfWidth + WallThickness * 0.5f, WallCenterY, shellCenterZ),
            new Vector3(WallThickness, WallHeight, shellDepth),
            _matExteriorWall);

        var frontSegmentWidth = BuildingHalfWidth - MainEntryWidth * 0.5f + WallCornerOverlap;
        var frontSegmentCenterX = BuildingHalfWidth - frontSegmentWidth * 0.5f;

        AddWall(
            shell,
            "Wall_Exterior_Front_Left",
            new Vector3(-frontSegmentCenterX, WallCenterY, BuildingFrontZ + WallThickness * 0.5f),
            new Vector3(frontSegmentWidth, WallHeight, WallThickness),
            _matExteriorWall);

        AddWall(
            shell,
            "Wall_Exterior_Front_Right",
            new Vector3(frontSegmentCenterX, WallCenterY, BuildingFrontZ + WallThickness * 0.5f),
            new Vector3(frontSegmentWidth, WallHeight, WallThickness),
            _matExteriorWall);
    }

    private void BuildVarandaWalls()
    {
        var varanda = new Node3D { Name = "VarandaWalls" };
        _interior.AddChild(varanda);

        const float varandaDepth = 6.2f;
        const float varandaCenterZ = 8.2f;

        AddWall(
            varanda,
            "Wall_Varanda_Left",
            new Vector3(-6.1f, WallCenterY, varandaCenterZ),
            new Vector3(WallThickness, WallHeight, varandaDepth + WallCornerOverlap),
            _matExteriorWall);

        AddWall(
            varanda,
            "Wall_Varanda_Right",
            new Vector3(6.1f, WallCenterY, varandaCenterZ),
            new Vector3(WallThickness, WallHeight, varandaDepth + WallCornerOverlap),
            _matExteriorWall);

        AddWall(
            varanda,
            "Wall_Varanda_FrontLeft",
            new Vector3(-4.8f, WallCenterY, BuildingFrontZ - 0.4f),
            new Vector3(2.4f + WallCornerOverlap, WallHeight, WallThickness),
            _matExteriorWall);

        AddWall(
            varanda,
            "Wall_Varanda_FrontRight",
            new Vector3(4.8f, WallCenterY, BuildingFrontZ - 0.4f),
            new Vector3(2.4f + WallCornerOverlap, WallHeight, WallThickness),
            _matExteriorWall);

        AddWall(
            varanda,
            "Wall_Varanda_BackLeft",
            new Vector3(-4.05f, WallCenterY, 5.15f),
            new Vector3(2.1f + WallCornerOverlap, WallHeight, WallThickness),
            _matExteriorWall);

        AddWall(
            varanda,
            "Wall_Varanda_BackRight",
            new Vector3(4.05f, WallCenterY, 5.15f),
            new Vector3(2.1f + WallCornerOverlap, WallHeight, WallThickness),
            _matExteriorWall);

        AddWall(
            varanda,
            "Wall_Varanda_CornerWest",
            new Vector3(-5.6f, WallCenterY, 6.6f),
            new Vector3(1.1f + WallCornerOverlap, WallHeight, WallThickness),
            _matExteriorWall);

        AddWall(
            varanda,
            "Wall_Varanda_CornerEast",
            new Vector3(5.6f, WallCenterY, 6.6f),
            new Vector3(1.1f + WallCornerOverlap, WallHeight, WallThickness),
            _matExteriorWall);
    }

    private void BuildReception()
    {
        var reception = new Node3D { Name = "ReceptionWalls" };
        _interior.AddChild(reception);

        AddWall(
            reception,
            "Wall_Reception_Left",
            new Vector3(-ReceptionHalfWidth - WallThickness * 0.5f, WallCenterY, -0.9f),
            new Vector3(WallThickness, WallHeight, 12.2f + WallCornerOverlap),
            _matInteriorWall);

        AddWall(
            reception,
            "Wall_Reception_Right",
            new Vector3(ReceptionHalfWidth + WallThickness * 0.5f, WallCenterY, -0.9f),
            new Vector3(WallThickness, WallHeight, 12.2f + WallCornerOverlap),
            _matInteriorWall);

        const float receptionNorthZ = -7.0f;
        const float northSegmentWidth = ReceptionHalfWidth - DoorWidth * 0.5f + WallCornerOverlap;

        AddWall(
            reception,
            "Wall_Reception_NorthLeft",
            new Vector3(-ReceptionHalfWidth + northSegmentWidth * 0.5f, WallCenterY, receptionNorthZ),
            new Vector3(northSegmentWidth, WallHeight, WallThickness),
            _matInteriorWall);

        AddWall(
            reception,
            "Wall_Reception_NorthRight",
            new Vector3(ReceptionHalfWidth - northSegmentWidth * 0.5f, WallCenterY, receptionNorthZ),
            new Vector3(northSegmentWidth, WallHeight, WallThickness),
            _matInteriorWall);

        const float receptionSouthZ = 1.2f;
        const float southSegmentWidth = ReceptionHalfWidth - DoorWidth * 0.5f + WallCornerOverlap;

        AddWall(
            reception,
            "Wall_Reception_SouthLeft",
            new Vector3(-ReceptionHalfWidth + southSegmentWidth * 0.5f, WallCenterY, receptionSouthZ),
            new Vector3(southSegmentWidth, WallHeight, WallThickness),
            _matInteriorWall);

        AddWall(
            reception,
            "Wall_Reception_SouthRight",
            new Vector3(ReceptionHalfWidth - southSegmentWidth * 0.5f, WallCenterY, receptionSouthZ),
            new Vector3(southSegmentWidth, WallHeight, WallThickness),
            _matInteriorWall);
    }

    private void BuildCorridor()
    {
        var corridor = new Node3D { Name = "CorridorWalls" };
        _interior.AddChild(corridor);

        const float centerZ = -17.5f;
        const float length = 18.0f;

        BuildWallWithTwoDoorGaps(
            corridor,
            "Wall_Corridor_Left",
            -CorridorWallX,
            centerZ,
            length,
            -15.5f,
            -25.5f,
            DoorWidth,
            _matInteriorWall);

        BuildWallWithDoorGap(
            corridor,
            "Wall_Corridor_Right",
            CorridorWallX,
            centerZ,
            length,
            -20.5f,
            DoorWidth,
            _matInteriorWall);

        var junctionDepth = 1.6f + WallCornerOverlap;
        var junctionCenterZ = -7.75f;
        var junctionWidth = ReceptionHalfWidth - CorridorWallX + WallCornerOverlap;

        AddWall(
            corridor,
            "Wall_Corridor_JunctionWest",
            new Vector3(-(CorridorWallX + ReceptionHalfWidth) * 0.5f, WallCenterY, junctionCenterZ),
            new Vector3(junctionWidth, WallHeight, junctionDepth),
            _matInteriorWall);

        AddWall(
            corridor,
            "Wall_Corridor_JunctionEast",
            new Vector3((CorridorWallX + ReceptionHalfWidth) * 0.5f, WallCenterY, junctionCenterZ),
            new Vector3(junctionWidth, WallHeight, junctionDepth),
            _matInteriorWall);
    }

    private void BuildRoom102()
    {
        var room = new Node3D { Name = "Room102Walls" };
        _interior.AddChild(room);

        const float roomCenterZ = -15.5f;
        const float roomDepth = 4.2f + WallCornerOverlap;
        const float roomWestX = -BuildingHalfWidth + WallThickness * 0.5f;
        const float roomInnerEastX = -CorridorWallX - WallThickness * 0.5f;
        const float roomSpanX = roomInnerEastX - roomWestX + WallCornerOverlap;
        const float roomSpanCenterX = (roomWestX + roomInnerEastX) * 0.5f;

        AddWall(
            room,
            "Wall_Room102_Left",
            new Vector3(roomWestX, WallCenterY, roomCenterZ),
            new Vector3(WallThickness, WallHeight, roomDepth),
            _matInteriorWall);

        AddWall(
            room,
            "Wall_Room102_Back",
            new Vector3(roomSpanCenterX, WallCenterY, -17.55f),
            new Vector3(roomSpanX, WallHeight, WallThickness),
            _matInteriorWall);

        AddWall(
            room,
            "Wall_Room102_Front",
            new Vector3(roomSpanCenterX, WallCenterY, -13.45f),
            new Vector3(roomSpanX, WallHeight, WallThickness),
            _matInteriorWall);

    }

    private void BuildKitchen()
    {
        var kitchen = new Node3D { Name = "KitchenWalls" };
        _interior.AddChild(kitchen);

        const float kitchenCenterZ = -20.5f;
        const float kitchenDepth = 4.2f + WallCornerOverlap;
        const float kitchenEastX = BuildingHalfWidth - WallThickness * 0.5f;
        const float kitchenInnerWestX = CorridorWallX + WallThickness * 0.5f;
        const float kitchenSpanX = kitchenEastX - kitchenInnerWestX + WallCornerOverlap;
        const float kitchenSpanCenterX = (kitchenEastX + kitchenInnerWestX) * 0.5f;

        AddWall(
            kitchen,
            "Wall_Kitchen_Right",
            new Vector3(kitchenEastX, WallCenterY, kitchenCenterZ),
            new Vector3(WallThickness, WallHeight, kitchenDepth),
            _matInteriorWall);

        AddWall(
            kitchen,
            "Wall_Kitchen_Back",
            new Vector3(kitchenSpanCenterX, WallCenterY, -22.55f),
            new Vector3(kitchenSpanX, WallHeight, WallThickness),
            _matInteriorWall);

        AddWall(
            kitchen,
            "Wall_Kitchen_Front",
            new Vector3(kitchenSpanCenterX, WallCenterY, -18.45f),
            new Vector3(kitchenSpanX, WallHeight, WallThickness),
            _matInteriorWall);
    }

    private void BuildStorage()
    {
        var storage = new Node3D { Name = "DepositArea" };
        _interior.AddChild(storage);

        const float depositDoorZ = -26.5f;
        const float depositBackZ = BuildingBackZ - WallThickness * 0.5f;
        const float depositDepth = depositDoorZ - depositBackZ + WallThickness + WallCornerOverlap;
        const float depositCenterZ = (depositDoorZ + depositBackZ) * 0.5f;
        const float buildingInnerX = BuildingHalfWidth - WallThickness * 0.5f;
        var alcoveSpanX = buildingInnerX - CorridorWallX + WallCornerOverlap;
        var alcoveCenterX = (CorridorWallX + buildingInnerX) * 0.5f;

        AddWall(
            storage,
            "Wall_Deposit_Back_Main",
            new Vector3(0, WallCenterY, depositBackZ),
            new Vector3(CorridorWidth + WallCornerOverlap, WallHeight, WallThickness),
            _matInteriorWall);

        AddWall(
            storage,
            "Wall_Deposit_AlcoveEast_Back",
            new Vector3(alcoveCenterX, WallCenterY, depositBackZ),
            new Vector3(alcoveSpanX, WallHeight, WallThickness),
            _matInteriorWall);

        AddWall(
            storage,
            "Wall_Deposit_Left",
            new Vector3(-CorridorWallX, WallCenterY, depositCenterZ),
            new Vector3(WallThickness, WallHeight, depositDepth),
            _matInteriorWall);

        AddWall(
            storage,
            "Wall_Deposit_Right",
            new Vector3(CorridorWallX, WallCenterY, depositCenterZ),
            new Vector3(WallThickness, WallHeight, depositDepth),
            _matInteriorWall);

        // West alcove converted to stairwell — no Wall_Deposit_AlcoveWest / SouthCapWest.

        AddWall(
            storage,
            "Wall_Deposit_AlcoveEast",
            new Vector3(alcoveCenterX, WallCenterY, depositCenterZ),
            new Vector3(alcoveSpanX, WallHeight, depositDepth),
            _matInteriorWall);

        AddWall(
            storage,
            "Wall_Deposit_AlcoveSouthCapEast",
            new Vector3(alcoveCenterX, WallCenterY, depositDoorZ),
            new Vector3(alcoveSpanX, WallHeight, WallThickness),
            _matInteriorWall);

        var frameWidth = (CorridorWidth - DoorWidth) * 0.5f + WallCornerOverlap * 0.5f;
        var frameCenterX = DoorWidth * 0.5f + frameWidth * 0.5f;

        AddWall(
            storage,
            "Wall_Deposit_DoorFrame_Left",
            new Vector3(-frameCenterX, WallCenterY, depositDoorZ),
            new Vector3(frameWidth, WallHeight, WallThickness),
            _matInteriorWall);

        AddWall(
            storage,
            "Wall_Deposit_DoorFrame_Right",
            new Vector3(frameCenterX, WallCenterY, depositDoorZ),
            new Vector3(frameWidth, WallHeight, WallThickness),
            _matInteriorWall);

        var headerHeight = WallHeight - DoorHeight;
        var headerCenterY = DoorHeight + headerHeight * 0.5f - WallEmbedBelowFloor;

        AddWall(
            storage,
            "Wall_Deposit_DoorHeader",
            new Vector3(0, headerCenterY, depositDoorZ),
            new Vector3(CorridorWidth + WallCornerOverlap, headerHeight, WallThickness),
            _matInteriorWall);

        AddBlockedDepositDoor(
            storage,
            new Vector3(0, DoorHeight * 0.5f - WallEmbedBelowFloor, depositDoorZ),
            new Vector3(DoorWidth, DoorHeight, 0.14f));

        AddVisualFloorPlate(
            storage,
            "Floor_Deposit_Interior",
            new Vector3(0, 0, depositCenterZ),
            new Vector2(CorridorWidth + 0.12f, depositDepth - 0.2f),
            InteriorFloorTopY,
            _matInteriorFloor);
    }

    private void BuildFurnitureCollisions()
    {
        var furniture = new Node3D { Name = "FurnitureCollisions" };
        _interior.AddChild(furniture);

        AddFurniture(
            furniture,
            "Furniture_Room102_Bed_Collision",
            new Vector3(-5.5f, 0.35f, -15.5f),
            new Vector3(2.0f, 0.7f, 1.8f),
            _matBed);

        AddFurniture(
            furniture,
            "Furniture_Reception_Counter_Collision",
            new Vector3(3.4f, 0.55f, -3.5f),
            new Vector3(2.4f, 1.1f, 0.6f),
            _matCounter);

        AddFurniture(
            furniture,
            "Furniture_Kitchen_Counter_Collision",
            new Vector3(3.4f, 0.55f, -19.7f),
            new Vector3(1.8f, 1.1f, 0.6f),
            _matCounter);
    }

    private void BuildDoorThresholds()
    {
        var thresholds = new Node3D { Name = "DoorThresholds" };
        _interior.AddChild(thresholds);

        var thresholdTop = InteriorFloorTopY + ThresholdLiftY;

        AddVisualFloorPlate(
            thresholds,
            "Floor_ReceptionToCorridor",
            new Vector3(0, 0, -7),
            new Vector2(DoorWidth + 0.2f, 0.32f),
            thresholdTop,
            _matInteriorWall);

        AddVisualFloorPlate(
            thresholds,
            "Floor_Room102_Door",
            new Vector3(-CorridorWallX, 0, -15.5f),
            new Vector2(0.32f, DoorWidth + 0.16f),
            thresholdTop,
            _matInteriorWall);

        AddVisualFloorPlate(
            thresholds,
            "Floor_Kitchen_Door",
            new Vector3(CorridorWallX, 0, -20.5f),
            new Vector2(0.32f, DoorWidth + 0.16f),
            thresholdTop,
            _matInteriorWall);

        AddVisualFloorPlate(
            thresholds,
            "Floor_DepositDoorThreshold",
            new Vector3(0, 0, -26.5f),
            new Vector2(DoorWidth + 0.2f, 0.36f),
            thresholdTop + 0.002f,
            _matDoor);
    }

    private void BuildStairIntegration()
    {
        const float alcoveCenterX = (CorridorWallX + (BuildingHalfWidth - WallThickness * 0.5f)) * 0.5f;
        const float stairFootX = -alcoveCenterX;
        const float stairFootZ = -30.5f;
        const float wellWidth = 5.6f;
        const float wellDepth = 7.8f;
        const float wellCenterZ = -27.4f;

        var stairWell = new Node3D { Name = "StairWell" };
        _interior.AddChild(stairWell);

        AddCollisionFloor(
            stairWell,
            "StairWell_Floor",
            new Vector3(stairFootX, FloorCenterY, wellCenterZ),
            new Vector3(wellWidth, FloorThickness, wellDepth));

        AddVisualFloorPlate(
            stairWell,
            "StairWell_FloorVisual",
            new Vector3(stairFootX, 0f, wellCenterZ),
            new Vector2(wellWidth, wellDepth),
            InteriorFloorTopY + 0.018f,
            _matCorridorFloor);

        var buildingInnerWestX = -BuildingHalfWidth + WallThickness * 0.5f;

        AddWall(
            stairWell,
            "StairWell_Wall_West",
            new Vector3(buildingInnerWestX, WallCenterY, wellCenterZ),
            new Vector3(WallThickness, WallHeight, wellDepth + WallCornerOverlap),
            _matInteriorWall);

        var footOrigin = new Node3D
        {
            Name = "StairAssembly",
            Position = new Vector3(stairFootX, 0f, stairFootZ)
        };
        stairWell.AddChild(footOrigin);

        var lowerFloor = new Node3D { Name = "StairLowerFloor" };
        var collisions = new Node3D { Name = "StairCollisions" };
        var visualSteps = new Node3D { Name = "StairVisualSteps" };
        var upperLanding = new Node3D { Name = "StairUpperLanding" };
        var sideGuides = new Node3D { Name = "StairSideGuides" };
        footOrigin.AddChild(lowerFloor);
        footOrigin.AddChild(collisions);
        footOrigin.AddChild(visualSteps);
        footOrigin.AddChild(upperLanding);
        footOrigin.AddChild(sideGuides);

        StairRampAssembly.Build(
            lowerFloor,
            collisions,
            visualSteps,
            upperLanding,
            sideGuides,
            _matStairStep,
            _matStairStringer,
            _matStairUpper,
            _matStairRail,
            upperLandingWidth: 5f,
            upperLandingDepth: 5f,
            collisionLayer: WorldLayer);

        AddVisualFloorPlate(
            stairWell,
            "StairWell_EntryThreshold",
            new Vector3(-CorridorWallX - 0.55f, 0f, -25.5f),
            new Vector2(DoorWidth + 0.16f, 0.32f),
            InteriorFloorTopY + ThresholdLiftY,
            _matInteriorWall);
    }

    private void BuildExteriorBoundaries()
    {
        AddBoundaryWall(_exterior, "BoundaryTrailEnd", new Vector3(0, 1.5f, 52), new Vector3(44, 3, WallThickness));
        AddBoundaryWall(_exterior, "BoundaryBuildingFar", new Vector3(0, 1.5f, -35), new Vector3(44, 3, WallThickness));
        AddBoundaryWall(_exterior, "BoundaryEast", new Vector3(22, 1.5f, 28), new Vector3(WallThickness, 3, 52));
        AddBoundaryWall(_exterior, "BoundaryWest", new Vector3(-22, 1.5f, 28), new Vector3(WallThickness, 3, 52));
    }

    private void BuildInteractions()
    {
        AddInteractableBody(
            _interactions,
            "JobOfferSign",
            new Vector3(1.15f, 1.25f, 37.5f),
            new Vector3(1.4f, 1.0f, 0.08f),
            "Ler placa",
            "OFERTA DE TRABALHO - MINERAÇÃO - PENSÃO SANTA LUZIA.",
            "job_offer_sign");

        AddInteractableArea(
            _interactions,
            "ReceptionBook",
            new Vector3(3.4f, 1.05f, -4.5f),
            new Vector3(0.75f, 0.35f, 0.55f),
            "Examinar livro",
            "Seu nome já está no registro.",
            "reception_book");

        AddInteractableArea(
            _interactions,
            "Room102Inspect",
            new Vector3(-3.4f, 1.4f, -15.5f),
            new Vector3(1.5f, 1.4f, 1.5f),
            "Examinar quarto",
            "Parece que este quarto foi preparado para mim.",
            "room_102");

        AddInteractableArea(
            _interactions,
            "KitchenInspect",
            new Vector3(3.4f, 1.4f, -20.5f),
            new Vector3(1.5f, 1.4f, 1.5f),
            "Examinar cozinha",
            "A cozinha está fria demais para uma pensão habitada.",
            "kitchen");
    }

    private void BuildWallWithTwoDoorGaps(
        Node3D parent,
        string baseName,
        float wallX,
        float centerZ,
        float totalLength,
        float door1CenterZ,
        float door2CenterZ,
        float doorWidth,
        StandardMaterial3D material)
    {
        var halfLength = totalLength * 0.5f;
        var southEnd = centerZ + halfLength;
        var northEnd = centerZ - halfLength;
        var doorHalf = doorWidth * 0.5f;

        var door1South = door1CenterZ + doorHalf;
        var door1North = door1CenterZ - doorHalf;
        var door2South = door2CenterZ + doorHalf;
        var door2North = door2CenterZ - doorHalf;

        var southSegmentLength = southEnd - door1South;
        if (southSegmentLength > 0.05f)
        {
            AddWall(
                parent,
                $"{baseName}South",
                new Vector3(wallX, WallCenterY, door1South + southSegmentLength * 0.5f),
                new Vector3(WallThickness, WallHeight, southSegmentLength + WallCornerOverlap),
                material);
        }

        var middleSegmentLength = door1North - door2South;
        if (middleSegmentLength > 0.05f)
        {
            AddWall(
                parent,
                $"{baseName}Middle",
                new Vector3(wallX, WallCenterY, door2South + middleSegmentLength * 0.5f),
                new Vector3(WallThickness, WallHeight, middleSegmentLength + WallCornerOverlap),
                material);
        }

        var northSegmentLength = door2North - northEnd;
        if (northSegmentLength > 0.05f)
        {
            AddWall(
                parent,
                $"{baseName}North",
                new Vector3(wallX, WallCenterY, door2North - northSegmentLength * 0.5f),
                new Vector3(WallThickness, WallHeight, northSegmentLength + WallCornerOverlap),
                material);
        }
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
                new Vector3(wallX, WallCenterY, doorSouth + southSegmentLength * 0.5f),
                new Vector3(WallThickness, WallHeight, southSegmentLength + WallCornerOverlap),
                material);
        }

        var northSegmentLength = doorNorth - northEnd;
        if (northSegmentLength > 0.05f)
        {
            AddWall(
                parent,
                $"{baseName}North",
                new Vector3(wallX, WallCenterY, doorNorth - northSegmentLength * 0.5f),
                new Vector3(WallThickness, WallHeight, northSegmentLength + WallCornerOverlap),
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

    private void AddWall(Node3D parent, string name, Vector3 center, Vector3 size, StandardMaterial3D material)
    {
        AddSolid(parent, name, center, size, material, WorldLayer);
    }

    private void AddBoundaryWall(Node3D parent, string name, Vector3 center, Vector3 size)
    {
        AddSolid(parent, name, center, size, _matExteriorWall, WorldLayer);
    }

    private void AddFurniture(Node3D parent, string name, Vector3 center, Vector3 size, StandardMaterial3D material)
    {
        AddSolid(parent, name, center, size, material, WorldLayer);
    }

    private void AddBlockedDepositDoor(Node3D parent, Vector3 center, Vector3 size)
    {
        var body = new StaticBody3D
        {
            Name = "Door_Deposit_Blocked",
            Position = center,
            CollisionLayer = WorldInteractableLayer,
            CollisionMask = 0
        };

        var collision = CreateBoxCollision(size);
        collision.Name = "Door_Deposit_Blocking_Collision";
        body.AddChild(collision);
        body.AddChild(CreateBoxMesh(size, _matDoor));
        parent.AddChild(body);
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

    private void AddVisualFloorPlate(
        Node3D parent,
        string name,
        Vector3 centerXZ,
        Vector2 sizeXZ,
        float topY,
        StandardMaterial3D material,
        float thickness = VisualFloorThickness)
    {
        var center = new Vector3(centerXZ.X, topY - thickness * 0.5f, centerXZ.Z);
        AddVisualOnly(parent, name, center, new Vector3(sizeXZ.X, thickness, sizeXZ.Y), material);
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
