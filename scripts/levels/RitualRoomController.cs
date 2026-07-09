namespace BREU.Scripts.Levels;

/// <summary>
/// Placeholder da Fase 2 — Sala dos Santos Secos.
/// </summary>
public partial class RitualRoomController : Node3D
{
    [Export] public NodePath PlayerPath { get; set; } = "Player";
    [Export] public NodePath PlayerSpawnPath { get; set; } = "SpawnPoints/PlayerSpawn";
    [Export] public string WelcomeMessage { get; set; } = "Fase 2 — Sala dos Santos Secos (placeholder)";

    public override void _Ready()
    {
        AddToGroup("ritual_room");
        PositionPlayerAtSpawn();
        ShowWelcomeMessage();
    }

    private void PositionPlayerAtSpawn()
    {
        var spawn = GetNodeOrNull<Marker3D>(PlayerSpawnPath);
        var player = GetNodeOrNull<PlayerController>(PlayerPath);
        if (spawn == null || player == null)
        {
            return;
        }

        player.GlobalPosition = spawn.GlobalPosition;
        player.GlobalRotation = spawn.GlobalRotation;
    }

    private void ShowWelcomeMessage()
    {
        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.ShowTemporaryMessage(WelcomeMessage, 5.0f);
        }
        else
        {
            GD.Print(WelcomeMessage);
        }
    }
}
