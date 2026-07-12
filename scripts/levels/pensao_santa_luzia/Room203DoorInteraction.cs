namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;

/// <summary>Sprint 17F hook: Room 203 remains permanently blocked.</summary>
public partial class Room203DoorInteraction : Node, IInteractable
{
    private PensaoPuzzleState? _state;
    private bool _sequenceRunning;

    public override void _Ready()
    {
        _state = GetTree().CurrentScene.GetNodeOrNull<PensaoPuzzleState>("PuzzleState");
        GetParent()?.GetParent()?.AddToGroup("interactable");
    }

    public string GetPromptText() => "Tentar abrir Quarto 203";

    public void Interact(Node interactor)
    {
        var hud = HUDController.FindActive(GetTree());
        if (_state?.HasReadOwnerLedger != true)
        {
            hud?.ShowMessage("A porta está bloqueada.", 3f);
            return;
        }

        if (_sequenceRunning) return;
        _sequenceRunning = true;
        hud?.ShowMessage("Algo pesado bloqueia a porta pelo outro lado.", 3.2f);
        PensionAudioManager.Find(GetTree())?.PlayOneShot("door_scratch_01", -13f);
        _ = FinishSequenceAsync();
    }

    private async System.Threading.Tasks.Task FinishSequenceAsync()
    {
        await ToSignal(GetTree().CreateTimer(3.3f), SceneTreeTimer.SignalName.Timeout);
        HUDController.FindActive(GetTree())?.ShowMessage(
            "Do outro lado, ouvi um som baixo, como unhas arrastando na madeira. Eu não devia abrir isso agora.", 5f);
        _sequenceRunning = false;
    }
}
