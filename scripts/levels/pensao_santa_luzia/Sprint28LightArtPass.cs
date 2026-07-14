namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Sprint 28 light art pass. This tree is deliberately visual-only: it creates
/// no collision, Area3D, navigation, rigid bodies, interaction or gameplay state.
/// </summary>
public partial class Sprint28LightArtPass : Node3D
{
    private StandardMaterial3D _wood = null!;
    private StandardMaterial3D _darkWood = null!;
    private StandardMaterial3D _metal = null!;
    private StandardMaterial3D _paper = null!;
    private StandardMaterial3D _cloth = null!;
    private StandardMaterial3D _mold = null!;
    private StandardMaterial3D _stain = null!;
    private StandardMaterial3D _oldBlood = null!;
    private StandardMaterial3D _glass = null!;
    private StandardMaterial3D _fadedRed = null!;
    private StandardMaterial3D _brass = null!;
    private StandardMaterial3D _upholstery = null!;

    public override void _Ready()
    {
        BuildMaterials();
        BuildReception(GetNode<Node3D>("Reception_Props"));
        BuildKitchen(GetNode<Node3D>("Kitchen_Props"));
        BuildBathroom(GetNode<Node3D>("Bathroom_Props"));
        BuildTechnicalRoom(GetNode<Node3D>("TechnicalRoom_Props"));
        BuildUpperRooms(GetNode<Node3D>("UpperRooms_Props"));
        BuildRoom203(GetNode<Node3D>("Room203_Props"));
        BuildCorridors(GetNode<Node3D>("Corridor_Props"));
        BuildStains(GetNode<Node3D>("Decals_Stains"));
        BuildCloths(GetNode<Node3D>("Cloths_Curtains"));
        GD.Print("[Sprint28Art] light room art pass loaded; visual meshes only, 0 collision/gameplay nodes.");
    }

    private void BuildReception(Node3D root)
    {
        AddRug(root, "Entrance_Runner", new(0f, 0.031f, 8.6f), new(2.2f, 0.008f, 4.5f), _fadedRed);
        AddBench(root, "Entrance_WaitingBench", new(-5.55f, 0.34f, 8.9f), 2.15f, 0f);
        AddCrateStack(root, "Entrance_LuggageStack", new(5.55f, 0.08f, 9.25f), 0.15f);
        AddCylinder(root, "Entrance_UmbrellaStand", new(5.7f, 0.32f, 7.55f), 0.22f, 0.62f, _metal);
        AddBox(root, "Entrance_Umbrella_A", new(5.7f, 0.82f, 7.55f), new(0.035f, 1.05f, 0.035f), _darkWood, new(0f, 0f, 0.12f));
        AddBox(root, "Entrance_Umbrella_B", new(5.62f, 0.78f, 7.55f), new(0.035f, 0.95f, 0.035f), _darkWood, new(0f, 0f, -0.09f));
        AddPicture(root, "Entrance_HouseRules", new(-6.82f, 1.45f, 7.3f), Mathf.Pi * 0.5f, 0.78f, 1.05f, -0.03f, _paper);
        AddPicture(root, "Entrance_FadedPortrait", new(6.82f, 1.55f, 6.85f), -Mathf.Pi * 0.5f, 0.72f, 0.92f, 0.04f);

        AddBox(root, "CashRegister_Base", new(3.25f, 1.14f, -3.48f), new(0.42f, 0.2f, 0.3f), _metal);
        AddBox(root, "CashRegister_Keys", new(3.25f, 1.255f, -3.39f), new(0.32f, 0.025f, 0.12f), _paper, new(-0.12f, 0f, 0f));
        AddPaper(root, "GuestPaper_A", new(3.72f, 1.13f, -3.47f), 0.18f);
        AddPaper(root, "GuestPaper_B", new(3.88f, 1.135f, -3.52f), -0.12f);
        AddPicture(root, "CrookedCrucifix", new(4.86f, 1.72f, -2.1f), Mathf.Pi * 0.5f, 0.3f, 0.52f, 0.12f);
        AddBox(root, "AgedCounterCloth", new(3.78f, 1.15f, -3.7f), new(0.55f, 0.018f, 0.34f), _cloth, new(0f, 0.18f, 0.03f));
        AddShelf(root, "Reception_LedgerShelf", new(4.72f, 1.28f, -5.28f), new(0.85f, 1.55f, 0.28f), Mathf.Pi * 0.5f, 3);
        AddCylinder(root, "Reception_ServiceBell", new(3.0f, 1.19f, -3.45f), 0.075f, 0.09f, _brass);
    }

    private void BuildKitchen(Node3D root)
    {
        AddCylinder(root, "DarkPan_A", new(6.82f, 1.45f, -19.55f), 0.22f, 0.055f, _metal, new(0f, 0f, Mathf.Pi * 0.5f));
        AddCylinder(root, "DarkPan_B", new(6.82f, 1.18f, -19.95f), 0.17f, 0.05f, _metal, new(0f, 0f, Mathf.Pi * 0.5f));
        AddBucket(root, "RustBucket", new(5.95f, 0.2f, -22.2f));
        AddBox(root, "CrookedShelf", new(6.76f, 1.65f, -21.25f), new(0.06f, 0.12f, 1.15f), _darkWood, new(0.06f, 0f, 0.05f));
        AddBox(root, "KitchenCloth", new(4.98f, 0.79f, -19.62f), new(0.52f, 0.02f, 0.48f), _cloth, new(0f, -0.15f, 0.05f));
    }

