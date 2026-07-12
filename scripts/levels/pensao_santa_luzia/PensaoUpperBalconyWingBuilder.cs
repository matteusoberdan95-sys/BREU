namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Sprint 17/17B — single green balcony door leads to functional exterior balcony + short wing.
/// </summary>
public partial class PensaoVerticalBlockout01Builder
{
    /// <summary>Outer edge of exterior balcony (south of building front wall).</summary>
    private const float BalconyOuterZ = -3.2f;

    private void BuildUpperBalconyWing()
    {
        var wing = new Node3D { Name = "UpperBalconyWing" };
        _secondFloor.AddChild(wing);

        OpenFrontWallForBalconyPassage();
        BuildWalkableBalcony(wing);
        BuildShortWingCorridor(wing);
        BuildWingCeilings();
        BuildWingLightingHints();
    }

    /// <summary>
    /// Wall_Second_Front was a solid box wall behind the door — cut a clean gap for balcony exit.
    /// </summary>
    private void OpenFrontWallForBalconyPassage()
    {
        _secondFloor.GetNodeOrNull("Wall_Second_Front")?.QueueFree();

        var frontZ = UpperFrontZ + WallThickness * 0.5f;
        var totalWidth = BuildingHalfWidth * 2f + WallThickness + WallCornerOverlap;
        var opening = DoorWidth + 0.25f;
        var doorHalf = opening * 0.5f;
        var halfTotal = totalWidth * 0.5f;
        var sideLen = halfTotal - doorHalf;

        if (sideLen > 0.1f)
        {
            AddWall(
                _secondFloor,
                "Wall_Second_Front_Left",
                new Vector3(-doorHalf - sideLen * 0.5f, SecondWallCenterY, frontZ),
                new Vector3(sideLen + WallCornerOverlap, WallHeight, WallThickness),
                _matExteriorWall);

            AddWall(
                _secondFloor,
                "Wall_Second_Front_Right",
                new Vector3(doorHalf + sideLen * 0.5f, SecondWallCenterY, frontZ),
                new Vector3(sideLen + WallCornerOverlap, WallHeight, WallThickness),
                _matExteriorWall);
        }

        AddDoorHeaderZWall(
            _secondFloor,
            "Header_BalconyExterior_Opening",
            new Vector3(0f, 0f, frontZ),
            opening,
            _matExteriorWall,
            SecondFloorTopY);

        GD.Print("[BalconyAccess] Opened Wall_Second_Front gap for exterior balcony.");
    }

    private void BuildWalkableBalcony(Node3D wing)
    {
        var walkable = new Node3D { Name = "UpperBalcony_Walkable" };
        wing.AddChild(walkable);

        var balconyDepth = BalconyOuterZ - BlockedDoorZ + FloorOverlap;
        var balconyCenterZ = (BalconyOuterZ + BlockedDoorZ) * 0.5f;
        var balconyWidth = UpperCorridorWidth + FloorWallLip * 2f;

        AddSecondFloorSlab(
            "UpperBalcony_Floor",
            new Vector3(0f, SecondFloorCenterY, balconyCenterZ),
            new Vector3(balconyWidth, FloorThickness, balconyDepth));

        const float railHeight = 1.05f;
        var railCenterY = SecondFloorTopY + railHeight * 0.5f;

        AddWall(
            walkable,
            "UpperBalcony_GuardRail_Front",
            new Vector3(0f, railCenterY, BalconyOuterZ - 0.06f),
            new Vector3(balconyWidth, railHeight, 0.12f),
            _matSecondRail);

        AddWall(
            walkable,
            "UpperBalcony_GuardRail_Left",
            new Vector3(-CorridorWallX - 0.05f, railCenterY, balconyCenterZ),
            new Vector3(0.12f, railHeight, balconyDepth),
            _matSecondRail);

        // Right rail with gap near front wall for wing entry (east).
        var wingGapHalf = (DoorWidth + 0.05f) * 0.5f;
        var innerFrontZ = UpperFrontZ + WallThickness * 0.5f;
        var northRailDepth = Mathf.Max(0.4f, (innerFrontZ - wingGapHalf) - BlockedDoorZ);
        var southRailDepth = Mathf.Max(0.4f, BalconyOuterZ - (innerFrontZ + wingGapHalf));

        AddWall(
            walkable,
            "UpperBalcony_GuardRail_Right_North",
            new Vector3(CorridorWallX + 0.05f, railCenterY, BlockedDoorZ + northRailDepth * 0.5f),
            new Vector3(0.12f, railHeight, northRailDepth),
            _matSecondRail);

        AddWall(
            walkable,
            "UpperBalcony_GuardRail_Right_South",
            new Vector3(CorridorWallX + 0.05f, railCenterY, BalconyOuterZ - southRailDepth * 0.5f),
            new Vector3(0.12f, railHeight, southRailDepth),
            _matSecondRail);

        walkable.AddChild(new Node3D
        {
            Name = "UpperBalconyWing_Entry",
            Position = new Vector3(CorridorWallX, SecondFloorTopY, innerFrontZ)
        });
    }

