using Godot;

namespace BREU.Scripts.Enemies;

public partial class EnemyHealth : Node
{
    [Signal] public delegate void DamagedEventHandler(float currentHealth, float damage);
    [Signal] public delegate void DiedEventHandler();

    [Export] public float MaxHealth { get; set; } = 60.0f;

    public float CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0.0f;

    public override void _Ready()
    {
        CurrentHealth = MaxHealth;
    }

    public void ApplyDamage(float amount)
    {
        if (IsDead)
        {
            return;
        }

        CurrentHealth = Mathf.Max(0.0f, CurrentHealth - amount);
        EmitSignal(SignalName.Damaged, CurrentHealth, amount);

        if (IsDead)
        {
            GD.Print($"{GetParent()?.Name} morreu.");
            EmitSignal(SignalName.Died);
        }
        else
        {
            GD.Print($"{GetParent()?.Name} recebeu {amount} de dano.");
        }
    }
}
