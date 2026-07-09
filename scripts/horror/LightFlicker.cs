namespace BREU.Scripts.Horror;

/// <summary>
/// Oscilacao discreta para luzes antigas ou instaveis.
/// </summary>
public partial class LightFlicker : Node
{
    [Export] public NodePath LightPath { get; set; } = new("");
    [Export] public float MinEnergy { get; set; } = 0.8f;
    [Export] public float MaxEnergy { get; set; } = 1.6f;
    [Export] public float FlickerSpeed { get; set; } = 0.12f;
    [Export] public bool StartsEnabled { get; set; } = true;

    private readonly RandomNumberGenerator _random = new();
    private Light3D? _light;
    private float _timeUntilNextFlicker;

    public override void _Ready()
    {
        _random.Randomize();
        _light = LightPath.IsEmpty
            ? GetParentOrNull<Light3D>()
            : GetNodeOrNull<Light3D>(LightPath);
        SetProcess(StartsEnabled && _light != null);
    }

    public override void _Process(double delta)
    {
        if (_light == null)
        {
            return;
        }

        _timeUntilNextFlicker -= (float)delta;
        if (_timeUntilNextFlicker > 0.0f)
        {
            return;
        }

        _light.LightEnergy = _random.RandfRange(MinEnergy, MaxEnergy);
        _timeUntilNextFlicker = FlickerSpeed * _random.RandfRange(0.65f, 1.65f);
    }
}
