namespace BREU.Scripts.Enemies;

/// <summary>
/// Contorno manual da mesa da RitualRoom usando waypoints e retangulo no plano X/Z.
/// </summary>
public partial class TableAvoidanceHelper : Node3D
{
    [Export] public NodePath TableCollisionPath { get; set; } = "../../Collisions/TableCollision";
    [Export] public Godot.Collections.Array<NodePath> WaypointPaths { get; set; } = [];

    [Export] public Vector2 TableMinXZ { get; set; } = new(-1.35f, -1.35f);
    [Export] public Vector2 TableMaxXZ { get; set; } = new(1.35f, -0.15f);

    private readonly List<Vector3> _waypoints = [];

    public override void _Ready()
    {
        CacheWaypoints();
    }

    public bool IsDirectPathBlockedByTable(Vector3 from, Vector3 to)
    {
        var start = new Vector2(from.X, from.Z);
        var end = new Vector2(to.X, to.Z);
        return SegmentIntersectsRect(start, end, TableMinXZ, TableMaxXZ);
    }

    public Vector3 GetBestWaypoint(Vector3 enemyPosition, Vector3 playerPosition)
    {
        CacheWaypoints();
        if (_waypoints.Count == 0)
        {
            return playerPosition;
        }

        Vector3? bestWaypoint = null;
        var bestCost = float.MaxValue;

        foreach (var waypoint in _waypoints)
        {
            if (IsInsideTable(waypoint))
            {
                continue;
            }

            var cost = enemyPosition.DistanceTo(waypoint) + waypoint.DistanceTo(playerPosition);
            if (cost < bestCost)
            {
                bestCost = cost;
                bestWaypoint = waypoint;
            }
        }

        return bestWaypoint ?? playerPosition;
    }

    public Vector3 GetAlternateWaypoint(Vector3 enemyPosition, Vector3 playerPosition, Vector3 currentWaypoint)
    {
        CacheWaypoints();
        if (_waypoints.Count <= 1)
        {
            return GetBestWaypoint(enemyPosition, playerPosition);
        }

        Vector3? bestWaypoint = null;
        var bestCost = float.MaxValue;

        foreach (var waypoint in _waypoints)
        {
            if (waypoint.DistanceSquaredTo(currentWaypoint) < 0.25f || IsInsideTable(waypoint))
            {
                continue;
            }

            var cost = enemyPosition.DistanceTo(waypoint) + waypoint.DistanceTo(playerPosition);
            if (cost < bestCost)
            {
                bestCost = cost;
                bestWaypoint = waypoint;
            }
        }

        return bestWaypoint ?? GetBestWaypoint(enemyPosition, playerPosition);
    }

    public bool IsInsideTableArea(Vector3 position)
    {
        return IsInsideTable(position);
    }

    private void CacheWaypoints()
    {
        if (_waypoints.Count > 0)
        {
            return;
        }

        foreach (var path in WaypointPaths)
        {
            if (GetNodeOrNull(path) is Node3D marker)
            {
                _waypoints.Add(marker.GlobalPosition);
            }
        }

        if (_waypoints.Count == 0)
        {
            foreach (var child in GetChildren())
            {
                if (child is Marker3D marker && child.Name.ToString().StartsWith("TableWaypoint", StringComparison.Ordinal))
                {
                    _waypoints.Add(marker.GlobalPosition);
                }
            }
        }
    }

    private bool IsInsideTable(Vector3 position)
    {
        return position.X >= TableMinXZ.X && position.X <= TableMaxXZ.X
            && position.Z >= TableMinXZ.Y && position.Z <= TableMaxXZ.Y;
    }

    private static bool SegmentIntersectsRect(Vector2 start, Vector2 end, Vector2 rectMin, Vector2 rectMax)
    {
        if (PointInRect(start, rectMin, rectMax) || PointInRect(end, rectMin, rectMax))
        {
            return true;
        }

        var topLeft = new Vector2(rectMin.X, rectMin.Y);
        var topRight = new Vector2(rectMax.X, rectMin.Y);
        var bottomLeft = new Vector2(rectMin.X, rectMax.Y);
        var bottomRight = new Vector2(rectMax.X, rectMax.Y);

        return SegmentsIntersect(start, end, topLeft, topRight)
            || SegmentsIntersect(start, end, topRight, bottomRight)
            || SegmentsIntersect(start, end, bottomRight, bottomLeft)
            || SegmentsIntersect(start, end, bottomLeft, topLeft);
    }

    private static bool PointInRect(Vector2 point, Vector2 rectMin, Vector2 rectMax)
    {
        return point.X >= rectMin.X && point.X <= rectMax.X
            && point.Y >= rectMin.Y && point.Y <= rectMax.Y;
    }

    private static bool SegmentsIntersect(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        var d1 = Direction(b1, b2, a1);
        var d2 = Direction(b1, b2, a2);
        var d3 = Direction(a1, a2, b1);
        var d4 = Direction(a1, a2, b2);

        if (((d1 > 0.0f && d2 < 0.0f) || (d1 < 0.0f && d2 > 0.0f))
            && ((d3 > 0.0f && d4 < 0.0f) || (d3 < 0.0f && d4 > 0.0f)))
        {
            return true;
        }

        return Mathf.IsZeroApprox(d1) && OnSegment(b1, b2, a1)
            || Mathf.IsZeroApprox(d2) && OnSegment(b1, b2, a2)
            || Mathf.IsZeroApprox(d3) && OnSegment(a1, a2, b1)
            || Mathf.IsZeroApprox(d4) && OnSegment(a1, a2, b2);
    }

    private static float Direction(Vector2 a, Vector2 b, Vector2 c)
    {
        return (c.X - a.X) * (b.Y - a.Y) - (c.Y - a.Y) * (b.X - a.X);
    }

    private static bool OnSegment(Vector2 a, Vector2 b, Vector2 c)
    {
        return c.X <= Mathf.Max(a.X, b.X) && c.X >= Mathf.Min(a.X, b.X)
            && c.Y <= Mathf.Max(a.Y, b.Y) && c.Y >= Mathf.Min(a.Y, b.Y);
    }
}
