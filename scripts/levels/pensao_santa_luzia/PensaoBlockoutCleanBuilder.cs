namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Constroi geometria 100% Godot da Pensao Santa Luzia: visuals + colisoes limpas.
/// Eixo: +Z trilha/exterior, -Z interior. Y altura. Paredes 3.0m.
/// </summary>
public partial class PensaoBlockoutCleanBuilder : Node3D
{
    private const float Floor02Y = 3.32f;
    private const float Floor02Thickness = 0.25f;
    private const float Floor02UpperRoofY = 7.8f;
    private const float UpperCorridorHalfWidth = 1.4f;
    private const float RampWidth = 2.0f;
    // Poco da escada — volume livre (head clearance >= 2.4m acima da rampa).
    private const float StairwellXMin = 3.2f;
    private const float StairwellXMax = 7.3f;
    private const float StairwellZMin = -12.5f;
    private const float StairwellZMax = -3.8f;
    private const float Floor02LandingZMin = -3.8f;
    private const float Floor02LandingZMax = -2.2f;
    private const float StairRampBottomY = 0.28f;
    private const float StairRampTopY = 3.27f;
    private const float StairRampBottomZ = -13.2f;
    private const float StairRampTopZ = -3.8f;

    private Material? _groundMat;
    private Material? _trailMat;
    private Material? _woodMat;
    private Material? _extWallMat;
    private Material? _intWallMat;
    private Material? _floorMat;
    private Material? _roofMat;
    private Material? _propMat;

    public override void _Ready()
    {
        LoadMaterials();
        BuildExterior();
        BuildFloor01();
        BuildFloor02();
        BuildCollisions();
    }

    private void LoadMaterials()
    {
        _groundMat = GD.Load<Material>("res://materials/blockout/mat_blockout_ground.tres");
        _trailMat = GD.Load<Material>("res://materials/blockout/mat_blockout_trail.tres");
        _woodMat = GD.Load<Material>("res://materials/blockout/mat_blockout_wood.tres");
        _extWallMat = GD.Load<Material>("res://materials/blockout/mat_blockout_wall_exterior.tres");
        _intWallMat = GD.Load<Material>("res://materials/blockout/mat_blockout_wall_interior.tres");
        _floorMat = GD.Load<Material>("res://materials/blockout/mat_blockout_floor_interior.tres");
        _roofMat = GD.Load<Material>("res://materials/blockout/mat_blockout_roof.tres");
        _propMat = GD.Load<Material>("res://materials/blockout/mat_blockout_prop.tres");
    }

