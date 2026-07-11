namespace BREU.Scripts.Player;

/// <summary>
/// Horizontal movement, gravity, sprint integration and floor physics for the FPS player.
/// Mouse look and crouch are handled by sibling components.
/// </summary>
public partial class PlayerController : CharacterBody3D
{
    [Export] public float WalkSpeed { get; set; } = 4.0f;
    [Export] public float SprintSpeed { get; set; } = 6.5f;
    [Export] public float CrouchSpeed { get; set; } = 2.0f;
    [Export] public float Acceleration { get; set; } = 12.0f;
    [Export] public float Deceleration { get; set; } = 14.0f;
    [Export] public float AirControl { get; set; } = 0.35f;
    [Export] public float Gravity { get; set; } = 9.8f;
    [Export] public float JumpVelocity { get; set; } = 4.2f;
    [Export] public float JumpStaminaCost { get; set; } = 15.0f;
    [Export] public float SprintStaminaDrainPerSecond { get; set; } = 18.0f;
    [Export] public bool CanJump { get; set; } = true;

    [Export] public NodePath StaminaPath { get; set; } = "PlayerStamina";
    [Export] public NodePath CrouchPath { get; set; } = "PlayerCrouch";

    public bool MovementEnabled { get; set; } = true;

    private PlayerStamina? _stamina;
    private PlayerCrouch? _crouch;
    private int _debugFramesRemaining = 3;

    public override void _Ready()
    {
        AddToGroup("player");
        _stamina = GetNodeOrNull<PlayerStamina>(StaminaPath);
        _crouch = GetNodeOrNull<PlayerCrouch>(CrouchPath);
        FloorSnapLength = 0.1f;
        FloorMaxAngle = Mathf.DegToRad(46.0f);
        SafeMargin = 0.08f;

        var collision = GetNodeOrNull<CollisionShape3D>("CollisionShape3D");
        var head = GetNodeOrNull<Node3D>("Head");
        var camera = head?.GetNodeOrNull<Camera3D>("Camera3D");

        GD.Print("[PlayerController] _Ready global pos=", GlobalPosition);
        GD.Print("[PlayerController] scene=", GetTree().CurrentScene?.SceneFilePath ?? "unknown");

        if (collision != null)
        {
            GD.Print("[PlayerController] CollisionShape local pos=", collision.Position);
            if (collision.Shape is CapsuleShape3D capsule)
            {
                GD.Print("[PlayerController] Capsule height=", capsule.Height, " radius=", capsule.Radius);
            }
        }

        if (camera != null)
        {
            GD.Print("[PlayerController] Camera global pos=", camera.GlobalPosition);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_debugFramesRemaining > 0)
        {
            _debugFramesRemaining--;
            if (_debugFramesRemaining == 0)
            {
                GD.Print("[PlayerController] IsOnFloor(after frames)=", IsOnFloor());
            }
        }

        if (!MovementEnabled)
        {
            ApplyGravityOnly(delta);
            return;
        }

        var input = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        var inputDirection = new Vector3(input.X, 0.0f, -input.Y);
        var direction = inputDirection.LengthSquared() > 0.001f
            ? (GlobalTransform.Basis * inputDirection).Normalized()
            : Vector3.Zero;

        var isCrouching = _crouch?.IsCrouching ?? false;
        var wantsSprint = Input.IsActionPressed("sprint") && input.LengthSquared() > 0.01f && !isCrouching;
        var canSprint = wantsSprint && (_stamina == null || _stamina.Current > 0.0f);
        var isSprinting = canSprint;
        var speed = isCrouching ? CrouchSpeed : isSprinting ? SprintSpeed : WalkSpeed;

        var velocity = Velocity;
        var onFloor = IsOnFloor();
        var targetHorizontal = new Vector2(direction.X * speed, direction.Z * speed);
        var currentHorizontal = new Vector2(velocity.X, velocity.Z);
        var rate = targetHorizontal.LengthSquared() > 0.001f ? Acceleration : Deceleration;
        if (!onFloor)
        {
            rate *= AirControl;
        }

        currentHorizontal = currentHorizontal.MoveToward(targetHorizontal, rate * (float)delta);
        velocity.X = currentHorizontal.X;
        velocity.Z = currentHorizontal.Y;

        if (TryJump(ref velocity, onFloor, isCrouching))
        {
            onFloor = false;
        }

        if (!onFloor)
        {
            velocity.Y -= Gravity * (float)delta;
        }
        else if (velocity.Y < 0.0f)
        {
            velocity.Y = 0.0f;
        }

        Velocity = velocity;
        MoveAndSlide();

        if (isSprinting && IsOnFloor() && input.LengthSquared() > 0.01f)
        {
            _stamina?.DrainPerSecond(SprintStaminaDrainPerSecond, delta);
        }
    }

    private void ApplyGravityOnly(double delta)
    {
        var velocity = Velocity;
        velocity.X = 0.0f;
        velocity.Z = 0.0f;

        if (!IsOnFloor())
        {
            velocity.Y -= Gravity * (float)delta;
        }
        else if (velocity.Y < 0.0f)
        {
            velocity.Y = 0.0f;
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    private bool TryJump(ref Vector3 velocity, bool onFloor, bool isCrouching)
    {
        if (!CanJump || !onFloor || isCrouching || !Input.IsActionJustPressed("jump"))
        {
            return false;
        }

        if (_stamina != null && !_stamina.HasStamina(JumpStaminaCost))
        {
            return false;
        }

        _stamina?.Consume(JumpStaminaCost);
        velocity.Y = JumpVelocity;
        return true;
    }
}
