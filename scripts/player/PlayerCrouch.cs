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
    [Export] public NodePath HeadPath { get; set; } = "../HeadBase";
    [Export] public NodePath CollisionShapePath { get; set; } = "../CollisionShape3D";

    public bool IsCrouching { get; private set; }

    private CharacterBody3D? _body;
    private Node3D? _head;
    private CollisionShape3D? _collisionShape;
    private CapsuleShape3D? _capsuleShape;
    private readonly BoxShape3D _standClearanceShape = new();
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

        // Test only the volume added above the current crouching capsule. The old
        // ray used the crouching center to derive the feet and overshot the real
        // standing height, so the upper-floor slab was mistaken for a low ceiling.
        var feetY = _body.GlobalPosition.Y + _standingCollisionCenterY - StandingCapsuleHeight * 0.5f;
        const float clearanceInset = 0.04f;
        var clearanceHeight = Mathf.Max(0.05f, extraHeight - clearanceInset * 2f);
        var clearanceWidth = Mathf.Max(0.1f, _capsuleShape.Radius * 1.8f);
        _standClearanceShape.Size = new Vector3(clearanceWidth, clearanceHeight, clearanceWidth);

        var clearanceCenterY = feetY + CrouchingCapsuleHeight + extraHeight * 0.5f;
        var query = new PhysicsShapeQueryParameters3D
        {
            Shape = _standClearanceShape,
            Transform = new Transform3D(Basis.Identity,
                new Vector3(_body.GlobalPosition.X, clearanceCenterY, _body.GlobalPosition.Z)),
            CollisionMask = _body.CollisionMask,
            CollideWithAreas = false,
            CollideWithBodies = true,
            Margin = 0.01f,
            Exclude = [_body.GetRid()]
        };
        return _body.GetWorld3D().DirectSpaceState.IntersectShape(query, 1).Count == 0;
    }
}
