namespace BREU.Scripts.Fx;

/// <summary>
/// Fullscreen depth fog quad preso a camera da TrailIntro.
/// Usa vertex em clip-space no shader; reparent para Camera3D em runtime.
/// </summary>
public partial class DepthFogPostProcess : MeshInstance3D
{
    [Export] public NodePath CameraPath { get; set; } = new("../../../Player/CameraPivot/Camera3D");

    public override void _Ready()
    {
        CastShadow = GeometryInstance3D.ShadowCastingSetting.Off;
        ExtraCullMargin = 16384f;

        if (Mesh is QuadMesh quadMesh)
        {
            quadMesh.Size = new Vector2(2f, 2f);
            quadMesh.FlipFaces = true;
        }

        if (MaterialOverride is ShaderMaterial material)
        {
            material.RenderPriority = 127;
        }

        CallDeferred(MethodName.AttachToCamera);
    }

    private void AttachToCamera()
    {
        if (CameraPath == null || CameraPath.IsEmpty)
        {
            GD.PushWarning("DepthFogPostProcess: CameraPath nao definido.");
            return;
        }

        var camera = GetNodeOrNull<Camera3D>(CameraPath);
        if (camera == null)
        {
            GD.PushWarning($"DepthFogPostProcess: camera nao encontrada em {CameraPath}.");
            return;
        }

        GetParent()?.RemoveChild(this);
        camera.AddChild(this);
        Transform = Transform3D.Identity;
    }
}