    private void BuildExterior()
    {
        var exterior = GetNode<Node3D>("Exterior");

        BlockoutSolid.CreateVisualOnly(exterior, "Ground", new Vector3(0, -0.08f, 2), new Vector3(30, 0.2f, 58), _groundMat);
        BlockoutSolid.CreateVisualOnly(exterior, "Trail", new Vector3(0, 0.04f, 8), new Vector3(4, 0.16f, 31), _trailMat);
        BlockoutSolid.CreateVisualOnly(exterior, "PorchDeck", new Vector3(0, 0.21f, -2.65f), new Vector3(15.2f, 0.22f, 2.8f), _woodMat);

        BlockoutSolid.CreateVisualOnly(exterior, "FrontWallLeft", new Vector3(-4.025f, 1.6f, -4), new Vector3(5.95f, 3f, 0.22f), _extWallMat);
        BlockoutSolid.CreateVisualOnly(exterior, "FrontWallRight", new Vector3(4.025f, 1.6f, -4), new Vector3(5.95f, 3f, 0.22f), _extWallMat);
        BlockoutSolid.CreateVisualOnly(exterior, "SideWallLeft", new Vector3(-7f, 1.6f, -14), new Vector3(0.22f, 3f, 20f), _extWallMat);
        BlockoutSolid.CreateVisualOnly(exterior, "SideWallRight", new Vector3(7f, 1.6f, -14), new Vector3(0.22f, 3f, 20f), _extWallMat);
        BlockoutSolid.CreateVisualOnly(exterior, "BackWall", new Vector3(0, 1.6f, -24), new Vector3(14f, 3f, 0.22f), _extWallMat);

        // Telhado térreo dividido — abertura sobre o poco da escada (evita laje na cabeca do player).
        BlockoutSolid.CreateVisualOnly(exterior, "Roof_LeftSection", new Vector3(-2.1f, 3.15f, -14f),
            new Vector3(9.8f, 0.3f, 20.4f), _roofMat);
        BlockoutSolid.CreateVisualOnly(exterior, "Roof_RightBack", new Vector3(5.2f, 3.15f, -18.25f),
            new Vector3(3.6f, 0.3f, 11.5f), _roofMat);
        // Recuado para nao cobrir o topo da rampa (Z > -3.8).
        BlockoutSolid.CreateVisualOnly(exterior, "Roof_RightFrontLanding", new Vector3(5.2f, 3.15f, -1.35f),
            new Vector3(3.6f, 0.3f, 1.1f), _roofMat);
        // StairwellCeilingOpening — vazio intencional acima da rampa.

        BlockoutSolid.CreateVisualOnly(exterior, "FenceLeft", new Vector3(-10f, 0.8f, 4), new Vector3(0.08f, 1.6f, 18), _woodMat);
        BlockoutSolid.CreateVisualOnly(exterior, "FenceRight", new Vector3(10f, 0.8f, 4), new Vector3(0.08f, 1.6f, 18), _woodMat);

        BlockoutSolid.CreateSign(exterior, "PensaoSign", new Vector3(0, 2.55f, -3.2f), "PENSAO SANTA LUZIA", 42);
        BlockoutSolid.CreateSign(exterior, "ReceptionSign", new Vector3(-5.1f, 2.15f, -7f), "RECEPCAO", 34);
    }

    private void BuildFloor01()
    {
        var floor01 = GetNode<Node3D>("Pension/Floor01");

        BlockoutSolid.CreateVisualOnly(floor01, "MainFloor", new Vector3(0, 0.17f, -14), new Vector3(14, 0.18f, 20), _floorMat);

        BuildInteriorWalls(floor01, 1.48f, false);

        BlockoutSolid.CreateVisualOnly(floor01, "ReceptionCounter", new Vector3(-5.1f, 0.64f, -7.2f), new Vector3(2.2f, 1f, 0.75f), _woodMat);
        BlockoutSolid.CreateVisualOnly(floor01, "KeyBoard", new Vector3(-5.5f, 1.35f, -6.4f), new Vector3(0.8f, 0.5f, 0.08f), _propMat);
        BlockoutSolid.CreateVisualOnly(floor01, "GuestBook", new Vector3(-4.7f, 0.95f, -7.1f), new Vector3(0.5f, 0.08f, 0.35f), _propMat);

        BlockoutSolid.CreateVisualOnly(floor01, "Room102Bed", new Vector3(-4.5f, 0.45f, -15.5f), new Vector3(2f, 0.5f, 1.1f), _woodMat);
        BlockoutSolid.CreateVisualOnly(floor01, "Room102Table", new Vector3(-5.2f, 0.55f, -13.5f), new Vector3(0.7f, 0.6f, 0.5f), _propMat);
        BlockoutSolid.CreateSign(floor01, "Room102Sign", new Vector3(-3.15f, 2f, -12.45f), "102", 36);

        BlockoutSolid.CreateVisualOnly(floor01, "KitchenCounter", new Vector3(4.8f, 0.7f, -14.5f), new Vector3(2.4f, 0.9f, 0.6f), _woodMat);
        BlockoutSolid.CreateSign(floor01, "KitchenSign", new Vector3(4.2f, 2f, -13.2f), "COZINHA", 28);

        BlockoutSolid.CreateVisualOnly(floor01, "DepositDoorVisual", new Vector3(1.46f, 1.15f, -18.05f), new Vector3(0.12f, 2.3f, 1.4f), _woodMat);
        BlockoutSolid.CreateSign(floor01, "DepositSign", new Vector3(1.75f, 2.05f, -18f), "DEPOSITO", 30);

        var (rampCenterY, rampCenterZ, rampLength, rampAngle) = ComputeStairRamp();
        BlockoutSolid.CreateVisualOnly(floor01, "StairVisual", new Vector3(5.1f, rampCenterY, rampCenterZ),
            new Vector3(RampWidth, 0.25f, rampLength), _woodMat, new Vector3(rampAngle, 0, 0));
    }

