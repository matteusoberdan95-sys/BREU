namespace BREU.Scripts.Doors;

/// <summary>
/// Audio 3D da porta. Streams nulos sao ignorados com Debug.Print.
/// TODO: res://assets/audio/sfx/doors/door_open_old_wood.ogg
/// TODO: res://assets/audio/sfx/doors/door_close_old_wood.ogg
/// TODO: res://assets/audio/sfx/doors/door_locked_rattle.ogg
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

        Stream = stream;
        VolumeDb = DoorVolumeDb;
        Play();
    }
}
