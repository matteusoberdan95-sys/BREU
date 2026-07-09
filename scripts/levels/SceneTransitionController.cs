namespace BREU.Scripts.Levels;

/// <summary>
/// Fade in/out global com mensagem opcional para costurar trocas de cena.
/// </summary>
public partial class SceneTransitionController : CanvasLayer
{
    public static SceneTransitionController? Instance { get; private set; }

    [Export] public float FadeDuration { get; set; } = 0.8f;
    [Export] public Color FadeColor { get; set; } = Colors.Black;
    [Export] public float MessageDuration { get; set; } = 2.0f;

    private ColorRect? _fadeRect;
    private Label? _messageLabel;
    private Godot.Timer? _messageTimer;
    private bool _isBusy;

    public override void _Ready()
    {
        Instance = this;
        Layer = 128;
        ProcessMode = ProcessModeEnum.Always;

        _fadeRect = new ColorRect
        {
            Name = "FadeRect",
            Color = FadeColor,
            MouseFilter = Control.MouseFilterEnum.Ignore
        };
        _fadeRect.SetAnchorsPreset(Control.LayoutPreset.FullRect);
        _fadeRect.Modulate = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        AddChild(_fadeRect);

        _messageLabel = new Label
        {
            Name = "MessageLabel",
            Text = "",
            Visible = false,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            MouseFilter = Control.MouseFilterEnum.Ignore
        };
        _messageLabel.SetAnchorsPreset(Control.LayoutPreset.FullRect);
        AddChild(_messageLabel);

        _messageTimer = new Godot.Timer
        {
            Name = "MessageTimer",
            OneShot = true,
            ProcessCallback = Godot.Timer.TimerProcessCallback.Idle
        };
        _messageTimer.Timeout += HideMessage;
        AddChild(_messageTimer);
    }

    public override void _ExitTree()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void ChangeScene(string scenePath)
    {
        ChangeSceneWithFade(scenePath);
    }

    public void ChangeSceneWithFade(string scenePath, string message = "")
    {
        if (_isBusy || string.IsNullOrWhiteSpace(scenePath))
        {
            return;
        }

        if (!ResourceLoader.Exists(scenePath))
        {
            GD.PrintErr($"SceneTransition: cena nao encontrada: {scenePath}");
            return;
        }

        _ = ChangeSceneAsync(scenePath, message);
    }

    public void ShowMessage(string message, float duration = 2.0f)
    {
        if (_messageLabel == null || string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        _messageLabel.Text = message;
        _messageLabel.Visible = true;

        if (_messageTimer != null)
        {
            _messageTimer.Stop();
            _messageTimer.WaitTime = Mathf.Max(0.2f, duration);
            _messageTimer.Start();
        }
    }

    public async void FadeIn()
    {
        await FadeTo(0.0f);
    }

    public async void FadeOut()
    {
        await FadeTo(1.0f);
    }

    private async System.Threading.Tasks.Task ChangeSceneAsync(string scenePath, string message)
    {
        _isBusy = true;
        ShowMessage(message, MessageDuration);

        await FadeTo(1.0f);

        var error = GetTree().ChangeSceneToFile(scenePath);
        if (error != Error.Ok)
        {
            GD.PrintErr($"SceneTransition: falha ao carregar {scenePath} ({error}).");
            await FadeTo(0.0f);
            _isBusy = false;
            return;
        }

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        await FadeTo(0.0f);
        HideMessage();
        _isBusy = false;
    }

    private async System.Threading.Tasks.Task FadeTo(float targetAlpha)
    {
        if (_fadeRect == null)
        {
            return;
        }

        var tween = CreateTween();
        tween.SetPauseMode(Tween.TweenPauseMode.Process);
        tween.TweenProperty(_fadeRect, "modulate:a", targetAlpha, FadeDuration);
        await ToSignal(tween, Tween.SignalName.Finished);
    }

    private void HideMessage()
    {
        if (_messageLabel == null)
        {
            return;
        }

        _messageLabel.Text = "";
        _messageLabel.Visible = false;
    }
}
