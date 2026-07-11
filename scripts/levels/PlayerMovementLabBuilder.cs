namespace BREU.Scripts.Levels;

/// <summary>
/// Procedural blockout for Sprint 02 movement testing. No art — only collision geometry.
/// </summary>
public partial class PlayerMovementLabBuilder : Node3D
{
    private static readonly Color BlockoutColor = new(0.35f, 0.35f, 0.38f);

    public override void _Ready()
    {
        BuildFloor();
        BuildPerimeterWalls();
        BuildDoorTests();
        BuildRamp();
        BuildLowTunnel();
        BuildLight();
    }

    private void BuildFloor()
    {
        AddBox(new Vector3(0.0f, -0.25f, 0.0f), new Vector3(28.0f, 0.5f, 22.0f));
    }

    private void BuildPerimeterWalls()
    {
        AddBox(new Vector3(0.0f, 1.5f, -11.0f), new Vector3(28.0f, 3.0f, 0.4f));
        AddBox(new Vector3(0.0f, 1.5f, 11.0f), new Vector3(28.0f, 3.0f, 0.4f));
        AddBox(new Vector3(-14.0f, 1.5f, 0.0f), new Vector3(0.4f, 3.0f, 22.0f));
        AddBox(new Vector3(14.0f, 1.5f, 0.0f), new Vector3(0.4f, 3.0f, 22.0f));
    }

    private void BuildDoorTests()
    {
        // Wide opening (~2.2 m) centered at x = -6
        AddBox(new Vector3(-10.1f, 1.5f, -11.0f), new Vector3(5.8f, 3.0f, 0.4f));
        AddBox(new Vector3(-1.9f, 1.5f, -11.0f), new Vector3(5.8f, 3.0f, 0.4f));

        // Narrow opening (~0.9 m) centered at x = 6
        AddBox(new Vector3(2.55f, 1.5f, -11.0f), new Vector3(5.1f, 3.0f, 0.4f));
        AddBox(new Vector3(9.45f, 1.5f, -11.0f), new Vector3(5.1f, 3.0f, 0.4f));
    }

    private void BuildRamp()
    {
        var ramp = new StaticBody3D { Name = "Ramp", Rotation = new Vector3(Mathf.DegToRad(-11.5f), 0.0f, 0.0f) };
        ramp.Position = new Vector3(9.0f, 0.65f, 2.0f);
        AddChild(ramp);

        var mesh = new BoxMesh { Size = new Vector3(3.0f, 0.3f, 7.0f) };
        var material = new StandardMaterial3D { AlbedoColor = BlockoutColor };
        mesh.SurfaceSetMaterial(0, material);

        var meshInstance = new MeshInstance3D { Mesh = mesh };
        ramp.AddChild(meshInstance);

        var collision = new CollisionShape3D
        {
            Shape = new BoxShape3D { Size = mesh.Size }
        };
        ramp.AddChild(collision);
    }

    private void BuildLowTunnel()
    {
        // Low ceiling section — standing blocked, crouch passes
        AddBox(new Vector3(-5.0f, 1.5f, 5.0f), new Vector3(0.4f, 3.0f, 8.0f));
        AddBox(new Vector3(1.0f, 1.5f, 5.0f), new Vector3(0.4f, 3.0f, 8.0f));
        AddBox(new Vector3(-2.0f, 1.32f, 5.0f), new Vector3(6.8f, 0.35f, 8.0f));
    }

    private void BuildLight()
    {
        var light = new DirectionalLight3D
        {
            Rotation = new Vector3(Mathf.DegToRad(-50.0f), Mathf.DegToRad(35.0f), 0.0f),
            LightEnergy = 0.65f,
            ShadowEnabled = true
        };
        AddChild(light);
    }

    private void AddBox(Vector3 center, Vector3 size)
    {
        var body = new StaticBody3D { Name = "Wall", Position = center };
        AddChild(body);

        var mesh = new BoxMesh { Size = size };
        var material = new StandardMaterial3D { AlbedoColor = BlockoutColor };
        mesh.SurfaceSetMaterial(0, material);

        body.AddChild(new MeshInstance3D { Mesh = mesh });
        body.AddChild(new CollisionShape3D { Shape = new BoxShape3D { Size = size } });
    }
}
