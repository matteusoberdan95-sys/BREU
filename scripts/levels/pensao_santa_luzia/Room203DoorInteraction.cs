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
        if (_state.HasTriggeredRoom203Warning)
        {
            hud?.ShowMessage("Algo pesado bloqueia a porta pelo outro lado.", 3.2f);
            return;
        }

        _sequenceRunning = true;
        _state.TriggerRoom203Warning();
        hud?.ShowMessage("Algo pesado bloqueia a porta pelo outro lado.", 3.2f);
        PensionAudioManager.Find(GetTree())?.PlayOneShot("door_scratch_01", -13f);
        _ = FinishSequenceAsync();
    }

    private async System.Threading.Tasks.Task FinishSequenceAsync()
    {
        await ToSignal(GetTree().CreateTimer(1.2f), SceneTreeTimer.SignalName.Timeout);
        PensionAudioManager.Find(GetTree())?.PlayOneShot("distant_step_01", -16f);
        await FlickerCorridorLightAsync();
        HUDController.FindActive(GetTree())?.ShowMessage(
            "Do outro lado, ouvi um som baixo, como unhas arrastando na madeira. Eu não estou sozinho aqui.", 5f);
        _sequenceRunning = false;
    }

    private async System.Threading.Tasks.Task FlickerCorridorLightAsync()
    {
        var light = GetTree().CurrentScene.GetNodeOrNull<OmniLight3D>("Lighting/UpperCorridorLight");
        if (light == null) return;
        var original = light.LightEnergy;
        for (var i = 0; i < 4; i++)
        {
            light.LightEnergy = i % 2 == 0 ? original * 0.35f : original;
            await ToSignal(GetTree().CreateTimer(0.16f), SceneTreeTimer.SignalName.Timeout);
        }
        light.LightEnergy = original;
    }
}