    /// <summary>
    /// Short east corridor off the balcony — clean placeholder, end clearly sealed.
    /// </summary>
    private void BuildShortWingCorridor(Node3D wing)
    {
        var corridor = new Node3D { Name = "UpperBalconyWing_Corridor" };
        wing.AddChild(corridor);

        var flankSpanX = BuildingHalfWidth - CorridorWallX - WallThickness * 0.5f;
        var flankCenterX = CorridorWallX + flankSpanX * 0.5f;
        var corridorDepth = 2.4f;
        var corridorCenterZ = UpperFrontZ + WallThickness + corridorDepth * 0.5f;

        AddSecondFloorSlab(
            "UpperBalconyWing_CorridorFloor",
            new Vector3(flankCenterX, SecondFloorCenterY, corridorCenterZ),
            new Vector3(flankSpanX, FloorThickness, corridorDepth + FloorOverlap));

        AddWall(
            corridor,
            "Wall_Wing_OuterEast",
            new Vector3(BuildingInnerEastX, SecondWallCenterY, corridorCenterZ),
            new Vector3(WallThickness, WallHeight, corridorDepth + WallCornerOverlap),
            _matInteriorWall);

        AddWall(
            corridor,
            "Wall_Wing_South",
            new Vector3(flankCenterX, SecondWallCenterY, corridorCenterZ + corridorDepth * 0.5f),
            new Vector3(flankSpanX + WallCornerOverlap, WallHeight, WallThickness),
            _matExteriorWall);

        // North wall with clean gap from balcony into wing (west side).
        var northZ = corridorCenterZ - corridorDepth * 0.5f;
        var entryHalf = (DoorWidth + 0.1f) * 0.5f;
        var westEdge = CorridorWallX + WallThickness * 0.5f;
        var eastEdge = BuildingInnerEastX - WallThickness * 0.5f;
        var entryCenterX = westEdge + entryHalf + 0.15f;
        var leftLen = Mathf.Max(0.2f, (entryCenterX - entryHalf) - westEdge);
        var rightLen = Mathf.Max(0.2f, eastEdge - (entryCenterX + entryHalf));

        if (leftLen > 0.15f)
        {
            AddWall(
                corridor,
                "Wall_Wing_North_West",
                new Vector3(westEdge + leftLen * 0.5f, SecondWallCenterY, northZ),
                new Vector3(leftLen, WallHeight, WallThickness),
                _matInteriorWall);
        }

        if (rightLen > 0.15f)
        {
            AddWall(
                corridor,
                "Wall_Wing_North_East",
                new Vector3(entryCenterX + entryHalf + rightLen * 0.5f, SecondWallCenterY, northZ),
                new Vector3(rightLen, WallHeight, WallThickness),
                _matInteriorWall);
        }

        AddDoorHeaderZWall(
            corridor,
            "Header_UpperBalconyWing_Entry",
            new Vector3(entryCenterX, 0f, northZ),
            DoorWidth + 0.1f,
            _matInteriorWall,
            SecondFloorTopY);

        // Clear end: locked stub door (not the balcony puzzle) — optional visual seal only.
        AddLockedDoorBlocker(
            corridor,
            "Door_WingStub_Locked",
            "Door_WingStub_Blocker",
            new Vector3(BuildingInnerEastX - 0.15f, SecondFloorTopY - WallEmbedBelowFloor, corridorCenterZ),
            1.0f,
            _matDoor,
            "Examinar porta",
            "Trancada. Ainda não é a hora.",
            floorTopY: 0f,
            panelOffsetX: 0f,
            rotationY: Mathf.DegToRad(-90f));

        // Keep Room_203 / office furniture as light props along the short corridor.
        AddFurniture(
            corridor,
            "Furniture_Room203_Table",
            new Vector3(flankCenterX - 0.4f, SecondFloorTopY + 0.38f, corridorCenterZ + 0.2f),
            new Vector3(0.65f, 0.76f, 0.45f),
            _matFurniture);
        corridor.AddChild(new Node3D { Name = "Room_203", Position = new Vector3(flankCenterX, SecondFloorTopY, corridorCenterZ) });
        corridor.AddChild(new Node3D { Name = "Room_OwnerOffice", Position = new Vector3(flankCenterX + 0.8f, SecondFloorTopY, corridorCenterZ - 0.3f) });

        AddFurniture(
            corridor,
            "Furniture_OwnerOffice_Desk",
            new Vector3(flankCenterX + 0.7f, SecondFloorTopY + 0.4f, corridorCenterZ - 0.35f),
            new Vector3(1.0f, 0.8f, 0.45f),
            _matFurniture);
    }

