namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;

/// <summary>Sprint 23 — first reusable interaction-driven hiding sequence.</summary>
public partial class FirstHidingSpot : Node, IInteractable
{
    private PensaoPuzzleState? _state;
    private bool _sequenceRunning;

    public override void _Ready()
    {
        _state = GetTree().CurrentScene?.GetNodeOrNull<PensaoPuzzleState>("PuzzleState");
    }

    public string GetPromptText() => _state switch
    {
        { SecondChaseStarted: true, SecondChaseFinished: false, PlayerInSafeZone: true, PlayerHidden: false } =>
            "Se esconder",
        { Sprint23Completed: false, PlayerInSafeZone: true, PlayerHidden: false, FirstChaseStarted: true } =>
            "Se esconder",
        _ => string.Empty
    };

    public void Interact(Node interactor)
    {
        if (_state?.SecondChaseStarted == true && !_state.SecondChaseFinished)
        {
            if (_state.BeginSecondChaseHide())
            {
                HUDController.FindActive(GetTree())?.ShowMessage("Não respire.", 2.8f);
                PensionAudioManager.Find(GetTree())?.PlayOneShot("old_house_settle_01", -19f);
            }
            return;
        }

        if (_sequenceRunning || _state?.BeginFirstHide() != true) return;
        _sequenceRunning = true;
        _ = HideAsync();
    }

    private async System.Threading.Tasks.Task HideAsync()
    {
        var hud = HUDController.FindActive(GetTree());
        var audio = PensionAudioManager.Find(GetTree());
        hud?.ShowMessage("Segure a respiração...", 2.8f);
        audio?.PlayOneShot("old_house_settle_01", -19f);
        await ToSignal(GetTree().CreateTimer(2.2f), SceneTreeTimer.SignalName.Timeout);
        if (!IsInsideTree()) return;
        audio?.PlayOneShot("distant_step_04", -18f);
        await ToSignal(GetTree().CreateTimer(1.4f), SceneTreeTimer.SignalName.Timeout);
        if (!IsInsideTree()) return;
        hud?.ShowMessage("Os passos se afastaram.", 2.6f);
        await ToSignal(GetTree().CreateTimer(2.7f), SceneTreeTimer.SignalName.Timeout);
        if (!IsInsideTree()) return;
        if (_state!.ShowFirstHideTutorial())
        {
            hud?.ShowMessage("Quando ouvir passos próximos, procure um lugar escuro.", 3.2f);
            await ToSignal(GetTree().CreateTimer(3.3f), SceneTreeTimer.SignalName.Timeout);
            if (!IsInsideTree()) return;
            hud?.ShowMessage("Fique quieto até eles se afastarem.", 3.2f);
            await ToSignal(GetTree().CreateTimer(3.3f), SceneTreeTimer.SignalName.Timeout);
            if (!IsInsideTree()) return;
        }
        _state.CompleteFirstHide();
        hud?.ShowMessage("Agora. Objetivo: Procure uma forma de entender o que está caçando você.", 6f);
        GD.Print("[SPRINT23] First hiding completed");
        _sequenceRunning = false;
    }
}
