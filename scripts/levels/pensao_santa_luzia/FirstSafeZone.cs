namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;

/// <summary>Sprint 23 — localized, non-blocking shelter trigger on the ground floor.</summary>
public partial class FirstSafeZone : Area3D
{
    private PensaoPuzzleState? _state;

    public override void _Ready()
    {
        _state = GetTree().CurrentScene?.GetNodeOrNull<PensaoPuzzleState>("PuzzleState");
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    public override void _ExitTree()
    {
        BodyEntered -= OnBodyEntered;
        BodyExited -= OnBodyExited;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body is not CharacterBody3D || body.GlobalPosition.Y > 1.7f ||
            _state?.EnterFirstSafeZone() != true) return;

        GD.Print("[SPRINT23] SafeZone_FirstShelter entered");
        // The Sprint 22 chase hid its scripted enemy when the player reached
        // this shelter. After Sprint 23 the same mesh belongs to the persistent
        // patrol AI, which handles losing the player without disappearing.
        if (_state.FirstChaseFinished && !_state.Sprint23Completed)
        {
            GetTree().CurrentScene?.GetNodeOrNull<FirstEnemyChaseController>("FirstEnemyChase")?.StopForShelter();
            PensionAudioManager.Find(GetTree())?.PlayOneShot("distant_step_03", -17f);
            HUDController.FindActive(GetTree())?.ShowMessage("Ele parou do lado de fora.", 3f);
        }

    }

    private void OnBodyExited(Node3D body)
    {
        if (body is not CharacterBody3D || body.GlobalPosition.Y > 1.7f) return;
        _state?.ExitFirstSafeZone();
    }

}
