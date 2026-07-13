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
        bool buildUpperBlockers = true,
        bool buildSideGuides = true,
        bool buildStringers = true,
        bool buildSlopedHandrails = false)
    {
        BuildApproachPatch(root, collisionLayer);
        BuildInvisibleRamp(collisionsParent, collisionLayer);
        BuildVisualSteps(visualStepsParent, matStep, matStringer, buildStringers);
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

        if (buildSideGuides)
        {
            BuildSideGuides(sideGuidesParent, matRail, collisionLayer);
        }

        if (buildSlopedHandrails)
        {
            BuildSlopedHandrails(sideGuidesParent, matStringer, collisionLayer);
        }
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

    private static void BuildVisualSteps(
        Node3D parent,
        StandardMaterial3D matStep,
        StandardMaterial3D matStringer,
        bool buildStringers)
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

        if (!buildStringers)
        {
            return;
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

    private static void BuildSlopedHandrails(
        Node3D parent,
        StandardMaterial3D material,
        uint collisionLayer)
    {
        const float railHeight = 0.9f;
        const float midRailHeight = 0.48f;
        const float postThickness = 0.10f;
        const float beamThickness = 0.12f;
        const int postCount = 5;
        var railX = StairWidth * 0.5f + 0.08f;
        var pitch = Mathf.Atan2(StairRise, StairRun);
        var cos = Mathf.Cos(pitch);
        var sin = Mathf.Sin(pitch);
        var slopeBasis = new Basis(1f, 0f, 0f, 0f, cos, sin, 0f, -sin, cos);

        BuildHandrailSide(parent, "Left", -railX, railHeight, midRailHeight,
            postThickness, beamThickness, postCount, slopeBasis, material, collisionLayer);
        BuildHandrailSide(parent, "Right", railX, railHeight, midRailHeight,
            postThickness, beamThickness, postCount, slopeBasis, material, collisionLayer);
    }

    private static void BuildHandrailSide(
        Node3D parent,
        string side,
        float x,
        float railHeight,
        float midRailHeight,
        float postThickness,
        float beamThickness,
        int postCount,
        Basis slopeBasis,
        StandardMaterial3D material,
        uint collisionLayer)
    {
        var host = new Node3D { Name = $"Stair_Handrail_{side}" };
        parent.AddChild(host);

        AddRailPiece(host, "TopRail",
            new Vector3(x, StairRise * 0.5f + railHeight, StairRun * 0.5f),
            new Vector3(beamThickness, beamThickness, SlopeLength() + 0.08f),
            slopeBasis, material, collisionLayer);

        AddRailPiece(host, "MidRail",
            new Vector3(x, StairRise * 0.5f + midRailHeight, StairRun * 0.5f),
            new Vector3(beamThickness * 0.82f, beamThickness * 0.82f, SlopeLength()),
            slopeBasis, material, collisionLayer);

        for (var i = 0; i < postCount; i++)
        {
            var t = i / (float)(postCount - 1);
            var z = StairRun * t;
            var stairY = StairRise * t;
            AddRailPiece(host, $"Post_{i + 1:D2}",
                new Vector3(x, stairY + railHeight * 0.5f, z),
                new Vector3(postThickness, railHeight, postThickness),
                Basis.Identity, material, collisionLayer);
        }
    }

    private static void AddRailPiece(
        Node3D parent,
        string name,
        Vector3 center,
        Vector3 size,
        Basis basis,
        StandardMaterial3D material,
        uint collisionLayer)
    {
        var body = new StaticBody3D
        {
            Name = name,
            Position = center,
            Basis = basis,
            CollisionLayer = collisionLayer,
            CollisionMask = 0
        };
        body.AddChild(new MeshInstance3D { Name = "Visual", Mesh = CreateBoxMesh(size, material) });
        body.AddChild(new CollisionShape3D
        {
            Name = "CollisionShape3D",
            Shape = new BoxShape3D { Size = size }
        });
        parent.AddChild(body);
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
