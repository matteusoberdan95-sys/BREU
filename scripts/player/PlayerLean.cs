namespace BREU.Scripts.Player;

/// <summary>
/// Lateral lean Q/R — visual offset and roll on LeanPivot only.
/// </summary>
public partial class PlayerLean : Node3D
{
    [Export] public NodePath BodyPath { get; set; } = new NodePath("../../..");
    [Export] public float LeanOffset { get; set; } = 0.32f;
    [Export] public float LeanRollDegrees { get; set; } = 8.0f;
    [Export] public float LeanSpeed { get; set; } = 8.0f;
    [Export] public float LeanWallProbeDistance { get; set; } = 0.45f;

    public float LeanBlend => _leanBlend;

    private CharacterBody3D? _body;
    private float _leanBlend;

    public override void _Ready()
    {
        _body = GetNodeOrNull<CharacterBody3D>(BodyPath);
    }

    public override void _PhysicsProcess(double delta)
    {
        var targetLean = 0.0f;
        if (Input.IsActionPressed("lean_left"))
        {
            targetLean = -1.0f;
        }
        else if (Input.IsActionPressed("lean_right"))
        {
            targetLean = 1.0f;
        }

        _leanBlend = Mathf.MoveToward(_leanBlend, targetLean, LeanSpeed * (float)delta);

        var leanOffset = ComputeLeanOffset();
        var leanRoll = Mathf.DegToRad(-_leanBlend * LeanRollDegrees);
        Position = new Vector3(leanOffset, 0.0f, 0.0f);
        Rotation = new Vector3(0.0f, 0.0f, leanRoll);
    }

    private float ComputeLeanOffset()
    {
        if (Mathf.IsZeroApprox(_leanBlend))
        {
            return 0.0f;
        }

        var desired = _leanBlend * LeanOffset;
        if (_body == null)
        {
            return desired;
        }

        var origin = GlobalPosition;
        var direction = GlobalTransform.Basis.X * Mathf.Sign(_leanBlend);
        var query = PhysicsRayQueryParameters3D.Create(origin, origin + direction * LeanWallProbeDistance);
        query.CollisionMask = 1;
        query.Exclude = [_body.GetRid()];

        var hit = _body.GetWorld3D().DirectSpaceState.IntersectRay(query);
        if (hit.Count == 0)
        {
            return desired;
        }

        var distance = origin.DistanceTo((Vector3)hit["position"]);
        var allowed = Mathf.Max(0.0f, distance - 0.12f);
        return Mathf.Clamp(Mathf.Abs(desired), 0.0f, allowed) * Mathf.Sign(_leanBlend);
    }
}
