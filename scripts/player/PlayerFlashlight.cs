namespace BREU.Scripts.Player;

/// <summary>
/// Base flashlight: SpotLight3D toggle. Battery system deferred to Sprint 03+.
/// Disabled by default in movement lab — scene uses DirectionalLight3D only.
/// </summary>
public partial class PlayerFlashlight : SpotLight3D
{
    [Export] public bool DebugInfiniteLantern { get; set; } = true;
    [Export] public bool StartsEnabled { get; set; } = false;

    public bool IsOn { get; private set; }

    public override void _Ready()
    {
        IsOn = StartsEnabled;
        Visible = IsOn;
        LightEnergy = IsOn ? 1.35f : 0.0f;
        SpotAngle = 28.0f;
        SpotAngleAttenuation = 1.2f;
        ShadowEnabled = false;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!@event.IsActionPressed("flashlight"))
        {
            return;
        }

        Toggle();
    }

    public void Toggle()
    {
        IsOn = !IsOn;
        Visible = IsOn;
        LightEnergy = IsOn ? 1.35f : 0.0f;
    }
}
