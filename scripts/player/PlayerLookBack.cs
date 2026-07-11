namespace BREU.Scripts.Player;

/// <summary>
/// Sprint look-back — visual yaw on LookBackPivot only. Never affects movement or sprint.
/// </summary>
public partial class PlayerLookBack : Node3D
{
    [Export] public NodePath ControllerPath { get; set; } = new NodePath("../../../..");
    [Export] public float LookBackAngleDegrees { get; set; } = 165.0f;
    [Export] public float LookBackEnterSpeed { get; set; } = 8.0f;
    [Export] public float LookBackExitSpeed { get; set; } = 10.0f;
    [Export] public bool AllowLookBackWithoutSprintForDebug { get; set; } = true;
    [Export] public bool DebugMode { get; set; } = false;

    public float LookBackBlend { get; private set; }

    private PlayerController? _controller;

    public override void _Ready()
    {
        _controller = GetNodeOrNull<PlayerController>(ControllerPath);
        if (_controller == null)
        {
            GD.PushWarning("[PlayerLookBack] PlayerController not found at: " + ControllerPath);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        var dt = (float)delta;
        var wantsLookBack = Input.IsActionPressed("look_back");
        var shouldLookBack = EvaluateShouldLookBack(wantsLookBack);

        var targetYaw = shouldLookBack ? Mathf.DegToRad(LookBackAngleDegrees) : 0.0f;
        var lerpSpeed = shouldLookBack ? LookBackEnterSpeed : LookBackExitSpeed;
        var newYaw = Mathf.LerpAngle(Rotation.Y, targetYaw, dt * lerpSpeed);

        Rotation = new Vector3(Rotation.X, newYaw, Rotation.Z);
        LookBackBlend = Mathf.Clamp(Mathf.Abs(newYaw / Mathf.DegToRad(LookBackAngleDegrees)), 0.0f, 1.0f);

        if (DebugMode && wantsLookBack)
        {
            GD.Print(
                $"[PlayerLookBack] input={wantsLookBack}, sprint={IsSprintHeld()}, " +
                $"forward={IsForwardHeld()}, should={shouldLookBack}, " +
                $"targetYaw={targetYaw:F3}, currentY={Rotation.Y:F3}, blend={LookBackBlend:F2}");
        }
    }

    private bool EvaluateShouldLookBack(bool wantsLookBack)
    {
        if (!wantsLookBack)
        {
            return false;
        }

        if (AllowLookBackWithoutSprintForDebug)
        {
            return true;
        }

        if (_controller != null && _controller.IsCrouching)
        {
            return false;
        }

        var isRunningForward = IsSprintHeld() && IsForwardHeld();
        if (isRunningForward)
        {
            return true;
        }

        return _controller != null
            && _controller.IsSprinting
            && _controller.HorizontalSpeed > _controller.WalkSpeed * 0.5f;
    }

    private static bool IsSprintHeld()
    {
        return Input.IsActionPressed("sprint") || Input.IsPhysicalKeyPressed(Key.Shift);
    }

    private static bool IsForwardHeld()
    {
        return Input.IsActionPressed("move_forward") || Input.IsPhysicalKeyPressed(Key.W);
    }
}
