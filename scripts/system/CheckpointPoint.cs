namespace BREU.Scripts.Systems;

/// <summary>
/// Ponto reutilizavel para registrar checkpoints em memoria.
/// </summary>
public partial class CheckpointPoint : Node3D
{
    [Export] public string CheckpointId { get; set; } = "";
    [Export] public bool ActivateOnReady { get; set; } = true;
    [Export] public bool ActivateOnPlayerEnter { get; set; }

    public override void _Ready()
    {
        if (ActivateOnReady)
        {
            RegisterCheckpoint();
        }

        if (ActivateOnPlayerEnter && GetNodeOrNull<Area3D>("TriggerArea") is { } area)
        {
            area.BodyEntered += OnBodyEntered;
        }
    }

    public void RegisterCheckpoint()
    {
        CheckpointManager.Instance?.SetCheckpoint(
            CheckpointId,
            GetTree().CurrentScene?.SceneFilePath ?? "",
            GlobalPosition,
            GlobalRotation);
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body.IsInGroup("player"))
        {
            RegisterCheckpoint();
        }
    }
}
