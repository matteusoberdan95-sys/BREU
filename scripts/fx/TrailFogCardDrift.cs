namespace BREU.Scripts.Fx;

/// <summary>
/// Drift leve para fog cards da TrailIntro — movimento organico quase imperceptivel.
/// </summary>
public partial class TrailFogCardDrift : Sprite3D
{
    [Export] public Vector3 DriftAxis { get; set; } = new(1.0f, 0.0f, 0.25f);
    [Export] public float DriftAmplitude { get; set; } = 0.45f;
    [Export] public float DriftSpeed { get; set; } = 0.12f;
    [Export] public float VerticalAmplitude { get; set; } = 0.06f;
    [Export] public float VerticalSpeed { get; set; } = 0.18f;
    [Export] public float PhaseOffset { get; set; }

    private Vector3 _origin;
    private bool _initialized;

    public override void _Ready()
    {
        _origin = Position;
        _initialized = true;
    }

    public override void _Process(double delta)
    {
        if (!_initialized)
        {
            return;
        }

        var time = (float)Time.GetTicksMsec() * 0.001f + PhaseOffset;
        var axis = DriftAxis.LengthSquared() > 0.0001f ? DriftAxis.Normalized() : Vector3.Right;
        var horizontal = axis * (Mathf.Sin(time * DriftSpeed) * DriftAmplitude);
        var vertical = Vector3.Up * (Mathf.Sin(time * VerticalSpeed + PhaseOffset * 1.7f) * VerticalAmplitude);
        Position = _origin + horizontal + vertical;
    }
}
