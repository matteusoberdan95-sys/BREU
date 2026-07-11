namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Interaction;
using BREU.Scripts.Levels;

/// <summary>
/// Sprint 10 rebuild — ground floor (inherited) + clean proportional second floor blockout.
/// </summary>
public partial class PensaoVerticalBlockout01Builder : PensaoTerreoBlockout01Builder
{
    private const float SecondFloorTopY = 2.8f;
    private const float SecondFloorVisualTopY = 2.82f;

    private const float SlabWidth = BuildingHalfWidth * 2f + FloorOverlap;
    private const float SlabDepth = 44.5f + FloorOverlap;
    private const float SlabCenterZ = -10.75f;

    private const float StairFootZ = -30.5f;
    private const float StairOpenWidth = 6.0f;
    private const float StairOpenDepth = 8.5f;
    private const float StairOpenCenterZ = -27.4f;

    private const float UpperCorridorWidth = CorridorWidth;
    private const float UpperCorridorSouthZ = -7.5f;
    private const float UpperCorridorNorthZ = -20.0f;

    private const float Room201CenterZ = -14.0f;
    private const float Room202CenterZ = -17.0f;
    private const float RoomDepth = 4.0f + WallCornerOverlap;
    private const float BlockedDoorZ = -7.5f;

    private static float StairFootX =>
        -((CorridorWallX + (BuildingHalfWidth - WallThickness * 0.5f)) * 0.5f);

    private static float RampTopZ => StairFootZ + StairRampAssembly.StairRun;

    private static float StairOpenSouthZ => StairOpenCenterZ + StairOpenDepth * 0.5f;

    private static float StairOpenNorthZ => StairOpenCenterZ - StairOpenDepth * 0.5f;

    private static float StairOpenEastX => StairFootX + StairOpenWidth * 0.5f;

    private static float BuildingInnerWestX => -BuildingHalfWidth + WallThickness * 0.5f;

    private static float BuildingInnerEastX => BuildingHalfWidth - WallThickness * 0.5f;

    private static float Room201CenterX => (BuildingInnerWestX + (-CorridorWallX)) * 0.5f;

    private static float Room202CenterX => (CorridorWallX + BuildingInnerEastX) * 0.5f;

    private StandardMaterial3D _matSecondFloor = null!;
    private StandardMaterial3D _matSecondCeiling = null!;
    private StandardMaterial3D _matFurniture = null!;

    private Node3D _secondFloor = null!;

    protected override bool IncludeStairUpperLanding => false;

    protected override bool IncludeStairUpperBlockers => false;

    public override void _Ready()
    {
        _secondFloor = GetNodeOrNull<Node3D>("../../PensionSecondFloor")
            ?? throw new System.InvalidOperationException("PensionSecondFloor node missing.");
        base._Ready();
    }

    protected override void BuildExtensionContent()
    {
        _matSecondFloor = Mat(new Color(0.46f, 0.44f, 0.48f));
        _matSecondCeiling = Mat(new Color(0.42f, 0.4f, 0.44f));
        _matFurniture = Mat(new Color(0.38f, 0.34f, 0.32f));
        BuildSecondFloor();
    }

    protected override void BuildExtensionInteractions()
    {
        BuildSecondFloorInteractions();
    }

    private static float SecondFloorCenterY => SecondFloorTopY - FloorThickness * 0.5f;

    private static float SecondWallCenterY => SecondFloorTopY + WallCenterY;

    private void BuildSecondFloor()
    {
        BuildFloorSecondMain();
        BuildUpperLandingMain();
        BuildUpperCorridorMain();
        BuildRoom201();
        BuildRoom202();
        BuildUpperBlockedDoor();
        BuildSecondFloorExteriorShell();
    }

    /// <summary>
    /// Full proportional slab matching térreo footprint, with stairwell opening only.
    /// </summary>
    private void BuildFloorSecondMain()
    {
        const float southEdgeZ = -6.0f;
        var southDepth = southEdgeZ - StairOpenSouthZ;
        var southCenterZ = (southEdgeZ + StairOpenSouthZ) * 0.5f;

        AddSecondFloorSlab(
            "Floor_Second_Main_South",
            new Vector3(0f, SecondFloorCenterY, southCenterZ),
            new Vector3(SlabWidth, FloorThickness, southDepth + FloorOverlap));

        var northEastWidth = SlabWidth * 0.5f - StairOpenEastX;
        var northEastCenterX = StairOpenEastX + northEastWidth * 0.5f;

        AddSecondFloorSlab(
            "Floor_Second_Main_NorthEast",
            new Vector3(northEastCenterX, SecondFloorCenterY, StairOpenCenterZ),
            new Vector3(northEastWidth + FloorOverlap, FloorThickness, StairOpenDepth + FloorOverlap));

        AddSecondFloorSlab(
            "Floor_Second_Main_NorthCap",
            new Vector3(0f, SecondFloorCenterY, BuildingBackZ + 1.2f),
            new Vector3(SlabWidth, FloorThickness, 2.4f));
    }

