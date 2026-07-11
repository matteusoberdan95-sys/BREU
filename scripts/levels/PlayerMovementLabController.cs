namespace BREU.Scripts.Levels;

/// <summary>
/// Sprint 02 movement lab — spawn, reset (F9), HUD welcome message.
/// </summary>
public partial class PlayerMovementLabController : Node3D
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
        CallDeferred(nameof(ShowStartupHudMessage));
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("debug_reset_player"))
        {
            ResetPlayerToSpawn();
            ShowHudMessage("Posicao resetada (F9).");
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

    private void ShowStartupHudMessage()
    {
        ShowHudMessage("PlayerMovementLab — HUD ativo. F: lanterna | Shift: sprint | F10/F11: debug");
    }

    private void ShowHudMessage(string text)
    {
        HUDController.FindActive(GetTree())?.ShowMessage(text);
    }
}
