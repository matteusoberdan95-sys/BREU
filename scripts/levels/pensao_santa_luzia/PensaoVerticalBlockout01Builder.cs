namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Interaction;
using BREU.Scripts.Levels;

/// <summary>
/// Sprint 10 — ground floor (inherited) + proportional second floor blockout.
/// Sprint 12 — ceiling and roof blockout.
/// </summary>
public partial class PensaoVerticalBlockout01Builder : PensaoTerreoBlockout01Builder
{
    private const float SecondFloorTopY = 2.8f;
    private const float SecondFloorVisualTopY = 2.82f;

    private const float SlabWidth = BuildingHalfWidth * 2f + FloorOverlap;

    private const float StairFootZ = -30.5f;
    private const float StairOpenWidth = 5.8f;
    private const float StairOpenDepth = 8.2f;
    private const float StairOpenCenterZ = -27.4f;

    private const float UpperCorridorWidth = CorridorWidth;
    private const float UpperCorridorSouthZ = -7.5f;
    private const float UpperCorridorNorthZ = -19.5f;

    private const float LandingCenterZ = -21.0f;
    private const float LandingDepth = 3.2f;
    private const float LandingWidth = 5.4f;

    private const float Room201CenterZ = -14.0f;
    private const float Room202CenterZ = -17.0f;
    private const float RoomDepth = 4.0f + WallCornerOverlap;
    private const float BlockedDoorZ = -7.5f;

    private const float CeilingThickness = 0.18f;
    private const float SecondSlabSouthZ = -5.8f;
    private const float RoofThickness = 0.28f;
    private const float RoofRidgeLiftY = 0.42f;

    private static float FirstFloorCeilingUndersideY => WallHeight - WallEmbedBelowFloor;

    private static float SecondFloorCeilingUndersideY => SecondFloorTopY + WallHeight;

    private static float StairFootX =>
        -((CorridorWallX + (BuildingHalfWidth - WallThickness * 0.5f)) * 0.5f);

    private static float RampTopZ => StairFootZ + StairRampAssembly.StairRun;

    private static float StairOpenSouthZ => StairOpenCenterZ + StairOpenDepth * 0.5f;

    private static float StairOpenNorthZ => StairOpenCenterZ - StairOpenDepth * 0.5f;

    private static float StairOpenEastX => StairFootX + StairOpenWidth * 0.5f;

    private static float StairOpenWestX => StairFootX - StairOpenWidth * 0.5f;

    private static float BuildingInnerWestX => -BuildingHalfWidth + WallThickness * 0.5f;

    private static float BuildingInnerEastX => BuildingHalfWidth - WallThickness * 0.5f;

    private static float Room201CenterX => (BuildingInnerWestX + (-CorridorWallX)) * 0.5f;

    private static float Room202CenterX => (CorridorWallX + BuildingInnerEastX) * 0.5f;

    private static float LandingCenterX => (StairFootX + 0f) * 0.5f;

    private StandardMaterial3D _matSecondFloor = null!;
    private StandardMaterial3D _matSecondCeiling = null!;
    private StandardMaterial3D _matSecondRail = null!;
    private StandardMaterial3D _matFurniture = null!;
    private StandardMaterial3D _matCeilingFirst = null!;
    private StandardMaterial3D _matCeilingSecond = null!;
    private StandardMaterial3D _matRoof = null!;

    private Node3D _secondFloor = null!;
    private Node3D _ceiling = null!;

    protected override bool IncludeStairUpperLanding => false;

    protected override bool IncludeStairUpperBlockers => false;

    public override void _Ready()
    {
        _secondFloor = GetNodeOrNull<Node3D>("../../PensionSecondFloor")
            ?? throw new System.InvalidOperationException("PensionSecondFloor node missing.");
        _ceiling = GetNodeOrNull<Node3D>("../../PensionCeiling")
            ?? throw new System.InvalidOperationException("PensionCeiling node missing.");
        base._Ready();
    }

    protected override void BuildExtensionContent()
    {
        _matSecondFloor = Mat(new Color(0.46f, 0.44f, 0.48f));
        _matSecondCeiling = Mat(new Color(0.42f, 0.4f, 0.44f));
        _matSecondRail = Mat(new Color(0.36f, 0.38f, 0.42f));
        _matFurniture = Mat(new Color(0.38f, 0.34f, 0.32f));
        _matCeilingFirst = Mat(new Color(0.34f, 0.31f, 0.28f));
        _matCeilingSecond = Mat(new Color(0.38f, 0.36f, 0.4f));
        _matRoof = Mat(new Color(0.32f, 0.3f, 0.28f));
        BuildSecondFloor();
        BuildCeilingBlockout();
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
        BuildUpperLandingAndCorridor();
        BuildRoom201();
        BuildRoom202();
        BuildUpperBlockedDoor();
        BuildStairwellBox();
        BuildSecondFloorSouthSealing();
        BuildSecondFloorExteriorShell();
    }

