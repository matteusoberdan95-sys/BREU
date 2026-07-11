namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>Constroi um blockout unico, fechado e navegavel para substituir o interior importado.</summary>
public partial class PensaoMansionBlockoutBuilder : Node3D
{
    private Node3D _visual = null!;
    private Node3D _collisions = null!;
    private StandardMaterial3D _wall = null!;
    private StandardMaterial3D _floor = null!;
    private StandardMaterial3D _wood = null!;

    public override void _Ready()
    {
        _visual = new Node3D { Name = "Visual" };
        _collisions = new Node3D { Name = "StaticGameplayCollisions" };
        AddChild(_visual);
        AddChild(_collisions);
        _wall = Material(new Color("665a48"));
        _floor = Material(new Color("352015"));
        _wood = Material(new Color("26150d"));
        BuildShell();
        BuildGroundFloor();
        BuildStairs();
        BuildUpperFloor();
        BuildLabels();
    }

    private static StandardMaterial3D Material(Color color) => new()
    {
        AlbedoColor = color,
        Roughness = 0.94f,
    };

    private void BuildShell()
    {
        Solid("Porch", new(0, 0.05f, -8.25f), new(14, 0.2f, 2.5f), _wood);
        Solid("GroundFloor", new(0, 0, -19), new(14, 0.2f, 20), _floor);
        Solid("FrontWallLeft", new(-4.1f, 1.6f, -9), new(5.8f, 3.2f, 0.2f), _wall);
        Solid("FrontWallRight", new(4.1f, 1.6f, -9), new(5.8f, 3.2f, 0.2f), _wall);
        Solid("LeftOuterWall", new(-7, 3.1f, -19), new(0.2f, 6.2f, 20), _wall);
        Solid("RightOuterWall", new(7, 3.1f, -19), new(0.2f, 6.2f, 20), _wall);
        Solid("BackWall", new(0, 3.1f, -29), new(14, 6.2f, 0.2f), _wall);
        Solid("UpperFrontWallLeft", new(-4.1f, 4.75f, -9), new(5.8f, 3.1f, 0.2f), _wall);
        Solid("UpperFrontWallRight", new(4.1f, 4.75f, -9), new(5.8f, 3.1f, 0.2f), _wall);
        Solid("Roof", new(0, 6.35f, -19), new(14.5f, 0.3f, 20.5f), _wood);
    }

    private void BuildGroundFloor()
    {
        // Divisoes transversais com portas de 1,2 m voltadas ao corredor central.
        SplitCrossWall("LeftReceptionRear", -15, true);
        SplitCrossWall("LeftRoom102Rear", -22, true);
        SplitCrossWall("RightKitchenRear", -16, false);
        SplitCrossWall("RightDepositRear", -21, false);
        Solid("ReceptionCounter", new(-4.6f, 0.55f, -12), new(3.2f, 1.1f, 0.75f), _wood);
        Solid("DepositDoorBlocker", new(1.3f, 1.2f, -18.5f), new(0.2f, 2.4f, 1.3f), _wood);
        Solid("StairAccessBlocker", new(4.5f, 1.2f, -21.15f), new(4.8f, 2.4f, 0.2f), _wood);
    }

    private void SplitCrossWall(string name, float z, bool left)
    {
        var side = left ? -1f : 1f;
        Solid(name + "Outer", new(side * 5f, 1.6f, z), new(4f, 3.2f, 0.2f), _wall);
        Solid(name + "Inner", new(side * 1.5f, 1.6f, z), new(0.6f, 3.2f, 0.2f), _wall);
    }

    private void BuildStairs()
    {
        const int steps = 11;
        for (var i = 0; i < steps; i++)
        {
            var t = i / (float)(steps - 1);
            VisualBox($"StairStep{i:00}", new(4.5f, 0.18f + t * 3.0f, -21.7f - t * 5.0f), new(3.0f, 0.22f, 0.58f), _wood);
        }
        Solid("StairRamp", new(4.5f, 1.62f, -24.2f), new(3.0f, 0.25f, 5.85f), _floor, new(-0.54f, 0, 0), false);
        Solid("StairRailLeft", new(2.95f, 2.0f, -24.2f), new(0.12f, 1.1f, 5.8f), _wood, new(-0.54f, 0, 0));
        Solid("StairRailRight", new(6.05f, 2.0f, -24.2f), new(0.12f, 1.1f, 5.8f), _wood, new(-0.54f, 0, 0));
    }

