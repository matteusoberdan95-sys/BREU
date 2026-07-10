namespace BREU.Scripts.Levels;

/// <summary>
/// Gera NavigationMesh da RitualRoom a partir das colisoes estaticas de gameplay.
/// </summary>
public partial class RitualRoomNavigationBake : NavigationRegion3D
{
    [Export] public NodePath CollisionRootPath { get; set; } = "../Collisions";

    public override void _Ready()
    {
        CallDeferred(MethodName.BakeFromCollisions);
    }

    private void BakeFromCollisions()
    {
        var collisionRoot = GetNodeOrNull<Node>(CollisionRootPath);
        if (collisionRoot == null)
        {
            GD.PrintErr("RitualRoomNavigationBake: Collisions nao encontrado.");
            return;
        }

        var navMesh = NavigationMesh ?? new NavigationMesh();
        navMesh.AgentRadius = 0.35f;
        navMesh.AgentHeight = 1.8f;
        navMesh.AgentMaxClimb = 0.2f;
        navMesh.AgentMaxSlope = 32.0f;
        navMesh.CellSize = 0.12f;
        navMesh.CellHeight = 0.12f;
        navMesh.FilterLowHangingObstacles = true;
        navMesh.FilterLedgeSpans = true;
        navMesh.FilterWalkableLowHeightSpans = true;

        NavigationMesh = navMesh;

        var sourceData = new NavigationMeshSourceGeometryData3D();
        foreach (var child in collisionRoot.GetChildren())
        {
            if (child.Name == "CeilingCollision")
            {
                continue;
            }

            NavigationServer3D.ParseSourceGeometryData(navMesh, sourceData, child);
        }

        NavigationServer3D.BakeFromSourceGeometryData(navMesh, sourceData);
        NavigationMesh = navMesh;

        GD.Print("RitualRoomNavigationBake: navmesh gerada a partir de Collisions.");
    }
}
