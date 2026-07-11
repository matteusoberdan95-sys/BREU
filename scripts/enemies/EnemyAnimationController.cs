namespace BREU.Scripts.Enemies;

/// <summary>
/// Controla animacoes placeholder do Hospede Seco sem rig/bones.
/// </summary>
public partial class EnemyAnimationController : Node
{
    [Export] public NodePath AnimationPlayerPath { get; set; } = "../AnimationPlayer";
    [Export] public NodePath VisualPath { get; set; } = "../Visual";

    private const string IdleAnimation = "enemy_idle";
    private const string WalkAnimation = "enemy_walk";
    private const string AttackAnimation = "enemy_attack";
    private const string HitAnimation = "enemy_hit";
    private const string StunnedAnimation = "enemy_stunned_idle";
    private const string DeathAnimation = "enemy_death_placeholder";

    private AnimationPlayer? _animationPlayer;
    private Node3D? _visual;
    private Vector3 _basePosition;
    private Vector3 _baseRotation;
    private Tween? _oneShotTween;
    private string _currentAnimation = "";
    private float _animationTime;
    private bool _oneShotActive;

    public override void _Ready()
    {
        _animationPlayer = GetNodeOrNull<AnimationPlayer>(AnimationPlayerPath);
        _visual = GetNodeOrNull<Node3D>(VisualPath);
        _basePosition = _visual?.Position ?? Vector3.Zero;
        _baseRotation = _visual?.Rotation ?? Vector3.Zero;
    }

    public override void _Process(double delta)
    {
        if (_visual == null || _oneShotActive)
        {
            return;
        }

        _animationTime += (float)delta;
        switch (_currentAnimation)
        {
            case IdleAnimation:
                ApplyIdlePose();
                break;
            case WalkAnimation:
                ApplyWalkPose();
                break;
            case StunnedAnimation:
                ApplyStunnedPose();
                break;
        }
    }

    public void PlayIdle()
    {
        PlayLoop(IdleAnimation);
    }

    public void PlayWalk()
    {
        PlayLoop(WalkAnimation);
    }

    public void PlayAttack()
    {
        PlayOneShot(AttackAnimation);
    }

    public void PlayHit()
    {
        PlayOneShot(HitAnimation);
    }

    public void PlayStunned()
    {
        PlayLoop(StunnedAnimation);
    }

    public void PlayDeath()
    {
        PlayOneShot(DeathAnimation);
    }

    public void Stop()
    {
        _oneShotTween?.Kill();
        _animationPlayer?.Stop();
        _currentAnimation = "";
        _oneShotActive = false;
        ResetVisualPose();
    }

    private void PlayLoop(string animationName)
    {
        if (_oneShotActive)
        {
            return;
        }

        if (_currentAnimation == animationName)
        {
            return;
        }

        _currentAnimation = animationName;
        _animationTime = 0.0f;
        ResetVisualPose();
    }

    private void PlayOneShot(string animationName)
    {
        if (_visual == null)
        {
            return;
        }

        _oneShotTween?.Kill();
        ResetVisualPose();
        _currentAnimation = animationName;
        _oneShotActive = true;

        switch (animationName)
        {
            case AttackAnimation:
                PlayAttackTween();
                break;
            case HitAnimation:
                PlayHitTween();
                break;
            case DeathAnimation:
                PlayDeathTween();
                break;
            default:
                FinishOneShot();
                break;
        }
    }

    private void PlayAttackTween()
    {
        if (_visual == null)
        {
            return;
        }

        _oneShotTween = CreateTween();
        _oneShotTween.TweenProperty(_visual, "position", _basePosition + new Vector3(0.0f, 0.0f, -0.18f), 0.18f);
        _oneShotTween.Parallel().TweenProperty(_visual, "rotation", _baseRotation + new Vector3(Mathf.DegToRad(-8.0f), 0.0f, 0.0f), 0.18f);
        _oneShotTween.TweenProperty(_visual, "position", _basePosition + new Vector3(0.0f, 0.0f, -0.08f), 0.17f);
        _oneShotTween.Parallel().TweenProperty(_visual, "rotation", _baseRotation + new Vector3(Mathf.DegToRad(-4.0f), 0.0f, 0.0f), 0.17f);
        _oneShotTween.TweenProperty(_visual, "position", _basePosition, 0.2f);
        _oneShotTween.Parallel().TweenProperty(_visual, "rotation", _baseRotation, 0.2f);
        _oneShotTween.TweenCallback(Callable.From(FinishOneShot));
    }

