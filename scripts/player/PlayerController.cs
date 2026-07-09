namespace BREU.Scripts.Player;

public partial class PlayerController : CharacterBody3D
{
    [Export] public float WalkSpeed { get; set; } = 3.2f;
    [Export] public float SprintSpeed { get; set; } = 5.2f;
    [Export] public float Gravity { get; set; } = 18.0f;
    [Export] public NodePath CameraPath { get; set; } = "CameraPivot/Camera3D";

    public bool MovementEnabled { get; set; } = true;

    public override void _Ready()
    {
        EnsureInputMap();
        AddToGroup("player");
        GetNodeOrNull<Camera3D>(CameraPath)?.MakeCurrent();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!MovementEnabled)
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
            return;
        }

        var input = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        var direction = (GlobalTransform.Basis.X * input.X + GlobalTransform.Basis.Z * input.Y).Normalized();
        var speed = Input.IsActionPressed("sprint") ? SprintSpeed : WalkSpeed;

        var moveVelocity = Velocity;
        moveVelocity.X = direction.X * speed;
        moveVelocity.Z = direction.Z * speed;

        if (!IsOnFloor())
        {
            moveVelocity.Y -= Gravity * (float)delta;
        }
        else if (moveVelocity.Y < 0.0f)
        {
            moveVelocity.Y = 0.0f;
        }

        Velocity = moveVelocity;
        MoveAndSlide();
    }

    private static void EnsureInputMap()
    {
        EnsureKeyAction("move_forward", Key.W);
        EnsureKeyAction("move_backward", Key.S);
        EnsureKeyAction("move_left", Key.A);
        EnsureKeyAction("move_right", Key.D);
        EnsureKeyAction("sprint", Key.Shift);
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
