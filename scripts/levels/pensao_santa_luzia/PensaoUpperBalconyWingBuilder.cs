namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>Sprint 17E — clean, self-contained rebuild of the balcony micro-area.</summary>
public partial class PensaoVerticalBlockout01Builder
{
    private const float BalconyFrontZ = -3.2f;
    private const float BalconyBackZ = -7.5f;
    private const float BalconyEastX = CorridorWallX;
    private const float BalconyWestX = -CorridorWallX;
    private static float WingEastX => BuildingInnerEastX;

    private StandardMaterial3D _matBloodStain = null!;
    private StandardMaterial3D _matDampStain = null!;

    private void BuildUpperBalconyWing()
    {
        // Sprint 17E/18B — FROZEN. Manual owner: areas/BalconyWing.tscn.
        // Do not recreate rooms, doors, floors or interactions here.
        GD.PushWarning(
            "[Frozen] BuildUpperBalconyWing skipped. Use BalconyWing.tscn / UpperWingExpansion.tscn.");
    }

    private void OpenFrontWallForBalconyPassage()
    {
        // Allowed structural edit only: cut Wall_Second_Front so the manual green door works.
        _secondFloor.GetNodeOrNull("Wall_Second_Front")?.QueueFree();

        var wallZ = UpperFrontZ + WallThickness * 0.5f;
        var totalWidth = BuildingHalfWidth * 2f + WallThickness + WallCornerOverlap;
        var openingWidth = DoorWidth + 0.25f;
        var sideWidth = (totalWidth - openingWidth) * 0.5f;

        AddWall(_secondFloor, "Wall_BalconyDoor_Left",
            new Vector3(-(openingWidth + sideWidth) * 0.5f, SecondWallCenterY, wallZ),
            new Vector3(sideWidth + WallCornerOverlap, WallHeight, WallThickness), _matExteriorWall);
        AddWall(_secondFloor, "Wall_BalconyDoor_Right",
            new Vector3((openingWidth + sideWidth) * 0.5f, SecondWallCenterY, wallZ),
            new Vector3(sideWidth + WallCornerOverlap, WallHeight, WallThickness), _matExteriorWall);
        AddDoorHeaderZWall(_secondFloor, "Header_BalconyDoor_Green",
            new Vector3(0f, 0f, wallZ), openingWidth, _matExteriorWall, SecondFloorTopY);
    }

    private void BuildBalconyLandingAndWalkway(Node3D wing)
    {
        // The main second-floor slab already covers door -> Z -5.8. Reuse it as
        // the landing instead of drawing a coplanar floor that flickers.
        const float landingFrontZ = UpperFrontZ;
        wing.AddChild(new Node3D
        {
            Name = "BalconyLanding",
            Position = new Vector3(0f, SecondFloorTopY, (landingFrontZ + BalconyBackZ) * 0.5f)
        });

        var balconyDepth = BalconyFrontZ - landingFrontZ;
        var balconyCenterZ = (BalconyFrontZ + landingFrontZ) * 0.5f;
        AddSecondFloorSlab("BalconyWalkable",
            new Vector3(0f, SecondFloorCenterY, balconyCenterZ),
            new Vector3(BalconyEastX - BalconyWestX, FloorThickness, balconyDepth));

        // The east side is occupied by the two room façades; this alias documents
        // that those walls, rather than an internal rail, close the right edge.
    }

