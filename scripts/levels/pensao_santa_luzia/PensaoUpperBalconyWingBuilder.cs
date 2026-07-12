namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Sprint 17B/17C — balcony + bathroom + owner bedroom wing (horror blockout, no enemy).
/// </summary>
public partial class PensaoVerticalBlockout01Builder
{
    private const float BalconyOuterZ = -3.2f;

    private StandardMaterial3D _matBloodStain = null!;
    private StandardMaterial3D _matDampStain = null!;

    private void BuildUpperBalconyWing()
    {
        _matBloodStain = Mat(new Color(0.28f, 0.08f, 0.08f));
        _matDampStain = Mat(new Color(0.22f, 0.24f, 0.26f));

        var wing = new Node3D { Name = "UpperBalconyWing" };
        _secondFloor.AddChild(wing);

        OpenFrontWallForBalconyPassage();
        BuildWalkableBalcony(wing);
        BuildWingCorridorAndRooms(wing);
        BuildWingCeilings();
        BuildWingLightingHints();
    }

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

        const float railHeight = 1.1f;
        var railCenterY = SecondFloorTopY + railHeight * 0.5f;

        AddWall(
            walkable,
            "UpperBalcony_GuardRail_Front",
            new Vector3(0f, railCenterY, BalconyOuterZ - 0.06f),
            new Vector3(balconyWidth, railHeight, 0.14f),
            _matSecondRail);

        AddWall(
            walkable,
            "UpperBalcony_GuardRail_Left",
            new Vector3(-CorridorWallX - 0.05f, railCenterY, balconyCenterZ),
            new Vector3(0.14f, railHeight, balconyDepth),
            _matSecondRail);

        var wingGapHalf = (DoorWidth + 0.05f) * 0.5f;
        var innerFrontZ = UpperFrontZ + WallThickness * 0.5f;
        var northRailDepth = Mathf.Max(0.4f, (innerFrontZ - wingGapHalf) - BlockedDoorZ);
        var southRailDepth = Mathf.Max(0.4f, BalconyOuterZ - (innerFrontZ + wingGapHalf));

        AddWall(
            walkable,
            "UpperBalcony_GuardRail_Right_North",
            new Vector3(CorridorWallX + 0.05f, railCenterY, BlockedDoorZ + northRailDepth * 0.5f),
            new Vector3(0.14f, railHeight, northRailDepth),
            _matSecondRail);

        AddWall(
            walkable,
            "UpperBalcony_GuardRail_Right_South",
            new Vector3(CorridorWallX + 0.05f, railCenterY, BalconyOuterZ - southRailDepth * 0.5f),
            new Vector3(0.14f, railHeight, southRailDepth),
            _matSecondRail);

        // Named alias for checklist (right rail is split around wing gap).
        walkable.AddChild(new Node3D
        {
            Name = "UpperBalcony_GuardRail_Right",
            Position = new Vector3(CorridorWallX + 0.05f, railCenterY, balconyCenterZ)
        });

