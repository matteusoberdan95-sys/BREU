namespace BREU.Scripts.Audio;

/// <summary>
/// Gerenciador simples de audio 2D/3D/UI. Instanciado em DemoRoom (grupo audio_manager).
/// Autoload opcional futuro: ver docs/technical/AUDIO_SYSTEM.md
/// </summary>
public partial class AudioManager : Node
{
    [Export] public NodePath UiPlayerPath { get; set; } = "UiPlayer";
    [Export] public NodePath Sfx2DPlayerPath { get; set; } = "Sfx2DPlayer";

    private AudioStreamPlayer? _uiPlayer;
    private AudioStreamPlayer? _sfx2DPlayer;

    public override void _Ready()
    {
        AddToGroup("audio_manager");
        _uiPlayer = GetNodeOrNull<AudioStreamPlayer>(UiPlayerPath);
        _sfx2DPlayer = GetNodeOrNull<AudioStreamPlayer>(Sfx2DPlayerPath);
    }

    public static AudioManager? Find(Node from)
    {
        return from.GetTree().GetFirstNodeInGroup("audio_manager") as AudioManager;
    }

    public void PlayUiSound(AudioStream? stream)
    {
        PlayOnPlayer(_uiPlayer, stream, "UI");
    }

    public void Play2DSound(AudioStream? stream)
    {
        PlayOnPlayer(_sfx2DPlayer, stream, "2D");
    }

    public void Play3DSound(AudioStream? stream, Vector3 position)
    {
        if (stream == null)
        {
            GD.Print("AudioManager: stream nao configurado.");
            return;
        }

        var player = new AudioStreamPlayer3D
        {
            Stream = stream,
            GlobalPosition = position
        };
        AddChild(player);
        player.Finished += () => player.QueueFree();
        player.Play();
    }

    public void Play3DSoundAtPath(string path, Vector3 position)
    {
        Play3DSound(AudioResourceLoader.TryLoad(path), position);
    }

    private static void PlayOnPlayer(AudioStreamPlayer? player, AudioStream? stream, string label)
    {
        if (stream == null)
        {
            GD.Print("AudioManager: stream nao configurado.");
            return;
        }

        if (player == null)
        {
            GD.Print($"AudioManager: player {label} nao encontrado.");
            return;
        }

        player.Stream = stream;
        player.Play();
    }
}
