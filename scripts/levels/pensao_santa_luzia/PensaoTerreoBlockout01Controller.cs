namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Sprint 05 — spawn at trail start, F9 reset, welcome HUD message.
/// </summary>
public partial class PensaoTerreoBlockout01Controller : Node3D
{
    [Export] public NodePath PlayerPath { get; set; } = new("Player");
    [Export] public NodePath SpawnPath { get; set; } = new("PlayerSpawn");

    private CharacterBody3D? _player;
    private Marker3D? _spawn;

    public override void _Ready()
    {
        _player = GetNodeOrNull<CharacterBody3D>(PlayerPath);
        _spawn = GetNodeOrNull<Marker3D>(SpawnPath);
        ResetPlayerToSpawn();
        CallDeferred(nameof(ShowWelcomeMessage));
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("debug_reset_player"))
        {
            ResetPlayerToSpawn();
            HUDController.FindActive(GetTree())?.ShowMessage("Posicao resetada (F9).", 2.5f);
        }
    }

    public void ResetPlayerToSpawn()
    {
        if (_player == null || _spawn == null)
        {
            return;
        }

        _player.GlobalPosition = _spawn.GlobalPosition;
        _player.GlobalRotation = _spawn.GlobalRotation;
        _player.Velocity = Vector3.Zero;
    }

    private void ShowWelcomeMessage()
    {
        HUDController.FindActive(GetTree())?.ShowMessage(
            "Pensao Santa Luzia — siga a trilha ate a varanda.",
            4.5f);
    }
}
