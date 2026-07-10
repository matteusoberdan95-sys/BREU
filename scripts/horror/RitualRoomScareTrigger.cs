namespace BREU.Scripts.Horror;

/// <summary>
/// Primeiro susto ritualistico da Sala dos Santos Secos.
/// </summary>
public partial class RitualRoomScareTrigger : Area3D
{
    [Export] public NodePath CandleLightMainPath { get; set; } = "../../Lighting/CandleLightMain";
    [Export] public NodePath BackAltarLightPath { get; set; } = "../../Lighting/BackAltarLight";
    [Export] public NodePath EnemyPath { get; set; } = "../../Enemies/EnemyPlaceholder";
    [Export] public NodePath RadioStaticPath { get; set; } = "../../Audio/RadioStaticPoint";
    [Export] public string ScareSoundPath { get; set; } = "res://assets/audio/sfx/horror/scare_stinger_01.ogg";

    private bool _triggered;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (_triggered || !body.IsInGroup("player"))
        {
            return;
        }

        _triggered = true;
        _ = TriggerScareAsync();
    }

    private async System.Threading.Tasks.Task TriggerScareAsync()
    {
        ShowHudMessage("As velas tremem sem vento.");
        PlayScareSound();
        StartRadioStatic();
        ActivateEnemy();

        GD.Print("RitualRoomScareTrigger ativado.");

        await FlickerLights(2.0f);
        StopRadioStatic();
    }

    private async System.Threading.Tasks.Task FlickerLights(float duration)
    {
        var candle = GetNodeOrNull<OmniLight3D>(CandleLightMainPath);
        var altar = GetNodeOrNull<OmniLight3D>(BackAltarLightPath);
        var candleBase = candle?.LightEnergy ?? 0.0f;
        var altarBase = altar?.LightEnergy ?? 0.0f;
        var elapsed = 0.0f;
        var random = new RandomNumberGenerator();
        random.Randomize();

        while (elapsed < duration)
        {
            if (candle != null)
            {
                candle.LightEnergy = random.RandfRange(0.25f, 2.7f);
            }

            if (altar != null)
            {
                altar.LightEnergy = random.RandfRange(0.15f, 1.5f);
            }

            await ToSignal(GetTree().CreateTimer(0.11f), SceneTreeTimer.SignalName.Timeout);
            elapsed += 0.11f;
        }

        if (candle != null)
        {
            candle.LightEnergy = candleBase;
        }

        if (altar != null)
        {
            altar.LightEnergy = altarBase;
        }
    }

    private void ActivateEnemy()
    {
        if (GetNodeOrNull<EnemyPlaceholderAI>(EnemyPath) is not { } enemy)
        {
            GD.Print("RitualRoomScareTrigger: EnemyPlaceholder nao encontrado.");
            return;
        }

        enemy.Visible = true;
        enemy.CanChase = true;
        enemy.ActivateEnemy();
        enemy.LookAtPlayer();
    }

    private void PlayScareSound()
    {
        var stream = AudioResourceLoader.TryLoad(ScareSoundPath);
        if (stream == null)
        {
            GD.Print($"RitualRoomScareTrigger: audio nao encontrado: {ScareSoundPath}");
            return;
        }

        if (AudioManager.Find(this) is { } manager)
        {
            manager.Play2DSound(stream);
            return;
        }

        var player = new AudioStreamPlayer { Stream = stream };
        AddChild(player);
        player.Finished += () => player.QueueFree();
        player.Play();
    }

    private void StartRadioStatic()
    {
        if (GetNodeOrNull<AudioStreamPlayer3D>(RadioStaticPath) is { } radio)
        {
            radio.Play();
        }
    }

    private void StopRadioStatic()
    {
        if (GetNodeOrNull<AudioStreamPlayer3D>(RadioStaticPath) is { } radio)
        {
            radio.Stop();
        }
    }

    private void ShowHudMessage(string message)
    {
        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.ShowTemporaryMessage(message, 3.5f);
            return;
        }

        GD.Print($"HUD mensagem: {message}");
    }
}
