namespace BREU.Scripts.Levels;

/// <summary>
/// Fade in/out simples e troca de cena. Autoload global.
/// </summary>
public partial class SceneTransitionController : CanvasLayer
{
    public static SceneTransitionController? Instance { get; private set; }

    [Export] public float FadeDuration { get; set; } = 0.75f;
    [Export] public Color FadeColor { get; set; } = Colors.Black;

    private ColorRect? _fadeRect;
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
        if (_isBusy || string.IsNullOrWhiteSpace(scenePath))
        {
            return;
        }

        _ = ChangeSceneAsync(scenePath);
    }

    public async void FadeIn()
    {
        await FadeTo(0.0f);
    }

    public async void FadeOut()
    {
        await FadeTo(1.0f);
    }

    private async System.Threading.Tasks.Task ChangeSceneAsync(string scenePath)
    {
        _isBusy = true;

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
}
