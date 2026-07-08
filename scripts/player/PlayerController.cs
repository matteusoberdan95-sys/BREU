using Godot;
using BREU.Scripts.Player;

namespace BREU.Scripts.Player;

public partial class PlayerController : CharacterBody3D
{
    [Export] public float WalkSpeed { get; set; } = 3.2f;
    [Export] public float SprintSpeed { get; set; } = 5.2f;
    [Export] public float Acceleration { get; set; } = 12.0f;
    [Export] public float Gravity { get; set; } = 18.0f;
    [Export] public NodePath StaminaPath { get; set; } = "PlayerStamina";
    [Export] public float SprintStaminaCostPerSecond { get; set; } = 18.0f;

    public PlayerStamina? Stamina { get; private set; }

    public override void _Ready()
    {
        AddToGroup("player");
        Stamina = GetNodeOrNull<PlayerStamina>(StaminaPath);
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
        {
            Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured
                ? Input.MouseModeEnum.Visible
                : Input.MouseModeEnum.Captured;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        var input = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        var basis = GlobalTransform.Basis;
        var direction = (basis.X * input.X + basis.Z * input.Y).Normalized();

        var wantsSprint = Input.IsActionPressed("sprint") && input.LengthSquared() > 0.01f;
        var canSprint = Stamina?.HasStamina(1.0f) ?? true;
        var sprinting = wantsSprint && canSprint;
        var targetSpeed = sprinting ? SprintSpeed : WalkSpeed;

        if (sprinting && Stamina != null)
        {
            Stamina.Consume(SprintStaminaCostPerSecond * (float)delta);
        }

        var velocity = Velocity;
        var targetVelocity = direction * targetSpeed;
        velocity.X = Mathf.Lerp(velocity.X, targetVelocity.X, Acceleration * (float)delta);
        velocity.Z = Mathf.Lerp(velocity.Z, targetVelocity.Z, Acceleration * (float)delta);

        if (!IsOnFloor())
        {
            velocity.Y -= Gravity * (float)delta;
        }
        else if (velocity.Y < 0.0f)
        {
            velocity.Y = -0.1f;
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}
