namespace BREU.Scripts.Player;

public partial class PlayerController : CharacterBody3D
{
    [Export] public float WalkSpeed { get; set; } = 3.2f;
    [Export] public float SprintSpeed { get; set; } = 5.2f;
    [Export] public float Gravity { get; set; } = 18.0f;
    [Export] public float JumpVelocity { get; set; } = 4.0f;
    [Export] public float JumpStaminaCost { get; set; } = 12.0f;
    [Export] public bool CanJump { get; set; } = true;
    [Export] public NodePath CameraPath { get; set; } = "CameraPivot/Camera3D";
    [Export] public NodePath FootstepAudioPath { get; set; } = "FootstepAudio";
    [Export] public NodePath StaminaPath { get; set; } = "PlayerStamina";

    public bool MovementEnabled { get; set; } = true;

    private PlayerFootstepAudio? _footstepAudio;
    private PlayerStamina? _stamina;
    private bool _wasOnFloor = true;

    public override void _Ready()
    {
        EnsureInputMap();
        AddToGroup("player");
        GetNodeOrNull<Camera3D>(CameraPath)?.MakeCurrent();
        _footstepAudio = GetNodeOrNull<PlayerFootstepAudio>(FootstepAudioPath);
        _stamina = GetNodeOrNull<PlayerStamina>(StaminaPath);
        _wasOnFloor = IsOnFloor();
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
        var isSprinting = Input.IsActionPressed("sprint") && input.LengthSquared() > 0.01f;
        var speed = isSprinting ? SprintSpeed : WalkSpeed;

        var moveVelocity = Velocity;
        moveVelocity.X = direction.X * speed;
        moveVelocity.Z = direction.Z * speed;

        var isOnFloorBeforeMove = IsOnFloor();

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

        var horizontalSpeed = new Vector2(Velocity.X, Velocity.Z).Length();
        var isMoving = isOnFloor && horizontalSpeed >= (_footstepAudio?.MinMoveSpeedForSteps ?? 0.2f);
        _footstepAudio?.UpdateMovementAudio(isMoving, isSprinting && isMoving, isOnFloor, delta);

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

    private bool TryJump(ref Vector3 velocity, bool isOnFloor)
    {
        if (!CanJump || !isOnFloor || !Input.IsActionJustPressed("jump"))
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
        EnsureKeyAction("jump", Key.Space);
        EnsureKeyAction("flashlight_toggle", Key.F);
        EnsureKeyAction("interact", Key.E);
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
