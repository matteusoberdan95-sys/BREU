namespace BREU.Scripts.Ui;

public partial class DeathScreen : CanvasLayer
{
    [Export] public NodePath RootPath { get; set; } = "Root";
    [Export] public NodePath BackgroundPath { get; set; } = "Root/Background";
    [Export] public NodePath RetryButtonPath { get; set; } = "Root/CenterContainer/VBoxContainer/RetryButton";
    [Export] public NodePath QuitButtonPath { get; set; } = "Root/CenterContainer/VBoxContainer/QuitButton";
    [Export] public float FadeInDuration { get; set; } = 0.85f;
    [Export] public string DeathStingerPath { get; set; } = "res://assets/audio/sfx/horror/death_stinger_01.ogg";
    [Export] public string DeathStingerFallbackPath { get; set; } = "res://assets/audio/sfx/horror/scare_stinger_01.ogg";

    private Control? _root;
    private ColorRect? _background;
    private Button? _retryButton;
    private Button? _quitButton;
    private AudioStreamPlayer? _deathAudio;
    private Tween? _fadeTween;
    private bool _deathAudioMissingLogged;

    public override void _Ready()
    {
        AddToGroup("death_screen");
        Layer = 120;
        ProcessMode = ProcessModeEnum.Always;

        _root = GetNodeOrNull<Control>(RootPath);
        _background = GetNodeOrNull<ColorRect>(BackgroundPath);
        _retryButton = GetNodeOrNull<Button>(RetryButtonPath);
        _quitButton = GetNodeOrNull<Button>(QuitButtonPath);
        _deathAudio = GetNodeOrNull<AudioStreamPlayer>("DeathAudio");
        if (_deathAudio == null)
        {
            _deathAudio = new AudioStreamPlayer { Name = "DeathAudio", VolumeDb = -6.0f };
            AddChild(_deathAudio);
        }

        ConfigureDeathAudio();

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

        PlayDeathAudio();
        FadeInBackground();
        Input.MouseMode = Input.MouseModeEnum.Visible;
        _retryButton?.GrabFocus();
    }

    public void HideDeathScreen()
    {
        RemoveFromGroup("death_screen_active");
        _fadeTween?.Kill();

        if (_root != null)
        {
            _root.Visible = false;
        }

        if (_background != null)
        {
            _background.Modulate = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }

    private void FadeInBackground()
    {
        if (_background == null)
        {
            return;
        }

        _fadeTween?.Kill();
        _background.Modulate = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        _fadeTween = CreateTween();
        _fadeTween.SetPauseMode(Tween.TweenPauseMode.Process);
        _fadeTween.TweenProperty(_background, "modulate:a", 1.0f, FadeInDuration);
    }

    private void ConfigureDeathAudio()
    {
        if (_deathAudio == null)
        {
            return;
        }

        _deathAudio.Stream = AudioResourceLoader.TryLoad(DeathStingerPath)
            ?? AudioResourceLoader.TryLoad(DeathStingerFallbackPath);

        if (_deathAudio.Stream == null && !_deathAudioMissingLogged)
        {
            _deathAudioMissingLogged = true;
            GD.Print("DeathScreen: nenhum stinger de morte encontrado.");
        }
    }

    private void PlayDeathAudio()
    {
        if (_deathAudio?.Stream != null)
        {
            _deathAudio.Play();
        }
    }

    private void OnRetryPressed()
    {
        HideDeathScreen();
        CheckpointManager.Instance?.RespawnFromLastCheckpoint();
    }
}
