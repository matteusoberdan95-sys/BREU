namespace BREU.Scripts.Systems;

/// <summary>
/// Checkpoint em memoria para a vertical slice. Persistencia em disco fica para depois.
/// </summary>
public partial class CheckpointManager : Node
{
    public static CheckpointManager? Instance { get; private set; }

    public string LastSceneName { get; private set; } = "";
    public string LastCheckpoint { get; private set; } = "";

    public override void _Ready()
    {
        Instance = this;
        ProcessMode = ProcessModeEnum.Always;
    }

    public override void _ExitTree()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void UpdateCheckpoint(string checkpointName)
    {
        if (string.IsNullOrWhiteSpace(checkpointName))
        {
            return;
        }

        LastCheckpoint = checkpointName;
        LastSceneName = GetTree().CurrentScene?.Name ?? "";

        GD.Print($"Checkpoint: {LastCheckpoint} ({LastSceneName})");
    }
}
