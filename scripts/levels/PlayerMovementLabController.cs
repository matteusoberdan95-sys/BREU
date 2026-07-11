namespace BREU.Scripts.Levels;

/// <summary>
/// Sprint 02 movement lab — spawn, reset (R) and startup collision debug.
/// </summary>
public partial class PlayerMovementLabController : Node3D
{
    [Export] public NodePath PlayerPath { get; set; } = new("Player");
    [Export] public NodePath SpawnPath { get; set; } = new("PlayerSpawn");

    private CharacterBody3D? _player;
    private Marker3D? _spawn;
    private int _debugFramesRemaining = 5;

    public override void _Ready()
    {
        _player = GetNodeOrNull<CharacterBody3D>(PlayerPath);
        _spawn = GetNodeOrNull<Marker3D>(SpawnPath);
        ResetPlayerToSpawn();
        GD.Print("[PlayerMovementLab] scene=", GetTree().CurrentScene?.SceneFilePath ?? "unknown");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("reset_player"))
        {
            ResetPlayerToSpawn();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_debugFramesRemaining <= 0 || _player == null)
        {
            return;
        }

        _debugFramesRemaining--;
        if (_debugFramesRemaining > 0)
        {
            return;
        }

        PrintPlayerDebug();
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
        GD.Print("[PlayerMovementLab] reset -> ", _player.GlobalPosition);
    }

    private void PrintPlayerDebug()
    {
        if (_player == null)
        {
            return;
        }

        var collision = _player.GetNodeOrNull<CollisionShape3D>("CollisionShape3D");
        var head = _player.GetNodeOrNull<Node3D>("Head");
        var camera = head?.GetNodeOrNull<Camera3D>("Camera3D");

        GD.Print("[PlayerMovementLab] Player global pos=", _player.GlobalPosition);
        GD.Print("[PlayerMovementLab] IsOnFloor=", _player.IsOnFloor());

        if (collision != null)
        {
            GD.Print("[PlayerMovementLab] CollisionShape local pos=", collision.Position);
            if (collision.Shape is CapsuleShape3D capsule)
            {
                GD.Print("[PlayerMovementLab] Capsule height=", capsule.Height, " radius=", capsule.Radius);
            }
        }

        if (camera != null)
        {
            GD.Print("[PlayerMovementLab] Camera global pos=", camera.GlobalPosition);
        }
    }
}
