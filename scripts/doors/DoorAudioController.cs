namespace BREU.Scripts.Doors;

/// <summary>
/// Audio 3D da porta. Carrega .ogg de AudioPaths se exports vazios.
/// </summary>
public partial class DoorAudioController : AudioStreamPlayer3D
{
    [Export] public AudioStream? OpenSound { get; set; }
    [Export] public AudioStream? CloseSound { get; set; }
    [Export] public AudioStream? LockedSound { get; set; }
    [Export] public float DoorVolumeDb { get; set; } = -6.0f;

    public override void _Ready()
    {
        VolumeDb = DoorVolumeDb;
        OpenSound ??= AudioResourceLoader.TryLoad(AudioPaths.DoorsOpen);
        CloseSound ??= AudioResourceLoader.TryLoad(AudioPaths.DoorsClose);
        LockedSound ??= AudioResourceLoader.TryLoad(AudioPaths.DoorsLocked);
    }

    public void PlayOpen()
    {
        PlayStream(OpenSound, "abrir");
    }

    public void PlayClose()
    {
        PlayStream(CloseSound, "fechar");
    }

    public void PlayLocked()
    {
        PlayStream(LockedSound, "trancada");
    }

    private void PlayStream(AudioStream? stream, string label)
    {
        if (stream == null)
        {
            GD.Print($"DoorAudio: som de {label} nao configurado.");
            return;
        }

        if (AudioManager.Find(this) is { } manager)
        {
            manager.Play3DSound(stream, GlobalPosition);
            return;
        }

        Stream = stream;
        VolumeDb = DoorVolumeDb;
        Play();
    }
}