        walkable.AddChild(new Node3D
        {
            Name = "UpperBalconyWing_Entry",
            Position = new Vector3(CorridorWallX, SecondFloorTopY, innerFrontZ)
        });
    }

    private void BuildWingCorridorAndRooms(Node3D wing)
    {
        var corridor = new Node3D { Name = "UpperBalconyWing_Corridor" };
        wing.AddChild(corridor);

        var flankSpanX = BuildingHalfWidth - CorridorWallX - WallThickness * 0.5f;
        var flankCenterX = CorridorWallX + flankSpanX * 0.5f;
        var corridorDepth = 3.4f;
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

        var northZ = corridorCenterZ - corridorDepth * 0.5f;
        var entryHalf = (DoorWidth + 0.1f) * 0.5f;
        var westEdge = CorridorWallX + WallThickness * 0.5f;
        var eastEdge = BuildingInnerEastX - WallThickness * 0.5f;
        var entryCenterX = westEdge + entryHalf + 0.2f;
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

        // Divider: bathroom (west, open) | owner bedroom (east, locked door in gap).
        var dividerX = westEdge + 2.55f;
        var doorHalf = (DoorWidth - 0.2f) * 0.5f;
        var dividerSpan = corridorDepth * 0.78f;
        var dividerCenterZ = corridorCenterZ + 0.2f;
        var dividerNorth = dividerCenterZ - dividerSpan * 0.5f;
        var dividerSouth = dividerCenterZ + dividerSpan * 0.5f;
        var gapCenterZ = corridorCenterZ - 0.05f;
        var northSeg = Mathf.Max(0.15f, (gapCenterZ - doorHalf) - dividerNorth);
        var southSeg = Mathf.Max(0.15f, dividerSouth - (gapCenterZ + doorHalf));

        if (northSeg > 0.12f)
        {
            AddWall(
                corridor,
                "Wall_Wing_RoomDivider_North",
                new Vector3(dividerX, SecondWallCenterY, dividerNorth + northSeg * 0.5f),
                new Vector3(WallThickness, WallHeight, northSeg),
                _matInteriorWall);
        }

        if (southSeg > 0.12f)
        {
            AddWall(
                corridor,
                "Wall_Wing_RoomDivider_South",
                new Vector3(dividerX, SecondWallCenterY, dividerSouth - southSeg * 0.5f),
                new Vector3(WallThickness, WallHeight, southSeg),
                _matInteriorWall);
        }

        AddDoorHeaderXWall(
            corridor,
            "Header_OwnerBedroom_Door",
            dividerX,
            gapCenterZ,
            DoorWidth - 0.2f,
            _matInteriorWall,
            SecondFloorTopY);

        var bathroomCenterX = (westEdge + dividerX) * 0.5f;
        var bedroomCenterX = (dividerX + eastEdge) * 0.5f;

        BuildBathroom(wing, bathroomCenterX, corridorCenterZ, dividerX, westEdge);
        BuildOwnerBedroom(wing, bedroomCenterX, corridorCenterZ, dividerX, eastEdge);

        AddLockedDoorBlocker(
            corridor,
            "Door_Room203_Blocked",
            "Door_Room203_Blocker",
            new Vector3(BuildingInnerEastX - 0.12f, SecondFloorTopY - WallEmbedBelowFloor, corridorCenterZ + 0.55f),
            1.05f,
            _matDoor,
            "Tentar abrir quarto 203",
            "Algo pesado bloqueia a porta pelo outro lado.",
            floorTopY: 0f,
            panelOffsetX: 0f,
            rotationY: Mathf.DegToRad(-90f));
    }

    private void BuildBathroom(
        Node3D wing,
        float centerX,
        float corridorZ,
        float eastWallX,
        float westWallX)
    {
        var room = new Node3D { Name = "Room_UpperBathroom" };
        wing.AddChild(room);

        var roomDepth = 2.7f;
        var roomWidth = eastWallX - westWallX - 0.08f;
        var roomZ = corridorZ + 0.05f;

        AddSecondFloorSlab(
            "Bathroom_Floor",
            new Vector3(centerX, SecondFloorCenterY, roomZ),
            new Vector3(roomWidth, FloorThickness, roomDepth));

        // Open doorway toward north entry — no decorative door.
        AddFurniture(
            room,
            "Bathroom_Sink",
            new Vector3(centerX - 0.45f, SecondFloorTopY + 0.55f, roomZ - 0.65f),
            new Vector3(0.7f, 0.55f, 0.45f),
            _matFurniture);

        AddFurniture(
            room,
            "Bathroom_Toilet",
            new Vector3(centerX + 0.5f, SecondFloorTopY + 0.28f, roomZ + 0.55f),
            new Vector3(0.45f, 0.55f, 0.55f),
            _matFurniture);

        AddVisualProp(
            room,
            "Bathroom_DampStain",
            new Vector3(centerX - 0.15f, SecondFloorTopY + 0.03f, roomZ + 0.15f),
            new Vector3(0.7f, 0.02f, 0.5f),
            _matDampStain);

        AddVisualProp(
            room,
            "Bathroom_BloodStain",
            new Vector3(centerX + 0.35f, SecondFloorTopY + 0.035f, roomZ - 0.25f),
            new Vector3(0.35f, 0.02f, 0.28f),
            _matBloodStain);

        AddVisualProp(
            room,
            "Bathroom_WallMark",
            new Vector3(westWallX + 0.12f, SecondFloorTopY + 1.2f, roomZ),
            new Vector3(0.04f, 0.55f, 0.35f),
            _matBloodStain);

        // Anchors for puzzle setup (world positions).
        room.AddChild(new Marker3D
        {
            Name = "Anchor_BathroomMirror",
            Position = new Vector3(centerX - 0.45f, SecondFloorTopY + 1.35f, roomZ - 0.9f)
        });
        room.AddChild(new Marker3D
        {
            Name = "Anchor_BathroomDrain",
            Position = new Vector3(centerX + 0.35f, SecondFloorTopY + 0.06f, roomZ - 0.2f)
        });
    }

    private void BuildOwnerBedroom(
        Node3D wing,
        float centerX,
        float corridorZ,
        float westWallX,
        float eastWallX)
    {
        var room = new Node3D { Name = "Room_OwnerBedroom" };
        wing.AddChild(room);

        var roomDepth = 2.9f;
        var roomWidth = eastWallX - westWallX - 0.1f;
        var roomZ = corridorZ - 0.05f;

        AddSecondFloorSlab(
            "OwnerBedroom_Floor",
            new Vector3(centerX, SecondFloorCenterY, roomZ),
            new Vector3(roomWidth, FloorThickness, roomDepth));

        // Door faces west into the wing corridor (local -Z → world -X).
        AddOwnerBedroomDoorBlocker(
            room,
            "Door_OwnerBedroom",
            "Door_OwnerBedroom_Blocker",
            new Vector3(westWallX + 0.12f, 0f, roomZ),
            DoorWidth - 0.2f,
            _matDoor,
            SecondFloorTopY,
            panelOffsetX: 0f,
            rotationY: Mathf.DegToRad(90f));

        AddFurniture(
            room,
            "OwnerBedroom_Bed",
            new Vector3(centerX + 0.35f, SecondFloorTopY + 0.32f, roomZ + 0.5f),
            new Vector3(1.6f, 0.64f, 1.0f),
            _matBed);

        AddFurniture(
            room,
            "OwnerBedroom_Desk",
            new Vector3(centerX - 0.25f, SecondFloorTopY + 0.4f, roomZ - 0.75f),
            new Vector3(1.0f, 0.8f, 0.5f),
            _matFurniture);

        AddFurniture(
            room,
            "OwnerBedroom_Cabinet",
            new Vector3(centerX + 0.75f, SecondFloorTopY + 0.75f, roomZ - 0.55f),
            new Vector3(0.6f, 1.5f, 0.45f),
            _matFurniture);

        AddVisualProp(
            room,
            "OwnerBedroom_FloorStain",
            new Vector3(centerX, SecondFloorTopY + 0.03f, roomZ + 0.15f),
            new Vector3(0.85f, 0.02f, 0.5f),
            _matBloodStain);

        AddVisualProp(
            room,
            "OwnerBedroom_NailMarks",
            new Vector3(eastWallX - 0.12f, SecondFloorTopY + 1.35f, roomZ),
            new Vector3(0.04f, 0.7f, 0.5f),
            _matDampStain);

        room.AddChild(new Marker3D
        {
            Name = "Anchor_OwnerLedger",
            Position = new Vector3(centerX - 0.25f, SecondFloorTopY + 0.85f, roomZ - 0.75f)
        });
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
        var corridorDepth = 3.4f;
        var corridorCenterZ = UpperFrontZ + WallThickness + corridorDepth * 0.5f;

        AddVisualCeilingPlate(
            _ceiling,
            "Ceiling_UpperBalconyWing",
            new Vector3(flankCenterX, SecondFloorCeilingUndersideY, corridorCenterZ),
            new Vector3(flankSpanX + 0.25f, CeilingThickness, corridorDepth + 0.4f),
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
        lighting.GetNodeOrNull("BathroomLight")?.QueueFree();
        lighting.GetNodeOrNull("OwnerBedroomLight")?.QueueFree();

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
            Name = "BathroomLight",
            Position = new Vector3(2.6f, 4.35f, -4.0f),
            LightColor = new Color(0.55f, 0.7f, 0.75f),
            LightEnergy = 0.35f,
            OmniRange = 4.0f,
            OmniAttenuation = 1.5f
        });

        lighting.AddChild(new OmniLight3D
        {
            Name = "OwnerBedroomLight",
            Position = new Vector3(5.0f, 4.4f, -4.2f),
            LightColor = new Color(0.85f, 0.65f, 0.45f),
            LightEnergy = 0.4f,
            OmniRange = 4.5f,
            OmniAttenuation = 1.4f
        });

        lighting.AddChild(new OmniLight3D
        {
            Name = "Room203Light",
            Position = new Vector3(6.2f, 4.3f, -3.6f),
            LightColor = new Color(0.5f, 0.45f, 0.55f),
            LightEnergy = 0.25f,
            OmniRange = 3.5f,
            OmniAttenuation = 1.6f
        });
    }
}