    private void BuildCeilingBlockout()
    {
        BuildFirstFloorCeilings();
        BuildSecondFloorCeilings();
        BuildStairwellCeiling();
        BuildRoofBlockout();
    }

    private void BuildFirstFloorCeilings()
    {
        var frontDepth = BuildingFrontZ - SecondSlabSouthZ + FloorOverlap;
        var frontCenterZ = (BuildingFrontZ + SecondSlabSouthZ) * 0.5f;

        AddVisualCeilingPlate(
            _ceiling,
            "Ceiling_FirstFloor_Main",
            new Vector3(0f, FirstFloorCeilingUndersideY, frontCenterZ),
            new Vector3(SlabWidth, CeilingThickness, frontDepth),
            _matCeilingFirst);
    }

    private void BuildSecondFloorCeilings()
    {
        var corridorDepth = SecondSlabSouthZ - UpperCorridorNorthZ + FloorOverlap;
        var corridorCenterZ = (SecondSlabSouthZ + UpperCorridorNorthZ) * 0.5f;

        AddVisualCeilingPlate(
            _ceiling,
            "Ceiling_SecondFloor_Main",
            new Vector3(0f, SecondFloorCeilingUndersideY, corridorCenterZ),
            new Vector3(SlabWidth, CeilingThickness, corridorDepth),
            _matCeilingSecond);

        var eastBandWidth = SlabWidth * 0.5f - StairOpenEastX;
        var eastBandCenterX = StairOpenEastX + eastBandWidth * 0.5f;
        var eastBandDepth = StairOpenSouthZ - UpperCorridorNorthZ + FloorOverlap;
        var eastBandCenterZ = (StairOpenSouthZ + UpperCorridorNorthZ) * 0.5f;

        AddVisualCeilingPlate(
            _ceiling,
            "Ceiling_SecondFloor_StairEastBand",
            new Vector3(eastBandCenterX, SecondFloorCeilingUndersideY, eastBandCenterZ),
            new Vector3(eastBandWidth + FloorOverlap, CeilingThickness, eastBandDepth),
            _matCeilingSecond);

        var northEastWidth = eastBandWidth;
        var northEastCenterX = eastBandCenterX;
        var northEastDepth = SecondSlabSouthZ - StairOpenNorthZ + FloorOverlap;
        var northEastCenterZ = (SecondSlabSouthZ + StairOpenNorthZ) * 0.5f;

        AddVisualCeilingPlate(
            _ceiling,
            "Ceiling_SecondFloor_Main_NorthEast",
            new Vector3(northEastCenterX, SecondFloorCeilingUndersideY, northEastCenterZ),
            new Vector3(northEastWidth + FloorOverlap, CeilingThickness, northEastDepth),
            _matCeilingSecond);

        AddVisualCeilingPlate(
            _ceiling,
            "Ceiling_SecondFloor_Main_NorthCap",
            new Vector3(0f, SecondFloorCeilingUndersideY, BuildingBackZ + 1.2f),
            new Vector3(SlabWidth, CeilingThickness, 2.4f),
            _matCeilingSecond);
    }

    private void BuildStairwellCeiling()
    {
        var westCapWidth = SlabWidth * 0.5f + StairOpenWestX;
        if (westCapWidth <= 0.05f)
        {
            return;
        }

        var westCapCenterX = -SlabWidth * 0.5f + westCapWidth * 0.5f;
        var northDepth = StairOpenNorthZ - BuildingBackZ + 2.6f;
        var northCenterZ = (StairOpenNorthZ + BuildingBackZ) * 0.5f + 0.3f;

        AddVisualCeilingPlate(
            _ceiling,
            "Ceiling_StairBox_WestCap",
            new Vector3(westCapCenterX, SecondFloorCeilingUndersideY, northCenterZ),
            new Vector3(westCapWidth, CeilingThickness, northDepth),
            _matCeilingSecond);
    }

