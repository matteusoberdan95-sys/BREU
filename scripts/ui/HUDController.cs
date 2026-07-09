namespace BREU.Scripts.Ui;

public partial class HUDController : CanvasLayer
{
    [Export] public NodePath InteractionPromptPath { get; set; } = "Root/InteractionPrompt";
    [Export] public NodePath StaminaLabelPath { get; set; } = "Root/StatusPanel/VBoxContainer/StaminaLabel";
    [Export] public NodePath FlashlightLabelPath { get; set; } = "Root/StatusPanel/VBoxContainer/FlashlightLabel";
    [Export] public NodePath WeaponLabelPath { get; set; } = "Root/StatusPanel/VBoxContainer/WeaponLabel";
    [Export] public NodePath MessagePanelPath { get; set; } = "Root/MessagePanel";
    [Export] public NodePath MessageLabelPath { get; set; } = "Root/MessagePanel/MessageLabel";
    [Export] public NodePath MessageTimerPath { get; set; } = "MessageTimer";

    private Label? _interactionPrompt;
    private Label? _staminaLabel;
    private Label? _flashlightLabel;
    private Label? _weaponLabel;
    private PanelContainer? _messagePanel;
    private Label? _messageLabel;
    private Godot.Timer? _messageTimer;

    public override void _Ready()
    {
        _interactionPrompt = GetNodeOrNull<Label>(InteractionPromptPath);
        _staminaLabel = GetNodeOrNull<Label>(StaminaLabelPath);
        _flashlightLabel = GetNodeOrNull<Label>(FlashlightLabelPath);
        _weaponLabel = GetNodeOrNull<Label>(WeaponLabelPath);
        _messagePanel = GetNodeOrNull<PanelContainer>(MessagePanelPath);
        _messageLabel = GetNodeOrNull<Label>(MessageLabelPath);
        _messageTimer = GetNodeOrNull<Godot.Timer>(MessageTimerPath);

        if (_messageTimer != null)
        {
            _messageTimer.Timeout += HideTemporaryMessage;
        }

        HideInteractionPrompt();
        SetWeapon("Maos vazias", 0, 0);
        CallDeferred(nameof(BindToPlayer));
    }

    public void BindToPlayer()
    {
        var player = GetTree().GetFirstNodeInGroup("player") as PlayerController;
        if (player == null)
        {
            return;
        }

        player.GetNodeOrNull<PlayerInteractor>("PlayerInteractor")?.Connect(
            PlayerInteractor.SignalName.FocusChanged,
            Callable.From<string>(OnInteractionFocusChanged));

        var stamina = player.GetNodeOrNull<PlayerStamina>("PlayerStamina");
        if (stamina != null)
        {
            stamina.Connect(
                PlayerStamina.SignalName.StaminaChanged,
                Callable.From<float, float>(SetStamina));
            SetStamina(stamina.Current, stamina.MaxStamina);
        }

        var flashlight = player.GetNodeOrNull<FlashlightController>("CameraPivot/Flashlight");
        if (flashlight != null)
        {
            flashlight.Connect(
                FlashlightController.SignalName.BatteryChanged,
                Callable.From<float, float>(SetFlashlight));
            SetFlashlight(flashlight.CurrentBattery, flashlight.MaxBattery);
        }

        player.GetNodeOrNull<WeaponController>("CameraPivot/Camera3D/WeaponHolder/WeaponController")?.Connect(
            WeaponController.SignalName.WeaponChanged,
            Callable.From<string, int, int>(SetWeapon));

        var inventory = player.GetNodeOrNull<PlayerInventory>("PlayerInventory");
        if (inventory != null)
        {
            inventory.InventoryChanged += () => SetInventoryWeapon(inventory);
            SetInventoryWeapon(inventory);
        }
    }

    public void SetStamina(float current, float max)
    {
        if (_staminaLabel != null)
        {
            _staminaLabel.Text = $"Stamina {current:0}/{max:0}";
        }
    }

    public void SetFlashlight(float current, float max)
    {
        if (_flashlightLabel != null)
        {
            _flashlightLabel.Text = $"Lanterna {current:0}/{max:0}";
        }
    }

    public void SetWeapon(string weaponName, int durability, int maxDurability)
    {
        if (_weaponLabel == null)
        {
            return;
        }

        _weaponLabel.Text = maxDurability <= 0
            ? $"Arma: {weaponName}"
            : $"Arma: {weaponName} {durability}/{maxDurability}";
    }

    public void ShowInteractionPrompt(string text)
    {
        if (_interactionPrompt == null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            HideInteractionPrompt();
            return;
        }

        _interactionPrompt.Text = text.StartsWith("[E]", StringComparison.Ordinal)
            ? text
            : $"[E] {text}";
        _interactionPrompt.Visible = true;
    }

    public void HideInteractionPrompt()
    {
        if (_interactionPrompt != null)
        {
            _interactionPrompt.Text = "";
            _interactionPrompt.Visible = false;
        }
    }

    public void ShowTemporaryMessage(string text, float duration = 3.0f)
    {
        if (_messageLabel == null || _messagePanel == null)
        {
            GD.Print($"HUD mensagem: {text}");
            return;
        }

        _messageLabel.Text = text;
        _messagePanel.Visible = true;

        if (_messageTimer != null)
        {
            _messageTimer.Stop();
            _messageTimer.WaitTime = Mathf.Max(0.5f, duration);
            _messageTimer.Start();
        }
    }

    public void HideTemporaryMessage()
    {
        if (_messagePanel != null)
        {
            _messagePanel.Visible = false;
        }

        if (_messageLabel != null)
        {
            _messageLabel.Text = "";
        }
    }

    private void OnInteractionFocusChanged(string prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            HideInteractionPrompt();
            return;
        }

        ShowInteractionPrompt(prompt);
    }

    private void SetInventoryWeapon(PlayerInventory inventory)
    {
        SetWeapon(
            inventory.HasHammer ? inventory.EquippedWeaponName : "Maos vazias",
            inventory.HasHammer ? inventory.EquippedWeaponDurability : 0,
            inventory.HasHammer ? 10 : 0);
    }
}
