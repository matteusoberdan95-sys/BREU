namespace BREU.Scripts.Ui;

/// <summary>
/// Minimal HUD: health, stamina, flashlight battery, temporary messages.
/// </summary>
public partial class HUDController : CanvasLayer
{
    [Export] public NodePath HealthLabelPath { get; set; } = new("Root/StatusPanel/Margin/VBox/HealthLabel");
    [Export] public NodePath StaminaBarPath { get; set; } = new("Root/StatusPanel/Margin/VBox/StaminaBar");
    [Export] public NodePath StaminaLabelPath { get; set; } = new("Root/StatusPanel/Margin/VBox/StaminaLabel");
    [Export] public NodePath LanternLabelPath { get; set; } = new("Root/StatusPanel/Margin/VBox/LanternLabel");
    [Export] public NodePath MessageLabelPath { get; set; } = new("Root/MessagePanel/MessageLabel");
    [Export] public NodePath MessagePanelPath { get; set; } = new("Root/MessagePanel");
    [Export] public NodePath InteractionPromptLabelPath { get; set; } = new("Root/InteractionPromptPanel/InteractionPromptLabel");
    [Export] public NodePath DebugLabelPath { get; set; } = new("Root/DebugPanel/DebugLabel");

    [Export] public float DefaultMessageSeconds { get; set; } = 3.5f;

    private Label? _healthLabel;
    private ProgressBar? _staminaBar;
    private Label? _staminaLabel;
    private Label? _lanternLabel;
    private Label? _messageLabel;
    private PanelContainer? _messagePanel;
    private Label? _interactionPromptLabel;
    private Label? _debugLabel;

    private PlayerHealth? _health;
    private PlayerStamina? _stamina;
    private PlayerFlashlight? _flashlight;
    private PlaytestDebugSettings? _debugSettings;

    private float _messageTimer;

    public override void _Ready()
    {
        AddToGroup("game_hud");
        Layer = 0;

        _healthLabel = GetNodeOrNull<Label>(HealthLabelPath);
        _staminaBar = GetNodeOrNull<ProgressBar>(StaminaBarPath);
        _staminaLabel = GetNodeOrNull<Label>(StaminaLabelPath);
        _lanternLabel = GetNodeOrNull<Label>(LanternLabelPath);
        _messageLabel = GetNodeOrNull<Label>(MessageLabelPath);
        _messagePanel = GetNodeOrNull<PanelContainer>(MessagePanelPath);
        _interactionPromptLabel = GetNodeOrNull<Label>(InteractionPromptLabelPath);
        _debugLabel = GetNodeOrNull<Label>(DebugLabelPath);

        if (_messagePanel != null)
        {
            _messagePanel.Visible = false;
        }

        if (_messageLabel != null)
        {
            _messageLabel.Text = string.Empty;
            _messageLabel.Visible = false;
        }

        if (_interactionPromptLabel != null && _interactionPromptLabel.GetParent() is PanelContainer promptPanel)
        {
            promptPanel.Visible = false;
        }

        CallDeferred(nameof(BindPlayer));
    }

    public override void _Process(double delta)
    {
        UpdateDebugPanel();

        if (_messageTimer <= 0.0f || _messageLabel == null)
        {
            return;
        }

        _messageTimer -= (float)delta;
        if (_messageTimer <= 0.0f)
        {
            _messageLabel.Text = string.Empty;
            _messageLabel.Visible = false;
            if (_messagePanel != null)
            {
                _messagePanel.Visible = false;
            }
        }
    }

    public static HUDController? FindActive(SceneTree tree)
    {
        return tree.GetFirstNodeInGroup("game_hud") as HUDController;
    }

