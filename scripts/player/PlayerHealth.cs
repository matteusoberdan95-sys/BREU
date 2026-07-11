namespace BREU.Scripts.Player;

/// <summary>
/// Player health pool for HUD display and future combat (Sprint 14+).
/// </summary>
public partial class PlayerHealth : Node
{
    [Signal] public delegate void HealthChangedEventHandler(int current, int maximum);

    [Export] public int MaxHealth { get; set; } = 100;

    public int Current { get; private set; }
    public bool IsDead { get; private set; }

    public override void _Ready()
    {
        ResetHealth();
    }

    public void ResetHealth()
    {
        IsDead = false;
        Current = MaxHealth;
        EmitSignal(SignalName.HealthChanged, Current, MaxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (IsDead || amount <= 0)
        {
            return;
        }

        Current = Math.Max(0, Current - amount);
        if (Current <= 0)
        {
            IsDead = true;
        }

        EmitSignal(SignalName.HealthChanged, Current, MaxHealth);
    }

    public void Heal(int amount)
    {
        if (IsDead || amount <= 0)
        {
            return;
        }

        Current = Math.Min(MaxHealth, Current + amount);
        EmitSignal(SignalName.HealthChanged, Current, MaxHealth);
    }
}
