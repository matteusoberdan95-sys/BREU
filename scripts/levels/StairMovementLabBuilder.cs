namespace BREU.Scripts.Levels;

/// <summary>
/// Sprint 08 — builds lower floor, invisible ramp collision, visual steps, upper platform and rails.
/// </summary>
public partial class StairMovementLabBuilder : Node3D
{
    public const float StairRise = 2.8f;
    public const float StairRun = 5.8f;
    public const float StairWidth = 2.2f;
    public const int StepCount = 14;
    public const float StepVisualHeight = 0.2f;
    public const float StepVisualDepth = StairRun / StepCount;
    public const float RampThickness = 0.22f;
    public const float FloorThickness = 0.2f;
    public const float FloorOverlap = 0.12f;

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
        BuildInvisibleRamp();
        BuildVisualSteps();
        BuildUpperFloor();
        BuildStairSideGuides();
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

    private static StandardMaterial3D Mat(Color color)
    {
        return new StandardMaterial3D { AlbedoColor = color };
    }

    private void BuildLowerFloor()
    {
        const float lowerDepth = 14f;
        const float lowerWidth = 18f;
        var centerZ = -lowerDepth * 0.5f + 1.0f;

        AddCollisionFloor(
            _lowerFloor,
            "LowerFloor_Main",
            new Vector3(0f, FloorCenterY(0f), centerZ),
            new Vector3(lowerWidth, FloorThickness, lowerDepth));

        AddVisualFloorPlate(
            _lowerFloor,
            "LowerFloor_Visual",
            new Vector3(0f, 0f, centerZ),
            new Vector2(lowerWidth, lowerDepth),
            0f,
            _matLowerFloor);

        AddCollisionFloor(
            _lowerFloor,
            "LowerFloor_StairApproach",
            new Vector3(0f, FloorCenterY(0f), 0.55f),
            new Vector3(StairWidth + 1.0f, FloorThickness, 1.35f));
    }

    private void BuildInvisibleRamp()
    {
        var pitch = Mathf.Atan2(StairRise, StairRun);
        var cos = Mathf.Cos(pitch);
        var sin = Mathf.Sin(pitch);
        var slopeLength = Mathf.Sqrt(StairRun * StairRun + StairRise * StairRise);

        var rampBody = new StaticBody3D
        {
            Name = "Stair_InvisibleRamp_Collision",
            Position = new Vector3(0f, StairRise * 0.5f, StairRun * 0.5f),
            Basis = new Basis(1f, 0f, 0f, 0f, cos, sin, 0f, -sin, cos),
            CollisionLayer = WorldLayer,
            CollisionMask = 0
        };

        var shape = new CollisionShape3D
        {
            Shape = new BoxShape3D
            {
                Size = new Vector3(StairWidth, RampThickness, slopeLength + FloorOverlap * 2f)
            }
        };

        rampBody.AddChild(shape);
        _collisions.AddChild(rampBody);
        _stairTest.AddChild(new Node3D { Name = "StairRampAnchor" });
    }

    private void BuildVisualSteps()
    {
        const float treadThickness = 0.06f;
        const float visualWidth = StairWidth - 0.16f;

        for (var i = 1; i <= StepCount; i++)
        {
            var topY = i * StepVisualHeight;
            var centerZ = (i - 0.5f) * StepVisualDepth;
            var centerY = topY - treadThickness * 0.5f;

            var step = new MeshInstance3D
            {
                Name = $"Stair_Step_{i:D2}",
                Position = new Vector3(0f, centerY, centerZ),
                Mesh = CreateBoxMesh(new Vector3(visualWidth, treadThickness, StepVisualDepth - 0.02f), _matStep)
            };

            _visualSteps.AddChild(step);
        }

        var leftStringer = new MeshInstance3D
        {
            Name = "Stair_Stringer_Left",
            Position = new Vector3(-StairWidth * 0.5f + 0.04f, StairRise * 0.5f, StairRun * 0.5f),
            Mesh = CreateBoxMesh(new Vector3(0.08f, StairRise, slopeLength()), _matStringer)
        };
        leftStringer.RotateX(Mathf.Atan2(StairRise, StairRun));
        _visualSteps.AddChild(leftStringer);

        var rightStringer = (MeshInstance3D)leftStringer.Duplicate();
        rightStringer.Name = "Stair_Stringer_Right";
        rightStringer.Position = new Vector3(StairWidth * 0.5f - 0.04f, StairRise * 0.5f, StairRun * 0.5f);
        _visualSteps.AddChild(rightStringer);
    }

