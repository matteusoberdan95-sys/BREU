namespace BREU.Scripts.Player;

/// <summary>
/// Mouse look: yaw on Player body, pitch on PitchPivot (this node).
/// </summary>
public partial class PlayerLook : Node3D
{
    [Export] public NodePath BodyPath { get; set; } = new NodePath("../../../../../");
    [Export] public float MouseSensitivity { get; set; } = 0.12f;
    [Export] public float MinPitchDegrees { get; set; } = -85.0f;
    [Export] public float MaxPitchDegrees { get; set; } = 85.0f;

    private Node3D? _body;
    private float _pitch;

    public override void _Ready()
    {
        _body = GetNodeOrNull<Node3D>(BodyPath);
        _pitch = Rotation.X;
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel") || @event.IsActionPressed("pause"))
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
            return;
        }

        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
            return;
        }

        if (@event is not InputEventMouseMotion motion || Input.MouseMode != Input.MouseModeEnum.Captured)
        {
            return;
        }

        var scale = MouseSensitivity * 0.025f;
        _body?.RotateY(-motion.Relative.X * scale);
        _pitch = Mathf.Clamp(
            _pitch - motion.Relative.Y * scale,
            Mathf.DegToRad(MinPitchDegrees),
            Mathf.DegToRad(MaxPitchDegrees));
        Rotation = new Vector3(_pitch, 0.0f, 0.0f);
    }
}
