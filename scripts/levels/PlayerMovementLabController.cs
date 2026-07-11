namespace BREU.Scripts.Levels;

/// <summary>
/// Sprint 02 movement lab — spawn, reset (F9) and startup debug.
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
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("debug_reset_player"))
        {
            ResetPlayerToSpawn();
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
}