    private void BuildBathroom(Node3D wing)
    {
        const float northZ = -6.25f;
        const float southZ = -4.45f;
        const float doorZ = -5.35f;
        var room = new Node3D { Name = "Room_Bathroom" };
        wing.AddChild(room);

        var centerX = (BalconyEastX + WingEastX) * 0.5f;
        var centerZ = (northZ + southZ) * 0.5f;
        // Collision spans the whole room. The visual starts at Z -5.8 because
        // the main second-floor visual already covers the northern strip.
        AddCollisionFloor(_secondFloor, "Room_Bathroom_Floor",
            new Vector3(centerX, SecondFloorCenterY, centerZ),
            new Vector3(WingEastX - BalconyEastX, FloorThickness, southZ - northZ));
        const float visualNorthZ = UpperFrontZ;
        var visualCenterZ = (visualNorthZ + southZ) * 0.5f;
        AddVisualFloorPlate(_secondFloor, "Room_Bathroom_Floor_Visual",
            new Vector3(centerX, 0f, visualCenterZ),
            new Vector2(WingEastX - BalconyEastX, southZ - visualNorthZ),
            SecondFloorVisualTopY, _matSecondFloor);

        AddWall(room, "Room_Bathroom_WallNorth", new Vector3(centerX, SecondWallCenterY, northZ),
            new Vector3(WingEastX - BalconyEastX, WallHeight, WallThickness), _matInteriorWall);
        AddWall(room, "Room_Bathroom_WallEast", new Vector3(WingEastX, SecondWallCenterY, centerZ),
            new Vector3(WallThickness, WallHeight, southZ - northZ), _matInteriorWall);
        const float bathroomDoorWidth = 1.3f;
        BuildWestWallWithDoor(room, "Room_Bathroom", northZ, southZ, doorZ, bathroomDoorWidth);

        room.AddChild(new Node3D
        {
            Name = "Room_BathroomDoor",
            Position = new Vector3(BalconyEastX, SecondFloorTopY, doorZ)
        });

        room.AddChild(new Marker3D
        {
            Name = "Anchor_BathroomDoor",
            Position = new Vector3(BalconyEastX - 0.18f, SecondFloorTopY + 1.35f, doorZ)
        });

        AddFurniture(room, "Bathroom_Sink",
            new Vector3(centerX + 0.55f, SecondFloorTopY + 0.52f, northZ + 0.35f),
            new Vector3(0.65f, 0.55f, 0.4f), _matFurniture);
        AddVisualProp(room, "Bathroom_DampStain",
            new Vector3(centerX, SecondFloorTopY + 0.03f, centerZ),
            new Vector3(0.65f, 0.02f, 0.45f), _matDampStain);

        room.AddChild(new Marker3D
        {
            Name = "Anchor_BathroomMirror",
            Position = new Vector3(centerX + 0.55f, SecondFloorTopY + 1.35f, northZ + 0.18f)
        });
        room.AddChild(new Marker3D
        {
            Name = "Anchor_BathroomDrain",
            Position = new Vector3(centerX - 0.55f, SecondFloorTopY + 0.06f, southZ - 0.35f)
        });
    }

    private void BuildOwnerBedroom(Node3D wing)
    {
        const float northZ = -4.35f;
        const float southZ = -2.35f;
        const float doorZ = -3.65f;
        var room = new Node3D { Name = "Room_OwnerBedroom" };
        wing.AddChild(room);

        var centerX = (BalconyEastX + WingEastX) * 0.5f;
        var centerZ = (northZ + southZ) * 0.5f;
        AddSecondFloorSlab("Room_OwnerBedroom_Floor",
            new Vector3(centerX, SecondFloorCenterY, centerZ),
            new Vector3(WingEastX - BalconyEastX, FloorThickness, southZ - northZ));

        AddWall(room, "Room_OwnerBedroom_WallNorth", new Vector3(centerX, SecondWallCenterY, northZ),
            new Vector3(WingEastX - BalconyEastX, WallHeight, WallThickness), _matInteriorWall);
        AddWall(room, "Room_OwnerBedroom_WallSouth", new Vector3(centerX, SecondWallCenterY, southZ),
            new Vector3(WingEastX - BalconyEastX, WallHeight, WallThickness), _matExteriorWall);
        AddWall(room, "Room_OwnerBedroom_WallEast", new Vector3(WingEastX, SecondWallCenterY, centerZ),
            new Vector3(WallThickness, WallHeight, southZ - northZ), _matInteriorWall);
        const float ownerDoorWidth = 1.35f;
        BuildWestWallWithDoor(room, "Room_OwnerBedroom", northZ, southZ, doorZ, ownerDoorWidth);

        AddOwnerBedroomDoorBlocker(room, "Room_OwnerDoor", "Room_OwnerDoor_Panel",
            new Vector3(BalconyEastX + 0.08f, 0f, doorZ), ownerDoorWidth,
            _matDoor, SecondFloorTopY, rotationY: Mathf.DegToRad(90f));

        AddFurniture(room, "OwnerBedroom_Bed",
            new Vector3(WingEastX - 1.05f, SecondFloorTopY + 0.3f, southZ + 0.55f),
            new Vector3(1.35f, 0.6f, 0.75f), _matBed);
        AddFurniture(room, "OwnerBedroom_Dresser",
            new Vector3(WingEastX - 0.55f, SecondFloorTopY + 0.55f, northZ + 0.35f),
            new Vector3(0.65f, 1.1f, 0.45f), _matFurniture);
        AddVisualProp(room, "OwnerBedroom_FloorStain",
            new Vector3(centerX + 0.2f, SecondFloorTopY + 0.03f, centerZ),
            new Vector3(0.55f, 0.02f, 0.4f), _matBloodStain);

        room.AddChild(new Marker3D
        {
            Name = "Anchor_OwnerLedger",
            Position = new Vector3(WingEastX - 0.55f, SecondFloorTopY + 1.12f, northZ + 0.35f)
        });
    }

