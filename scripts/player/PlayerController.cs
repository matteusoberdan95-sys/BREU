namespace BREU.Scripts.Player;

public partial class PlayerController : CharacterBody3D
{
    [Export] public float WalkSpeed { get; set; } = 3.2f;
    [Export] public float SprintSpeed { get; set; } = 5.2f;
    [Export] public float CrouchSpeed { get; set; } = 1.5f;
    [Export] public float Gravity { get; set; } = 18.0f;
    [Export] public float JumpVelocity { get; set; } = 4.0f;
    [Export] public float JumpStaminaCost { get; set; } = 12.0f;
    [Export] public float SprintStaminaDrainPerSecond { get; set; } = 14.0f;
    [Export] public float Acceleration { get; set; } = 12.0f;
    [Export] public float Deceleration { get; set; } = 14.0f;
    [Export] public float AirControl { get; set; } = 0.4f;
    [Export] public bool CanJump { get; set; } = true;
    [Export] public float StandingCapsuleHeight { get; set; } = 1.8f;
    [Export] public float CrouchingCapsuleHeight { get; set; } = 1.0f;
    [Export] public float StandingCameraHeight { get; set; } = 0.6f;
    [Export] public float CrouchingCameraHeight { get; set; } = 0.35f;
    [Export] public float CrouchTransitionSpeed { get; set; } = 12.0f;
    [Export] public NodePath CameraPath { get; set; } = "CameraPivot/Camera3D";
    [Export] public NodePath CameraPivotPath { get; set; } = "CameraPivot";
    [Export] public NodePath CollisionShapePath { get; set; } = "CollisionShape3D";
    [Export] public NodePath FootstepAudioPath { get; set; } = "FootstepAudio";
    [Export] public NodePath StaminaPath { get; set; } = "PlayerStamina";

    public bool MovementEnabled { get; set; } = true;
    public bool IsCrouching { get; private set; }

    private PlayerFootstepAudio? _footstepAudio;
    private PlayerStamina? _stamina;
    private CollisionShape3D? _collisionShape;
    private CapsuleShape3D? _capsuleShape;
    private Node3D? _cameraPivot;
    private bool _wasOnFloor = true;
    private float _crouchBlend;
    private float _standingCollisionCenterY;
    private Vector3 _cameraPivotBasePosition;
    private Tween? _cameraShakeTween;

    public override void _Ready()
    {
        EnsureInputMap();
        AddToGroup("player");
        GetNodeOrNull<Camera3D>(CameraPath)?.MakeCurrent();
        _footstepAudio = GetNodeOrNull<PlayerFootstepAudio>(FootstepAudioPath);
        _stamina = GetNodeOrNull<PlayerStamina>(StaminaPath);
        _collisionShape = GetNodeOrNull<CollisionShape3D>(CollisionShapePath);
        _cameraPivot = GetNodeOrNull<Node3D>(CameraPivotPath);
        _cameraPivotBasePosition = _cameraPivot?.Position ?? Vector3.Zero;
        if (_collisionShape?.Shape is CapsuleShape3D capsule)
        {
            _capsuleShape = capsule;
            StandingCapsuleHeight = capsule.Height;
            _standingCollisionCenterY = _collisionShape.Position.Y;
        }

        _wasOnFloor = IsOnFloor();
        ApplyCrouchPose(_crouchBlend);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!MovementEnabled)
        {
            ProcessBlockedMovement(delta);
            return;
        }

        var input = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        var direction = (GlobalTransform.Basis.X * input.X + GlobalTransform.Basis.Z * input.Y).Normalized();
        UpdateCrouchState(delta);

        var wantsSprint = Input.IsActionPressed("sprint") && input.LengthSquared() > 0.01f && !IsCrouching;
        var canSprint = wantsSprint && (_stamina == null || _stamina.Current > 0.0f);
        var isSprinting = canSprint;
        var speed = IsCrouching ? CrouchSpeed : isSprinting ? SprintSpeed : WalkSpeed;

        var moveVelocity = Velocity;
        var isOnFloorBeforeMove = IsOnFloor();
        var targetHorizontal = new Vector2(direction.X * speed, direction.Z * speed);
        var currentHorizontal = new Vector2(moveVelocity.X, moveVelocity.Z);
        var movementRate = targetHorizontal.LengthSquared() > 0.001f ? Acceleration : Deceleration;
        if (!isOnFloorBeforeMove)
        {
            movementRate *= AirControl;
        }

        currentHorizontal = currentHorizontal.MoveToward(targetHorizontal, movementRate * (float)delta);
        moveVelocity.X = currentHorizontal.X;
        moveVelocity.Z = currentHorizontal.Y;

        if (TryJump(ref moveVelocity, isOnFloorBeforeMove))
        {
            _footstepAudio?.PlayJump();
        }

        if (!isOnFloorBeforeMove)
        {
            moveVelocity.Y -= Gravity * (float)delta;
        }
        else if (moveVelocity.Y < 0.0f)
        {
            moveVelocity.Y = 0.0f;
        }

        var verticalBeforeMove = moveVelocity.Y;
        Velocity = moveVelocity;
        MoveAndSlide();

        var isOnFloor = IsOnFloor();
        if (isOnFloor && !_wasOnFloor)
        {
            _footstepAudio?.PlayLand(verticalBeforeMove);
        }

        if (isSprinting && isOnFloor && input.LengthSquared() > 0.01f)
        {
            _stamina?.DrainPerSecond(SprintStaminaDrainPerSecond, delta);
        }