    private void PlayHitTween()
    {
        if (_visual == null)
        {
            return;
        }

        _oneShotTween = CreateTween();
        _oneShotTween.TweenProperty(_visual, "position", _basePosition + new Vector3(0.0f, 0.0f, 0.12f), 0.12f);
        _oneShotTween.Parallel().TweenProperty(_visual, "rotation", _baseRotation + new Vector3(Mathf.DegToRad(8.0f), 0.0f, 0.0f), 0.12f);
        _oneShotTween.TweenProperty(_visual, "position", _basePosition, 0.23f);
        _oneShotTween.Parallel().TweenProperty(_visual, "rotation", _baseRotation, 0.23f);
        _oneShotTween.TweenCallback(Callable.From(FinishOneShot));
    }

    private void PlayDeathTween()
    {
        if (_visual == null)
        {
            return;
        }

        _oneShotTween = CreateTween();
        _oneShotTween.TweenProperty(_visual, "position", _basePosition + new Vector3(0.05f, -0.15f, 0.05f), 0.35f);
        _oneShotTween.Parallel().TweenProperty(_visual, "rotation", _baseRotation + new Vector3(Mathf.DegToRad(10.0f), 0.0f, Mathf.DegToRad(12.0f)), 0.35f);
        _oneShotTween.TweenProperty(_visual, "position", _basePosition + new Vector3(0.12f, -0.35f, 0.08f), 0.55f);
        _oneShotTween.Parallel().TweenProperty(_visual, "rotation", _baseRotation + new Vector3(Mathf.DegToRad(18.0f), 0.0f, Mathf.DegToRad(28.0f)), 0.55f);
    }

    private void ApplyIdlePose()
    {
        if (_visual == null)
        {
            return;
        }

        var phase = Mathf.Tau * (_animationTime / 1.6f);
        var breath = (Mathf.Sin(phase - Mathf.Pi * 0.5f) + 1.0f) * 0.5f;
        _visual.Position = _basePosition + new Vector3(0.0f, breath * 0.025f, 0.0f);
        _visual.Rotation = _baseRotation + new Vector3(Mathf.DegToRad(1.5f) * breath, 0.0f, 0.0f);
    }

    private void ApplyWalkPose()
    {
        if (_visual == null)
        {
            return;
        }

        var phase = Mathf.Tau * (_animationTime / 0.75f);
        var bob = (Mathf.Sin(phase - Mathf.Pi * 0.5f) + 1.0f) * 0.5f;
        _visual.Position = _basePosition + new Vector3(0.0f, bob * 0.035f, 0.0f);
        _visual.Rotation = _baseRotation + new Vector3(0.0f, 0.0f, Mathf.DegToRad(2.0f) * Mathf.Sin(phase));
    }

    private void ApplyStunnedPose()
    {
        if (_visual == null)
        {
            return;
        }

        var phase = Mathf.Tau * (_animationTime / 0.6f);
        _visual.Position = _basePosition + new Vector3(Mathf.Sin(phase * 2.0f) * 0.025f, 0.0f, 0.0f);
        _visual.Rotation = _baseRotation + new Vector3(0.0f, 0.0f, Mathf.DegToRad(1.5f) * Mathf.Sin(phase * 2.0f));
    }

    private void FinishOneShot()
    {
        _oneShotActive = false;
        _currentAnimation = "";
        ResetVisualPose();
    }

    private void ResetVisualPose()
    {
        if (_visual == null)
        {
            return;
        }

        _visual.Position = _basePosition;
        _visual.Rotation = _baseRotation;
    }
}
