namespace BREU.Scripts.Player;

/// <summary>
/// Sprint look-back (Alt) — visual yaw on LookBackPivot. Does not affect movement.
/// </summary>
public partial class PlayerLookBack : Node3D
{
    [Export] public NodePath ControllerPath { get; set; } = new NodePath("../../..");
    [Export] public float LookBackAngleDegrees { get; set; } = 170.0f;
    [Export] public float LookBackSpeed { get; set; } = 10.0f;

    public float LookBackBlend => _lookBackBlend;

    private PlayerController? _controller;
    private float _lookBackBlend;

    public override void _Ready()
    {
        _controller = GetNodeOrNull<PlayerController>(ControllerPath);
    }

    public override void _PhysicsProcess(double delta)
    {
        var wantsLookBack = false;
        if (_controller != null)
        {
            wantsLookBack = Input.IsActionPressed("look_back")
                && _controller.IsSprinting
                && _controller.IsMovingForward;
        }

        var target = wantsLookBack ? 1.0f : 0.0f;
        _lookBackBlend = Mathf.MoveToward(_lookBackBlend, target, LookBackSpeed * (float)delta);
        Rotation = new Vector3(0.0f, Mathf.DegToRad(LookBackAngleDegrees) * _lookBackBlend, 0.0f);
    }
}
