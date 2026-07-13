namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;

public partial class Room203ExitPresenceTrigger : Area3D
{
    private PensaoPuzzleState? _state;

    public override void _Ready()
    {
        Monitoring = true;
        Monitorable = false;
        CollisionLayer = 0;
        CollisionMask = 16;
        BodyEntered += OnBodyEntered;
        _state = GetTree().CurrentScene?.GetNodeOrNull<PensaoPuzzleState>("PuzzleState");
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body is not CharacterBody3D || body.GlobalPosition.Y < 2.6f ||
            _state?.Room203EventPlayed != true || _state.FirstPresenceHintPlayed) return;
        _state.MarkFirstPresenceHintPlayed();
        SetDeferred(Area3D.PropertyName.Monitoring, false);
        PensionAudioManager.Find(GetTree())?.PlayOneShot("distant_step_03", -16f);
        HUDController.FindActive(GetTree())?.ShowMessage(
            "Você não está mais sozinho. Objetivo: Desça para verificar o barulho.", 5f);
        var light = GetTree().CurrentScene?.GetNodeOrNull<OmniLight3D>("Lighting/UpperCorridorLight");
        if (light != null) _ = FlickerAsync(light);
    }

    private async System.Threading.Tasks.Task FlickerAsync(OmniLight3D light)
    {
        var energy = light.LightEnergy;
        light.LightEnergy = energy * 0.1f;
        await ToSignal(GetTree().CreateTimer(1f), SceneTreeTimer.SignalName.Timeout);
        if (GodotObject.IsInstanceValid(light)) light.LightEnergy = energy;
    }
}
