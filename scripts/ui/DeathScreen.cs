namespace BREU.Scripts.Ui;

public partial class DeathScreen : CanvasLayer
{
    [Export] public NodePath RootPath { get; set; } = "Root";
    [Export] public NodePath RetryButtonPath { get; set; } = "Root/CenterContainer/VBoxContainer/RetryButton";
    [Export] public NodePath QuitButtonPath { get; set; } = "Root/CenterContainer/VBoxContainer/QuitButton";

    private Control? _root;
    private Button? _retryButton;
    private Button? _quitButton;

    public override void _Ready()
    {
        AddToGroup("death_screen");
        Layer = 120;
        ProcessMode = ProcessModeEnum.Always;

        _root = GetNodeOrNull<Control>(RootPath);
        _retryButton = GetNodeOrNull<Button>(RetryButtonPath);
        _quitButton = GetNodeOrNull<Button>(QuitButtonPath);

        if (_retryButton != null)
        {
            _retryButton.Pressed += OnRetryPressed;
        }

        if (_quitButton != null)
        {
            _quitButton.Visible = false;
        }

        HideDeathScreen();
    }

    public void ShowDeathScreen()
    {
        AddToGroup("death_screen_active");

        if (_root != null)
        {
            _root.Visible = true;
        }

        Input.MouseMode = Input.MouseModeEnum.Visible;
        _retryButton?.GrabFocus();
    }

    public void HideDeathScreen()
    {
        RemoveFromGroup("death_screen_active");

        if (_root != null)
        {
            _root.Visible = false;
        }
    }

    private void OnRetryPressed()
    {
        HideDeathScreen();
        CheckpointManager.Instance?.RespawnFromLastCheckpoint();
    }
}