    private static (float centerY, float centerZ, float length, float angleX) ComputeStairRamp()
    {
        var rise = StairRampTopY - StairRampBottomY;
        var run = StairRampTopZ - StairRampBottomZ;
        var length = MathF.Sqrt(rise * rise + run * run);
        var angle = -MathF.Asin(rise / length);
        var centerY = (StairRampBottomY + StairRampTopY) * 0.5f;
        var centerZ = (StairRampBottomZ + StairRampTopZ) * 0.5f;
        return (centerY, centerZ, length, angleX: angle);
    }

    private static float LandingCenterZ() => (Floor02LandingZMin + Floor02LandingZMax) * 0.5f;
    private static float LandingDepth() => Floor02LandingZMax - Floor02LandingZMin;

    private void BuildFloor02()
    {
        var floor02 = GetNode<Node3D>("Pension/Floor02");

        // --- Pisos caminháveis (blocos separados; StairwellOpening fica vazio) ---
        AddFloor02Visual(floor02, "Floor02_LeftSection", -1.8f, -14f, 10.4f, 20.4f);
        AddFloor02Visual(floor02, "Floor02_EastEdge", 2.05f, -13.75f, 1.3f, 18.5f);
        AddFloor02Visual(floor02, "Floor02_LandingSection", 5.2f, LandingCenterZ(), 3.6f, LandingDepth());
        AddFloor02Visual(floor02, "Floor02_BackSection", 5.2f, -18.25f, 3.6f, 11.5f);
        AddFloor02Visual(floor02, "Floor02_StairLandingBridge", 3.05f, -3.05f, 1.5f, 1.8f);
        AddFloor02Visual(floor02, "Floor02_RightRoomFloor", 5.2f, -22.2f, 3.6f, 3.8f);

        // StairwellOpening — vao real (X 3.2–7.3, Z -12.5 a -3.8), sem mesh.

        BlockoutSolid.CreateVisualOnly(floor02, "UpperFrontWall", new Vector3(0, 4.65f, -4), new Vector3(14f, 3f, 0.22f), _extWallMat);
        BlockoutSolid.CreateVisualOnly(floor02, "UpperLeftWall", new Vector3(-7f, 4.65f, -14), new Vector3(0.22f, 3f, 20f), _extWallMat);
        BlockoutSolid.CreateVisualOnly(floor02, "UpperRightWall", new Vector3(7f, 4.65f, -14), new Vector3(0.22f, 3f, 20f), _extWallMat);
        BlockoutSolid.CreateVisualOnly(floor02, "UpperBackWall", new Vector3(0, 4.65f, -24), new Vector3(14f, 3f, 0.22f), _extWallMat);
        BlockoutSolid.CreateVisualOnly(floor02, "UpperRoof", new Vector3(0, Floor02UpperRoofY, -14),
            new Vector3(14.4f, 0.3f, 20.4f), _roofMat);

        BuildUpperInteriorWalls(floor02);

        BlockoutSolid.CreateVisualOnly(floor02, "ManagerDesk", new Vector3(-5f, 3.75f, -22.2f), new Vector3(1.6f, 0.8f, 0.9f), _woodMat);
        BlockoutSolid.CreateVisualOnly(floor02, "ManagerDocs", new Vector3(-4.6f, 4.15f, -22.5f), new Vector3(0.6f, 0.08f, 0.4f), _propMat);
        BlockoutSolid.CreateSign(floor02, "ManagerSign", new Vector3(-5.2f, 4.6f, -21.5f), "GERENTE", 28);

        BlockoutSolid.CreateVisualOnly(floor02, "BathroomSink", new Vector3(-3.8f, 3.85f, -11.5f), new Vector3(0.6f, 0.5f, 0.4f), _propMat);
        BlockoutSolid.CreateVisualOnly(floor02, "BathroomToilet", new Vector3(-4.8f, 3.55f, -12.2f), new Vector3(0.5f, 0.6f, 0.6f), _propMat);
        BlockoutSolid.CreateSign(floor02, "BathroomSign", new Vector3(-3.2f, 4.3f, -10.8f), "BANHEIRO", 24);

        BlockoutSolid.CreateVisualOnly(floor02, "LockedDoor", new Vector3(4.1f, 4.15f, -22.2f), new Vector3(0.12f, 2.3f, 1.4f), _woodMat);

        BlockoutSolid.CreateVisualOnly(floor02, "StairwellGuardLeft", new Vector3(StairwellXMin - 0.05f, 3.75f, -8.2f),
            new Vector3(0.08f, 0.85f, 4.2f), _woodMat);
        BlockoutSolid.CreateVisualOnly(floor02, "StairwellGuardRight", new Vector3(StairwellXMax + 0.05f, 3.75f, -8.2f),
            new Vector3(0.08f, 0.85f, 4.2f), _woodMat);
        BlockoutSolid.CreateVisualOnly(floor02, "StairwellGuardBack", new Vector3(5.25f, 3.75f, StairwellZMin - 0.05f),
            new Vector3(3.8f, 0.85f, 0.08f), _woodMat);
    }

