namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>Localized safe volume inside an authored wardrobe.</summary>
public partial class WardrobeSafeZone : Area3D
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
            _state?.EnterReusableSafeZone() != true) return;
        // Keep the legacy visual shutdown limited to the original scripted
        // chase. The Sprint 24 patrol must remain visible and resume its route.
        if (!_state.Sprint23Completed)
            GetTree().CurrentScene?.GetNodeOrNull<FirstEnemyChaseController>("FirstEnemyChase")?.StopForShelter();
    }

    private void OnBodyExited(Node3D body)
    {
        if (body is not CharacterBody3D || body.GlobalPosition.Y > 1.7f) return;
        _state?.ExitFirstSafeZone();
    }
}
