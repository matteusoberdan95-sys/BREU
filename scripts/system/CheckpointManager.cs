namespace BREU.Scripts.Systems;

/// <summary>
/// Checkpoint em memoria para a vertical slice. Persistencia em disco fica para depois.
/// </summary>
public partial class CheckpointManager : Node
{
    public static CheckpointManager? Instance { get; private set; }

    public string LastCheckpointId { get; private set; } = "";
    public string LastScenePath { get; private set; } = "";
    public Vector3 LastPlayerPosition { get; private set; }
    public Vector3 LastPlayerRotation { get; private set; }
    public bool HasCheckpoint { get; private set; }

    public string LastSceneName { get; private set; } = "";
    public string LastCheckpoint => LastCheckpointId;

    public string CheckpointWeaponName { get; private set; } = "";
    public int CheckpointWeaponDurability { get; private set; }
    public int CheckpointWeaponMaxDurability { get; private set; }
    public bool CheckpointHasRustyHammer { get; private set; }
    public bool CheckpointHasOldKey { get; private set; }

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

        var scene = GetTree().CurrentScene;
        var player = GetTree().GetFirstNodeInGroup("player") as Node3D;
        SetCheckpoint(
            checkpointName,
            scene?.SceneFilePath ?? "",
            player?.GlobalPosition ?? Vector3.Zero,
            player?.GlobalRotation ?? Vector3.Zero);
    }

    public void SetCheckpoint(string checkpointId, string scenePath, Vector3 position, Vector3 rotation)
    {
        if (string.IsNullOrWhiteSpace(checkpointId))
        {
            return;
        }

        LastCheckpointId = checkpointId;
        LastScenePath = string.IsNullOrWhiteSpace(scenePath)
            ? GetTree().CurrentScene?.SceneFilePath ?? ""
            : scenePath;
        LastSceneName = GetTree().CurrentScene?.Name ?? "";
        LastPlayerPosition = position;
        LastPlayerRotation = rotation;
        HasCheckpoint = !string.IsNullOrWhiteSpace(LastScenePath);

        CaptureGameSessionSnapshot();

        GD.Print($"Checkpoint: {LastCheckpointId} ({LastSceneName})");
    }

    public bool HasValidCheckpoint()
    {
        return HasCheckpoint && !string.IsNullOrWhiteSpace(LastScenePath);
    }

    public void RespawnFromLastCheckpoint()
    {
        RestoreGameSessionSnapshot();

        var scenePath = HasValidCheckpoint()
            ? LastScenePath
            : ProjectSettings.GetSetting("application/run/main_scene").AsString();

        if (string.IsNullOrWhiteSpace(scenePath))
        {
            GD.PrintErr("CheckpointManager: nenhum checkpoint ou main scene valido para respawn.");
            return;
        }

        if (SceneTransitionController.Instance != null)
        {
            SceneTransitionController.Instance.ChangeSceneWithFade(scenePath, "A casa ainda esta ouvindo.");
            return;
        }

        GetTree().ChangeSceneToFile(scenePath);
    }

    public bool IsCheckpointScene(string scenePath)
    {
        return HasValidCheckpoint() && LastScenePath == scenePath;
    }

    private void CaptureGameSessionSnapshot()
    {
        if (GetNodeOrNull<GameSession>("/root/GameSession") is not { } session)
        {
            return;
        }

        CheckpointHasRustyHammer = session.HasRustyHammer;
        CheckpointHasOldKey = session.HasOldKey;
        CheckpointWeaponName = session.CurrentWeaponName;
        CheckpointWeaponDurability = session.CurrentWeaponDurability;
        CheckpointWeaponMaxDurability = session.CurrentWeaponMaxDurability;
    }

    private void RestoreGameSessionSnapshot()
    {
        if (GetNodeOrNull<GameSession>("/root/GameSession") is not { } session)
        {
            return;
        }

        session.RestoreSnapshot(
            CheckpointHasRustyHammer,
            CheckpointHasOldKey,
            CheckpointWeaponName,
            CheckpointWeaponDurability,
            CheckpointWeaponMaxDurability);
    }
}