    private void BuildRoofBlockout()
    {
        var exterior = GetNode<Node3D>("../../Exterior");
        var roofSpanZ = BuildingFrontZ - BuildingBackZ + 1.2f;
        var roofCenterZ = (BuildingFrontZ + BuildingBackZ) * 0.5f;
        var roofUndersideY = SecondFloorCeilingUndersideY + CeilingThickness + 0.08f;
        var roofWidth = SlabWidth + WallThickness * 2f + 0.4f;

        AddVisualCeilingPlate(
            exterior,
            "Roof_Blockout_Main",
            new Vector3(0f, roofUndersideY + RoofThickness * 0.5f, roofCenterZ),
            new Vector3(roofWidth, RoofThickness, roofSpanZ),
            _matRoof);

        const float upperFrontZ = -5.8f;
        var ridgeCenterZ = (upperFrontZ + BuildingBackZ) * 0.5f;
        AddVisualCeilingPlate(
            exterior,
            "Roof_Blockout_Ridge",
            new Vector3(0f, roofUndersideY + RoofThickness + RoofRidgeLiftY * 0.5f, ridgeCenterZ),
            new Vector3(roofWidth * 0.92f, RoofRidgeLiftY, (upperFrontZ - BuildingBackZ) + 0.8f),
            _matRoof);
    }

    private void AddVisualCeilingPlate(
        Node3D parent,
        string name,
        Vector3 center,
        Vector3 size,
        StandardMaterial3D material)
    {
        var visual = new Node3D { Name = name, Position = center };
        visual.AddChild(new MeshInstance3D
        {
            Mesh = new BoxMesh { Size = size },
            MaterialOverride = material
        });
        parent.AddChild(visual);
    }

    private void BuildFloorSecondMain()
    {
        const float southEdgeZ = -5.8f;
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

        const float northCapCenterZ = -31.4f;
        const float northCapDepth = 2.4f;
        AddSecondFloorSlab(
            "Floor_Second_Main_NorthCap",
            new Vector3(0f, SecondFloorCenterY, northCapCenterZ),
            new Vector3(SlabWidth, FloorThickness, northCapDepth));

        var northBridgeSouthZ = northCapCenterZ + northCapDepth * 0.5f;
        var northBridgeDepth = StairOpenNorthZ - northBridgeSouthZ + FloorOverlap;
        if (northBridgeDepth > 0.05f)
        {
            AddSecondFloorSlab(
                "Floor_Second_Main_NorthBridge",
                new Vector3(northEastCenterX, SecondFloorCenterY, northBridgeSouthZ + northBridgeDepth * 0.5f),
                new Vector3(northEastWidth + FloorOverlap, FloorThickness, northBridgeDepth));
        }

        const float edgeStripWidth = 0.35f;
        var westEdgeCenterX = BuildingInnerWestX + edgeStripWidth * 0.5f;
        AddSecondFloorSlab(
            "Floor_Second_Main_WestEdge",
            new Vector3(westEdgeCenterX, SecondFloorCenterY, southCenterZ),
            new Vector3(edgeStripWidth, FloorThickness, southDepth + FloorOverlap));

        AddSecondFloorSlab(
            "Floor_Second_Main_EastEdge",
            new Vector3(BuildingInnerEastX - edgeStripWidth * 0.5f, SecondFloorCenterY, southCenterZ),
            new Vector3(edgeStripWidth, FloorThickness, southDepth + FloorOverlap));

        AddSecondFloorSlab(
            "Floor_Second_Main_NorthWestCap",
            new Vector3(westEdgeCenterX, SecondFloorCenterY, northCapCenterZ),
            new Vector3(edgeStripWidth, FloorThickness, northCapDepth + FloorOverlap));
    }