    private void BuildUpperLandingMain()
    {
        const float landingWidth = 3.5f;
        const float landingDepth = 3.5f;
        const float landingCenterZ = -22.0f;

        AddSecondFloorSlab(
            "UpperLanding_Main",
            new Vector3(StairFootX, SecondFloorCenterY, landingCenterZ),
            new Vector3(landingWidth, FloorThickness, landingDepth));

        AddSecondFloorSlab(
            "UpperLanding_StairBridge",
            new Vector3(StairFootX, SecondFloorCenterY, RampTopZ + 0.85f),
            new Vector3(StairRampAssembly.StairWidth + 1.0f, FloorThickness, 2.2f));

        AddSecondFloorSlab(
            "UpperLanding_CorridorBridge",
            new Vector3((StairFootX + 0f) * 0.5f, SecondFloorCenterY, -20.5f),
            new Vector3(Mathf.Abs(StairFootX) + UpperCorridorWidth * 0.5f, FloorThickness, 2.0f));
    }

    private void BuildUpperCorridorMain()
    {
        var corridorLength = UpperCorridorSouthZ - UpperCorridorNorthZ;
        var corridorCenterZ = (UpperCorridorSouthZ + UpperCorridorNorthZ) * 0.5f;

        AddSecondFloorSlab(
            "UpperCorridor_Main",
            new Vector3(0f, SecondFloorCenterY, corridorCenterZ),
            new Vector3(UpperCorridorWidth + FloorWallLip * 2f, FloorThickness, corridorLength + FloorOverlap));

        BuildWallWithDoorGap(
            _secondFloor,
            "Wall_UpperCorridor_Left",
            -CorridorWallX,
            corridorCenterZ,
            corridorLength,
            Room201CenterZ,
            DoorWidth,
            _matInteriorWall,
            SecondWallCenterY);

        BuildWallWithDoorGap(
            _secondFloor,
            "Wall_UpperCorridor_Right",
            CorridorWallX,
            corridorCenterZ,
            corridorLength,
            Room202CenterZ,
            DoorWidth,
            _matInteriorWall,
            SecondWallCenterY);
    }

    private void BuildRoom201()
    {
        var roomSpanX = (-CorridorWallX) - BuildingInnerWestX + WallCornerOverlap;

        AddWall(
            _secondFloor,
            "Wall_Room201_Left",
            new Vector3(BuildingInnerWestX, SecondWallCenterY, Room201CenterZ),
            new Vector3(WallThickness, WallHeight, RoomDepth),
            _matInteriorWall);

        AddWall(
            _secondFloor,
            "Wall_Room201_Back",
            new Vector3(Room201CenterX, SecondWallCenterY, Room201CenterZ - RoomDepth * 0.5f),
            new Vector3(roomSpanX, WallHeight, WallThickness),
            _matInteriorWall);

        AddWall(
            _secondFloor,
            "Wall_Room201_Front",
            new Vector3(Room201CenterX, SecondWallCenterY, Room201CenterZ + RoomDepth * 0.5f),
            new Vector3(roomSpanX, WallHeight, WallThickness),
            _matInteriorWall);

        AddFurniture(
            _secondFloor,
            "Furniture_Room201_Bed",
            new Vector3(Room201CenterX + 0.5f, SecondFloorTopY + 0.35f, Room201CenterZ),
            new Vector3(2.0f, 0.7f, 1.8f),
            _matFurniture);
    }

    private void BuildRoom202()
    {
        var roomSpanX = BuildingInnerEastX - CorridorWallX + WallCornerOverlap;

        AddWall(
            _secondFloor,
            "Wall_Room202_Right",
            new Vector3(BuildingInnerEastX, SecondWallCenterY, Room202CenterZ),
            new Vector3(WallThickness, WallHeight, RoomDepth),
            _matInteriorWall);

        AddWall(
            _secondFloor,
            "Wall_Room202_Back",
            new Vector3(Room202CenterX, SecondWallCenterY, Room202CenterZ - RoomDepth * 0.5f),
            new Vector3(roomSpanX, WallHeight, WallThickness),
            _matInteriorWall);

        AddWall(
            _secondFloor,
            "Wall_Room202_Front",
            new Vector3(Room202CenterX, SecondWallCenterY, Room202CenterZ + RoomDepth * 0.5f),
            new Vector3(roomSpanX, WallHeight, WallThickness),
            _matInteriorWall);

        AddFurniture(
            _secondFloor,
            "Furniture_Room202_Cabinet",
            new Vector3(Room202CenterX - 0.6f, SecondFloorTopY + 0.75f, Room202CenterZ - 0.5f),
            new Vector3(1.0f, 1.5f, 0.55f),
            _matFurniture);
    }