    private void BuildWingCeilings()
    {
        var balconyDepth = BalconyOuterZ - BlockedDoorZ + FloorOverlap;
        var balconyCenterZ = (BalconyOuterZ + BlockedDoorZ) * 0.5f;

        AddVisualCeilingPlate(
            _ceiling,
            "Ceiling_UpperBalcony",
            new Vector3(0f, SecondFloorCeilingUndersideY, balconyCenterZ),
            new Vector3(UpperCorridorWidth + WallCornerOverlap, CeilingThickness, balconyDepth),
            _matCeilingSecond);

        var flankSpanX = BuildingHalfWidth - CorridorWallX - WallThickness * 0.5f;
        var flankCenterX = CorridorWallX + flankSpanX * 0.5f;
        var corridorDepth = 2.4f;
        var corridorCenterZ = UpperFrontZ + WallThickness + corridorDepth * 0.5f;

        AddVisualCeilingPlate(
            _ceiling,
            "Ceiling_UpperBalconyWing",
            new Vector3(flankCenterX, SecondFloorCeilingUndersideY, corridorCenterZ),
            new Vector3(flankSpanX + 0.2f, CeilingThickness, corridorDepth + 0.3f),
            _matCeilingSecond);
    }

    private void BuildWingLightingHints()
    {
        var lighting = GetNodeOrNull<Node3D>("../../Lighting");
        if (lighting == null)
        {
            return;
        }

        lighting.GetNodeOrNull("BalconyWingLight")?.QueueFree();
        lighting.GetNodeOrNull("Room203Light")?.QueueFree();

        lighting.AddChild(new OmniLight3D
        {
            Name = "BalconyWingLight",
            Position = new Vector3(0f, 4.5f, -4.5f),
            LightColor = new Color(0.72f, 0.78f, 0.9f),
            LightEnergy = 0.55f,
            OmniRange = 6.0f,
            OmniAttenuation = 1.3f
        });

        lighting.AddChild(new OmniLight3D
        {
            Name = "Room203Light",
            Position = new Vector3(4.0f, 4.4f, -5.0f),
            LightColor = new Color(0.85f, 0.7f, 0.5f),
            LightEnergy = 0.4f,
            OmniRange = 4.5f,
            OmniAttenuation = 1.4f
        });
    }
}