    private void BuildUpperLandingAndCorridor()
    {
        AddSecondFloorSlab(
            "UpperLanding_StairBridge",
            new Vector3(StairFootX, SecondFloorCenterY, RampTopZ + 0.9f),
            new Vector3(StairRampAssembly.StairWidth + 1.2f, FloorThickness, 2.4f));

        AddSecondFloorSlab(
            "UpperLanding_Main",
            new Vector3(LandingCenterX, SecondFloorCenterY, LandingCenterZ),
            new Vector3(LandingWidth, FloorThickness, LandingDepth));

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

    /// <summary>
    /// Full-height stair shaft sides + low guardrails on the south opening only.
    /// </summary>
    private void BuildStairwellBox()
    {
        const float railHeight = 1.05f;
        const float railThickness = 0.18f;
        const float exitClearHalfWidth = StairRampAssembly.StairWidth * 0.5f + 0.55f;

        var boxEastX = StairOpenEastX + WallThickness * 0.5f;
        var railCenterY = SecondFloorTopY + railHeight * 0.5f;
        var southGuardZ = StairOpenSouthZ + railThickness * 0.5f;

        AddWall(
            _secondFloor,
            "StairBox_Wall_West",
            new Vector3(BuildingInnerWestX, SecondWallCenterY, StairOpenCenterZ),
            new Vector3(WallThickness, WallHeight, StairOpenDepth + WallCornerOverlap),
            _matInteriorWall);

        var eastFullDepth = StairOpenDepth * 0.58f;
        var eastFullCenterZ = StairOpenNorthZ + eastFullDepth * 0.5f;
        AddWall(
            _secondFloor,
            "StairBox_Wall_East",
            new Vector3(boxEastX, SecondWallCenterY, eastFullCenterZ),
            new Vector3(WallThickness, WallHeight, eastFullDepth + WallCornerOverlap),
            _matInteriorWall);

        AddWall(
            _secondFloor,
            "StairBox_Wall_North",
            new Vector3(StairFootX, SecondWallCenterY, StairOpenNorthZ - WallThickness * 0.5f),
            new Vector3(StairOpenWidth + WallThickness, WallHeight, WallThickness),
            _matInteriorWall);

        AddWall(
            _secondFloor,
            "Stairwell_Rail_Left",
            new Vector3(StairOpenWestX - railThickness * 0.5f, railCenterY, StairOpenCenterZ),
            new Vector3(railThickness, railHeight, StairOpenDepth + WallCornerOverlap),
            _matSecondRail);

        var eastRailDepth = StairOpenDepth * 0.42f;
        var eastRailCenterZ = StairOpenNorthZ + eastRailDepth * 0.5f;
        AddWall(
            _secondFloor,
            "Stairwell_Rail_Right",
            new Vector3(boxEastX + railThickness * 0.5f, railCenterY, eastRailCenterZ),
            new Vector3(railThickness, railHeight, eastRailDepth + WallCornerOverlap),
            _matSecondRail);

        AddWall(
            _secondFloor,
            "Stairwell_Rail_Front_Side_West",
            new Vector3(StairFootX - exitClearHalfWidth - 0.55f, railCenterY, southGuardZ),
            new Vector3(1.0f, railHeight, railThickness),
            _matSecondRail);

        AddWall(
            _secondFloor,
            "Stairwell_Rail_Front_Side_East",
            new Vector3(StairFootX + exitClearHalfWidth + 0.55f, railCenterY, southGuardZ),
            new Vector3(1.0f, railHeight, railThickness),
            _matSecondRail);

        var eastWingWallX = CorridorWallX + (BuildingInnerEastX - CorridorWallX) * 0.5f;
        var eastWingLength = UpperCorridorNorthZ - StairOpenSouthZ + WallCornerOverlap;
        AddWall(
            _secondFloor,
            "StairBox_Wall_EastWing",
            new Vector3(eastWingWallX, SecondWallCenterY, (UpperCorridorNorthZ + StairOpenSouthZ) * 0.5f),
            new Vector3(WallThickness, WallHeight, eastWingLength),
            _matInteriorWall);
    }

    /// <summary>
    /// South flank only — no north cap across corridor (hotfix 3: removed CorridorNorthCap).
    /// </summary>
    private void BuildSecondFloorSouthSealing()
    {
        const float upperFrontZ = -5.8f;
        var flankDepth = upperFrontZ - BlockedDoorZ + WallCornerOverlap;
        var flankCenterZ = (upperFrontZ + BlockedDoorZ) * 0.5f;
        var flankSpanX = BuildingHalfWidth - CorridorWallX - WallThickness * 0.5f;

        AddWall(
            _secondFloor,
            "Wall_Second_SouthFlank_West",
            new Vector3(-(CorridorWallX + flankSpanX * 0.5f), SecondWallCenterY, flankCenterZ),
            new Vector3(flankSpanX, WallHeight, flankDepth),
            _matInteriorWall);

        AddWall(
            _secondFloor,
            "Wall_Second_SouthFlank_East",
            new Vector3(CorridorWallX + flankSpanX * 0.5f, SecondWallCenterY, flankCenterZ),
            new Vector3(flankSpanX, WallHeight, flankDepth),
            _matInteriorWall);
    }

    private void BuildSecondFloorExteriorShell()
    {
        const float upperFrontZ = -5.8f;
        var upperDepth = upperFrontZ - BuildingBackZ + WallCornerOverlap;
        var upperCenterZ = (upperFrontZ + BuildingBackZ) * 0.5f;
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
            new Vector3(0f, SecondWallCenterY, upperFrontZ + WallThickness * 0.5f),
            new Vector3(shellSpanX, WallHeight, WallThickness),
            _matExteriorWall);

        AddWall(
            _secondFloor,
            "Wall_Second_Left",
            new Vector3(-BuildingHalfWidth - WallThickness * 0.5f, SecondWallCenterY, upperCenterZ),
            new Vector3(WallThickness, WallHeight, upperDepth),
            _matExteriorWall);

        AddWall(
            _secondFloor,
            "Wall_Second_Right",
            new Vector3(BuildingHalfWidth + WallThickness * 0.5f, SecondWallCenterY, upperCenterZ),
            new Vector3(WallThickness, WallHeight, upperDepth),
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