    private void BuildUpperFloor()
    {
        // Piso dividido: vao real da escada em X 3..6, Z -21..-27.
        Solid("UpperFloorMain", new(-2, 3.2f, -19), new(10, 0.2f, 20), _floor);
        Solid("UpperFloorRightFront", new(5, 3.2f, -15), new(4, 0.2f, 12), _floor);
        Solid("UpperFloorRightRear", new(5, 3.2f, -28), new(4, 0.2f, 2), _floor);
        SplitUpperWall("ManagerFront", -16, true);
        SplitUpperWall("ManagerRear", -23, true);
        SplitUpperWall("BathroomRear", -17, false);
        SplitUpperWall("LockedRoomFront", -23, false);
        Solid("StairGuardFront", new(4.5f, 3.75f, -20.9f), new(3.2f, 1.1f, 0.12f), _wood);
        Solid("StairGuardSide", new(2.9f, 3.75f, -24), new(0.12f, 1.1f, 6.2f), _wood);
        Solid("ManagerDesk", new(-4.6f, 3.65f, -20.2f), new(2.6f, 0.9f, 1.0f), _wood);
        Solid("LockedUpperDoor", new(1.3f, 4.35f, -25.5f), new(0.2f, 2.3f, 1.3f), _wood);
    }

    private void SplitUpperWall(string name, float z, bool left)
    {
        var side = left ? -1f : 1f;
        Solid(name + "Outer", new(side * 5f, 4.75f, z), new(4f, 3.1f, 0.2f), _wall);
        Solid(name + "Inner", new(side * 1.5f, 4.75f, z), new(0.6f, 3.1f, 0.2f), _wall);
    }

    private void BuildLabels()
    {
        Label("RECEPCAO", new(-4.5f, 2.1f, -9.15f));
        Label("QUARTO 102", new(-4.5f, 2.1f, -15.15f));
        Label("COZINHA", new(4.5f, 2.1f, -9.15f));
        Label("DEPOSITO", new(1.15f, 2.0f, -18.5f), true);
        Label("GERENTE", new(-4.5f, 5.1f, -16.15f));
        Label("BANHEIRO", new(4.5f, 5.1f, -17.15f));
        Label("TRANCADO", new(1.15f, 5.0f, -25.5f), true);
    }

    private void Label(string text, Vector3 position, bool side = false)
    {
        var label = new Label3D
        {
            Name = text.Replace(" ", "") + "Label",
            Text = text,
            Position = position,
            FontSize = 42,
            PixelSize = 0.003f,
            Modulate = new Color("c5a873"),
            OutlineSize = 5,
            Billboard = BaseMaterial3D.BillboardModeEnum.Enabled,
        };
        _visual.AddChild(label);
    }

    private void Solid(string name, Vector3 position, Vector3 size, Material material, Vector3? rotation = null, bool visible = true)
    {
        if (visible)
        {
            VisualBox(name, position, size, material, rotation);
        }
        var body = new StaticBody3D { Name = name };
        var shape = new CollisionShape3D { Name = "Shape", Shape = new BoxShape3D { Size = size } };
        body.Position = position;
        body.Rotation = rotation ?? Vector3.Zero;
        body.AddChild(shape);
        _collisions.AddChild(body);
    }

    private void VisualBox(string name, Vector3 position, Vector3 size, Material material, Vector3? rotation = null)
    {
        var mesh = new MeshInstance3D
        {
            Name = name,
            Position = position,
            Rotation = rotation ?? Vector3.Zero,
            Mesh = new BoxMesh { Size = size, Material = material },
        };
        _visual.AddChild(mesh);
    }
}
