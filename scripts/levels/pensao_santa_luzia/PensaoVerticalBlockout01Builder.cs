namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Interaction;
using BREU.Scripts.Levels;

/// <summary>
/// Sprint 10 — ground floor (inherited) + second floor blockout on PensaoVerticalBlockout01.
/// </summary>
public partial class PensaoVerticalBlockout01Builder : PensaoTerreoBlockout01Builder
{
    private const float SecondFloorTopY = 2.8f;
    private const float SecondFloorVisualTopY = 2.82f;
    private const float UpperCorridorWidth = 2.4f;

    private const float StairFootX = -4.1f;
    private const float StairFootZ = -30.5f;
    private static float RampTopZ => StairFootZ + StairRampAssembly.StairRun;

    private StandardMaterial3D _matSecondFloor = null!;
    private StandardMaterial3D _matSecondRail = null!;
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
        _matSecondRail = Mat(new Color(0.36f, 0.38f, 0.42f));
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
        const float landingDepth = 3.5f;
        const float landingWidth = 3.5f;
        const float landingCenterZ = -21.0f;
        const float corridorEndZ = -7.5f;
        const float room201CenterZ = -14.0f;
        const float room202CenterZ = -17.0f;
        const float roomDepth = 4.0f + WallCornerOverlap;
        const float roomSpan = 3.8f + WallCornerOverlap;
        const float buildingInnerWestX = -BuildingHalfWidth + WallThickness * 0.5f;
        const float upperEastCapX = -1.0f;
        const float room201CenterX = -6.15f;
        const float room202CenterX = -1.55f;
        const float blockedDoorZ = -7.5f;

        var corridorStartZ = landingCenterZ + landingDepth * 0.5f - FloorOverlap;
        var corridorLength = corridorEndZ - corridorStartZ;
        var corridorCenterZ = corridorStartZ + corridorLength * 0.5f;