    private void BuildUpperInteriorWalls(Node3D floor02)
    {
        const float wallY = 4.47f;
        var hw = UpperCorridorHalfWidth;
        BlockoutSolid.CreateVisualOnly(floor02, "UpperCorridorLeftA", new Vector3(-hw, wallY, -8.5f),
            new Vector3(0.22f, 2.45f, 7f), _intWallMat);
        BlockoutSolid.CreateVisualOnly(floor02, "UpperCorridorLeftB", new Vector3(-hw, wallY, -18.65f),
            new Vector3(0.22f, 2.45f, 9.3f), _intWallMat);
        BlockoutSolid.CreateVisualOnly(floor02, "UpperCorridorRightA", new Vector3(hw, wallY, -8.6f),
            new Vector3(0.22f, 2.45f, 7f), _intWallMat);
        BlockoutSolid.CreateVisualOnly(floor02, "UpperCorridorRightB", new Vector3(hw, wallY, -18.75f),
            new Vector3(0.22f, 2.45f, 9.3f), _intWallMat);
        BlockoutSolid.CreateVisualOnly(floor02, "LockedRoomPartition", new Vector3(4.12f, wallY, -22.2f),
            new Vector3(5.75f, 2.45f, 0.22f), _intWallMat);
    }

    private void AddFloor02Visual(Node3D parent, string name, float cx, float cz, float sx, float sz)
    {
        BlockoutSolid.CreateVisualOnly(parent, name, new Vector3(cx, Floor02Y, cz),
            new Vector3(sx, 0.18f, sz), _floorMat);
    }

    private void AddFloor02Collision(Node3D parent, string name, float cx, float cz, float sx, float sz)
    {
        BlockoutSolid.CreateCollisionOnly(parent, name, new Vector3(cx, Floor02Y, cz),
            new Vector3(sx, Floor02Thickness, sz));
    }