    private void BuildWestWallWithDoor(Node3D room, string prefix, float northZ, float southZ, float doorZ, float width)
    {
        var half = width * 0.5f;
        var northLength = doorZ - half - northZ;
        var southLength = southZ - (doorZ + half);

        if (northLength > 0.08f)
        {
            AddWall(room, $"{prefix}_WallWestNorth",
                new Vector3(BalconyEastX, SecondWallCenterY, northZ + northLength * 0.5f),
                new Vector3(WallThickness, WallHeight, northLength), _matInteriorWall);
        }
        if (southLength > 0.08f)
        {
            AddWall(room, $"{prefix}_WallWestSouth",
                new Vector3(BalconyEastX, SecondWallCenterY, doorZ + half + southLength * 0.5f),
                new Vector3(WallThickness, WallHeight, southLength), _matInteriorWall);
        }
        AddDoorHeaderXWall(room, $"{prefix}_DoorHeader", BalconyEastX, doorZ, width,
            _matInteriorWall, SecondFloorTopY);
    }

    private void BuildBalconyWingCeilings()
    {
        AddVisualCeilingPlate(_ceiling, "Ceiling_BalconyLanding",
            new Vector3(0f, SecondFloorCeilingUndersideY, (BalconyBackZ - 6.15f) * 0.5f),
            new Vector3(UpperCorridorWidth + WallCornerOverlap, CeilingThickness, 1.35f), _matCeilingSecond);
        AddVisualCeilingPlate(_ceiling, "Ceiling_BalconyWalkable",
            new Vector3(0f, SecondFloorCeilingUndersideY, (BalconyFrontZ - 6.15f) * 0.5f),
            new Vector3(UpperCorridorWidth + WallCornerOverlap, CeilingThickness, 2.95f), _matCeilingSecond);
        AddVisualCeilingPlate(_ceiling, "Ceiling_BalconyRooms",
            new Vector3((BalconyEastX + WingEastX) * 0.5f, SecondFloorCeilingUndersideY, -4.3f),
            new Vector3(WingEastX - BalconyEastX, CeilingThickness, 3.9f), _matCeilingSecond);
    }

    private void BuildWingLightingHints()
    {
        var lighting = GetNodeOrNull<Node3D>("../../Lighting");
        if (lighting == null) return;

        foreach (var name in new[] { "BalconyWingLight", "Room203Light", "BathroomLight", "OwnerBedroomLight" })
        {
            lighting.GetNodeOrNull(name)?.QueueFree();
        }
        lighting.AddChild(new OmniLight3D { Name = "BalconyWingLight", Position = new Vector3(0f, 4.5f, -4.8f), LightColor = new Color(0.72f, 0.78f, 0.9f), LightEnergy = 0.55f, OmniRange = 6f });
        lighting.AddChild(new OmniLight3D { Name = "BathroomLight", Position = new Vector3(3.5f, 4.35f, -5.3f), LightColor = new Color(0.55f, 0.7f, 0.75f), LightEnergy = 0.35f, OmniRange = 4f });
        lighting.AddChild(new OmniLight3D { Name = "OwnerBedroomLight", Position = new Vector3(4.8f, 4.35f, -3.4f), LightColor = new Color(0.85f, 0.65f, 0.45f), LightEnergy = 0.4f, OmniRange = 4.5f });
    }
}