    private void BuildBathroom(Node3D root)
    {
        AddCylinder(root, "DrainOuterRing", new(-4.5f, 2.907f, 3.5f), 0.34f, 0.025f, _metal);
        AddBox(root, "DrainSlat_A", new(-4.5f, 2.923f, 3.5f), new(0.48f, 0.012f, 0.035f), _darkWood);
        AddBox(root, "DrainSlat_B", new(-4.5f, 2.925f, 3.36f), new(0.48f, 0.012f, 0.035f), _darkWood);
        AddBox(root, "DrainSlat_C", new(-4.5f, 2.925f, 3.64f), new(0.48f, 0.012f, 0.035f), _darkWood);
        AddCylinder(root, "AgedPipe", new(-7.02f, 3.65f, 4.55f), 0.07f, 1.25f, _metal, new(Mathf.Pi * 0.5f, 0f, 0f));
        AddBucket(root, "BathroomBucket", new(-6.3f, 3.05f, 4.75f));
        AddBox(root, "BrokenMirrorShard", new(-6.91f, 4.06f, 2.95f), new(0.02f, 0.42f, 0.26f), _glass, new(0.05f, 0f, -0.2f));
        AddBox(root, "Bathroom_TubBody", new(-6.35f, 3.28f, 2.35f), new(1.25f, 0.7f, 0.62f), _cloth);
        AddBox(root, "Bathroom_TubRim", new(-6.35f, 3.64f, 2.35f), new(1.38f, 0.08f, 0.73f), _metal);
        AddBox(root, "Bathroom_ToiletBase", new(-2.05f, 3.16f, 4.8f), new(0.55f, 0.48f, 0.62f), _cloth);
        AddBox(root, "Bathroom_ToiletTank", new(-2.05f, 3.58f, 5.08f), new(0.62f, 0.68f, 0.3f), _cloth);
        AddBox(root, "Bathroom_TowelRail", new(-6.92f, 4.1f, 4.85f), new(0.035f, 0.055f, 0.9f), _metal);
        AddBox(root, "Bathroom_HangingTowel", new(-6.89f, 3.78f, 4.85f), new(0.025f, 0.62f, 0.64f), _cloth);
    }

    private void BuildTechnicalRoom(Node3D root)
    {
        AddBox(root, "PanelSoot", new(13.885f, 3.94f, 3.7f), new(0.012f, 1.58f, 1.48f), _stain);
        AddBox(root, "PanelBorderTop", new(13.815f, 4.62f, 3.7f), new(0.035f, 0.055f, 1.24f), _brass);
        AddBox(root, "PanelBorderBottom", new(13.815f, 3.28f, 3.7f), new(0.035f, 0.055f, 1.24f), _brass);
        AddBox(root, "FuseLabel_A", new(13.805f, 4.02f, 3.48f), new(0.025f, 0.16f, 0.12f), _paper);
        AddBox(root, "FuseLabel_B", new(13.805f, 4.02f, 3.92f), new(0.025f, 0.16f, 0.12f), _paper);
        AddBox(root, "CableVertical_A", new(13.9f, 3.72f, 2.72f), new(0.035f, 1.35f, 0.025f), _metal, new(0f, 0f, 0.07f));
        AddBox(root, "CableVertical_B", new(13.9f, 3.65f, 4.68f), new(0.03f, 1.15f, 0.025f), _metal, new(0f, 0f, -0.08f));
        AddWorkbench(root, "Technical_Workbench", new(8.25f, 3.26f, 5.12f), 3.4f);
        AddCabinet(root, "Technical_TallCabinet", new(12.85f, 3.72f, 2.15f), new(0.8f, 1.75f, 0.55f), -Mathf.Pi * 0.5f);
        AddCabinet(root, "Technical_Generator", new(11.25f, 3.35f, 4.75f), new(1.45f, 1.0f, 0.75f), 0f);
        AddCylinder(root, "CableCoil", new(6.9f, 3.04f, 4.65f), 0.35f, 0.045f, _metal);
        AddBox(root, "CeilingPipe_A", new(8.0f, 5.28f, 2.05f), new(8.8f, 0.08f, 0.08f), _metal);
        AddBox(root, "CeilingPipe_B", new(9.2f, 5.18f, 2.2f), new(7.0f, 0.06f, 0.06f), _metal);
    }