        var horizontalSpeed = new Vector2(Velocity.X, Velocity.Z).Length();
        var isMoving = isOnFloor && horizontalSpeed >= (_footstepAudio?.MinMoveSpeedForSteps ?? 0.2f);
        _footstepAudio?.UpdateMovementAudio(isMoving, isSprinting && isMoving, IsCrouching && isMoving, isOnFloor, delta);

        _wasOnFloor = isOnFloor;
    }

    private void ProcessBlockedMovement(double delta)
    {
        var blockedVelocity = Velocity;
        blockedVelocity.X = 0.0f;
        blockedVelocity.Z = 0.0f;
        if (!IsOnFloor())
        {
            blockedVelocity.Y -= Gravity * (float)delta;
        }
        else if (blockedVelocity.Y < 0.0f)
        {
            blockedVelocity.Y = 0.0f;
        }

        Velocity = blockedVelocity;
        MoveAndSlide();
        _wasOnFloor = IsOnFloor();
    }

    private void UpdateCrouchState(double delta)
    {
        var wantsCrouch = Input.IsActionPressed("crouch");
        if (!wantsCrouch && IsCrouching && !CanStandUp())
        {
            wantsCrouch = true;
        }

        IsCrouching = wantsCrouch;

        var targetBlend = IsCrouching ? 1.0f : 0.0f;
        _crouchBlend = Mathf.MoveToward(_crouchBlend, targetBlend, CrouchTransitionSpeed * (float)delta);
        ApplyCrouchPose(_crouchBlend);
    }

    private void ApplyCrouchPose(float blend)
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

        if (_cameraPivot != null)
        {
            var cameraHeight = Mathf.Lerp(StandingCameraHeight, CrouchingCameraHeight, blend);
            _cameraPivotBasePosition = new Vector3(0.0f, cameraHeight, 0.0f);
            if (_cameraShakeTween == null || !_cameraShakeTween.IsRunning())
            {
                _cameraPivot.Position = _cameraPivotBasePosition;
            }
        }
    }

    public void ApplyCameraShake(float strength, float duration)
    {
        if (_cameraPivot == null || strength <= 0.0f || duration <= 0.0f)
        {
            return;
        }

        _cameraShakeTween?.Kill();
        var random = new RandomNumberGenerator();
        random.Randomize();
        var offset = new Vector3(
            random.RandfRange(-strength, strength),
            random.RandfRange(-strength * 0.5f, strength * 0.5f),
            0.0f);

        _cameraPivot.Position = _cameraPivotBasePosition + offset;
        _cameraShakeTween = CreateTween();
        _cameraShakeTween.TweenProperty(_cameraPivot, "position", _cameraPivotBasePosition, duration);
    }

    private bool CanStandUp()
    {
        if (_capsuleShape == null)
        {
            return true;
        }

        var extraHeight = StandingCapsuleHeight - CrouchingCapsuleHeight;
        if (extraHeight <= 0.01f)
        {
            return true;
        }

        var feetY = GlobalPosition.Y + _standingCollisionCenterY - CrouchingCapsuleHeight * 0.5f;
        var from = new Vector3(GlobalPosition.X, feetY + CrouchingCapsuleHeight, GlobalPosition.Z);
        var to = from + Vector3.Up * extraHeight;
        var query = PhysicsRayQueryParameters3D.Create(from, to);
        query.CollisionMask = CollisionMask;
        query.Exclude = [GetRid()];

        return GetWorld3D().DirectSpaceState.IntersectRay(query).Count == 0;
    }

    private bool TryJump(ref Vector3 velocity, bool isOnFloor)
    {
        if (!CanJump || !isOnFloor || IsCrouching || !Input.IsActionJustPressed("jump"))
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

    private static void EnsureInputMap()
    {
        EnsureKeyAction("move_forward", Key.W);
        EnsureKeyAction("move_backward", Key.S);
        EnsureKeyAction("move_left", Key.A);
        EnsureKeyAction("move_right", Key.D);
        EnsureKeyAction("sprint", Key.Shift);
        EnsureKeyAction("crouch", Key.Ctrl);
        EnsureKeyAction("crouch", Key.C);
        EnsureKeyAction("jump", Key.Space);
        EnsureKeyAction("flashlight_toggle", Key.F);
        EnsureKeyAction("interact", Key.E);
        EnsureKeyAction("lean_left", Key.Q);
        EnsureKeyAction("lean_right", Key.R);
        EnsureKeyAction("pause", Key.Escape);
        EnsureMouseButtonAction("attack_primary", MouseButton.Left);
    }

    private static void EnsureKeyAction(string actionName, Key key)
    {
        if (!InputMap.HasAction(actionName))
        {
            InputMap.AddAction(actionName);
        }

        foreach (var existingEvent in InputMap.ActionGetEvents(actionName))
        {
            if (existingEvent is InputEventKey existingKey &&
                (existingKey.Keycode == key || existingKey.PhysicalKeycode == key))
            {
                return;
            }
        }

        InputMap.ActionAddEvent(actionName, new InputEventKey
        {
            Keycode = key,
            PhysicalKeycode = key
        });
    }

    private static void EnsureMouseButtonAction(string actionName, MouseButton button)
    {
        if (!InputMap.HasAction(actionName))
        {
            InputMap.AddAction(actionName);
        }

        foreach (var existingEvent in InputMap.ActionGetEvents(actionName))
        {
            if (existingEvent is InputEventMouseButton existingButton &&
                existingButton.ButtonIndex == button)
            {
                return;
            }
        }

        InputMap.ActionAddEvent(actionName, new InputEventMouseButton
        {
            ButtonIndex = button
        });
    }
}