    private void BuildUpperFloor()
    {
        const float upperDepth = 10f;
        const float upperWidth = 14f;
        var upperTopY = StairRise;
        var centerZ = StairRun + upperDepth * 0.5f - FloorOverlap;

        AddCollisionFloor(
            _upperFloor,
            "UpperFloor_Main",
            new Vector3(0f, FloorCenterY(upperTopY), centerZ),
            new Vector3(upperWidth, FloorThickness, upperDepth));

        AddVisualFloorPlate(
            _upperFloor,
            "UpperFloor_Visual",
            new Vector3(0f, 0f, centerZ),
            new Vector2(upperWidth, upperDepth),
            upperTopY,
            _matUpperFloor);

        var railHeight = 1.1f;
        var railThickness = 0.18f;
        var railHalfWidth = StairWidth * 0.5f + 0.35f;

        AddWall(
            _upperFloor,
            "UpperFloor_Rail_Left",
            new Vector3(-railHalfWidth, upperTopY + railHeight * 0.5f, centerZ),
            new Vector3(railThickness, railHeight, upperDepth),
            _matRail);

        AddWall(
            _upperFloor,
            "UpperFloor_Rail_Right",
            new Vector3(railHalfWidth, upperTopY + railHeight * 0.5f, centerZ),
            new Vector3(railThickness, railHeight, upperDepth),
            _matRail);

        AddWall(
            _upperFloor,
            "UpperFloor_BackWall",
            new Vector3(0f, upperTopY + railHeight * 0.5f, centerZ + upperDepth * 0.5f),
            new Vector3(upperWidth, railHeight, railThickness),
            _matRail);
    }

    private void BuildStairSideGuides()
    {
        const float guideHeight = 0.9f;
        const float guideThickness = 0.12f;
        var halfWidth = StairWidth * 0.5f + 0.08f;
        var guideCenterY = guideHeight * 0.5f;
        var guideCenterZ = StairRun * 0.5f;

        AddWall(
            _stairTest,
            "Stair_Guide_Left",
            new Vector3(-halfWidth, guideCenterY, guideCenterZ),
            new Vector3(guideThickness, guideHeight, StairRun + 0.4f),
            _matRail);

        AddWall(
            _stairTest,
            "Stair_Guide_Right",
            new Vector3(halfWidth, guideCenterY, guideCenterZ),
            new Vector3(guideThickness, guideHeight, StairRun + 0.4f),
            _matRail);
    }

    private static float FloorCenterY(float topY)
    {
        return topY - FloorThickness * 0.5f;
    }

    private static float slopeLength()
    {
        return Mathf.Sqrt(StairRun * StairRun + StairRise * StairRise);
    }

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
        var thickness = 0.12f;
        var center = new Vector3(centerXZ.X, topY - thickness * 0.5f, centerXZ.Z);
        var visual = new MeshInstance3D
        {
            Name = name,
            Position = center,
            Mesh = CreateBoxMesh(new Vector3(sizeXZ.X, thickness, sizeXZ.Y), material)
        };
        parent.AddChild(visual);
    }

    private void AddWall(Node3D parent, string name, Vector3 center, Vector3 size, StandardMaterial3D material)
    {
        var body = new StaticBody3D
        {
            Name = name,
            Position = center,
            CollisionLayer = WorldLayer,
            CollisionMask = 0
        };

        body.AddChild(new CollisionShape3D { Shape = new BoxShape3D { Size = size } });
        body.AddChild(new MeshInstance3D { Mesh = CreateBoxMesh(size, material) });
        parent.AddChild(body);
    }

    private static BoxMesh CreateBoxMesh(Vector3 size, StandardMaterial3D material)
    {
        return new BoxMesh
        {
            Size = size,
            Material = material
        };
    }
}
