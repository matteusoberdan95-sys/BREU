namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Sprint 17 — walkable upper balcony pocket + east wing rooms (inside footprint).
/// </summary>
public partial class PensaoVerticalBlockout01Builder
{
    private void BuildUpperBalconyWing()
    {
        var wing = new Node3D { Name = "UpperBalconyWing" };
        _secondFloor.AddChild(wing);

        BuildWalkableBalcony(wing);
        BuildEastWingRooms(wing);
        BuildWingCeilings();
        BuildWingLightingHints();
    }

    private void BuildWalkableBalcony(Node3D wing)
    {
        var walkable = new Node3D { Name = "UpperBalcony_Walkable" };
        wing.AddChild(walkable);

        const float balconyFrontZ = UpperFrontZ + WallThickness * 0.5f;
        var balconyDepth = balconyFrontZ - BlockedDoorZ + FloorOverlap;
        var balconyCenterZ = (balconyFrontZ + BlockedDoorZ) * 0.5f;
        var balconyWidth = UpperCorridorWidth + FloorWallLip * 2f;

        AddSecondFloorSlab(
            "UpperBalcony_Floor",
            new Vector3(0f, SecondFloorCenterY, balconyCenterZ),
            new Vector3(balconyWidth, FloorThickness, balconyDepth));

        const float railHeight = 1.05f;
        var railCenterY = SecondFloorTopY + railHeight * 0.5f;

        // Front rail with clean gap for Door_BalconyWing_Entry into east wing.
        var gapHalf = (DoorWidth + 0.1f) * 0.5f;
        var railSpan = balconyWidth * 0.5f - gapHalf;
        AddWall(
            walkable,
            "UpperBalcony_Rail_Front_Left",
            new Vector3(-gapHalf - railSpan * 0.5f, railCenterY, balconyFrontZ - 0.1f),
            new Vector3(railSpan, railHeight, 0.12f),
            _matSecondRail);
        AddWall(
            walkable,
            "UpperBalcony_Rail_Front_Right",
            new Vector3(gapHalf + railSpan * 0.5f, railCenterY, balconyFrontZ - 0.1f),
            new Vector3(railSpan, railHeight, 0.12f),
            _matSecondRail);

        // Marker only — open vão, no panel.
        walkable.AddChild(new Node3D
        {
            Name = "Door_BalconyWing_Entry",
            Position = new Vector3(0f, SecondFloorTopY, balconyFrontZ)
        });
    }

    private void BuildEastWingRooms(Node3D wing)
    {
        // Uses the former solid east south-flank volume (opened into corridor + rooms).
        var flankDepth = UpperFrontZ - BlockedDoorZ + WallCornerOverlap;
        var flankCenterZ = (UpperFrontZ + BlockedDoorZ) * 0.5f;
        var flankSpanX = BuildingHalfWidth - CorridorWallX - WallThickness * 0.5f;
        var flankCenterX = CorridorWallX + flankSpanX * 0.5f;

        var corridor = new Node3D { Name = "UpperBalconyWing_Corridor" };
        wing.AddChild(corridor);

        AddSecondFloorSlab(
            "UpperBalconyWing_CorridorFloor",
            new Vector3(flankCenterX, SecondFloorCenterY, flankCenterZ),
            new Vector3(flankSpanX, FloorThickness, flankDepth));

        // Outer east seal (building edge).
        AddWall(
            corridor,
            "Wall_Wing_OuterEast",
            new Vector3(BuildingInnerEastX, SecondWallCenterY, flankCenterZ),
            new Vector3(WallThickness, WallHeight, flankDepth + WallCornerOverlap),
            _matInteriorWall);

        // Front/back seals of the wing pocket.
        AddWall(
            corridor,
            "Wall_Wing_Front",
            new Vector3(flankCenterX, SecondWallCenterY, UpperFrontZ + WallThickness * 0.5f),
            new Vector3(flankSpanX + WallCornerOverlap, WallHeight, WallThickness),
            _matExteriorWall);
        AddWall(
            corridor,
            "Wall_Wing_Back",
            new Vector3(flankCenterX, SecondWallCenterY, BlockedDoorZ - WallThickness * 0.15f),
            new Vector3(flankSpanX + WallCornerOverlap, WallHeight, WallThickness),
            _matInteriorWall);

        BuildRoom203(wing, flankCenterX, flankCenterZ, flankSpanX, flankDepth);
        BuildOwnerOffice(wing, flankCenterX, flankCenterZ, flankSpanX, flankDepth);
    }

