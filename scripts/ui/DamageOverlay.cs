namespace BREU.Scripts.Ui;

public partial class DamageOverlay : CanvasLayer
{
    [Export] public NodePath DamageRectPath { get; set; } = "DamageRect";
    [Export] public float FadeOutTime { get; set; } = 0.35f;

    private ColorRect? _damageRect;
    private Tween? _fadeTween;

    public override void _Ready()
    {
        AddToGroup("damage_overlay");
        Layer = 90;
        _damageRect = GetNodeOrNull<ColorRect>(DamageRectPath);
        HideOverlay();
    }

    public void FlashDamage()
    {
        if (_damageRect == null)
        {
            return;
        }

        _fadeTween?.Kill();
        _damageRect.Visible = true;
        _damageRect.Modulate = Colors.White;

        _fadeTween = CreateTween();
        _fadeTween.TweenProperty(_damageRect, "modulate:a", 0.0f, FadeOutTime);
        _fadeTween.TweenCallback(Callable.From(HideOverlay));
    }

    private void HideOverlay()
    {
        if (_damageRect == null)
        {
            return;
        }

        _damageRect.Visible = false;
        _damageRect.Modulate = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }
}
