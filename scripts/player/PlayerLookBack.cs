namespace BREU.Scripts.Player;

/// <summary>
/// Sprint look-back — visual yaw on LookBackPivot only. Never affects movement or sprint state.
/// </summary>
public partial class PlayerLookBack : Node3D
{
    [Export] public NodePath ControllerPath { get; set; } = new NodePath("../../..");
    [Export] public float LookBackAngleDegrees { get; set; } = 165.0f;
    [Export] public float LookBackEnterSpeed { get; set; } = 8.0f;
    [Export] public float LookBackExitSpeed { get; set; } = 10.0f;

    public float LookBackBlend => _lookBackBlend;

    private PlayerController? _controller;
    private float _lookBackBlend;

    public override void _Ready()
    {
        _controller = GetNodeOrNull<PlayerController>(ControllerPath);
    }

    public override void _PhysicsProcess(double delta)
    {
        var dt = (float)delta;
        var wantsLookBack = EvaluateLookBackRequest();
        var speed = wantsLookBack ? LookBackEnterSpeed : LookBackExitSpeed;
        var target = wantsLookBack ? 1.0f : 0.0f;
        _lookBackBlend = Mathf.MoveToward(_lookBackBlend, target, speed * dt);

        // Local Y only — camera looks over shoulder backward while body keeps running forward.
        Rotation = new Vector3(0.0f, Mathf.DegToRad(LookBackAngleDegrees) * _lookBackBlend, 0.0f);
    }

    private bool EvaluateLookBackRequest()
    {
        if (!Input.IsActionPressed("look_back"))
        {
            return false;
        }

        if (!Input.IsActionPressed("sprint"))
        {
            return false;
        }

        if (_controller == null || _controller.IsCrouching)
        {
            return false;
        }

        // Use raw actions — Alt+W breaks Input.GetVector on some platforms.
        var forwardHeld = Input.IsActionPressed("move_forward");
        var stillRunning = _controller.HorizontalSpeed > _controller.WalkSpeed * 0.75f;

        return forwardHeld || stillRunning;
    }
}
