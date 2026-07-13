namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;

public partial class Room203LedgerInteraction : Node, IInteractable
{
    private PensaoPuzzleState? _state;
    private bool _running;

    public override void _Ready()
    {
        _state = GetTree().CurrentScene?.GetNodeOrNull<PensaoPuzzleState>("PuzzleState");
        GetParent()?.GetParent()?.AddToGroup("interactable");
    }

    public string GetPromptText() => _state?.Room203EventPlayed == true ? string.Empty : "Ler página rasgada";

    public void Interact(Node interactor)
    {
        if (_state?.Room203Opened != true || _state.Room203EventPlayed || _running) return;
        _running = true;
        _state.TriggerRoom203Event();
        HUDController.FindActive(GetTree())?.ShowMessage(
            "Ela voltou a bater por dentro da parede. Não era hóspede. Nunca foi.", 6f);
        _ = PlayEventAsync();
    }

    private async System.Threading.Tasks.Task PlayEventAsync()
    {
        var audio = PensionAudioManager.Find(GetTree());
        var room = GetParent()?.GetParent()?.GetParent();
        var light = room?.GetNodeOrNull<OmniLight3D>("Room203_LightWeak");
        audio?.PlayOneShot("old_house_settle_02", -12f);
        await ToSignal(GetTree().CreateTimer(0.7f), SceneTreeTimer.SignalName.Timeout);
        if (light != null)
        {
            var energy = light.LightEnergy;
            for (var i = 0; i < 6; i++)
            {
                light.LightEnergy = i % 2 == 0 ? energy * 0.12f : energy;
                await ToSignal(GetTree().CreateTimer(0.13f), SceneTreeTimer.SignalName.Timeout);
            }
            light.LightEnergy = energy * 0.65f;
        }
        audio?.PlayOneShot("door_scratch_02", -14f);
        await ToSignal(GetTree().CreateTimer(0.8f), SceneTreeTimer.SignalName.Timeout);
        audio?.PlayOneShot("distant_step_04", -15f);
        HUDController.FindActive(GetTree())?.ShowMessage(
            "Alguma coisa desceu as escadas. Objetivo: Volte para o corredor principal.", 5f);
        if (GetParent() is Area3D area) area.Monitoring = false;
        _running = false;
    }
}