    private void BuildUpperRooms(Node3D root)
    {
        // Room 201: Sprint 30A replaces the complete two-piece placeholder bed.
        // Keep this old overlay in the explicit hidden rollback container.
        var sprint30ABackup = GetNodeOrNull<Node3D>(
            "../Sprint30A_BlenderAssetPilot/Backup_Placeholders_Sprint30A");
        AddBox(
            sprint30ABackup ?? root,
            "Room201_ThinMattress",
            new(-4.15f, 3.32f, -14.0f),
            new(1.55f, 0.16f, 1.05f),
            _cloth,
            new(0f, 0.07f, 0f));
        AddBox(root, "Room201_Suitcase", new(-5.7f, 3.08f, -12.65f), new(0.62f, 0.32f, 0.42f), _darkWood, new(0f, -0.18f, 0f));
        AddPaper(root, "Room201_FloorPaper", new(-3.0f, 2.9f, -15.25f), 0.22f, floor: true);
        AddPicture(root, "Room201_CrookedPicture", new(-3.7f, 4.3f, -12.08f), 0f, 0.62f, 0.45f, -0.09f);
        AddCabinet(root, "Room201_Wardrobe", new(-6.45f, 3.72f, -12.65f), new(0.72f, 1.72f, 0.58f), 0f);
        AddRug(root, "Room201_FadedRug", new(-4.25f, 2.889f, -15.2f), new(2.3f, 0.006f, 1.15f), _fadedRed);

        // Room 202: the original chair/cabinet stay untouched; these are edge details.
        AddBox(root, "Room202_ClosedBox", new(5.85f, 3.12f, -18.35f), new(0.72f, 0.5f, 0.62f), _darkWood, new(0f, 0.12f, 0f));
        AddBox(root, "Room202_CoveredObject", new(3.2f, 3.35f, -18.2f), new(0.72f, 0.72f, 0.55f), _cloth, new(0f, -0.08f, 0.03f));
        AddPicture(root, "Room202_CrookedPicture", new(4.2f, 4.3f, -18.92f), Mathf.Pi, 0.58f, 0.42f, 0.11f);
        AddIronBed(root, "Room202_SecondBed", new(5.45f, 3.15f, -15.55f), 0.08f);
        AddRug(root, "Room202_FadedRug", new(3.65f, 2.889f, -17.35f), new(1.55f, 0.006f, 1.0f), _fadedRed);

        AddIronBed(root, "Room204_EastBed", new(10.45f, 3.15f, -0.65f), -0.06f);
        AddCabinet(root, "Room204_EastWardrobe", new(13.55f, 3.73f, 0.72f), new(0.78f, 1.78f, 0.62f), -Mathf.Pi * 0.5f);
        AddChair(root, "Room204_WindowChair", new(11.9f, 3.22f, 0.85f), 0.18f);
        AddTrunk(root, "Room204_FootTrunk", new(8.3f, 3.12f, 0.75f), 0.08f);
        AddRug(root, "Room204_CentralRug", new(8.0f, 2.889f, -0.35f), new(3.2f, 0.006f, 1.45f), _fadedRed);
        AddPicture(root, "Room204_EastPortrait", new(11.2f, 4.35f, 1.48f), Mathf.Pi, 0.72f, 0.88f, -0.05f);

        // The former full-height west archive crossed the bathroom/office sightline.
        // Keep storage low and fully inside the office, against its north wall.
        AddCabinet(root, "Office_NorthLowCabinet", new(-1.35f, 3.3f, 7.78f), new(1.45f, 0.8f, 0.35f), 0f);
        AddBookStack(root, "Office_NorthCabinetBooks", new(-1.35f, 3.76f, 7.75f));
        AddRug(root, "Office_ThreadbareRug", new(-4.3f, 2.889f, 6.75f), new(2.8f, 0.006f, 1.35f), _fadedRed);
        AddBookStack(root, "Office_DeskLedgers", new(-4.45f, 3.75f, 7.0f));
    }

    private void BuildRoom203(Node3D root)
    {
        AddBox(root, "TornPaper_A", new(-2.75f, 2.91f, -9.55f), new(0.24f, 0.008f, 0.18f), _paper, new(0f, 0.24f, 0f));
        AddBox(root, "TornPaper_B", new(-3.05f, 2.912f, -9.72f), new(0.18f, 0.008f, 0.14f), _paper, new(0f, -0.14f, 0f));
        AddPicture(root, "Room203_WallSymbol", new(-5.025f, 4.48f, -9.65f), Mathf.Pi * 0.5f, 0.62f, 0.7f, 0.06f, _oldBlood);
        AddBox(root, "Room203_TornCurtain", new(-5.0f, 4.12f, -10.82f), new(0.025f, 1.15f, 0.48f), _cloth, new(0.04f, 0f, 0.08f));
    }

