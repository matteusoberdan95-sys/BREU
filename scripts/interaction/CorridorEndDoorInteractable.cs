namespace BREU.Scripts.Interaction;

/// <summary>
/// Porta final do corredor. Trancada ate o susto; depois permite entrar na Fase 2.
/// </summary>
public partial class CorridorEndDoorInteractable : Area3D, IInteractable
{
    [Export] public string LockedInteractionText { get; set; } = "A porta esta trancada";
    [Export] public string EnterInteractionText { get; set; } = "Entrar";
    [Export] public string LockedHudMessage { get; set; } = "A porta esta trancada.";
    [Export] public string NextScenePath { get; set; } = "res://scenes/levels/phase_02/RitualRoom.tscn";
    [Export] public bool RequireScareBeforeUnlock { get; set; } = true;
    [Export] public NodePath SequenceControllerPath { get; set; } = "../../DemoRoomSequenceController";
    [Export] public NodePath DoorAudioPath { get; set; } = "DoorAudio";

    private DoorAudioController? _doorAudio;
    private bool _transitionStarted;

    public override void _Ready()
    {
        _doorAudio = GetNodeOrNull<DoorAudioController>(DoorAudioPath);
    }

    public string GetInteractionText()
    {
        return IsUnlocked() ? EnterInteractionText : LockedInteractionText;
    }

    public void Interact(PlayerController player)
    {
        if (_transitionStarted)
        {
            return;
        }

        if (!IsUnlocked())
        {
            _doorAudio?.PlayLocked();
            ShowHudMessage(LockedHudMessage);
            GD.Print(LockedHudMessage);
            return;
        }

        _transitionStarted = true;
        _doorAudio?.PlayOpen();
        GD.Print($"CorridorEndDoor: entrando em {NextScenePath}");

        if (SceneTransitionController.Instance != null)
        {
            SceneTransitionController.Instance.ChangeScene(NextScenePath);
            return;
        }

        GetTree().ChangeSceneToFile(NextScenePath);
    }

    private bool IsUnlocked()
    {
        if (!RequireScareBeforeUnlock)
        {
            return true;
        }

        if (GetNodeOrNull(SequenceControllerPath) is DemoRoomSequenceController sequence)
        {
            return sequence.HasTriggeredCorridorScare;
        }

        return GetTree().GetFirstNodeInGroup("demo_sequence") is DemoRoomSequenceController fallback
            && fallback.HasTriggeredCorridorScare;
    }

    private void ShowHudMessage(string message)
    {
        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.ShowTemporaryMessage(message, 3.5f);
        }
    }
}
