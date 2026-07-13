namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;

/// <summary>Reusable short hide interaction for open antique wardrobes.</summary>
public partial class WardrobeHidingSpot : Node, IInteractable
{
    private PensaoPuzzleState? _state;
    private bool _running;

    public override void _Ready()
    {
        _state = GetTree().CurrentScene?.GetNodeOrNull<PensaoPuzzleState>("PuzzleState");
    }

    public string GetPromptText() => _state switch
    {
        { PlayerInSafeZone: true, PlayerHidden: false, FirstChaseStarted: true } =>
            "Se esconder no guarda-roupa",
        _ => string.Empty
    };

    public void Interact(Node interactor)
    {
        if (_running || _state?.BeginReusableHide() != true) return;
        _running = true;
        _ = HideAsync();
    }

    private async System.Threading.Tasks.Task HideAsync()
    {
        var hud = HUDController.FindActive(GetTree());
        var audio = PensionAudioManager.Find(GetTree());
        hud?.ShowMessage("Fique quieto...", 2.4f);
        audio?.PlayOneShot("distant_step_03", -19f);
        await ToSignal(GetTree().CreateTimer(3.2f), SceneTreeTimer.SignalName.Timeout);
        if (!IsInsideTree()) return;
        _state!.CompleteReusableHide();
        hud?.ShowMessage("Os passos passaram.", 2.8f);
        _running = false;
    }
}
