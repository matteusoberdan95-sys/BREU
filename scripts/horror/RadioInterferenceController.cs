namespace BREU.Scripts.Horror;

/// <summary>
/// Radio/interferencia atmosferica. Streams opcionais via AudioPaths.
/// </summary>
public partial class RadioInterferenceController : Node3D
{
    [Export] public AudioStream? StaticLoop { get; set; }
    [Export] public AudioStream? WhisperSound { get; set; }
    [Export] public bool StartsActive { get; set; }
    [Export] public float VolumeDb { get; set; } = -14.0f;
    [Export] public NodePath RadioAudioPath { get; set; } = "RadioAudio";

    private AudioStreamPlayer3D? _radioAudio;
    private SceneTreeTimer? _pulseTimer;

    public override void _Ready()
    {
        _radioAudio = GetNodeOrNull<AudioStreamPlayer3D>(RadioAudioPath);
        if (_radioAudio != null)
        {
            _radioAudio.VolumeDb = VolumeDb;
        }

        StaticLoop ??= AudioResourceLoader.TryLoad(AudioPaths.RadioStatic, loop: true);
        WhisperSound ??= AudioResourceLoader.TryLoad(AudioPaths.RadioWhisper);

        if (StartsActive)
        {
            StartStatic();
        }
    }

    public void StartStatic()
    {
        if (_radioAudio == null)
        {
            GD.Print("Radio: player de audio nao configurado.");
            NotifyHudIfNeeded();
            return;
        }

        if (StaticLoop == null)
        {
            GD.Print("Radio: interferencia iniciada.");
            NotifyHudIfNeeded();
            return;
        }

        _radioAudio.Stream = StaticLoop;
        _radioAudio.VolumeDb = VolumeDb;
        _radioAudio.Play();
        GD.Print("Radio: interferencia iniciada.");
    }

    public void StopStatic()
    {
        _pulseTimer = null;
        _radioAudio?.Stop();
        GD.Print("Radio: interferencia parada.");
    }

    public void PlayWhisper()
    {
        if (WhisperSound == null || _radioAudio == null)
        {
            GD.Print("Radio: sussurro disparado.");
            NotifyHudIfNeeded();
            return;
        }

        _radioAudio.Stream = WhisperSound;
        _radioAudio.VolumeDb = VolumeDb;
        _radioAudio.Play();
        GD.Print("Radio: sussurro disparado.");
    }

    public void PulseInterference(float duration)
    {
        StartStatic();
        NotifyHudIfNeeded();

        if (StaticLoop != null)
        {
            PlayWhisper();
        }

        if (duration <= 0.0f)
        {
            return;
        }

        _pulseTimer = GetTree().CreateTimer(duration);
        _pulseTimer.Timeout += () => StopStatic();
    }

    private void NotifyHudIfNeeded()
    {
        if (StaticLoop != null)
        {
            return;
        }

        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.ShowTemporaryMessage("O radio chia em algum lugar...");
        }
    }
}
