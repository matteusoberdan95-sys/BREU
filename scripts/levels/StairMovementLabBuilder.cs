namespace BREU.Scripts.Levels;

/// <summary>
/// Sprint 08 — builds lower floor, invisible ramp collision, visual steps, upper platform and rails.
/// </summary>
public partial class StairMovementLabBuilder : Node3D
{
    private const uint WorldLayer = 1;

    private Node3D _lowerFloor = null!;
    private Node3D _stairTest = null!;
    private Node3D _upperFloor = null!;
    private Node3D _collisions = null!;
    private Node3D _visualSteps = null!;

    private StandardMaterial3D _matLowerFloor = null!;
    private StandardMaterial3D _matUpperFloor = null!;
    private StandardMaterial3D _matStep = null!;
    private StandardMaterial3D _matRail = null!;
    private StandardMaterial3D _matStringer = null!;

    public override void _Ready()
    {
        CreateMaterials();
        ResolveNodes();
        BuildLowerFloor();
        BuildStairAssembly();
    }

    private void ResolveNodes()
    {
        _lowerFloor = GetNode<Node3D>("LowerFloor");
        _stairTest = GetNode<Node3D>("StairTest");
        _upperFloor = GetNode<Node3D>("UpperFloor");
        _collisions = GetNode<Node3D>("Collisions");
        _visualSteps = GetNode<Node3D>("VisualSteps");
    }

    private void CreateMaterials()
    {
        _matLowerFloor = Mat(new Color(0.48f, 0.5f, 0.54f));
        _matUpperFloor = Mat(new Color(0.52f, 0.54f, 0.58f));
        _matStep = Mat(new Color(0.58f, 0.46f, 0.34f));
        _matRail = Mat(new Color(0.38f, 0.4f, 0.44f));
        _matStringer = Mat(new Color(0.34f, 0.32f, 0.36f));
    }

    private static StandardMaterial3D Mat(Color color) =>
        new() { AlbedoColor = color };

    private void BuildLowerFloor()
    {
        const float lowerDepth = 14f;
        const float lowerWidth = 18f;
        var centerZ = -lowerDepth * 0.5f + 1.0f;

        AddCollisionFloor(
            _lowerFloor,
            "LowerFloor_Main",
            new Vector3(0f, FloorCenterY(0f), centerZ),
            new Vector3(lowerWidth, StairRampAssembly.FloorThickness, lowerDepth));

        AddVisualFloorPlate(
            _lowerFloor,
            "LowerFloor_Visual",
            new Vector3(0f, 0f, centerZ),
            new Vector2(lowerWidth, lowerDepth),
            0f,
            _matLowerFloor);
    }

    private void BuildStairAssembly()
    {
        StairRampAssembly.Build(
            _lowerFloor,
            _collisions,
            _visualSteps,
            _upperFloor,
            _stairTest,
            _matStep,
            _matStringer,
            _matUpperFloor,
            _matRail,
            upperLandingWidth: 14f,
            upperLandingDepth: 10f,
            collisionLayer: WorldLayer);
    }

    private static float FloorCenterY(float topY) =>
        topY - StairRampAssembly.FloorThickness * 0.5f;

    private void AddCollisionFloor(Node3D parent, string name, Vector3 center, Vector3 size)
    {
        var body = new StaticBody3D
        {
            Name = name,
            Position = center,
            CollisionLayer = WorldLayer,
            CollisionMask = 0
        };
        body.AddChild(new CollisionShape3D { Shape = new BoxShape3D { Size = size } });
        parent.AddChild(body);
    }

    private void AddVisualFloorPlate(
        Node3D parent,
        string name,
        Vector3 centerXZ,
        Vector2 sizeXZ,
        float topY,
        StandardMaterial3D material)
    {
        const float thickness = 0.12f;
        var center = new Vector3(centerXZ.X, topY - thickness * 0.5f, centerXZ.Z);
        parent.AddChild(new MeshInstance3D
        {
            Name = name,
            Position = center,
            Mesh = new BoxMesh { Size = new Vector3(sizeXZ.X, thickness, sizeXZ.Y), Material = material }
        });
    }
}
