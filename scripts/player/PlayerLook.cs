using Godot;

namespace BREU.Scripts.Player;

public partial class PlayerLook : Node3D
{
    [Export] public NodePath BodyPath { get; set; } = "..";
    [Export] public float MouseSensitivity { get; set; } = 0.0022f;
    [Export] public float MinPitchDegrees { get; set; } = -82.0f;
    [Export] public float MaxPitchDegrees { get; set; } = 82.0f;

    private CharacterBody3D? _body;
    private float _pitch;

    public override void _Ready()
    {
        _body = GetNodeOrNull<CharacterBody3D>(BodyPath);
        _pitch = Rotation.X;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventMouseMotion motion || Input.MouseMode != Input.MouseModeEnum.Captured)
        {
            return;
        }

        _body?.RotateY(-motion.Relative.X * MouseSensitivity);
        _pitch = Mathf.Clamp(
            _pitch - motion.Relative.Y * MouseSensitivity,
            Mathf.DegToRad(MinPitchDegrees),
            Mathf.DegToRad(MaxPitchDegrees));
        Rotation = new Vector3(_pitch, 0.0f, 0.0f);
    }
}
