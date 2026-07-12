namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;

/// <summary>Sprint 19 — one-shot scratch when leaving Room 204 after reading the note.</summary>
public partial class UpperWingRoom204ExitScare : Area3D
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
        if (body is not CharacterBody3D || _state == null)
        {
            return;
        }

        if (!_state.ReadRoom204Note || _state.Room204ExitScarePlayed || body.GlobalPosition.Y < 2.6f)
        {
            return;
        }

        _state.MarkRoom204ExitScarePlayed();
        Monitoring = false;
        PensionAudioManager.Find(GetTree())?.PlayOneShot("door_scratch_02", -15f);
        HUDController.FindActive(GetTree())?.ShowMessage(
            "Por um segundo, você ouviu unhas na madeira.", 3.5f);
    }
}
