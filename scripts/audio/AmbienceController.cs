namespace BREU.Scripts.Audio;

/// <summary>
/// Loops de ambiencia do Quarto 07 e corredor. Streams do pack breu_de_dentro_audio_pack_v01.
/// </summary>
public partial class AmbienceController : Node
{
    [Export] public float RoomVolumeDb { get; set; } = -18.0f;
    [Export] public float CorridorVolumeDb { get; set; } = -22.0f;
    [Export] public float WindVolumeDb { get; set; } = -24.0f;
    [Export] public bool PlayCorridorOnStart { get; set; }
    [Export] public NodePath RoomPlayerPath { get; set; } = "RoomAmbience";
    [Export] public NodePath CorridorPlayerPath { get; set; } = "CorridorAmbience";
    [Export] public NodePath WindPlayerPath { get; set; } = "WindAmbience";

    private AudioStreamPlayer? _roomPlayer;
    private AudioStreamPlayer? _corridorPlayer;
    private AudioStreamPlayer? _windPlayer;

    public override void _Ready()
    {
        AddToGroup("ambience_controller");
        _roomPlayer = GetNodeOrNull<AudioStreamPlayer>(RoomPlayerPath);
        _corridorPlayer = GetNodeOrNull<AudioStreamPlayer>(CorridorPlayerPath);
        _windPlayer = GetNodeOrNull<AudioStreamPlayer>(WindPlayerPath);

        StartLoop(_roomPlayer, AudioPaths.AmbienceRoomTone, RoomVolumeDb, "quarto");
        StartLoop(_windPlayer, AudioPaths.AmbienceWind, WindVolumeDb, "vento");

        if (PlayCorridorOnStart)
        {
            StartCorridorAmbience();
        }
    }

    public void StartCorridorAmbience()
    {
        StartLoop(_corridorPlayer, AudioPaths.AmbienceCorridorTone, CorridorVolumeDb, "corredor");
    }

    public void StopCorridorAmbience()
    {
        _corridorPlayer?.Stop();
    }

    private static void StartLoop(AudioStreamPlayer? player, string path, float volumeDb, string label)
    {
        if (player == null)
        {
            GD.Print($"Ambience: player de {label} nao encontrado.");
            return;
        }

        var stream = AudioResourceLoader.TryLoad(path, loop: true);
        if (stream == null)
        {
            GD.Print($"Ambience: stream de {label} nao configurado ({path}).");
            return;
        }

        player.Stream = stream;
        player.VolumeDb = volumeDb;
        player.Play();
        GD.Print($"Ambience: {label} iniciado.");
    }
}
