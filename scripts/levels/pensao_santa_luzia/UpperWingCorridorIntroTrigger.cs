namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;

/// <summary>
/// Sprint 19 — one-shot corridor intro. Small Area3D; CollisionMask player only.
/// Must stay on second floor (Y >= 2.8).
/// </summary>
public partial class UpperWingCorridorIntroTrigger : Area3D
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
        if (body is not CharacterBody3D || _state == null || _state.CorridorIntroPlayed)
        {
            return;
        }

        if (body.GlobalPosition.Y < 2.6f)
        {
            return;
        }

        _state.MarkCorridorIntroPlayed();
        Monitoring = false;
        HUDController.FindActive(GetTree())?.ShowMessage(
            "O ar aqui em cima parece mais pesado.", 3.5f);
        PensionAudioManager.Find(GetTree())?.PlayOneShot("old_house_settle_01", -14f);

        var light = GetTree().CurrentScene?.GetNodeOrNull<OmniLight3D>(
            "World/Level/SecondFloor/UpperWingRooms/CorridorLight");
        if (light != null)
        {
            _ = FlickerOnceAsync(light);
        }
    }

    private async System.Threading.Tasks.Task FlickerOnceAsync(OmniLight3D light)
    {
        var original = light.LightEnergy;
        light.LightEnergy = original * 0.2f;
        await ToSignal(GetTree().CreateTimer(0.35f), SceneTreeTimer.SignalName.Timeout);
        light.LightEnergy = original;
    }
}
