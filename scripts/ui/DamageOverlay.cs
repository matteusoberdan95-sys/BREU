namespace BREU.Scripts.Ui;

public partial class DamageOverlay : CanvasLayer
{
    [Export] public NodePath DamageRectPath { get; set; } = "DamageRect";
    [Export] public float FlashDuration { get; set; } = 0.35f;
    [Export] public float MaxAlpha { get; set; } = 0.35f;
    [Export] public float MessageCooldown { get; set; } = 1.5f;
    [Export] public float CameraShakeStrength { get; set; } = 0.04f;
    [Export] public float CameraShakeDuration { get; set; } = 0.12f;
    [Export] public string HurtAudioPath { get; set; } = "res://assets/audio/sfx/player/player_hurt_01.ogg";
    [Export] public string HurtFallbackAudioPath { get; set; } = "res://assets/audio/sfx/horror/corridor_hit_01.ogg";
    [Export] public string DamageMessage { get; set; } = "Voce foi atingido.";

    private ColorRect? _damageRect;
    private AudioStreamPlayer? _hurtPlayer;
    private Tween? _fadeTween;
    private float _messageCooldownRemaining;

    public override void _Ready()
    {
        AddToGroup("damage_overlay");
        Layer = 90;
        _damageRect = GetNodeOrNull<ColorRect>(DamageRectPath);
        _hurtPlayer = GetNodeOrNull<AudioStreamPlayer>("HurtAudio");
        if (_hurtPlayer == null)
        {
            _hurtPlayer = new AudioStreamPlayer { Name = "HurtAudio", VolumeDb = -8.0f };
            AddChild(_hurtPlayer);
        }

        ConfigureHurtAudio();
        HideOverlay();
    }

    public override void _Process(double delta)
    {
        _messageCooldownRemaining = Mathf.Max(0.0f, _messageCooldownRemaining - (float)delta);
    }

    public void FlashDamage()
    {
        if (_damageRect == null)
        {
            return;
        }

        _fadeTween?.Kill();
        _damageRect.Visible = true;
        _damageRect.Modulate = new Color(1.0f, 1.0f, 1.0f, MaxAlpha);

        _fadeTween = CreateTween();
        _fadeTween.TweenProperty(_damageRect, "modulate:a", 0.0f, FlashDuration);
        _fadeTween.TweenCallback(Callable.From(HideOverlay));

        PlayHurtAudio();
        ShakePlayerCamera();
        ShowDamageMessage();
    }

    private void ConfigureHurtAudio()
    {
        if (_hurtPlayer == null)
        {
            return;
        }

        _hurtPlayer.Stream = AudioResourceLoader.TryLoad(HurtAudioPath)
            ?? AudioResourceLoader.TryLoad(HurtFallbackAudioPath);
    }

    private void PlayHurtAudio()
    {
        if (_hurtPlayer?.Stream != null)
        {
            _hurtPlayer.Play();
        }
    }

    private void ShakePlayerCamera()
    {
        if (GetTree().GetFirstNodeInGroup("player") is PlayerController player)
        {
            player.ApplyCameraShake(CameraShakeStrength, CameraShakeDuration);
        }
    }

    private void ShowDamageMessage()
    {
        if (_messageCooldownRemaining > 0.0f)
        {
            return;
        }

        _messageCooldownRemaining = MessageCooldown;

        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.ShowTemporaryMessage(DamageMessage, 2.0f);
        }
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
