namespace BREU.Scripts.Player;

public partial class PlayerStamina : Node
{
    [Signal] public delegate void StaminaChangedEventHandler(float current, float maximum);

    [Export] public float MaxStamina { get; set; } = 100.0f;
    [Export] public float RegenPerSecond { get; set; } = 22.0f;
    [Export] public float RegenDelaySeconds { get; set; } = 0.65f;

    public float Current { get; private set; }

    private double _regenDelayRemaining;

    public override void _Ready()
    {
        Current = MaxStamina;
        EmitSignal(SignalName.StaminaChanged, Current, MaxStamina);
    }

    public override void _Process(double delta)
    {
        if (_regenDelayRemaining > 0.0)
        {
            _regenDelayRemaining -= delta;
            return;
        }

        if (Current >= MaxStamina)
        {
            return;
        }

        Current = Mathf.Min(MaxStamina, Current + RegenPerSecond * (float)delta);
        EmitSignal(SignalName.StaminaChanged, Current, MaxStamina);
    }

    public bool HasStamina(float amount) => Current >= amount;

    public bool Consume(float amount)
    {
        if (!HasStamina(amount))
        {
            return false;
        }

        Current = Mathf.Max(0.0f, Current - amount);
        _regenDelayRemaining = RegenDelaySeconds;
        EmitSignal(SignalName.StaminaChanged, Current, MaxStamina);
        return true;
    }

    public void DrainPerSecond(float perSecond, double delta)
    {
        if (perSecond <= 0.0f || Current <= 0.0f)
        {
            return;
        }

        Current = Mathf.Max(0.0f, Current - perSecond * (float)delta);
        _regenDelayRemaining = RegenDelaySeconds;
        EmitSignal(SignalName.StaminaChanged, Current, MaxStamina);
    }
}
