namespace BREU.Scripts.Player;

/// <summary>
/// Procedural head bob, idle sway and strafe tilt — local camera offsets only.
/// </summary>
public partial class PlayerCameraEffects : Node3D
{
    [Export] public NodePath ControllerPath { get; set; } = new NodePath("../../..");

    [Export] public float IdleBobAmount { get; set; } = 0.015f;
    [Export] public float WalkBobAmount { get; set; } = 0.045f;
    [Export] public float SprintBobAmount { get; set; } = 0.075f;
    [Export] public float CrouchBobAmount { get; set; } = 0.025f;

    [Export] public float WalkBobFrequency { get; set; } = 7.0f;
    [Export] public float SprintBobFrequency { get; set; } = 11.0f;
    [Export] public float CrouchBobFrequency { get; set; } = 5.0f;
    [Export] public float IdleSwayFrequency { get; set; } = 1.2f;

    [Export] public float StrafeTiltDegrees { get; set; } = 2.5f;
    [Export] public float SprintExtraTiltDegrees { get; set; } = 1.5f;
    [Export] public float EffectBlendSpeed { get; set; } = 10.0f;

    private PlayerController? _controller;
    private float _bobPhase;
    private float _idlePhase;
    private Vector3 _currentOffset;
    private float _currentRoll;

    public override void _Ready()
    {
        _controller = GetNodeOrNull<PlayerController>(ControllerPath);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_controller == null)
        {
            return;
        }

        var dt = (float)delta;
        var speed = _controller.HorizontalSpeed;
        var isMoving = _controller.IsMovingHorizontally;
        var isSprinting = _controller.IsSprinting;
        var isCrouching = _controller.IsCrouching;
        var strafeInput = _controller.MoveInput.X;

        var bobAmount = IdleBobAmount;
        var bobFrequency = IdleSwayFrequency;

        if (isMoving)
        {
            if (isCrouching)
            {
                bobAmount = CrouchBobAmount;
                bobFrequency = CrouchBobFrequency;
            }
            else if (isSprinting)
            {
                bobAmount = SprintBobAmount;
                bobFrequency = SprintBobFrequency;
            }
            else
            {
                bobAmount = WalkBobAmount;
                bobFrequency = WalkBobFrequency;
            }

            _bobPhase += dt * bobFrequency * Mathf.Max(0.35f, speed / _controller.WalkSpeed);
        }
        else
        {
            _idlePhase += dt * IdleSwayFrequency;
            _bobPhase = Mathf.Lerp(_bobPhase, 0.0f, dt * EffectBlendSpeed);
        }

        var activePhase = isMoving ? _bobPhase : _idlePhase;
        var vertical = Mathf.Sin(activePhase) * bobAmount;
        var horizontal = Mathf.Cos(activePhase * 0.5f) * bobAmount * 0.55f;

        var targetOffset = isMoving
            ? new Vector3(horizontal, vertical, 0.0f)
            : new Vector3(
                Mathf.Sin(_idlePhase) * IdleBobAmount * 0.35f,
                Mathf.Sin(_idlePhase * 1.7f) * IdleBobAmount * 0.25f,
                0.0f);

        var tilt = -strafeInput * Mathf.DegToRad(StrafeTiltDegrees);
        if (isSprinting && isMoving)
        {
            tilt -= strafeInput * Mathf.DegToRad(SprintExtraTiltDegrees);
        }

        _currentOffset = _currentOffset.Lerp(targetOffset, dt * EffectBlendSpeed);
        _currentRoll = Mathf.LerpAngle(_currentRoll, tilt, dt * EffectBlendSpeed);

        Position = _currentOffset;
        Rotation = new Vector3(0.0f, 0.0f, _currentRoll);
    }
}