    private void BuildInteriorWalls(Node3D parent, float wallY, bool upperFloor)
    {
        if (!upperFloor)
        {
            BlockoutSolid.CreateVisualOnly(parent, "CorridorLeftA", new Vector3(-1.25f, wallY, -10.9f), new Vector3(0.22f, 2.75f, 1.3f), _intWallMat);
            BlockoutSolid.CreateVisualOnly(parent, "CorridorLeftB", new Vector3(-1.25f, wallY, -14.9f), new Vector3(0.22f, 2.75f, 3.3f), _intWallMat);
            BlockoutSolid.CreateVisualOnly(parent, "CorridorLeftC", new Vector3(-1.25f, wallY, -20.95f), new Vector3(0.22f, 2.75f, 5.1f), _intWallMat);
            BlockoutSolid.CreateVisualOnly(parent, "CorridorRightA", new Vector3(1.25f, wallY, -11.05f), new Vector3(0.22f, 2.75f, 1.3f), _intWallMat);
            BlockoutSolid.CreateVisualOnly(parent, "CorridorRightB", new Vector3(1.25f, wallY, -15.35f), new Vector3(0.22f, 2.75f, 3.3f), _intWallMat);
            BlockoutSolid.CreateVisualOnly(parent, "CorridorRightC", new Vector3(1.25f, wallY, -21.25f), new Vector3(0.22f, 2.75f, 5.1f), _intWallMat);
            BlockoutSolid.CreateVisualOnly(parent, "Room102Back", new Vector3(-4.12f, wallY, -15.8f), new Vector3(5.75f, 2.75f, 0.22f), _intWallMat);
            BlockoutSolid.CreateVisualOnly(parent, "KitchenBack", new Vector3(4.12f, wallY, -16.8f), new Vector3(5.75f, 2.75f, 0.22f), _intWallMat);
            BlockoutSolid.CreateVisualOnly(parent, "DepositBack", new Vector3(4.12f, wallY, -21.4f), new Vector3(5.75f, 2.75f, 0.22f), _intWallMat);
            return;
        }

        BlockoutSolid.CreateVisualOnly(parent, "UpperCorridorLeftA", new Vector3(-1.25f, wallY, -8.5f), new Vector3(0.22f, 2.45f, 7f), _intWallMat);
        BlockoutSolid.CreateVisualOnly(parent, "UpperCorridorLeftB", new Vector3(-1.25f, wallY, -18.65f), new Vector3(0.22f, 2.45f, 9.3f), _intWallMat);
        BlockoutSolid.CreateVisualOnly(parent, "UpperCorridorRightA", new Vector3(1.25f, wallY, -8.6f), new Vector3(0.22f, 2.45f, 7f), _intWallMat);
        BlockoutSolid.CreateVisualOnly(parent, "UpperCorridorRightB", new Vector3(1.25f, wallY, -18.75f), new Vector3(0.22f, 2.45f, 9.3f), _intWallMat);
        BlockoutSolid.CreateVisualOnly(parent, "LockedRoomPartition", new Vector3(4.12f, wallY, -22.2f), new Vector3(5.75f, 2.45f, 0.22f), _intWallMat);
    }

