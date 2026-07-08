using Godot;

namespace BREU.Scripts.Player;

public partial class FlashlightController : SpotLight3D
{
    [Signal] public delegate void BatteryChangedEventHandler(float current, float maximum);

    [Export] public float MaxBattery { get; set; } = 100.0f;
    [Export] public float DrainPerSecond { get; set; } = 3.5f;
    [Export] public float FlickerNoise { get; set; } = 0.08f;

    public float CurrentBattery { get; private set; }
    public bool IsOn { get; private set; } = true;

    private float _baseEnergy;
    private RandomNumberGenerator _rng = new();

    public override void _Ready()
    {
        CurrentBattery = MaxBattery;
        _baseEnergy = LightEnergy;
        Visible = IsOn;
        EmitSignal(SignalName.BatteryChanged, CurrentBattery, MaxBattery);
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("flashlight_toggle"))
        {
            Toggle();
        }

        if (!IsOn)
        {
            return;
        }

        CurrentBattery = Mathf.Max(0.0f, CurrentBattery - DrainPerSecond * (float)delta);
        if (CurrentBattery <= 0.0f)
        {
            IsOn = false;
            Visible = false;
        }

        LightEnergy = _baseEnergy + _rng.RandfRange(-FlickerNoise, FlickerNoise);
        EmitSignal(SignalName.BatteryChanged, CurrentBattery, MaxBattery);
    }

    public void Toggle()
    {
        if (CurrentBattery <= 0.0f)
        {
            GD.Print("Lanterna sem bateria.");
            return;
        }

        IsOn = !IsOn;
        Visible = IsOn;
        GD.Print(IsOn ? "Lanterna ligada." : "Lanterna desligada.");
    }

    public void Recharge(float amount)
    {
        CurrentBattery = Mathf.Min(MaxBattery, CurrentBattery + amount);
        EmitSignal(SignalName.BatteryChanged, CurrentBattery, MaxBattery);
    }
}
