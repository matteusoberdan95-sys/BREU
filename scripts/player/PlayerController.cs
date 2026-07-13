namespace BREU.Scripts.Player;

/// <summary>
/// Horizontal movement, gravity, sprint integration and floor physics for the FPS player.
/// Movement uses flattened body yaw (-Basis.Z forward, Basis.X right).
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
    [Export] public float FloorSnapDistance { get; set; } = 0.5f;
    [Export] public float JumpStaminaCost { get; set; } = 15.0f;
    [Export] public float SprintStaminaDrainPerSecond { get; set; } = 18.0f;
    [Export] public bool CanJump { get; set; } = true;

    [Export] public NodePath StaminaPath { get; set; } = "PlayerStamina";
    [Export] public NodePath CrouchPath { get; set; } = "PlayerCrouch";

    public bool MovementEnabled { get; set; } = true;
    public Vector2 MoveInput { get; private set; }
    public bool IsSprinting { get; private set; }
    public bool IsMovingHorizontally { get; private set; }
    public bool IsMovingForward { get; private set; }
    public bool IsCrouching => _crouch?.IsCrouching ?? false;
    public float HorizontalSpeed { get; private set; }
    public bool IsOnGround { get; private set; }

    private PlayerStamina? _stamina;
    private PlayerCrouch? _crouch;
    private bool _wasOnFloor;
    private float _lastVerticalVelocity;
    private float _pendingLandingImpact;

    public override void _Ready()
    {
        AddToGroup("player");
        _stamina = GetNodeOrNull<PlayerStamina>(StaminaPath);
        _crouch = GetNodeOrNull<PlayerCrouch>(CrouchPath);
        // Keep the capsule attached to authored stair ramps while descending.
        // The previous 0.1 m snap was shorter than the per-frame drop at sprint
        // speed, causing repeated airborne/landing cycles and downhill launches.
        FloorSnapLength = FloorSnapDistance;
        FloorConstantSpeed = true;
        FloorMaxAngle = Mathf.DegToRad(46.0f);
        SafeMargin = 0.08f;
        _wasOnFloor = IsOnFloor();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!MovementEnabled)
        {
            MoveInput = Vector2.Zero;
            IsSprinting = false;
            IsMovingHorizontally = false;
            IsMovingForward = false;
            HorizontalSpeed = 0.0f;
            ApplyGravityOnly(delta);
            return;
        }

        var input = ReadMoveInput();
        MoveInput = input;

        var direction = GetHorizontalMoveDirection(input);
        var isCrouching = IsCrouching;
        var hasMoveIntent = HasMoveIntent(input);
        var wantsSprint = Input.IsActionPressed("sprint") && hasMoveIntent && !isCrouching;
        var canSprint = wantsSprint && (_stamina == null || _stamina.Current > 0.0f);
        IsSprinting = canSprint;
        IsMovingForward = input.Y > 0.15f || Input.IsActionPressed("move_forward");

        var speed = isCrouching ? CrouchSpeed : IsSprinting ? SprintSpeed : WalkSpeed;

        var velocity = Velocity;
        var onFloor = IsOnFloor();
        IsOnGround = onFloor;
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
        HorizontalSpeed = currentHorizontal.Length();
        IsMovingHorizontally = HorizontalSpeed > 0.15f;

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

        if (onFloor && !_wasOnFloor && _lastVerticalVelocity < -1.0f)
        {
            _pendingLandingImpact = Mathf.Clamp(Mathf.Abs(_lastVerticalVelocity) / 6.0f, 0.05f, 1.0f);
        }

        _lastVerticalVelocity = Velocity.Y;
        _wasOnFloor = IsOnFloor();

        if (IsSprinting && IsOnFloor() && hasMoveIntent)
        {
            _stamina?.DrainPerSecond(SprintStaminaDrainPerSecond, delta);
        }
    }

    /// <summary>
    /// Reads movement from individual actions so Alt/modifiers do not zero out GetVector.
    /// </summary>
    private static Vector2 ReadMoveInput()
    {
        var input = new Vector2(
            Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
            Input.GetActionStrength("move_forward") - Input.GetActionStrength("move_backward"));

        // Alt+W can fail action strength on Windows — preserve forward while look_back + sprint.
        if (Input.IsActionPressed("look_back")
            && (Input.IsActionPressed("sprint") || Input.IsPhysicalKeyPressed(Key.Shift))
            && Input.IsPhysicalKeyPressed(Key.W)
            && input.Y < 0.5f)
        {
            input.Y = 1.0f;
        }

        return input;
    }

    private static bool HasMoveIntent(Vector2 input)
    {
        return input.LengthSquared() > 0.01f
            || Input.IsActionPressed("move_forward")
            || Input.IsActionPressed("move_backward")
            || Input.IsActionPressed("move_left")
            || Input.IsActionPressed("move_right")
            || Input.IsPhysicalKeyPressed(Key.W)
            || Input.IsPhysicalKeyPressed(Key.A)
            || Input.IsPhysicalKeyPressed(Key.S)
            || Input.IsPhysicalKeyPressed(Key.D);
    }

    private Vector3 GetHorizontalMoveDirection(Vector2 input)
    {
        if (input.LengthSquared() <= 0.001f)
        {
            return Vector3.Zero;
        }

        var basis = GlobalTransform.Basis;
        // Body yaw forward: -Z in Godot. Flatten so pitch does not affect movement.
        var forward = -basis.Z;
        var right = basis.X;
        forward.Y = 0.0f;
        right.Y = 0.0f;

        if (forward.LengthSquared() > 0.0001f)
        {
            forward = forward.Normalized();
        }

        if (right.LengthSquared() > 0.0001f)
        {
            right = right.Normalized();
        }

        var direction = forward * input.Y + right * input.X;
        return direction.LengthSquared() > 0.001f ? direction.Normalized() : Vector3.Zero;
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

    /// <summary>Consumed once by PlayerCameraFeel for landing bob.</summary>
    public float ConsumeLandingImpact()
    {
        var impact = _pendingLandingImpact;
        _pendingLandingImpact = 0.0f;
        return impact;
    }
}