    private void BuildUpperBlockedDoor()
    {
        var frameWidth = (UpperCorridorWidth - DoorWidth) * 0.5f + WallCornerOverlap * 0.5f;
        var frameCenterX = DoorWidth * 0.5f + frameWidth * 0.5f;

        AddWall(
            _secondFloor,
            "Wall_UpperBlockedDoor_Frame_Left",
            new Vector3(-frameCenterX, SecondWallCenterY, BlockedDoorZ),
            new Vector3(frameWidth, WallHeight, WallThickness),
            _matInteriorWall);

        AddWall(
            _secondFloor,
            "Wall_UpperBlockedDoor_Frame_Right",
            new Vector3(frameCenterX, SecondWallCenterY, BlockedDoorZ),
            new Vector3(frameWidth, WallHeight, WallThickness),
            _matInteriorWall);

        var headerHeight = WallHeight - DoorHeight;
        var headerCenterY = SecondFloorTopY + DoorHeight + headerHeight * 0.5f - WallEmbedBelowFloor;

        AddWall(
            _secondFloor,
            "Wall_UpperBlockedDoor_Header",
            new Vector3(0f, headerCenterY, BlockedDoorZ),
            new Vector3(UpperCorridorWidth + WallCornerOverlap, headerHeight, WallThickness),
            _matInteriorWall);
    }

    private void BuildSecondFloorExteriorShell()
    {
        var shellDepth = BuildingFrontZ - BuildingBackZ + WallCornerOverlap;
        var shellCenterZ = (BuildingFrontZ + BuildingBackZ) * 0.5f;
        var shellSpanX = BuildingHalfWidth * 2f + WallThickness + WallCornerOverlap;

        AddWall(
            _secondFloor,
            "Wall_Second_Back",
            new Vector3(0f, SecondWallCenterY, BuildingBackZ - WallThickness * 0.5f),
            new Vector3(shellSpanX, WallHeight, WallThickness),
            _matExteriorWall);

        AddWall(
            _secondFloor,
            "Wall_Second_Front",
            new Vector3(0f, SecondWallCenterY, -5.8f),
            new Vector3(shellSpanX, WallHeight, WallThickness),
            _matExteriorWall);

        AddWall(
            _secondFloor,
            "Wall_Second_Left",
            new Vector3(-BuildingHalfWidth - WallThickness * 0.5f, SecondWallCenterY, shellCenterZ),
            new Vector3(WallThickness, WallHeight, shellDepth),
            _matExteriorWall);

        AddWall(
            _secondFloor,
            "Wall_Second_Right",
            new Vector3(BuildingHalfWidth + WallThickness * 0.5f, SecondWallCenterY, shellCenterZ),
            new Vector3(WallThickness, WallHeight, shellDepth),
            _matExteriorWall);
    }

    private void BuildSecondFloorInteractions()
    {
        AddInteractableArea(
            _interactions,
            "Room201Inspect",
            new Vector3(Room201CenterX, SecondFloorTopY + 1.4f, Room201CenterZ),
            new Vector3(1.0f, 1.2f, 1.0f),
            "Examinar quarto 201",
            "O quarto está vazio, mas a cama parece ter sido usada recentemente.",
            "room_201");

        AddInteractableArea(
            _interactions,
            "Room202Inspect",
            new Vector3(Room202CenterX, SecondFloorTopY + 1.4f, Room202CenterZ),
            new Vector3(1.0f, 1.2f, 1.0f),
            "Examinar quarto 202",
            "Há marcas de arrasto no chão.",
            "room_202");

        AddInteractableBody(
            _interactions,
            "UpperBlockedDoor",
            new Vector3(0f, SecondFloorTopY + DoorHeight * 0.5f - WallEmbedBelowFloor, BlockedDoorZ),
            new Vector3(DoorWidth, DoorHeight, 0.14f),
            "Tentar abrir porta",
            "Está trancada por dentro.",
            "room_203_locked");
    }

    private void AddSecondFloorSlab(string name, Vector3 center, Vector3 size)
    {
        AddCollisionFloor(_secondFloor, name, center, size);
        AddVisualFloorPlate(
            _secondFloor,
            $"{name}_Visual",
            new Vector3(center.X, 0f, center.Z),
            new Vector2(size.X, size.Z),
            SecondFloorVisualTopY,
            name.Contains("Main") || name.Contains("Landing") || name.Contains("Corridor")
                ? _matSecondCeiling
                : _matSecondFloor);
    }

    private void BuildWallWithDoorGap(
        Node3D parent,
        string baseName,
        float wallX,
        float centerZ,
        float totalLength,
        float doorCenterZ,
        float doorWidth,
        StandardMaterial3D material,
        float wallCenterY)
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
                new Vector3(wallX, wallCenterY, doorSouth + southSegmentLength * 0.5f),
                new Vector3(WallThickness, WallHeight, southSegmentLength + WallCornerOverlap),
                material);
        }

        var northSegmentLength = doorNorth - northEnd;
        if (northSegmentLength > 0.05f)
        {
            AddWall(
                parent,
                $"{baseName}North",
                new Vector3(wallX, wallCenterY, doorNorth - northSegmentLength * 0.5f),
                new Vector3(WallThickness, WallHeight, northSegmentLength + WallCornerOverlap),
                material);
        }
    }

    private static StandardMaterial3D Mat(Color color) =>
        new() { AlbedoColor = color };
}