    private void BuildCollisions()
    {
        var collisions = GetParent()?.GetNode<Node3D>("StaticGameplayCollisions")
            ?? throw new InvalidOperationException("StaticGameplayCollisions nao encontrado.");

        BlockoutSolid.CreateBox(collisions, "ExteriorGroundCollision", new Vector3(0, -0.08f, 2), new Vector3(30, 0.2f, 58), null);
        BlockoutSolid.CreateBox(collisions, "CentralPathCollision", new Vector3(0, 0.04f, 8), new Vector3(4, 0.16f, 31), null);
        BlockoutSolid.CreateBox(collisions, "PorchCollision", new Vector3(0, 0.21f, -2.65f), new Vector3(15.2f, 0.22f, 2.8f), null);
        BlockoutSolid.CreateBox(collisions, "InteriorFloor01Collision", new Vector3(0, 0.17f, -14), new Vector3(14, 0.18f, 20), null);
        BuildFloor02SafeWalkableCollisions(collisions);

        var exteriorWalls = new Node3D { Name = "ExteriorWallCollisions" };
        collisions.AddChild(exteriorWalls);
        BlockoutSolid.CreateBox(exteriorWalls, "FrontLeft", new Vector3(-4.025f, 1.6f, -4), new Vector3(5.95f, 3f, 0.22f), _extWallMat);
        BlockoutSolid.CreateBox(exteriorWalls, "FrontRight", new Vector3(4.025f, 1.6f, -4), new Vector3(5.95f, 3f, 0.22f), _extWallMat);
        BlockoutSolid.CreateBox(exteriorWalls, "Left", new Vector3(-7f, 1.6f, -14), new Vector3(0.22f, 3f, 20f), _extWallMat);
        BlockoutSolid.CreateBox(exteriorWalls, "Right", new Vector3(7f, 1.6f, -14), new Vector3(0.22f, 3f, 20f), _extWallMat);
        BlockoutSolid.CreateBox(exteriorWalls, "Back", new Vector3(0, 1.6f, -24), new Vector3(14f, 3f, 0.22f), _extWallMat);
        BlockoutSolid.CreateBox(exteriorWalls, "UpperFront", new Vector3(0, 4.65f, -4), new Vector3(14f, 3f, 0.22f), _extWallMat);
        BlockoutSolid.CreateBox(exteriorWalls, "UpperLeft", new Vector3(-7f, 4.65f, -14), new Vector3(0.22f, 3f, 20f), _extWallMat);
        BlockoutSolid.CreateBox(exteriorWalls, "UpperRight", new Vector3(7f, 4.65f, -14), new Vector3(0.22f, 3f, 20f), _extWallMat);
        BlockoutSolid.CreateBox(exteriorWalls, "UpperBack", new Vector3(0, 4.65f, -24), new Vector3(14f, 3f, 0.22f), _extWallMat);

        var interiorWalls = new Node3D { Name = "InteriorWallCollisions" };
        collisions.AddChild(interiorWalls);
        BlockoutSolid.CreateBox(interiorWalls, "LeftA", new Vector3(-1.25f, 1.48f, -10.9f), new Vector3(0.22f, 2.75f, 1.3f), _intWallMat);
        BlockoutSolid.CreateBox(interiorWalls, "LeftB", new Vector3(-1.25f, 1.48f, -14.9f), new Vector3(0.22f, 2.75f, 3.3f), _intWallMat);
        BlockoutSolid.CreateBox(interiorWalls, "LeftC", new Vector3(-1.25f, 1.48f, -20.95f), new Vector3(0.22f, 2.75f, 5.1f), _intWallMat);
        BlockoutSolid.CreateBox(interiorWalls, "RightA", new Vector3(1.25f, 1.48f, -11.05f), new Vector3(0.22f, 2.75f, 1.3f), _intWallMat);
        BlockoutSolid.CreateBox(interiorWalls, "RightB", new Vector3(1.25f, 1.48f, -15.35f), new Vector3(0.22f, 2.75f, 3.3f), _intWallMat);
        BlockoutSolid.CreateBox(interiorWalls, "RightC", new Vector3(1.25f, 1.48f, -21.25f), new Vector3(0.22f, 2.75f, 5.1f), _intWallMat);
        BlockoutSolid.CreateBox(interiorWalls, "Room102Back", new Vector3(-4.12f, 1.48f, -15.8f), new Vector3(5.75f, 2.75f, 0.22f), _intWallMat);
        BlockoutSolid.CreateBox(interiorWalls, "KitchenBack", new Vector3(4.12f, 1.48f, -16.8f), new Vector3(5.75f, 2.75f, 0.22f), _intWallMat);
        BlockoutSolid.CreateBox(interiorWalls, "DepositBack", new Vector3(4.12f, 1.48f, -21.4f), new Vector3(5.75f, 2.75f, 0.22f), _intWallMat);
        BlockoutSolid.CreateBox(interiorWalls, "UpperLeftA", new Vector3(-UpperCorridorHalfWidth, 4.47f, -8.5f), new Vector3(0.22f, 2.45f, 7f), _intWallMat);
        BlockoutSolid.CreateBox(interiorWalls, "UpperLeftB", new Vector3(-UpperCorridorHalfWidth, 4.47f, -18.65f), new Vector3(0.22f, 2.45f, 9.3f), _intWallMat);
        BlockoutSolid.CreateBox(interiorWalls, "UpperRightA", new Vector3(UpperCorridorHalfWidth, 4.47f, -8.6f), new Vector3(0.22f, 2.45f, 7f), _intWallMat);
        BlockoutSolid.CreateBox(interiorWalls, "UpperRightB", new Vector3(UpperCorridorHalfWidth, 4.47f, -18.75f), new Vector3(0.22f, 2.45f, 9.3f), _intWallMat);
        BlockoutSolid.CreateBox(interiorWalls, "LockedRoomPartition", new Vector3(4.12f, 4.47f, -22.2f), new Vector3(5.75f, 2.45f, 0.22f), _intWallMat);

        BlockoutSolid.CreateBox(collisions, "ReceptionCounterCollision", new Vector3(-5.1f, 0.64f, -7.2f), new Vector3(2.2f, 1f, 0.75f), _woodMat);

        BlockoutSolid.CreateBox(collisions, "DepositDoorCollision", new Vector3(1.46f, 1.125f, -18.05f),
            new Vector3(0.1f, 2.05f, 0.9f), _woodMat);
        BlockoutSolid.CreateBox(collisions, "StairAccessBlocker", new Vector3(5.1f, 1.2f, -6.2f),
            new Vector3(2.2f, 2.4f, 0.25f), _woodMat);

        var (rampCenterY, rampCenterZ, rampLength, rampAngle) = ComputeStairRamp();
        BlockoutSolid.CreateCollisionOnly(collisions, "StairRampCollision", new Vector3(5.1f, rampCenterY, rampCenterZ),
            new Vector3(RampWidth, 0.3f, rampLength), rotation: new Vector3(rampAngle, 0, 0));

        BuildFloor02StairwellGuards(collisions);

        var bounds = new Node3D { Name = "WorldBoundsCollision" };
        collisions.AddChild(bounds);
        BlockoutSolid.CreateBox(bounds, "Left", new Vector3(-15f, 3f, 2f), new Vector3(0.5f, 6f, 58f), null);
        BlockoutSolid.CreateBox(bounds, "Right", new Vector3(15f, 3f, 2f), new Vector3(0.5f, 6f, 58f), null);
        BlockoutSolid.CreateBox(bounds, "Start", new Vector3(0, 3f, 31f), new Vector3(30f, 6f, 0.5f), null);
        BlockoutSolid.CreateBox(bounds, "Rear", new Vector3(0, 3f, -27f), new Vector3(30f, 6f, 0.5f), null);
    }

