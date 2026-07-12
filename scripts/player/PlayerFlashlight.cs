namespace BREU.Scripts.Player;

using BREU.Scripts.Audio;

/// <summary>
/// Flashlight with battery drain. Infinite battery when PlaytestDebugSettings allows.
/// Sprint 16 — optional click SFX via PensionAudioManager (no logic change).
/// </summary>
public partial class PlayerFlashlight : SpotLight3D
{
    [Signal] public delegate void BatteryChangedEventHandler(float current, float maximum);
    [Signal] public delegate void LanternToggledEventHandler(bool isOn);

    [Export] public bool StartsEnabled { get; set; } = false;
    [Export] public float MaxBattery { get; set; } = 100.0f;
    [Export] public float DrainPerSecond { get; set; } = 0.167f;

    public bool IsOn { get; private set; }
    public float CurrentBattery { get; private set; }

    public override void _Ready()
    {
        CurrentBattery = MaxBattery;
        IsOn = StartsEnabled && CurrentBattery > 0.0f;
        ApplyLightState();
        EmitSignal(SignalName.BatteryChanged, CurrentBattery, MaxBattery);
        EmitSignal(SignalName.LanternToggled, IsOn);
    }

    public override void _Process(double delta)
    {
        if (!IsOn || CurrentBattery <= 0.0f || IsInfiniteBattery())
        {
            return;
        }

        var previous = CurrentBattery;
        CurrentBattery = Mathf.Max(0.0f, CurrentBattery - DrainPerSecond * (float)delta);
        if (!Mathf.IsEqualApprox(previous, CurrentBattery))
        {
            EmitSignal(SignalName.BatteryChanged, CurrentBattery, MaxBattery);
        }

        if (CurrentBattery <= 0.0f)
        {
            ForceOff("Bateria esgotada.");
        }
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
        if (IsOn)
        {
            IsOn = false;
            ApplyLightState();
            EmitSignal(SignalName.LanternToggled, IsOn);
            PensionAudioManager.Find(GetTree())?.PlayFlashlightClick(turningOn: false);
            return;
        }

        if (CurrentBattery <= 0.0f && !IsInfiniteBattery())
        {
            ForceOff("Lanterna sem carga.");
            return;
        }

        IsOn = true;
        ApplyLightState();
        EmitSignal(SignalName.LanternToggled, IsOn);
        PensionAudioManager.Find(GetTree())?.PlayFlashlightClick(turningOn: true);
    }

    public void Recharge(float amount)
    {
        if (amount <= 0.0f)
        {
            return;
        }

        CurrentBattery = Mathf.Min(MaxBattery, CurrentBattery + amount);
        EmitSignal(SignalName.BatteryChanged, CurrentBattery, MaxBattery);
    }

    private void ForceOff(string? hudMessage = null)
    {
        var wasOn = IsOn;
        IsOn = false;
        ApplyLightState();
        EmitSignal(SignalName.LanternToggled, IsOn);
        if (wasOn)
        {
            PensionAudioManager.Find(GetTree())?.PlayFlashlightClick(turningOn: false);
        }

        if (!string.IsNullOrEmpty(hudMessage))
        {
            HUDController.FindActive(GetTree())?.ShowMessage(hudMessage, 2.5f);
        }
    }

    private void ApplyLightState()
    {
        Visible = IsOn;
        LightEnergy = IsOn ? 1.35f : 0.0f;
        SpotAngle = 28.0f;
        SpotAngleAttenuation = 1.2f;
        ShadowEnabled = false;
    }

    private bool IsInfiniteBattery()
    {
        return PlaytestDebugSettings.Instance?.InfiniteLanternBattery ?? true;
    }
}
