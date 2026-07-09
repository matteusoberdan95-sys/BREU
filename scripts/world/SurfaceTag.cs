namespace BREU.Scripts.World;

public partial class SurfaceTag : Node
{
    [Export] public SurfaceType SurfaceType { get; set; } = SurfaceType.Concrete;
}