    /// <summary>
    /// Colisoes completas do 2o andar — cobre marrom escuro e claro, com vao real na escada.
    /// </summary>
    private void BuildFloor02SafeWalkableCollisions(Node3D collisions)
    {
        var safe = new Node3D { Name = "Floor02SafeWalkableCollisions" };
        collisions.AddChild(safe);

        // Mesmas dimensoes dos visuais — colisao 1:1, leve overlap nas bordas.
        AddFloor02Collision(safe, "Floor02_Corridor_Collision", 0f, -14f, UpperCorridorHalfWidth * 2f + 0.2f, 20f);
        AddFloor02Collision(safe, "Floor02_LeftSection_Collision", -1.8f, -14f, 10.4f, 20.4f);
        AddFloor02Collision(safe, "Floor02_EastEdge_Collision", 2.05f, -13.75f, 1.3f, 18.5f);
        AddFloor02Collision(safe, "Floor02_ManagerRoom_Collision", -4.25f, -22f, 5.5f, 5f);
        AddFloor02Collision(safe, "Floor02_Bathroom_Collision", -4f, -11.5f, 3.8f, 4.5f);
        AddFloor02Collision(safe, "Floor02_Landing_Collision", 5.2f, LandingCenterZ(), 3.6f, LandingDepth());
        AddFloor02Collision(safe, "Floor02_BackSection_Collision", 5.2f, -18.25f, 3.6f, 11.5f);
        AddFloor02Collision(safe, "Floor02_RightRoom_Collision", 5.2f, -22.2f, 3.6f, 3.8f);
        AddFloor02Collision(safe, "Floor02_StairLandingBridge_Collision", 3.05f, -3.05f, 1.5f, 1.8f);
    }

    private void BuildFloor02StairwellGuards(Node3D collisions)
    {
        var guards = new Node3D { Name = "Floor02StairwellGuards" };
        collisions.AddChild(guards);

        BlockoutSolid.CreateCollisionOnly(guards, "Floor02_StairwellGuard_Left",
            new Vector3(StairwellXMin - 0.05f, 3.75f, -8.2f), new Vector3(0.08f, 0.85f, 4.2f));
        BlockoutSolid.CreateCollisionOnly(guards, "Floor02_StairwellGuard_Right",
            new Vector3(StairwellXMax + 0.05f, 3.75f, -8.2f), new Vector3(0.08f, 0.85f, 4.2f));
        BlockoutSolid.CreateCollisionOnly(guards, "Floor02_StairwellGuard_Back",
            new Vector3(5.25f, 3.75f, StairwellZMin - 0.05f), new Vector3(3.8f, 0.85f, 0.08f));
    }
}
