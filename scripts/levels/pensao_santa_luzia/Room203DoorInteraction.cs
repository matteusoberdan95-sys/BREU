namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;

/// <summary>Sprint 20 — stable Room 203 door: blocked, forceable, then permanently open.</summary>
public partial class Room203DoorInteraction : Node, IInteractable
{
    private PensaoPuzzleState? _state;
    private bool _sequenceRunning;
    private MeshInstance3D? _panel;
    private CollisionShape3D? _blocker;

    public override void _Ready()
    {
        _state = GetTree().CurrentScene.GetNodeOrNull<PensaoPuzzleState>("PuzzleState");
        var door = GetParent()?.GetParent();
        door?.AddToGroup("interactable");
        _panel = door?.GetNodeOrNull<MeshInstance3D>("DoorPanel");
        _blocker = door?.GetNodeOrNull<CollisionShape3D>("Door_Blocker_StaticBody/CollisionShape3D");
        ApplyOpenState();
    }

    public string GetPromptText() => _state switch
    {
        { Room203Opened: true } => string.Empty,
        { Room203CanBeForced: true } => "Forçar porta do Quarto 203",
        _ => "Tentar abrir Quarto 203"
    };

    public void Interact(Node interactor)
    {
        var hud = HUDController.FindActive(GetTree());
        if (_state == null || _sequenceRunning) return;
        if (_state.Room203Opened)
        {
            hud?.ShowMessage("O quarto está aberto.", 2.5f);
            return;
        }
        if (_state.Room203CanBeForced)
        {
            _sequenceRunning = true;
            _ = ForceOpenAsync();
            return;
        }
        _state.TriggerRoom203Warning();
        hud?.ShowMessage("Algo pesado bloqueia a porta pelo outro lado.", 3.2f);
        PensionAudioManager.Find(GetTree())?.PlayOneShot("door_scratch_01", -13f);
    }

    private async System.Threading.Tasks.Task ForceOpenAsync()
    {
        var hud = HUDController.FindActive(GetTree());
        var audio = PensionAudioManager.Find(GetTree());
        hud?.ShowMessage("Há algo arrastando do outro lado. Talvez agora dê para forçar.", 4f);
        audio?.PlayOneShot("door_scratch_02", -12f);
        await ToSignal(GetTree().CreateTimer(0.85f), SceneTreeTimer.SignalName.Timeout);
        audio?.PlayOneShot("old_house_settle_02", -13f);
        _state!.OpenRoom203();
        ApplyOpenState();
        hud?.ShowMessage("A porta cede. Objetivo: Investigue o Quarto 203.", 4f);
        _sequenceRunning = false;
    }

    private void ApplyOpenState()
    {
        if (_state?.Room203Opened != true) return;
        if (_panel != null)
        {
            _panel.Position = new Vector3(-1.72f, 3.9f, -10.78f);
            _panel.Rotation = new Vector3(0f, Mathf.DegToRad(78f), 0f);
        }
        _blocker?.SetDeferred(CollisionShape3D.PropertyName.Disabled, true);
        if (GetParent() is Area3D area) area.Monitoring = false;
    }
}
