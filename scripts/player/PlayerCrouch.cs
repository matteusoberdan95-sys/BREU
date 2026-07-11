namespace BREU.Scripts.Player;

/// <summary>
/// Crouch pose: smooth camera height and capsule resize with ceiling check before standing up.
/// </summary>
public partial class PlayerCrouch : Node
{
    [Export] public float StandingCapsuleHeight { get; set; } = 1.8f;
    [Export] public float CrouchingCapsuleHeight { get; set; } = 1.0f;
    [Export] public float CameraHeightStanding { get; set; } = 1.65f;
    [Export] public float CameraHeightCrouching { get; set; } = 1.05f;
    [Export] public float TransitionSpeed { get; set; } = 10.0f;

    [Export] public NodePath BodyPath { get; set; } = "..";
    [Export] public NodePath HeadPath { get; set; } = "../Head";
    [Export] public NodePath CollisionShapePath { get; set; } = "../CollisionShape3D";

    public bool IsCrouching { get; private set; }

    private CharacterBody3D? _body;
    private Node3D? _head;
    private CollisionShape3D? _collisionShape;
    private CapsuleShape3D? _capsuleShape;
    private float _crouchBlend;
    private float _standingCollisionCenterY;

    public override void _Ready()
    {
        _body = GetNodeOrNull<CharacterBody3D>(BodyPath);
        _head = GetNodeOrNull<Node3D>(HeadPath);
        _collisionShape = GetNodeOrNull<CollisionShape3D>(CollisionShapePath);

        if (_collisionShape?.Shape is CapsuleShape3D capsule)
        {
            _capsuleShape = capsule;
            StandingCapsuleHeight = capsule.Height;
            _standingCollisionCenterY = _collisionShape.Position.Y;
        }

        ApplyPose(_crouchBlend);
    }

    public override void _PhysicsProcess(double delta)
    {
        var wantsCrouch = Input.IsActionPressed("crouch");
        if (!wantsCrouch && IsCrouching && !CanStandUp())
        {
            wantsCrouch = true;
        }

        IsCrouching = wantsCrouch;

        var targetBlend = IsCrouching ? 1.0f : 0.0f;
        _crouchBlend = Mathf.MoveToward(_crouchBlend, targetBlend, TransitionSpeed * (float)delta);
        ApplyPose(_crouchBlend);
    }

    private void ApplyPose(float blend)
    {
        var capsuleHeight = Mathf.Lerp(StandingCapsuleHeight, CrouchingCapsuleHeight, blend);
        if (_capsuleShape != null)
        {
            _capsuleShape.Height = capsuleHeight;
        }

        if (_collisionShape != null)
        {
            var feetY = _standingCollisionCenterY - StandingCapsuleHeight * 0.5f;
            _collisionShape.Position = new Vector3(0.0f, feetY + capsuleHeight * 0.5f, 0.0f);
        }

        if (_head != null)
        {
            var cameraHeight = Mathf.Lerp(CameraHeightStanding, CameraHeightCrouching, blend);
            _head.Position = new Vector3(0.0f, cameraHeight, 0.0f);
        }
    }

    private bool CanStandUp()
    {
        if (_body == null || _capsuleShape == null)
        {
            return true;
        }

        var extraHeight = StandingCapsuleHeight - CrouchingCapsuleHeight;
        if (extraHeight <= 0.01f)
        {
            return true;
        }

        var feetY = _body.GlobalPosition.Y + _standingCollisionCenterY - CrouchingCapsuleHeight * 0.5f;
        var from = new Vector3(_body.GlobalPosition.X, feetY + CrouchingCapsuleHeight, _body.GlobalPosition.Z);
        var to = from + Vector3.Up * extraHeight;
        var query = PhysicsRayQueryParameters3D.Create(from, to);
        query.CollisionMask = _body.CollisionMask;
        query.Exclude = [_body.GetRid()];

        return _body.GetWorld3D().DirectSpaceState.IntersectRay(query).Count == 0;
    }
}
