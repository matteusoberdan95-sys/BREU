namespace BREU.Scripts.Levels;

/// <summary>
/// Sprint 04 interaction lab — spawn, reset (F9), welcome message, interaction debug.
/// </summary>
public partial class InteractionLabController : Node3D
{
    [Export] public NodePath PlayerPath { get; set; } = new("Player");
    [Export] public NodePath SpawnPath { get; set; } = new("PlayerSpawn");
    [Export] public bool EnableInteractionDebug { get; set; } = true;

    private CharacterBody3D? _player;
    private Marker3D? _spawn;

    public override void _Ready()
    {
        _player = GetNodeOrNull<CharacterBody3D>(PlayerPath);
        _spawn = GetNodeOrNull<Marker3D>(SpawnPath);

        if (EnableInteractionDebug && _player != null)
        {
            var interaction = _player.GetNodeOrNull<PlayerInteractionRaycast>("PlayerInteractionRaycast");
            if (interaction != null)
            {
                interaction.DebugMode = true;
            }
        }

        ResetPlayerToSpawn();
        CallDeferred(nameof(ShowStartupHudMessage));
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

    private void ShowStartupHudMessage()
    {
        HUDController.FindActive(GetTree())?.ShowMessage(
            "InteractionLab — olhe para os objetos e pressione E.",
            4.0f);
    }
}
