namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>Sprint 25 — small ground-floor-only, one-shot second-chase trigger.</summary>
public partial class SecondChaseTrigger : Area3D
{
    private PensaoPuzzleState? _state;
    private EnemyPresenceAI? _enemyAi;

    public override void _Ready()
    {
        _state = GetTree().CurrentScene?.GetNodeOrNull<PensaoPuzzleState>("PuzzleState");
        _enemyAi = GetParentOrNull<EnemyPresenceAI>();
        BodyEntered += OnBodyEntered;
    }

    public override void _ExitTree() => BodyEntered -= OnBodyEntered;

    private void OnBodyEntered(Node3D body)
    {
        if (body is not CharacterBody3D player || player.GlobalPosition.Y > 1.7f ||
            _state?.SecondChaseAvailable != true || _state.SecondChaseStarted || _state.SecondChaseFinished) return;

        if (_enemyAi?.TryStartSecondChase() != true) return;
        SetDeferred(Area3D.PropertyName.Monitoring, false);
    }
}