    private void BuildCorridors(Node3D root)
    {
        AddRug(root, "GroundCorridor_Runner", new(0f, 0.031f, -15.5f), new(1.55f, 0.006f, 10.5f), _fadedRed);
        AddBox(root, "GroundCorridor_Radiator", new(1.08f, 0.6f, -11.8f), new(0.18f, 1.0f, 1.35f), _metal);
        AddPicture(root, "GroundHall_Scratches", new(-1.19f, 1.0f, -19.4f), -Mathf.Pi * 0.5f, 0.42f, 0.72f, 0.04f, _oldBlood);
        AddBox(root, "UpperCeilingWire", new(0.78f, 5.29f, -17.2f), new(0.025f, 0.025f, 1.3f), _metal, new(0.04f, 0.08f, 0f));
        AddBox(root, "LeaningBoard_BackHall", new(1.08f, 0.75f, -26.2f), new(0.12f, 1.45f, 0.1f), _darkWood, new(0f, 0f, -0.16f));
        AddCrateStack(root, "StairFoot_TravelCases", new(-5.9f, 0.08f, -26.0f), 0.12f);
        AddPicture(root, "StairFoot_Landscape", new(-6.82f, 1.45f, -25.15f), Mathf.Pi * 0.5f, 0.92f, 0.62f, 0.03f);

        // Arrival composition: one focal wall and one isolated clock. Large props
        // no longer compete in the middle of the landing or at the stair exit.
        AddRug(root, "UpperArrival_Rug", new(-2.05f, 2.889f, -21.0f), new(3.55f, 0.006f, 1.85f), _fadedRed);
        AddBench(root, "UpperArrival_Settee", new(-2.72f, 3.18f, -19.78f), 1.55f, 0f);
        AddPicture(root, "UpperArrival_FamilyPortrait", new(-2.72f, 4.48f, -19.56f), Mathf.Pi, 1.0f, 0.72f, -0.025f);
        AddChandelier(root, "UpperArrival_Chandelier", new(-2.05f, 5.38f, -21.0f));

        // East side of the stair hall, shown empty in the follow-up playtest.
        // Everything hugs the outside wall; the broad route around the stair stays open.
        AddRug(root, "UpperEastLounge_Rug", new(4.55f, 2.889f, -26.7f), new(3.25f, 0.006f, 5.7f), _fadedRed);
        AddFloorOratory(root, "UpperEastLounge_FloorOratory", new(6.48f, 2.89f, -21.1f), Mathf.Pi * 0.5f);
        AddGrandfatherClock(root, "UpperEastLounge_GrandfatherClock", new(6.5f, 2.89f, -23.3f), Mathf.Pi * 0.5f);
        AddBench(root, "UpperEastLounge_Settee", new(6.42f, 3.18f, -25.55f), 1.85f, Mathf.Pi * 0.5f);
        AddSideTable(root, "UpperEastLounge_SideTable", new(5.72f, 2.89f, -24.45f));
        AddTrunk(root, "UpperEastLounge_TravelTrunk", new(6.2f, 2.89f, -27.1f), Mathf.Pi * 0.5f);
        AddPicture(root, "UpperEastLounge_Portrait", new(6.82f, 4.35f, -25.55f), -Mathf.Pi * 0.5f, 0.82f, 1.0f, -0.035f);
        AddPicture(root, "UpperEastLounge_MourningPortrait", new(6.82f, 4.3f, -28.45f), -Mathf.Pi * 0.5f, 0.66f, 0.92f, 0.045f);

        AddArmchair(root, "UpperEastLounge_ArmchairNorth", new(3.35f, 2.89f, -25.75f), 0f);
        AddArmchair(root, "UpperEastLounge_ArmchairSouth", new(3.35f, 2.89f, -27.6f), Mathf.Pi);
        AddRoundTable(root, "UpperEastLounge_CardTable", new(3.35f, 2.89f, -26.68f));
        AddBookStack(root, "UpperEastLounge_CardTableBooks", new(3.35f, 3.66f, -26.68f));
        AddCabinet(root, "UpperEastLounge_NorthCabinet", new(4.85f, 3.64f, -31.02f), new(1.5f, 1.48f, 0.48f), 0f);
        AddCoatStand(root, "UpperEastLounge_CoatStand", new(6.25f, 2.89f, -29.65f));

        // Two independent visual pockets opposite Room 203. They provide rhythm
        // without placing floor props in the narrow central corridor.
        AddArmchair(root, "Upper203Opposite_ReadingChair", new(6.22f, 2.89f, -12.35f), -Mathf.Pi * 0.5f);
        AddSideTable(root, "Upper203Opposite_SideTable", new(6.08f, 2.89f, -11.35f));
        AddBookStack(root, "Upper203Opposite_Books", new(6.08f, 3.56f, -11.35f));
        AddPicture(root, "Upper203Opposite_Landscape", new(6.82f, 4.3f, -12.3f), -Mathf.Pi * 0.5f, 0.9f, 0.62f, 0.025f);

        AddCrateStack(root, "Upper203Opposite_Luggage", new(6.28f, 2.89f, -8.75f), -0.08f);
        AddPicture(root, "Upper203Opposite_GuestPortrait", new(6.82f, 4.28f, -9.15f), -Mathf.Pi * 0.5f, 0.62f, 0.82f, -0.04f);

        AddRug(root, "UpperCorridor_Runner", new(0f, 2.889f, -15.15f), new(1.25f, 0.006f, 7.75f), _fadedRed);
        AddCeilingFixture(root, "UpperCorridor_FixtureNear", new(0f, 5.32f, -12.7f));
        AddCeilingFixture(root, "UpperCorridor_FixtureFar", new(0f, 5.32f, -17.25f));
    }