    public void ShowMessage(string text, float duration = -1f)
    {
        if (_messageLabel == null || string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        _messageLabel.Text = text;
        _messageLabel.Visible = true;
        if (_messagePanel != null)
        {
            _messagePanel.Visible = true;
        }

        _messageTimer = duration > 0.0f ? duration : DefaultMessageSeconds;
    }

    public void ShowInteractionPrompt(string promptText)
    {
        if (_interactionPromptLabel == null || string.IsNullOrWhiteSpace(promptText))
        {
            HideInteractionPrompt();
            return;
        }

        var trimmed = promptText.Trim();
        _interactionPromptLabel.Text = trimmed.StartsWith("[E]", StringComparison.OrdinalIgnoreCase)
            ? trimmed
            : $"[E] {trimmed}";

        if (_interactionPromptLabel.GetParent() is PanelContainer panel)
        {
            panel.Visible = true;
        }
    }

    public void HideInteractionPrompt() => ClearInteractionPrompt();

    public void ClearInteractionPrompt()
    {
        if (_interactionPromptLabel == null)
        {
            return;
        }

        _interactionPromptLabel.Text = string.Empty;
        if (_interactionPromptLabel.GetParent() is PanelContainer panel)
        {
            panel.Visible = false;
        }
    }

    private void BindPlayer()
    {
        var player = GetTree().GetFirstNodeInGroup("player") as Node;
        if (player == null)
        {
            GD.PushWarning("[HUD] Player group not found — retrying bind.");
            CallDeferred(nameof(BindPlayer));
            return;
        }

        _health = player.GetNodeOrNull<PlayerHealth>("PlayerHealth");
        _stamina = player.GetNodeOrNull<PlayerStamina>("PlayerStamina");
        _flashlight = player.GetNodeOrNull<PlayerFlashlight>(
            "HeadBase/BodyMotionPivot/LeanPivot/LookBackPivot/Camera3D/Flashlight");

        if (_health != null)
        {
            _health.HealthChanged += OnHealthChanged;
            OnHealthChanged(_health.Current, _health.MaxHealth);
        }

        if (_stamina != null)
        {
            _stamina.StaminaChanged += OnStaminaChanged;
            OnStaminaChanged(_stamina.Current, _stamina.MaxStamina);
        }

        if (_flashlight != null)
        {
            _flashlight.BatteryChanged += OnBatteryChanged;
            _flashlight.LanternToggled += OnLanternToggled;
            OnBatteryChanged(_flashlight.CurrentBattery, _flashlight.MaxBattery);
            OnLanternToggled(_flashlight.IsOn);
        }

        _debugSettings = PlaytestDebugSettings.Instance
            ?? GetTree().GetFirstNodeInGroup("playtest_debug_settings") as PlaytestDebugSettings;
        UpdateDebugPanel();
    }

    public override void _ExitTree()
    {
        if (_health != null)
        {
            _health.HealthChanged -= OnHealthChanged;
        }

        if (_stamina != null)
        {
            _stamina.StaminaChanged -= OnStaminaChanged;
        }

        if (_flashlight != null)
        {
            _flashlight.BatteryChanged -= OnBatteryChanged;
            _flashlight.LanternToggled -= OnLanternToggled;
        }
    }

    private void OnHealthChanged(int current, int maximum)
    {
        if (_healthLabel != null)
        {
            _healthLabel.Text = $"Vida {current}/{maximum}";
        }
    }

    private void OnStaminaChanged(float current, float maximum)
    {
        if (_staminaBar != null)
        {
            _staminaBar.MaxValue = maximum;
            _staminaBar.Value = current;
        }

        if (_staminaLabel != null)
        {
            _staminaLabel.Text = $"Stamina {Mathf.RoundToInt(current)}/{Mathf.RoundToInt(maximum)}";
        }
    }

    private void OnBatteryChanged(float current, float maximum)
    {
        UpdateLanternLabel(_flashlight?.IsOn ?? false, current, maximum);
    }

    private void OnLanternToggled(bool isOn)
    {
        if (_flashlight == null)
        {
            return;
        }

        UpdateLanternLabel(isOn, _flashlight.CurrentBattery, _flashlight.MaxBattery);
    }

    private void UpdateLanternLabel(bool isOn, float current, float maximum)
    {
        if (_lanternLabel == null)
        {
            return;
        }

        var state = isOn ? "Ligada" : "Desligada";
        var infinite = _debugSettings?.InfiniteLanternBattery ?? false;
        var suffix = infinite ? " (debug inf.)" : string.Empty;
        _lanternLabel.Text = $"Lanterna {Mathf.RoundToInt(current)}/{Mathf.RoundToInt(maximum)} — {state}{suffix}";
    }

    private void UpdateDebugPanel()
    {
        if (_debugLabel == null)
        {
            return;
        }

        _debugSettings ??= PlaytestDebugSettings.Instance
            ?? GetTree().GetFirstNodeInGroup("playtest_debug_settings") as PlaytestDebugSettings;

        if (_debugSettings == null)
        {
            _debugLabel.Text = "Debug: —";
            return;
        }

        var lantern = _debugSettings.InfiniteLanternBattery ? "inf ON" : "inf OFF";
        var fog = _debugSettings.ReducedFog ? "fog reduzida" : "fog normal";
        _debugLabel.Text = $"Debug F10: lanterna {lantern} | F11: {fog}";
    }
}
