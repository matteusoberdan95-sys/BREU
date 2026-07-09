using Godot;

namespace BREU.Scripts.Player;

public partial class FlashlightController : SpotLight3D
{
    [Signal] public delegate void BatteryChangedEventHandler(float current, float maximum);

    [Export] public float MaxBattery { get; set; } = 100.0f;
    [Export] public float DrainPerSecond { get; set; } = 3.5f;

    private float _battery;
    private bool _isOn = true;
    private float _debugPrintTimer;

    public override void _Ready()
    {
        _battery = MaxBattery;
        Visible = true;
        EmitSignal(SignalName.BatteryChanged, _battery, MaxBattery);
        GD.Print($"Lanterna: {_battery:0}%");
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("flashlight_toggle"))
        {
            Toggle();
        }

        if (!_isOn)
        {
            return;
        }

        _battery = Mathf.Max(0.0f, _battery - DrainPerSecond * (float)delta);
        if (_battery <= 0.0f)
        {
            _isOn = false;
            Visible = false;
            EmitSignal(SignalName.BatteryChanged, _battery, MaxBattery);
            GD.Print("Lanterna: 0% - bateria esgotada.");
            return;
        }

        EmitSignal(SignalName.BatteryChanged, _battery, MaxBattery);
        _debugPrintTimer += (float)delta;
        if (_debugPrintTimer >= 1.0f)
        {
            _debugPrintTimer = 0.0f;
            GD.Print($"Lanterna: {_battery:0}%");
        }
    }

    private void Toggle()
    {
        if (_battery <= 0.0f)
        {
            GD.Print("Lanterna sem bateria.");
            return;
        }

        _isOn = !_isOn;
        Visible = _isOn;
        GD.Print(_isOn ? "Lanterna ligada." : "Lanterna desligada.");
    }
}
