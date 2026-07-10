namespace BREU.Scripts.Player;

public partial class PlayerHealth : Node
{
    [Export] public int MaxHealth { get; set; } = 100;

    public int CurrentHealth { get; private set; }

    public override void _Ready()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        GD.Print($"Player tomou dano: {amount}. Vida: {CurrentHealth}/{MaxHealth}");
        ShowHudMessage("Voce foi atingido.");

        if (CurrentHealth <= 0)
        {
            GD.Print("Player morreu. TODO: implementar tela de morte.");
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
        GD.Print($"Player curado: {amount}. Vida: {CurrentHealth}/{MaxHealth}");
    }

    private void ShowHudMessage(string message)
    {
        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.ShowTemporaryMessage(message, 2.5f);
        }
    }
}
