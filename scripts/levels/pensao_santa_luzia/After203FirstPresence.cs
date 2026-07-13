namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;

/// <summary>Sprint 21 — small ground-floor one-shot after the completed Room 203 event.</summary>
public partial class After203FirstPresence : Area3D
{
    private PensaoPuzzleState? _state;
    private MeshInstance3D? _shadow;
    private MeshInstance3D? _clue;

    public override void _Ready()
    {
        Monitoring = true;
        Monitorable = false;
        CollisionLayer = 0;
        CollisionMask = 16;
        BodyEntered += OnBodyEntered;
        _state = GetTree().CurrentScene?.GetNodeOrNull<PensaoPuzzleState>("PuzzleState");
        _shadow = GetParent()?.GetNodeOrNull<MeshInstance3D>("FirstPresence_Shadow");
        _clue = GetParent()?.GetNodeOrNull<MeshInstance3D>("Downstairs_Clue_After203");
        if (_shadow != null) _shadow.Visible = false;
        if (_clue != null) _clue.Visible = _state?.FirstPresencePlayed == true;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body is not CharacterBody3D || body.GlobalPosition.Y > 1.7f ||
            _state?.Room203EventPlayed != true || !_state.FirstPresenceHintPlayed ||
            _state.FirstPresencePlayed) return;

        _state.StartFirstPresence();
        SetDeferred(Area3D.PropertyName.Monitoring, false);
        _ = PlaySequenceAsync();
    }

    private async System.Threading.Tasks.Task PlaySequenceAsync()
    {
        var audio = PensionAudioManager.Find(GetTree());
        await ToSignal(GetTree().CreateTimer(0.45f), SceneTreeTimer.SignalName.Timeout);
        audio?.PlayOneShot("old_house_settle_01", -15f);
        await ToSignal(GetTree().CreateTimer(0.65f), SceneTreeTimer.SignalName.Timeout);
        audio?.PlayOneShot("distant_knock_02", -16f);
        await FlickerReceptionAsync();
        audio?.PlayOneShot("distant_step_02", -16f);
        if (_shadow != null)
        {
            _shadow.Visible = true;
            await ToSignal(GetTree().CreateTimer(1.35f), SceneTreeTimer.SignalName.Timeout);
            if (GodotObject.IsInstanceValid(_shadow)) _shadow.Visible = false;
        }
        HUDController.FindActive(GetTree())?.ShowMessage(
            "Alguém passou pelo corredor. Objetivo: Verifique a recepção.", 5f);
        if (_clue != null) _clue.Visible = true;
    }

    private async System.Threading.Tasks.Task FlickerReceptionAsync()
    {
        var light = GetTree().CurrentScene?.GetNodeOrNull<OmniLight3D>("Lighting/ReceptionLight");
        if (light == null) return;
        var energy = light.LightEnergy;
        light.LightEnergy = energy * 0.12f;
        await ToSignal(GetTree().CreateTimer(0.55f), SceneTreeTimer.SignalName.Timeout);
        if (GodotObject.IsInstanceValid(light)) light.LightEnergy = energy;
    }
}
