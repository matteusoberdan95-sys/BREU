namespace BREU.Scripts.Fx;

public partial class FogCard3D : Node3D
{
    [Export] public Color FogColor { get; set; } = new(0.086275f, 0.133333f, 0.172549f);
    [Export(PropertyHint.Range, "0,1,0.001")] public float Alpha { get; set; } = 0.08f;
    [Export(PropertyHint.Range, "0.1,4,0.01")] public float Softness { get; set; } = 1.4f;
    [Export(PropertyHint.Range, "0,1,0.01")] public float NoiseStrength { get; set; } = 0.25f;
    [Export(PropertyHint.Range, "0.1,16,0.1")] public float NoiseScale { get; set; } = 3.0f;
    [Export] public Vector2 QuadSize { get; set; } = new(6.0f, 3.0f);
    [Export] public bool YAxisBillboard { get; set; } = true;

    private MeshInstance3D? _quad;
    private ShaderMaterial? _material;

    public override void _Ready()
    {
        _quad = GetNode<MeshInstance3D>("FogQuad");
        _quad.CastShadow = GeometryInstance3D.ShadowCastingSetting.Off;

        if (_quad.Mesh is QuadMesh quadMesh)
        {
            quadMesh.Size = QuadSize;
        }

        if (_quad.GetSurfaceOverrideMaterial(0) is ShaderMaterial sourceMaterial)
        {
            _material = (ShaderMaterial)sourceMaterial.Duplicate();
            _quad.SetSurfaceOverrideMaterial(0, _material);
        }
        else if (_quad.Mesh?.SurfaceGetMaterial(0) is ShaderMaterial meshMaterial)
        {
            _material = (ShaderMaterial)meshMaterial.Duplicate();
            _quad.SetSurfaceOverrideMaterial(0, _material);
        }

        ApplyShaderParameters();
    }

    public override void _Process(double delta)
    {
        if (!YAxisBillboard)
        {
            return;
        }

        var camera = GetViewport()?.GetCamera3D();
        if (camera == null)
        {
            return;
        }

        var position = GlobalPosition;
        var toCamera = camera.GlobalPosition - position;
        toCamera.Y = 0.0f;

        if (toCamera.LengthSquared() < 0.0001f)
        {
            return;
        }

        LookAt(position + toCamera.Normalized(), Vector3.Up);
    }

    private void ApplyShaderParameters()
    {
        if (_material == null)
        {
            return;
        }

        _material.SetShaderParameter("fog_color", FogColor);
        _material.SetShaderParameter("alpha", Alpha);
        _material.SetShaderParameter("softness", Softness);
        _material.SetShaderParameter("noise_strength", NoiseStrength);
        _material.SetShaderParameter("noise_scale", NoiseScale);
    }
}