    private void BuildStains(Node3D root)
    {
        AddFloorStain(root, "Kitchen_DarkFloorStain", new(5.65f, 0.032f, -20.75f), new(0.85f, 0.008f, 0.52f), 0.22f, _stain);
        AddFloorStain(root, "Bathroom_WaterStain", new(-4.5f, 2.887f, 3.5f), new(1.05f, 0.006f, 0.78f), -0.12f, _mold);
        AddFloorStain(root, "Room202_DarkMark", new(4.75f, 2.887f, -17.45f), new(0.95f, 0.006f, 0.52f), 0.18f, _stain);
        AddFloorStain(root, "Balcony_AgedWaterMark", new(0.25f, 2.887f, -6.3f), new(0.7f, 0.006f, 0.4f), -0.09f, _mold);
        AddPicture(root, "Reception_DampPatch", new(-4.89f, 0.85f, -5.2f), -Mathf.Pi * 0.5f, 0.65f, 0.55f, -0.03f, _mold);
    }

    private void BuildCloths(Node3D root)
    {
        AddBox(root, "Bathroom_WetCloth", new(-6.35f, 3.65f, 2.79f), new(0.42f, 0.58f, 0.018f), _cloth, new(0.05f, 0f, 0.12f));
        AddBox(root, "Room201_TornCurtain", new(-6.76f, 4.15f, -14.0f), new(0.025f, 1.05f, 0.5f), _cloth, new(-0.06f, 0f, 0.09f));
        AddBox(root, "Room202_WindowCloth", new(6.76f, 4.15f, -17.0f), new(0.025f, 0.9f, 0.42f), _cloth, new(0.04f, 0f, -0.07f));
        AddBox(root, "Balcony_LooseCloth", new(-0.62f, 2.94f, -6.9f), new(0.5f, 0.018f, 0.38f), _cloth, new(0f, 0.2f, 0.03f));
    }

    private void BuildMaterials()
    {
        _wood = Material(new(0.27f, 0.155f, 0.082f));
        _darkWood = Material(new(0.15f, 0.082f, 0.048f));
        _metal = Material(new(0.12f, 0.13f, 0.13f), metallic: 0.55f);
        _paper = Material(new(0.46f, 0.4f, 0.29f));
        _cloth = Material(new(0.19f, 0.175f, 0.15f, 0.92f), transparent: true);
        _mold = Material(new(0.055f, 0.08f, 0.055f, 0.72f), transparent: true);
        _stain = Material(new(0.055f, 0.038f, 0.03f, 0.74f), transparent: true);
        _oldBlood = Material(new(0.14f, 0.028f, 0.02f, 0.78f), transparent: true);
        _glass = Material(new(0.055f, 0.07f, 0.075f, 0.72f), transparent: true, metallic: 0.2f);
        _fadedRed = Material(new(0.215f, 0.068f, 0.045f, 0.88f), transparent: true);
        _brass = Material(new(0.38f, 0.26f, 0.09f), metallic: 0.5f);
        _upholstery = Material(new(0.19f, 0.205f, 0.145f));
    }

    private void AddRug(Node3D root, string name, Vector3 position, Vector3 size, Material material) =>
        AddBox(root, name, position, size, material);

    private void AddBench(Node3D root, string name, Vector3 position, float width, float yaw)
    {
        var holder = AddHolder(root, name, position, yaw);
        AddBox(holder, "Seat", new(0f, 0.18f, 0f), new(width, 0.14f, 0.48f), _darkWood);
        AddBox(holder, "Back", new(0f, 0.62f, 0.2f), new(width, 0.72f, 0.12f), _darkWood);
        AddBox(holder, "LegA", new(-width * 0.4f, -0.02f, -0.14f), new(0.12f, 0.45f, 0.12f), _wood);
        AddBox(holder, "LegB", new(width * 0.4f, -0.02f, -0.14f), new(0.12f, 0.45f, 0.12f), _wood);
        AddBox(holder, "LegC", new(-width * 0.4f, -0.02f, 0.14f), new(0.12f, 0.45f, 0.12f), _wood);
        AddBox(holder, "LegD", new(width * 0.4f, -0.02f, 0.14f), new(0.12f, 0.45f, 0.12f), _wood);
    }

    private void AddCrateStack(Node3D root, string name, Vector3 position, float yaw)
    {
        var holder = AddHolder(root, name, position, yaw);
        AddBox(holder, "CaseBottom", new(0f, 0.22f, 0f), new(1.0f, 0.44f, 0.62f), _darkWood);
        AddBox(holder, "CaseMiddle", new(0.08f, 0.62f, -0.02f), new(0.82f, 0.34f, 0.54f), _wood, new(0f, 0.06f, 0f));
        AddBox(holder, "CaseTop", new(-0.08f, 0.94f, 0.02f), new(0.64f, 0.28f, 0.46f), _darkWood, new(0f, -0.08f, 0f));
    }

