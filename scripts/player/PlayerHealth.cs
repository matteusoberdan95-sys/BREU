namespace BREU.Scripts.Player;

public partial class PlayerHealth : Node
{
    [Signal] public delegate void HealthChangedEventHandler(int current, int maximum);

    [Export] public int MaxHealth { get; set; } = 100;
    [Export] public float DamageInvulnerabilityTime { get; set; } = 0.7f;

    public int CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    private float _invulnerabilityTimer;

    public override void _Ready()
    {
        ResetHealth();
    }

    public override void _Process(double delta)
    {
        _invulnerabilityTimer = Mathf.Max(0.0f, _invulnerabilityTimer - (float)delta);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || IsDead || _invulnerabilityTimer > 0.0f)
        {
            return;
        }

        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        _invulnerabilityTimer = DamageInvulnerabilityTime;
        GD.Print($"Player tomou dano: {amount}. Vida: {CurrentHealth}/{MaxHealth}");
        FlashDamageOverlay();
        PlayDamageCameraShake();
        UpdateHud();

        if (CurrentHealth <= 0)
        {
            Kill();
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || IsDead)
        {
            return;
        }

        CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
        GD.Print($"Player curado: {amount}. Vida: {CurrentHealth}/{MaxHealth}");
        UpdateHud();
    }

    public void Kill()
    {
        if (IsDead)
        {
            return;
        }

        IsDead = true;
        CurrentHealth = 0;
        DisablePlayerInput();
        UpdateHud();
        ShowDeathScreen();
        GD.Print("Player morreu.");
    }

    public void ResetHealth()
    {
        CurrentHealth = MaxHealth;
        IsDead = false;
        _invulnerabilityTimer = 0.0f;
        EnablePlayerInput();
        UpdateHud();
    }

    private void UpdateHud()
    {
        EmitSignal(SignalName.HealthChanged, CurrentHealth, MaxHealth);

        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.SetHealth(CurrentHealth, MaxHealth);
        }
    }

    private void FlashDamageOverlay()
    {
        if (GetTree().GetFirstNodeInGroup("damage_overlay") is DamageOverlay overlay)
        {
            overlay.FlashDamage();
        }
    }

    private void PlayDamageCameraShake()
    {
        if (GetParent()?.GetNodeOrNull<PlayerBodyMotion>("PlayerBodyMotion") is { } bodyMotion)
        {
            bodyMotion.PlayDamageShake();
            return;
        }

        GetParent()?.GetNodeOrNull<PlayerCameraFeel>("PlayerCameraFeel")?.PlayDamageShake();
    }

    private void ShowDeathScreen()
    {
        if (GetTree().GetFirstNodeInGroup("death_screen") is DeathScreen deathScreen)
        {
            deathScreen.ShowDeathScreen();
        }
    }

    private void DisablePlayerInput()
    {
        if (GetParent() is PlayerController controller)
        {
            controller.MovementEnabled = false;
            controller.Velocity = Vector3.Zero;
        }

        SetNodeProcess("PlayerInteractor", false);
        SetNodeProcess("PlayerMeleeAttack", false);
        SetNodeProcess("PlayerBodyMotion", false);
        SetNodeProcess("PlayerCameraFeel", false);
        SetNodeProcess("CameraPivot", false);
        SetNodeProcess("CameraPivot/Flashlight", false);
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    private void EnablePlayerInput()
    {
        if (GetParent() is PlayerController controller)
        {
            controller.MovementEnabled = true;
        }

        SetNodeProcess("PlayerInteractor", true);
        SetNodeProcess("PlayerMeleeAttack", true);
        SetNodeProcess("PlayerBodyMotion", true);
        SetNodeProcess("PlayerCameraFeel", true);
        SetNodeProcess("CameraPivot", true);
        SetNodeProcess("CameraPivot/Flashlight", true);
    }

    private void SetNodeProcess(NodePath path, bool enabled)
    {
        if (GetParent()?.GetNodeOrNull<Node>(path) is not { } node)
        {
            return;
        }

        node.SetProcess(enabled);
        node.SetPhysicsProcess(enabled);
        node.SetProcessInput(enabled);
        node.SetProcessUnhandledInput(enabled);
    }
}
