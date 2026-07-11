namespace BREU.Scripts.Player;

/// <summary>
/// Base flashlight: SpotLight3D toggle. Battery system deferred to Sprint 03+.
/// </summary>
public partial class PlayerFlashlight : SpotLight3D
{
    [Export] public bool DebugInfiniteLantern { get; set; } = true;
    [Export] public bool StartsEnabled { get; set; } = true;

    public bool IsOn { get; private set; }

    public override void _Ready()
    {
        IsOn = StartsEnabled;
        Visible = IsOn;
        SpotAngle = 28.0f;
        SpotAngleAttenuation = 1.2f;
        LightEnergy = 1.35f;
        ShadowEnabled = true;
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
    }
}
