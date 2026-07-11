namespace BREU.Scripts.Levels;

/// <summary>
/// Shared Sprint 08/09A stair pattern — invisible ramp, visual steps, temporary upper landing.
/// Local space: foot at origin, ramp runs +Z, rise +Y.
/// </summary>
public static class StairRampAssembly
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

    public static void Build(
        Node3D root,
        Node3D collisionsParent,
        Node3D visualStepsParent,
        Node3D upperLandingParent,
        Node3D sideGuidesParent,
        StandardMaterial3D matStep,
        StandardMaterial3D matStringer,
        StandardMaterial3D matUpperFloor,
        StandardMaterial3D matRail,
        float upperLandingWidth = 5f,
        float upperLandingDepth = 5f,
        uint collisionLayer = 1,
        bool buildUpperLanding = true,
        bool buildUpperBlockers = true)
    {
        BuildApproachPatch(root, collisionLayer);
        BuildInvisibleRamp(collisionsParent, collisionLayer);
        BuildVisualSteps(visualStepsParent, matStep, matStringer);
        if (buildUpperLanding)
        {
            BuildUpperLanding(
                upperLandingParent,
                matUpperFloor,
                matRail,
                upperLandingWidth,
                upperLandingDepth,
                collisionLayer,
                buildUpperBlockers);
        }

        BuildSideGuides(sideGuidesParent, matRail, collisionLayer);
    }

    private static void BuildApproachPatch(Node3D parent, uint collisionLayer)
    {
        AddCollisionFloor(
            parent,
            "Stair_Approach_Floor",
            new Vector3(0f, FloorCenterY(0f), 0.55f),
            new Vector3(StairWidth + 1.0f, FloorThickness, 1.35f),
            collisionLayer);
    }

    private static void BuildInvisibleRamp(Node3D parent, uint collisionLayer)
    {
        var pitch = Mathf.Atan2(StairRise, StairRun);
        var cos = Mathf.Cos(pitch);
        var sin = Mathf.Sin(pitch);
        var slopeLen = SlopeLength();

        var rampBody = new StaticBody3D
        {
            Name = "Stair_InvisibleRamp_Collision",
            Position = new Vector3(0f, StairRise * 0.5f, StairRun * 0.5f),
            Basis = new Basis(1f, 0f, 0f, 0f, cos, sin, 0f, -sin, cos),
            CollisionLayer = collisionLayer,
            CollisionMask = 0
        };

        rampBody.AddChild(new CollisionShape3D
        {
            Shape = new BoxShape3D
            {
                Size = new Vector3(StairWidth, RampThickness, slopeLen + FloorOverlap * 2f)
            }
        });

        parent.AddChild(rampBody);
    }

    private static void BuildVisualSteps(Node3D parent, StandardMaterial3D matStep, StandardMaterial3D matStringer)
    {
        const float treadThickness = 0.06f;
        const float visualWidth = StairWidth - 0.16f;

        for (var i = 1; i <= StepCount; i++)
        {
            var topY = i * StepVisualHeight;
            var centerZ = (i - 0.5f) * StepVisualDepth;
            var centerY = topY - treadThickness * 0.5f;

            parent.AddChild(new MeshInstance3D
            {
                Name = $"Stair_Step_{i:D2}",
                Position = new Vector3(0f, centerY, centerZ),
                Mesh = CreateBoxMesh(new Vector3(visualWidth, treadThickness, StepVisualDepth - 0.02f), matStep)
            });
        }

        var leftStringer = new MeshInstance3D
        {
            Name = "Stair_Stringer_Left",
            Position = new Vector3(-StairWidth * 0.5f + 0.04f, StairRise * 0.5f, StairRun * 0.5f),
            Mesh = CreateBoxMesh(new Vector3(0.08f, StairRise, SlopeLength()), matStringer)
        };
        leftStringer.RotateX(Mathf.Atan2(StairRise, StairRun));
        parent.AddChild(leftStringer);

        var rightStringer = (MeshInstance3D)leftStringer.Duplicate();
        rightStringer.Name = "Stair_Stringer_Right";
        rightStringer.Position = new Vector3(StairWidth * 0.5f - 0.04f, StairRise * 0.5f, StairRun * 0.5f);
        parent.AddChild(rightStringer);
    }

    private static void BuildUpperLanding(
        Node3D parent,
        StandardMaterial3D matUpperFloor,
        StandardMaterial3D matRail,
        float landingWidth,
        float landingDepth,
        uint collisionLayer,
        bool buildBlockers)
    {
        var upperTopY = StairRise;
        var centerZ = StairRun + landingDepth * 0.5f - FloorOverlap;

        AddCollisionFloor(
            parent,
            "UpperLanding_Temporary",
            new Vector3(0f, FloorCenterY(upperTopY), centerZ),
            new Vector3(landingWidth, FloorThickness, landingDepth),
            collisionLayer);

        AddVisualFloorPlate(
            parent,
            "UpperLanding_Visual",
            new Vector3(0f, 0f, centerZ),
            new Vector2(landingWidth, landingDepth),
            upperTopY,
            matUpperFloor);

        if (!buildBlockers)
        {
            return;
        }

        const float railHeight = 1.1f;
        const float railThickness = 0.18f;
        var railHalfWidth = landingWidth * 0.5f - railThickness * 0.5f;

        AddWall(
            parent,
            "UpperLanding_Blocker_Left",
            new Vector3(-railHalfWidth, upperTopY + railHeight * 0.5f, centerZ),
            new Vector3(railThickness, railHeight, landingDepth),
            matRail,
            collisionLayer);

        AddWall(
            parent,
            "UpperLanding_Blocker_Right",
            new Vector3(railHalfWidth, upperTopY + railHeight * 0.5f, centerZ),
            new Vector3(railThickness, railHeight, landingDepth),
            matRail,
            collisionLayer);

        AddWall(
            parent,
            "UpperLanding_Blocker_Back",
            new Vector3(0f, upperTopY + railHeight * 0.5f, centerZ + landingDepth * 0.5f),
            new Vector3(landingWidth, railHeight, railThickness),
            matRail,
            collisionLayer);
    }

    private static void BuildSideGuides(Node3D parent, StandardMaterial3D matRail, uint collisionLayer)
    {
        const float guideHeight = 0.9f;
        const float guideThickness = 0.12f;
        var halfWidth = StairWidth * 0.5f + 0.08f;
        var guideCenterY = guideHeight * 0.5f;
        var guideCenterZ = StairRun * 0.5f;

        AddWall(
            parent,
            "Stair_Guide_Left",
            new Vector3(-halfWidth, guideCenterY, guideCenterZ),
            new Vector3(guideThickness, guideHeight, StairRun + 0.4f),
            matRail,
            collisionLayer);

        AddWall(
            parent,
            "Stair_Guide_Right",
            new Vector3(halfWidth, guideCenterY, guideCenterZ),
            new Vector3(guideThickness, guideHeight, StairRun + 0.4f),
            matRail,
            collisionLayer);
    }

    private static float FloorCenterY(float topY) => topY - FloorThickness * 0.5f;

    private static float SlopeLength() =>
        Mathf.Sqrt(StairRun * StairRun + StairRise * StairRise);

    private static void AddCollisionFloor(
        Node3D parent,
        string name,
        Vector3 center,
        Vector3 size,
        uint collisionLayer)
    {
        var body = new StaticBody3D
        {
            Name = name,
            Position = center,
            CollisionLayer = collisionLayer,
            CollisionMask = 0
        };
        body.AddChild(new CollisionShape3D { Shape = new BoxShape3D { Size = size } });
        parent.AddChild(body);
    }

    private static void AddVisualFloorPlate(
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
            Mesh = CreateBoxMesh(new Vector3(sizeXZ.X, thickness, sizeXZ.Y), material)
        });
    }

    private static void AddWall(
        Node3D parent,
        string name,
        Vector3 center,
        Vector3 size,
        StandardMaterial3D material,
        uint collisionLayer)
    {
        var body = new StaticBody3D
        {
            Name = name,
            Position = center,
            CollisionLayer = collisionLayer,
            CollisionMask = 0
        };
        body.AddChild(new CollisionShape3D { Shape = new BoxShape3D { Size = size } });
        body.AddChild(new MeshInstance3D { Mesh = CreateBoxMesh(size, material) });
        parent.AddChild(body);
    }

    private static BoxMesh CreateBoxMesh(Vector3 size, StandardMaterial3D material) =>
        new() { Size = size, Material = material };
}