    private void AddShelf(Node3D root, string name, Vector3 position, Vector3 size, float yaw, int shelves)
    {
        var holder = AddHolder(root, name, position, yaw);
        AddBox(holder, "SideA", new(-size.X * 0.47f, 0f, 0f), new(size.X * 0.06f, size.Y, size.Z), _darkWood);
        AddBox(holder, "SideB", new(size.X * 0.47f, 0f, 0f), new(size.X * 0.06f, size.Y, size.Z), _darkWood);
        AddBox(holder, "Top", new(0f, size.Y * 0.48f, 0f), new(size.X, size.Y * 0.05f, size.Z), _darkWood);
        AddBox(holder, "Bottom", new(0f, -size.Y * 0.48f, 0f), new(size.X, size.Y * 0.05f, size.Z), _darkWood);
        for (var i = 1; i <= shelves; i++)
        {
            var y = -size.Y * 0.48f + (size.Y * i / (shelves + 1f));
            AddBox(holder, $"Shelf_{i}", new(0f, y, 0f), new(size.X * 0.94f, size.Y * 0.035f, size.Z), _wood);
        }
    }

    private void AddWorkbench(Node3D root, string name, Vector3 position, float width)
    {
        var holder = AddHolder(root, name, position, 0f);
        AddBox(holder, "Top", new(0f, 0.52f, 0f), new(width, 0.16f, 0.68f), _darkWood);
        AddBox(holder, "Backboard", new(0f, 1.12f, 0.28f), new(width, 0.95f, 0.08f), _wood);
        AddBox(holder, "LegA", new(-width * 0.44f, 0f, 0f), new(0.16f, 1.05f, 0.16f), _metal);
        AddBox(holder, "LegB", new(width * 0.44f, 0f, 0f), new(0.16f, 1.05f, 0.16f), _metal);
        AddBox(holder, "ToolA", new(-0.55f, 1.1f, 0.22f), new(0.08f, 0.55f, 0.035f), _metal, new(0f, 0f, 0.25f));
        AddBox(holder, "ToolB", new(0.35f, 1.18f, 0.22f), new(0.42f, 0.08f, 0.035f), _metal);
    }

    private void AddCabinet(Node3D root, string name, Vector3 position, Vector3 size, float yaw)
    {
        var holder = AddHolder(root, name, position, yaw);
        AddBox(holder, "Body", Vector3.Zero, size, _darkWood);
        AddBox(holder, "DoorA", new(-size.X * 0.245f, 0f, -size.Z * 0.51f), new(size.X * 0.47f, size.Y * 0.9f, 0.04f), _wood);
        AddBox(holder, "DoorB", new(size.X * 0.245f, 0f, -size.Z * 0.51f), new(size.X * 0.47f, size.Y * 0.9f, 0.04f), _wood);
        AddCylinder(holder, "HandleA", new(-size.X * 0.06f, 0f, -size.Z * 0.56f), 0.035f, 0.08f, _brass, new(Mathf.Pi * 0.5f, 0f, 0f));
        AddCylinder(holder, "HandleB", new(size.X * 0.06f, 0f, -size.Z * 0.56f), 0.035f, 0.08f, _brass, new(Mathf.Pi * 0.5f, 0f, 0f));
    }

    private void AddIronBed(Node3D root, string name, Vector3 position, float yaw)
    {
        var holder = AddHolder(root, name, position, yaw);
        AddBox(holder, "Mattress", new(0f, 0.18f, 0f), new(2.25f, 0.22f, 1.05f), _cloth);
        AddBox(holder, "Frame", new(0f, 0.02f, 0f), new(2.38f, 0.1f, 1.16f), _metal);
        AddBox(holder, "HeadTop", new(-1.13f, 0.72f, 0f), new(0.08f, 0.08f, 1.16f), _metal);
        for (var i = -2; i <= 2; i++)
            AddBox(holder, $"HeadBar_{i + 3}", new(-1.13f, 0.42f, i * 0.22f), new(0.06f, 0.72f, 0.06f), _metal);
    }

    private void AddChair(Node3D root, string name, Vector3 position, float yaw)
    {
        var holder = AddHolder(root, name, position, yaw);
        AddBox(holder, "Seat", new(0f, 0.18f, 0f), new(0.55f, 0.12f, 0.55f), _wood);
        AddBox(holder, "Back", new(0f, 0.62f, 0.23f), new(0.55f, 0.72f, 0.1f), _darkWood);
        foreach (var x in new[] { -0.2f, 0.2f })
        foreach (var z in new[] { -0.2f, 0.2f })
            AddBox(holder, $"Leg_{x}_{z}", new(x, -0.08f, z), new(0.08f, 0.52f, 0.08f), _darkWood);
    }

    private void AddArmchair(Node3D root, string name, Vector3 floorPosition, float yaw)
    {
        var holder = AddHolder(root, name, floorPosition, yaw);
        AddBox(holder, "Seat", new(0f, 0.38f, 0f), new(0.72f, 0.22f, 0.68f), _upholstery);
        AddBox(holder, "Back", new(0f, 0.83f, 0.27f), new(0.72f, 0.78f, 0.16f), _upholstery, new(-0.08f, 0f, 0f));
        AddBox(holder, "ArmA", new(-0.4f, 0.53f, 0f), new(0.12f, 0.36f, 0.68f), _wood);
        AddBox(holder, "ArmB", new(0.4f, 0.53f, 0f), new(0.12f, 0.36f, 0.68f), _wood);
        foreach (var x in new[] { -0.3f, 0.3f })
        foreach (var z in new[] { -0.22f, 0.22f })
            AddBox(holder, $"Leg_{x}_{z}", new(x, 0.12f, z), new(0.08f, 0.24f, 0.08f), _darkWood);
    }

