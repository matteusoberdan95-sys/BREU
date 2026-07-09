namespace BREU.Scripts.Player;

public partial class PlayerGroundSurfaceDetector : Node
{
    [Export] public NodePath GroundRayPath { get; set; } = "../GroundRay";
    [Export] public SurfaceType DefaultSurface { get; set; } = SurfaceType.Concrete;
    [Export] public bool DebugPrintOnSurfaceChange { get; set; } = true;

    public SurfaceType CurrentSurface { get; private set; } = SurfaceType.Concrete;

    private RayCast3D? _groundRay;

    public override void _Ready()
    {
        _groundRay = GetNodeOrNull<RayCast3D>(GroundRayPath);
        CurrentSurface = DefaultSurface;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_groundRay == null)
        {
            return;
        }

        var detected = _groundRay.IsColliding()
            ? ResolveSurface(_groundRay.GetCollider())
            : DefaultSurface;

        if (detected == CurrentSurface)
        {
            return;
        }

        CurrentSurface = detected;

        if (DebugPrintOnSurfaceChange)
        {
            GD.Print($"Surface changed: {CurrentSurface}");
        }
    }

    private static SurfaceType ResolveSurface(GodotObject? collider)
    {
        if (collider is not Node node)
        {
            return SurfaceType.Concrete;
        }

        var tag = FindSurfaceTag(node);
        if (tag != null)
        {
            return tag.SurfaceType;
        }

        if (node.IsInGroup("surface_wood"))
        {
            return SurfaceType.Wood;
        }

        if (node.IsInGroup("surface_concrete"))
        {
            return SurfaceType.Concrete;
        }

        if (node.IsInGroup("surface_dirt"))
        {
            return SurfaceType.Dirt;
        }

        if (node.IsInGroup("surface_metal"))
        {
            return SurfaceType.Metal;
        }

        return SurfaceType.Concrete;
    }

    private static SurfaceTag? FindSurfaceTag(Node node)
    {
        var current = node;
        while (current != null)
        {
            if (current is SurfaceTag tagOnNode)
            {
                return tagOnNode;
            }

            foreach (var child in current.GetChildren())
            {
                if (child is SurfaceTag childTag)
                {
                    return childTag;
                }
            }

            current = current.GetParent();
        }

        return null;
    }
}