    private void BuildRoom203(Node3D wing, float flankCenterX, float flankCenterZ, float flankSpanX, float flankDepth)
    {
        var room = new Node3D { Name = "Room_203" };
        wing.AddChild(room);

        var roomZ = flankCenterZ + flankDepth * 0.22f;
        AddFurniture(
            room,
            "Furniture_Room203_Bed",
            new Vector3(flankCenterX + 0.5f, SecondFloorTopY + 0.32f, roomZ),
            new Vector3(1.6f, 0.64f, 0.95f),
            _matBed);
        AddFurniture(
            room,
            "Furniture_Room203_Table",
            new Vector3(flankCenterX - 0.7f, SecondFloorTopY + 0.38f, roomZ - 0.15f),
            new Vector3(0.65f, 0.76f, 0.45f),
            _matFurniture);
    }

    private void BuildOwnerOffice(Node3D wing, float flankCenterX, float flankCenterZ, float flankSpanX, float flankDepth)
    {
        var room = new Node3D { Name = "Room_OwnerOffice" };
        wing.AddChild(room);

        var roomZ = flankCenterZ - flankDepth * 0.22f;
        AddFurniture(
            room,
            "Furniture_OwnerOffice_Desk",
            new Vector3(flankCenterX + 0.35f, SecondFloorTopY + 0.4f, roomZ),
            new Vector3(1.1f, 0.8f, 0.5f),
            _matFurniture);
        AddFurniture(
            room,
            "Furniture_OwnerOffice_Cabinet",
            new Vector3(flankCenterX - 0.85f, SecondFloorTopY + 0.7f, roomZ + 0.1f),
            new Vector3(0.65f, 1.35f, 0.4f),
            _matFurniture);
    }

    private void BuildWingCeilings()
    {
        const float balconyFrontZ = UpperFrontZ + WallThickness * 0.5f;
        var balconyDepth = balconyFrontZ - BlockedDoorZ + FloorOverlap;
        var balconyCenterZ = (balconyFrontZ + BlockedDoorZ) * 0.5f;

        AddVisualCeilingPlate(
            _ceiling,
            "Ceiling_UpperBalcony",
            new Vector3(0f, SecondFloorCeilingUndersideY, balconyCenterZ),
            new Vector3(UpperCorridorWidth + WallCornerOverlap, CeilingThickness, balconyDepth),
            _matCeilingSecond);

        var flankDepth = UpperFrontZ - BlockedDoorZ + WallCornerOverlap;
        var flankCenterZ = (UpperFrontZ + BlockedDoorZ) * 0.5f;
        var flankSpanX = BuildingHalfWidth - CorridorWallX - WallThickness * 0.5f;
        var flankCenterX = CorridorWallX + flankSpanX * 0.5f;

        AddVisualCeilingPlate(
            _ceiling,
            "Ceiling_UpperBalconyWing",
            new Vector3(flankCenterX, SecondFloorCeilingUndersideY, flankCenterZ),
            new Vector3(flankSpanX + 0.2f, CeilingThickness, flankDepth + 0.2f),
            _matCeilingSecond);
    }

    private void BuildWingLightingHints()
    {
        var lighting = GetNodeOrNull<Node3D>("../../Lighting");
        if (lighting == null)
        {
            return;
        }

        if (lighting.GetNodeOrNull("BalconyWingLight") == null)
        {
            lighting.AddChild(new OmniLight3D
            {
                Name = "BalconyWingLight",
                Position = new Vector3(4.0f, 4.5f, -6.5f),
                LightColor = new Color(0.72f, 0.78f, 0.9f),
                LightEnergy = 0.5f,
                OmniRange = 5.5f,
                OmniAttenuation = 1.3f
            });
        }

        if (lighting.GetNodeOrNull("Room203Light") == null)
        {
            lighting.AddChild(new OmniLight3D
            {
                Name = "Room203Light",
                Position = new Vector3(4.2f, 4.4f, -6.0f),
                LightColor = new Color(0.85f, 0.7f, 0.5f),
                LightEnergy = 0.4f,
                OmniRange = 4.5f,
                OmniAttenuation = 1.4f
            });
        }
    }
}