    private void AddSideTable(Node3D root, string name, Vector3 floorPosition)
    {
        var holder = AddHolder(root, name, floorPosition, 0f);
        AddCylinder(holder, "Top", new(0f, 0.62f, 0f), 0.38f, 0.1f, _wood);
        AddBox(holder, "Stem", new(0f, 0.34f, 0f), new(0.11f, 0.54f, 0.11f), _darkWood);
        AddBox(holder, "FootA", new(0f, 0.08f, 0f), new(0.62f, 0.08f, 0.12f), _darkWood);
        AddBox(holder, "FootB", new(0f, 0.08f, 0f), new(0.12f, 0.08f, 0.62f), _darkWood);
    }

    private void AddRoundTable(Node3D root, string name, Vector3 floorPosition)
    {
        var holder = AddHolder(root, name, floorPosition, 0f);
        AddCylinder(holder, "Top", new(0f, 0.72f, 0f), 0.58f, 0.12f, _darkWood);
        AddBox(holder, "Apron", new(0f, 0.62f, 0f), new(0.82f, 0.18f, 0.82f), _wood);
        foreach (var x in new[] { -0.3f, 0.3f })
        foreach (var z in new[] { -0.3f, 0.3f })
            AddBox(holder, $"Leg_{x}_{z}", new(x, 0.31f, z), new(0.09f, 0.62f, 0.09f), _darkWood);
    }

    private void AddCoatStand(Node3D root, string name, Vector3 floorPosition)
    {
        var holder = AddHolder(root, name, floorPosition, 0f);
        AddCylinder(holder, "Base", new(0f, 0.08f, 0f), 0.34f, 0.12f, _darkWood);
        AddCylinder(holder, "Post", new(0f, 0.92f, 0f), 0.055f, 1.72f, _wood);
        for (var i = 0; i < 4; i++)
        {
            var angle = i * Mathf.Pi * 0.5f;
            var direction = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            AddBox(holder, $"Hook_{i}", direction * 0.17f + new Vector3(0f, 1.63f, 0f), new(0.34f, 0.055f, 0.055f), _brass, new(0f, -angle, 0.2f));
        }
    }

    private void AddFloorOratory(Node3D root, string name, Vector3 floorPosition, float yaw)
    {
        var holder = AddHolder(root, name, floorPosition, yaw);
        AddBox(holder, "LowerCabinet", new(0f, 0.42f, 0f), new(1.18f, 0.84f, 0.48f), _darkWood);
        AddBox(holder, "AltarTop", new(0f, 0.9f, -0.05f), new(1.32f, 0.14f, 0.62f), _wood);
        AddBox(holder, "Backboard", new(0f, 1.43f, 0.16f), new(1.05f, 0.94f, 0.12f), _darkWood);
        AddBox(holder, "Canopy", new(0f, 1.94f, 0.11f), new(1.22f, 0.16f, 0.45f), _wood);
        AddBox(holder, "CrossVertical", new(0f, 1.48f, -0.02f), new(0.09f, 0.48f, 0.055f), _brass);
        AddBox(holder, "CrossHorizontal", new(0f, 1.55f, -0.02f), new(0.34f, 0.08f, 0.055f), _brass);
        AddCylinder(holder, "VotiveLeft", new(-0.34f, 1.08f, -0.2f), 0.055f, 0.25f, _paper);
        AddCylinder(holder, "VotiveRight", new(0.34f, 1.08f, -0.2f), 0.055f, 0.25f, _paper);
    }

    private void AddTrunk(Node3D root, string name, Vector3 position, float yaw)
    {
        var holder = AddHolder(root, name, position, yaw);
        AddBox(holder, "Body", new(0f, 0.25f, 0f), new(1.25f, 0.5f, 0.72f), _darkWood);
        AddBox(holder, "Lid", new(0f, 0.55f, 0f), new(1.3f, 0.14f, 0.76f), _wood);
        AddBox(holder, "BandA", new(-0.4f, 0.38f, -0.38f), new(0.08f, 0.62f, 0.04f), _metal);
        AddBox(holder, "BandB", new(0.4f, 0.38f, -0.38f), new(0.08f, 0.62f, 0.04f), _metal);
    }

    private void AddBookStack(Node3D root, string name, Vector3 position)
    {
        AddBox(root, $"{name}_A", position, new(0.52f, 0.08f, 0.34f), _darkWood, new(0f, 0.08f, 0f));
        AddBox(root, $"{name}_B", position + new Vector3(0.03f, 0.09f, 0f), new(0.46f, 0.08f, 0.31f), _paper, new(0f, -0.05f, 0f));
        AddBox(root, $"{name}_C", position + new Vector3(-0.02f, 0.18f, 0.01f), new(0.5f, 0.08f, 0.33f), _darkWood, new(0f, 0.12f, 0f));
    }

