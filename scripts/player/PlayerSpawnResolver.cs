namespace BREU.Scripts.Player;

/// <summary>
/// Reposiciona o player no marcador de spawn da cena ao iniciar.
/// </summary>
public partial class PlayerSpawnResolver : Node
{
    [Export] public NodePath PlayerPath { get; set; } = "Player";
    [Export] public NodePath PlayerSpawnPath { get; set; } = "PlayerSpawn";
    [Export] public string CheckpointName { get; set; } = "";
    [Export] public string StartMessage { get; set; } = "";
    [Export] public float StartMessageDuration { get; set; } = 3.0f;

    public override void _Ready()
    {
        CallDeferred(nameof(ResolveSpawn));
    }

    public void ResolveSpawn()
    {
        var player = FindPlayer();
        var spawn = FindSpawn();

        if (player != null && spawn != null)
        {
            player.GlobalPosition = spawn.GlobalPosition;
            player.GlobalRotation = spawn.GlobalRotation;

            if (player is CharacterBody3D body)
            {
                body.Velocity = Vector3.Zero;
            }
        }

        CheckpointManager.Instance?.UpdateCheckpoint(CheckpointName);
        ShowStartMessage();
    }

    private Node3D? FindPlayer()
    {
        if (!PlayerPath.IsEmpty && GetNodeOrNull<Node3D>(PlayerPath) is { } player)
        {
            return player;
        }

        return GetTree().GetFirstNodeInGroup("player") as Node3D;
    }

    private Marker3D? FindSpawn()
    {
        if (!PlayerSpawnPath.IsEmpty && GetNodeOrNull<Marker3D>(PlayerSpawnPath) is { } spawn)
        {
            return spawn;
        }

        foreach (var path in new[] { "PlayerSpawn", "SpawnPoints/PlayerSpawn", "SpawnPoints/PlayerStart" })
        {
            if (GetNodeOrNull<Marker3D>(path) is { } fallback)
            {
                return fallback;
            }
        }

        return null;
    }

    private void ShowStartMessage()
    {
        if (string.IsNullOrWhiteSpace(StartMessage))
        {
            return;
        }

        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.ShowTemporaryMessage(StartMessage, StartMessageDuration);
            return;
        }

        GD.Print($"HUD mensagem: {StartMessage}");
    }
}
