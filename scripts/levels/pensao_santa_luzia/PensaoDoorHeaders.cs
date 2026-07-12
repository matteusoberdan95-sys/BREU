namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Sprint 14Z — header walls above open door gaps (no frames, no decorative panels).
/// </summary>
public partial class PensaoTerreoBlockout01Builder
{
    private static float DoorHeaderCenterY(float floorTopY) =>
        floorTopY + DoorHeight + (WallHeight - DoorHeight) * 0.5f - WallEmbedBelowFloor;

    protected void AddDoorHeaderXWall(
        Node3D parent,
        string name,
        float wallX,
        float doorCenterZ,
        float openingWidth,
        StandardMaterial3D material,
        float floorTopY = 0f)
    {
        var headerHeight = WallHeight - DoorHeight;
        AddWall(
            parent,
            name,
            new Vector3(wallX, DoorHeaderCenterY(floorTopY), doorCenterZ),
            new Vector3(WallThickness, headerHeight, openingWidth + WallCornerOverlap),
            material);
    }

    protected void AddDoorHeaderZWall(
        Node3D parent,
        string name,
        Vector3 center,
        float openingWidth,
        StandardMaterial3D material,
        float floorTopY = 0f)
    {
        var headerHeight = WallHeight - DoorHeight;
        AddWall(
            parent,
            name,
            new Vector3(center.X, DoorHeaderCenterY(floorTopY), center.Z),
            new Vector3(openingWidth + WallCornerOverlap, headerHeight, WallThickness),
            material);
    }
}
