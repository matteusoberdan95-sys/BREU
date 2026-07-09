namespace BREU.Scripts.Horror;

/// <summary>
/// Primeiro susto do corredor — dispara uma vez ao entrar na area.
/// TODO: res://assets/audio/sfx/horror/scare_stinger_01.ogg
/// </summary>
public partial class CorridorScareTrigger : Area3D
{
    [Export] public NodePath CorridorLightPath { get; set; } = "../../CorridorLight_01";
    [Export] public NodePath RadioControllerPath { get; set; } = "../../../Horror/RadioInterference";
    [Export] public NodePath PlaceholderEnemyPath { get; set; } = "../../EnemyPlaceholder";
    [Export] public NodePath HudPath { get; set; } = "../../../UI/HUD";
    [Export] public NodePath SequenceControllerPath { get; set; } = "../../DemoRoomSequenceController";
    [Export] public AudioStream? ScareSound { get; set; }
    [Export] public float FlickerDuration { get; set; } = 2.0f;
    [Export] public bool HideEnemyAfterScare { get; set; } = true;

    private bool _triggered;
    private AudioStreamPlayer3D? _scarePlayer;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        _scarePlayer = GetNodeOrNull<AudioStreamPlayer3D>("ScareAudio");
    }

    private void OnBodyEntered(Node3D body)
    {
        if (_triggered || !body.IsInGroup("player"))
        {
            return;
        }

        _triggered = true;
        TriggerScare();
    }

    private async void TriggerScare()
    {
        GD.Print("CorridorScare: primeiro susto disparado.");

        var hud = ResolveHud();
        hud?.ShowTemporaryMessage("Voce ouviu isso?");

        RunFlickerCorridorLight();
        PulseRadio();
        PlayScareSound();
        PlayCorridorHitSound();
        ActivateEnemy();

        if (GetNodeOrNull(SequenceControllerPath) is DemoRoomSequenceController sequence)
        {
            sequence.OnCorridorScareTriggered();
        }
        else if (GetTree().GetFirstNodeInGroup("demo_sequence") is DemoRoomSequenceController fallbackSequence)
        {
            fallbackSequence.OnCorridorScareTriggered();
        }

        if (HideEnemyAfterScare)
        {
            await ToSignal(GetTree().CreateTimer(1.75), SceneTreeTimer.SignalName.Timeout);
            DeactivateEnemy();
        }
    }

    private HUDController? ResolveHud()
    {
        if (GetNodeOrNull(HudPath) is HUDController hudFromPath)
        {
            return hudFromPath;
        }

        return GetTree().GetFirstNodeInGroup("hud") as HUDController;
    }

    private async void RunFlickerCorridorLight()
    {
        var light = GetNodeOrNull<OmniLight3D>(CorridorLightPath);
        if (light == null)
        {
            GD.Print("CorridorScare: luz do corredor nao encontrada.");
            return;
        }

        var originalEnergy = light.LightEnergy;
        var elapsed = 0.0f;
        var toggle = false;

        while (elapsed < FlickerDuration)
        {
            light.LightEnergy = toggle ? 0.8f : 0.0f;
            toggle = !toggle;
            await ToSignal(GetTree().CreateTimer(0.18), SceneTreeTimer.SignalName.Timeout);
            elapsed += 0.18f;
        }

        light.LightEnergy = 0.25f;
        GD.Print($"CorridorScare: flicker concluido. Energia final: {light.LightEnergy:0.00}");
    }

    private void PulseRadio()
    {
        if (GetNodeOrNull(RadioControllerPath) is RadioInterferenceController radio)
        {
            radio.PulseInterference(3.5f);
            return;
        }

        GD.Print("CorridorScare: RadioInterference nao encontrado.");
    }

    private void PlayScareSound()
    {
        var stream = ScareSound ?? AudioResourceLoader.TryLoad(AudioPaths.HorrorScareStinger);
        if (stream == null)
        {
            GD.Print("CorridorScare: som de susto nao configurado.");
            return;
        }

        if (AudioManager.Find(this) is { } manager)
        {
            manager.Play3DSound(stream, GlobalPosition);
            return;
        }

        if (_scarePlayer == null)
        {
            _scarePlayer = new AudioStreamPlayer3D
            {
                Name = "ScareAudioRuntime",
                VolumeDb = -8.0f
            };
            AddChild(_scarePlayer);
        }

        _scarePlayer.Stream = stream;
        _scarePlayer.Play();
    }

    private void PlayCorridorHitSound()
    {
        var stream = AudioResourceLoader.TryLoad(AudioPaths.HorrorCorridorHit);
        if (stream == null)
        {
            return;
        }

        AudioManager.Find(this)?.Play3DSound(stream, GlobalPosition);
    }

    private void ActivateEnemy()
    {
        if (GetNodeOrNull(PlaceholderEnemyPath) is EnemyPlaceholder enemy)
        {
            enemy.Activate();
            return;
        }

        GD.Print("CorridorScare: EnemyPlaceholder nao encontrado.");
    }

    private void DeactivateEnemy()
    {
        if (GetNodeOrNull(PlaceholderEnemyPath) is EnemyPlaceholder enemy)
        {
            enemy.Deactivate();
        }
    }
}
