namespace BREU.Scripts.Player;

/// <summary>
/// Mouse look (yaw on body, pitch on camera rig), lean Q/R and sprint look-back (Alt).
/// Visual only — does not affect movement direction or capsule collision.
/// </summary>
public partial class PlayerLook : Node3D
{
    [Export] public NodePath BodyPath { get; set; } = "../..";
    [Export] public NodePath ControllerPath { get; set; } = "../..";
    [Export] public float MouseSensitivity { get; set; } = 0.12f;
    [Export] public float MinPitchDegrees { get; set; } = -85.0f;
    [Export] public float MaxPitchDegrees { get; set; } = 85.0f;

    [Export] public float LeanOffset { get; set; } = 0.32f;
    [Export] public float LeanRollDegrees { get; set; } = 8.0f;
    [Export] public float LeanSpeed { get; set; } = 8.0f;
    [Export] public float LeanWallProbeDistance { get; set; } = 0.45f;

    [Export] public float LookBackAngleDegrees { get; set; } = 170.0f;
    [Export] public float LookBackSpeed { get; set; } = 10.0f;

    public float PitchRadians => _pitch;
    public float LookBackBlend => _lookBackBlend;

    private Node3D? _body;
    private PlayerController? _controller;
    private float _pitch;
    private float _leanBlend;
    private float _lookBackBlend;

    public override void _Ready()
    {
        _body = GetNodeOrNull<Node3D>(BodyPath);
        _controller = GetNodeOrNull<PlayerController>(ControllerPath);
        _pitch = Rotation.X;
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel") || @event.IsActionPressed("pause"))
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
            return;
        }

        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
            return;
        }

        if (@event is not InputEventMouseMotion motion || Input.MouseMode != Input.MouseModeEnum.Captured)
        {
            return;
        }

        var scale = MouseSensitivity * 0.025f;
        _body?.RotateY(-motion.Relative.X * scale);
        _pitch = Mathf.Clamp(
            _pitch - motion.Relative.Y * scale,
            Mathf.DegToRad(MinPitchDegrees),
            Mathf.DegToRad(MaxPitchDegrees));
    }

    public override void _PhysicsProcess(double delta)
    {
        UpdateLean((float)delta);
        UpdateLookBack((float)delta);
        ApplyCameraRigPose();
    }

    private void UpdateLean(float delta)
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

        _leanBlend = Mathf.MoveToward(_leanBlend, targetLean, LeanSpeed * delta);
    }

    private void UpdateLookBack(float delta)
    {
        var wantsLookBack = false;
        if (_controller != null)
        {
            wantsLookBack = Input.IsActionPressed("look_back")
                && _controller.IsSprinting
                && _controller.IsMovingForward;
        }

        var target = wantsLookBack ? 1.0f : 0.0f;
        _lookBackBlend = Mathf.MoveToward(_lookBackBlend, target, LookBackSpeed * delta);
    }

    private void ApplyCameraRigPose()
    {
        var leanOffset = ComputeLeanOffset();
        var leanRoll = Mathf.DegToRad(-_leanBlend * LeanRollDegrees);
        var lookBackYaw = Mathf.DegToRad(LookBackAngleDegrees) * _lookBackBlend;

        Position = new Vector3(leanOffset, 0.0f, 0.0f);
        Rotation = new Vector3(_pitch, lookBackYaw, leanRoll);
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
        if (_body is CollisionObject3D collisionObject)
        {
            query.Exclude = [collisionObject.GetRid()];
        }

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