    private void AddGrandfatherClock(Node3D root, string name, Vector3 floorPosition, float yaw)
    {
        var holder = AddHolder(root, name, floorPosition, yaw);
        AddBox(holder, "Plinth", new(0f, 0.12f, 0f), new(0.72f, 0.24f, 0.42f), _darkWood);
        AddBox(holder, "Case", new(0f, 0.92f, 0f), new(0.58f, 1.55f, 0.34f), _wood);
        AddBox(holder, "Head", new(0f, 1.83f, 0f), new(0.72f, 0.58f, 0.42f), _darkWood);
        AddCylinder(holder, "ClockFace", new(0f, 1.85f, -0.225f), 0.22f, 0.035f, _paper, new(Mathf.Pi * 0.5f, 0f, 0f));
        AddBox(holder, "PendulumWindow", new(0f, 0.95f, -0.185f), new(0.28f, 0.62f, 0.025f), _glass);
        AddCylinder(holder, "Pendulum", new(0f, 0.78f, -0.205f), 0.09f, 0.025f, _brass, new(Mathf.Pi * 0.5f, 0f, 0f));
    }

    private void AddChandelier(Node3D root, string name, Vector3 ceilingPosition)
    {
        var holder = AddHolder(root, name, ceilingPosition, 0f);
        AddCylinder(holder, "Chain", new(0f, -0.22f, 0f), 0.025f, 0.44f, _metal);
        AddCylinder(holder, "Hub", new(0f, -0.47f, 0f), 0.14f, 0.1f, _brass);
        for (var i = 0; i < 4; i++)
        {
            var angle = i * Mathf.Pi * 0.5f;
            var direction = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            AddBox(holder, $"Arm_{i}", direction * 0.24f + new Vector3(0f, -0.47f, 0f), new(0.48f, 0.035f, 0.035f), _brass, new(0f, -angle, 0f));
            AddCylinder(holder, $"Candle_{i}", direction * 0.47f + new Vector3(0f, -0.35f, 0f), 0.035f, 0.24f, _paper);
        }
    }

    private void AddCeilingFixture(Node3D root, string name, Vector3 position)
    {
        AddCylinder(root, $"{name}_Mount", position, 0.18f, 0.08f, _metal);
        AddCylinder(root, $"{name}_Shade", position + new Vector3(0f, -0.12f, 0f), 0.26f, 0.16f, _brass);
    }

    private static Node3D AddHolder(Node3D root, string name, Vector3 position, float yaw)
    {
        var holder = new Node3D { Name = name, Position = position, Rotation = new(0f, yaw, 0f) };
        root.AddChild(holder);
        return holder;
    }

    private void AddPaper(Node3D root, string name, Vector3 position, float yaw, bool floor = false) =>
        AddBox(root, name, position, new(0.28f, floor ? 0.006f : 0.012f, 0.2f), _paper, new(0f, yaw, 0f));

    private void AddBucket(Node3D root, string name, Vector3 position)
    {
        AddCylinder(root, $"{name}_Body", position, 0.25f, 0.36f, _metal);
        AddCylinder(root, $"{name}_Rim", position + new Vector3(0f, 0.19f, 0f), 0.285f, 0.035f, _metal);
    }

    private void AddPicture(Node3D root, string name, Vector3 position, float yaw, float width, float height, float roll, Material? face = null)
    {
        var holder = new Node3D { Name = name, Position = position, Rotation = new(0f, yaw, roll) };
        root.AddChild(holder);
        AddBox(holder, "Frame", Vector3.Zero, new(width, height, 0.035f), _darkWood);
        AddBox(holder, "Face", new(0f, 0f, 0.022f), new(width - 0.08f, height - 0.08f, 0.012f), face ?? _stain);
    }

    private void AddFloorStain(Node3D root, string name, Vector3 position, Vector3 size, float yaw, Material material) =>
        AddBox(root, name, position, size, material, new(0f, yaw, 0f));

    private static MeshInstance3D AddBox(Node3D root, string name, Vector3 position, Vector3 size, Material material, Vector3? rotation = null)
    {
        var mesh = new MeshInstance3D { Name = name, Position = position, Rotation = rotation ?? Vector3.Zero,
            Mesh = new BoxMesh { Size = size, Material = material }, CastShadow = GeometryInstance3D.ShadowCastingSetting.Off };
        root.AddChild(mesh);
        return mesh;
    }

    private static MeshInstance3D AddCylinder(Node3D root, string name, Vector3 position, float radius, float height, Material material, Vector3? rotation = null)
    {
        var mesh = new MeshInstance3D { Name = name, Position = position, Rotation = rotation ?? Vector3.Zero,
            Mesh = new CylinderMesh { TopRadius = radius, BottomRadius = radius * 0.92f, Height = height, RadialSegments = 12, Material = material },
            CastShadow = GeometryInstance3D.ShadowCastingSetting.Off };
        root.AddChild(mesh);
        return mesh;
    }

    private static StandardMaterial3D Material(Color color, bool transparent = false, float metallic = 0f) => new()
    {
        AlbedoColor = color,
        Roughness = 0.86f,
        Metallic = metallic,
        Transparency = transparent ? BaseMaterial3D.TransparencyEnum.Alpha : BaseMaterial3D.TransparencyEnum.Disabled
    };
}