        BuildUpperLandingMain(landingCenterZ, landingWidth, landingDepth);
        BuildStairTopTransition();
        BuildUpperCorridorMain(corridorCenterZ, corridorLength);
        BuildUpperLandingRails(landingCenterZ, landingDepth);
        BuildUpperCorridorWalls(corridorCenterZ, corridorLength, room201CenterZ, room202CenterZ);
        BuildRoom201(room201CenterX, room201CenterZ, roomDepth, roomSpan, buildingInnerWestX);
        BuildRoom202(room202CenterX, room202CenterZ, roomDepth, roomSpan, upperEastCapX);
        BuildBlockedUpperDoor(blockedDoorZ);
        BuildSecondFloorShell(corridorCenterZ, corridorLength, buildingInnerWestX, upperEastCapX);
    }

    private void BuildUpperLandingMain(float centerZ, float width, float depth)
    {
        AddCollisionFloor(
            _secondFloor,
            "UpperLanding_Main",
            new Vector3(StairFootX, SecondFloorCenterY, centerZ),
            new Vector3(width, FloorThickness, depth));

        AddVisualFloorPlate(
            _secondFloor,
            "UpperLanding_Main_Visual",
            new Vector3(StairFootX, 0f, centerZ),
            new Vector2(width, depth),
            SecondFloorVisualTopY,
            _matSecondFloor);
    }

    private void BuildStairTopTransition()
    {
        AddCollisionFloor(
            _secondFloor,
            "Floor_Second_StairTransition",
            new Vector3(StairFootX, SecondFloorCenterY, RampTopZ + 1.25f),
            new Vector3(StairRampAssembly.StairWidth + 1.2f, FloorThickness, 3.2f));

        AddVisualFloorPlate(
            _secondFloor,
            "Floor_Second_StairTransition_Visual",
            new Vector3(StairFootX, 0f, RampTopZ + 1.25f),
            new Vector2(StairRampAssembly.StairWidth + 1.2f, 3.2f),
            SecondFloorVisualTopY,
            _matSecondFloor);
    }

    private void BuildUpperCorridorMain(float corridorCenterZ, float corridorLength)
    {
        AddCollisionFloor(
            _secondFloor,
            "UpperCorridor_Main",
            new Vector3(StairFootX, SecondFloorCenterY, corridorCenterZ),
            new Vector3(UpperCorridorWidth + FloorWallLip * 2f, FloorThickness, corridorLength + FloorOverlap));

        AddVisualFloorPlate(
            _secondFloor,
            "UpperCorridor_Main_Visual",
            new Vector3(StairFootX, 0f, corridorCenterZ),
            new Vector2(UpperCorridorWidth, corridorLength),
            SecondFloorVisualTopY,
            _matSecondFloor);

        AddCollisionFloor(
            _secondFloor,
            "Floor_Second_Main",
            new Vector3(StairFootX, SecondFloorCenterY, -14.5f),
            new Vector3(10.5f, FloorThickness, 16.0f));

        AddVisualFloorPlate(
            _secondFloor,
            "Floor_Second_Main_Visual",
            new Vector3(StairFootX, 0f, -14.5f),
            new Vector2(10.5f, 16.0f),
            SecondFloorVisualTopY,
            _matSecondFloor);
    }

    private void BuildUpperLandingRails(float landingCenterZ, float landingDepth)
    {
        const float railHeight = 1.1f;
        const float railThickness = 0.18f;
        const float upperWestWallX = -5.4f;
        const float upperEastWallX = -2.8f;
        var railDepth = landingDepth * 0.55f;
        var railCenterZ = landingCenterZ - landingDepth * 0.15f;

        AddWall(
            _secondFloor,
            "UpperLanding_Rail_Left",
            new Vector3(upperWestWallX - railThickness * 0.5f, SecondFloorTopY + railHeight * 0.5f, railCenterZ),
            new Vector3(railThickness, railHeight, railDepth),
            _matSecondRail);

        AddWall(
            _secondFloor,
            "UpperLanding_Rail_Right",
            new Vector3(upperEastWallX + railThickness * 0.5f, SecondFloorTopY + railHeight * 0.5f, railCenterZ),
            new Vector3(railThickness, railHeight, railDepth),
            _matSecondRail);
    }

    private void BuildUpperCorridorWalls(
        float corridorCenterZ,
        float corridorLength,
        float room201CenterZ,
        float room202CenterZ)
    {
        const float upperWestWallX = -5.4f;
        const float upperEastWallX = -2.8f;

        BuildWallWithDoorGap(
            _secondFloor,
            "Wall_Second_Corridor_Left",
            upperWestWallX,
            corridorCenterZ,
            corridorLength,
            room201CenterZ,
            DoorWidth,
            _matInteriorWall,
            SecondWallCenterY);

        BuildWallWithDoorGap(
            _secondFloor,
            "Wall_Second_Corridor_Right",
            upperEastWallX,
            corridorCenterZ,
            corridorLength,
            room202CenterZ,
            DoorWidth,
            _matInteriorWall,
            SecondWallCenterY);
    }

    private void BuildRoom201(
        float roomCenterX,
        float roomCenterZ,
        float roomDepth,
        float roomSpan,
        float buildingInnerWestX)
    {
        AddWall(
            _secondFloor,
            "Wall_Room201_Left",
            new Vector3(buildingInnerWestX, SecondWallCenterY, roomCenterZ),
            new Vector3(WallThickness, WallHeight, roomDepth),
            _matInteriorWall);

        AddWall(
            _secondFloor,
            "Wall_Room201_Back",
            new Vector3(roomCenterX, SecondWallCenterY, roomCenterZ - roomDepth * 0.5f),
            new Vector3(roomSpan, WallHeight, WallThickness),
            _matInteriorWall);

        AddWall(
            _secondFloor,
            "Wall_Room201_Front",
            new Vector3(roomCenterX, SecondWallCenterY, roomCenterZ + roomDepth * 0.5f),
            new Vector3(roomSpan, WallHeight, WallThickness),
            _matInteriorWall);

        AddFurniture(
            _secondFloor,
            "Furniture_Room201_Bed",
            new Vector3(roomCenterX + 0.6f, SecondFloorTopY + 0.35f, roomCenterZ),
            new Vector3(2.0f, 0.7f, 1.8f),
            _matFurniture);
    }

    private void BuildRoom202(
        float roomCenterX,
        float roomCenterZ,
        float roomDepth,
        float roomSpan,
        float roomEastWallX)
    {
        AddWall(
            _secondFloor,
            "Wall_Room202_Right",
            new Vector3(roomEastWallX, SecondWallCenterY, roomCenterZ),
            new Vector3(WallThickness, WallHeight, roomDepth),
            _matInteriorWall);

        AddWall(
            _secondFloor,
            "Wall_Room202_Back",
            new Vector3(roomCenterX, SecondWallCenterY, roomCenterZ - roomDepth * 0.5f),
            new Vector3(roomSpan, WallHeight, WallThickness),
            _matInteriorWall);

        AddWall(
            _secondFloor,
            "Wall_Room202_Front",
            new Vector3(roomCenterX, SecondWallCenterY, roomCenterZ + roomDepth * 0.5f),
            new Vector3(roomSpan, WallHeight, WallThickness),
            _matInteriorWall);

        AddFurniture(
            _secondFloor,
            "Furniture_Room202_Cabinet",
            new Vector3(roomCenterX + 0.8f, SecondFloorTopY + 0.75f, roomCenterZ - 0.6f),
            new Vector3(1.0f, 1.5f, 0.55f),
            _matFurniture);
    }

    private void BuildBlockedUpperDoor(float blockedDoorZ)
    {
        var frameWidth = (UpperCorridorWidth - DoorWidth) * 0.5f + WallCornerOverlap * 0.5f;
        var frameCenterX = DoorWidth * 0.5f + frameWidth * 0.5f;

        AddWall(
            _secondFloor,
            "Wall_UpperBlockedDoor_Frame_Left",
            new Vector3(StairFootX - frameCenterX, SecondWallCenterY, blockedDoorZ),
            new Vector3(frameWidth, WallHeight, WallThickness),
            _matInteriorWall);

        AddWall(
            _secondFloor,
            "Wall_UpperBlockedDoor_Frame_Right",
            new Vector3(StairFootX + frameCenterX, SecondWallCenterY, blockedDoorZ),
            new Vector3(frameWidth, WallHeight, WallThickness),
            _matInteriorWall);

        var headerHeight = WallHeight - DoorHeight;
        var headerCenterY = SecondFloorTopY + DoorHeight + headerHeight * 0.5f - WallEmbedBelowFloor;

        AddWall(
            _secondFloor,
            "Wall_UpperBlockedDoor_Header",
            new Vector3(StairFootX, headerCenterY, blockedDoorZ),
            new Vector3(UpperCorridorWidth + WallCornerOverlap, headerHeight, WallThickness),
            _matInteriorWall);
    }

    private void BuildSecondFloorShell(
        float corridorCenterZ,
        float corridorLength,
        float buildingInnerWestX,
        float buildingInnerEastX)
    {
        const float northBackZ = -27.5f;

        AddWall(
            _secondFloor,
            "Wall_Second_Back",
            new Vector3(StairFootX, SecondWallCenterY, northBackZ),
            new Vector3(10.0f, WallHeight, WallThickness),
            _matInteriorWall);

        AddWall(
            _secondFloor,
            "Wall_Second_Front",
            new Vector3(StairFootX, SecondWallCenterY, -6.2f),
            new Vector3(10.0f, WallHeight, WallThickness),
            _matInteriorWall);

        AddWall(
            _secondFloor,
            "Wall_Second_West",
            new Vector3(buildingInnerWestX, SecondWallCenterY, corridorCenterZ),
            new Vector3(WallThickness, WallHeight, corridorLength + 8.0f),
            _matExteriorWall);

        AddWall(
            _secondFloor,
            "Wall_Second_East",
            new Vector3(buildingInnerEastX, SecondWallCenterY, corridorCenterZ),
            new Vector3(WallThickness, WallHeight, corridorLength + 8.0f),
            _matExteriorWall);
    }

    private void BuildSecondFloorInteractions()
    {
        const float room201CenterX = -6.15f;
        const float room202CenterX = -1.55f;

        AddInteractableArea(
            _interactions,
            "Room201Inspect",
            new Vector3(room201CenterX, SecondFloorTopY + 1.4f, -14.0f),
            new Vector3(1.0f, 1.2f, 1.0f),
            "Examinar quarto 201",
            "O quarto está vazio, mas a cama parece ter sido usada recentemente.",
            "room_201");

        AddInteractableArea(
            _interactions,
            "Room202Inspect",
            new Vector3(room202CenterX, SecondFloorTopY + 1.4f, -17.0f),
            new Vector3(1.0f, 1.2f, 1.0f),
            "Examinar quarto 202",
            "Há marcas de arrasto no chão.",
            "room_202");

        AddInteractableBody(
            _interactions,
            "UpperBlockedDoor",
            new Vector3(StairFootX, SecondFloorTopY + DoorHeight * 0.5f - WallEmbedBelowFloor, -7.5f),
            new Vector3(DoorWidth, DoorHeight, 0.14f),
            "Tentar abrir porta",
            "Está trancada por dentro.",
            "room_203_locked");
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
