namespace BREU.Scripts.Enemies;

public enum PlaceholderEnemyState
{
    Dormant,
    Idle,
    Alert,
    Chasing,
    Attacking,
    Stunned
}

/// <summary>
/// IA simples do inimigo placeholder. Sem pathfinding avancado e sem animacao final.
/// </summary>
public partial class EnemyPlaceholderAI : CharacterBody3D
{
    [Export] public float MoveSpeed { get; set; } = 1.1f;
    [Export] public float ChaseSpeed { get; set; } = 1.65f;
    [Export] public float DetectionRange { get; set; } = 7.0f;
    [Export] public float AttackRange { get; set; } = 1.25f;
    [Export] public float AttackCooldown { get; set; } = 2.0f;
    [Export] public float StunDuration { get; set; } = 1.25f;
    [Export] public int Damage { get; set; } = 10;
    [Export] public float AlertDuration { get; set; } = 1.2f;
    [Export] public float LoseSightGraceTime { get; set; } = 1.5f;
    [Export] public bool StartDormant { get; set; } = true;
    [Export] public bool CanChase { get; set; } = true;
    [Export] public bool PlayAudioOnActivate { get; set; } = true;
    [Export] public NodePath PlayerPath { get; set; } = "../../Player";
    [Export] public float StepInterval { get; set; } = 0.55f;
    [Export] public float GroundY { get; set; } = 0.05f;
    [Export] public float FallRecoveryY { get; set; } = -0.5f;
    [Export] public bool LockVerticalMovement { get; set; } = true;

    [Export] public string BreathAudioPath { get; set; } = "res://assets/audio/sfx/enemies/enemy_breath_01.ogg";
    [Export] public string StepAudioPath { get; set; } = "res://assets/audio/sfx/enemies/enemy_step_01.ogg";
    [Export] public string GrowlAudioPath { get; set; } = "res://assets/audio/sfx/enemies/enemy_growl_01.ogg";

    public PlaceholderEnemyState State { get; private set; } = PlaceholderEnemyState.Dormant;
    public bool IsActive { get; private set; }

    private AudioStreamPlayer3D? _breathAudio;
    private AudioStreamPlayer3D? _stepAudio;
    private AudioStreamPlayer3D? _growlAudio;
    private CollisionShape3D? _collisionShape;
    private Area3D? _hurtbox;
    private CollisionShape3D? _hurtboxShape;
    private Node3D? _visual;
    private Vector3 _visualBaseScale = Vector3.One;
    private Node3D? _player;
    private float _attackTimer;
    private float _stepTimer;
    private float _alertTimer;
    private float _stunTimer;
    private float _loseSightTimer;
    private float _gravity;
    private Vector3 _lastSafePosition;
    private Vector3 _visualBasePosition;
    private bool _floorRecoveryAttempted;
    private bool _chaseStartedLogged;
    private Tween? _hitFeedbackTween;

    public override void _Ready()
    {
        AddToGroup("enemies");
        GetNodeOrNull<Area3D>("EnemyHurtbox")?.AddToGroup("enemy_hurtbox");
        _breathAudio = GetNodeOrNull<AudioStreamPlayer3D>("Audio/BreathAudio");
        _stepAudio = GetNodeOrNull<AudioStreamPlayer3D>("Audio/StepAudio");
        _growlAudio = GetNodeOrNull<AudioStreamPlayer3D>("Audio/GrowlAudio");
        _collisionShape = GetNodeOrNull<CollisionShape3D>("CollisionShape3D");
        _hurtbox = GetNodeOrNull<Area3D>("EnemyHurtbox");
        _hurtboxShape = GetNodeOrNull<CollisionShape3D>("EnemyHurtbox/CollisionShape3D");
        _visual = GetNodeOrNull<Node3D>("Visual");
        _visualBaseScale = _visual?.Scale ?? Vector3.One;
        _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
        _lastSafePosition = GlobalPosition;
        _visualBasePosition = _visual?.Position ?? Vector3.Zero;

        ConfigureAudio();

        if (StartDormant)
        {
            Deactivate();
        }
        else
        {
            State = PlaceholderEnemyState.Idle;
            IsActive = true;
            Visible = true;
            SetCollisionEnabled(true);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        var dt = (float)delta;
        _attackTimer = Mathf.Max(0.0f, _attackTimer - dt);

        if (!IsActive || State == PlaceholderEnemyState.Dormant)
        {
            Velocity = Vector3.Zero;
            return;
        }

        KeepOnGroundPlane();
        _player ??= ResolvePlayer();
        if (_player == null)
        {
            Velocity = ApplyGravity(Vector3.Zero, dt);
            MoveAndSlide();
            KeepOnGroundPlane();
            return;
        }

        LookAtPlayer();
        TrackSafePosition();

        if (IsPlayerDead())
        {
            StopBecausePlayerDied();
            return;
        }

        matchState(dt);
        KeepOnGroundPlane();
    }

    public void Activate()
    {
        ActivateEnemy();
    }

    public void ActivateEnemy()
    {
        IsActive = true;
        Visible = true;
        _floorRecoveryAttempted = false;
        _chaseStartedLogged = false;
        TrySnapToFloorOnce();
        KeepOnGroundPlane();
        _lastSafePosition = GlobalPosition;
        SetVisualVisible(true);
        SetCollisionEnabled(true);
        _player = ResolvePlayer();
        _alertTimer = AlertDuration;
        _loseSightTimer = 0.0f;
        State = PlaceholderEnemyState.Alert;
        Velocity = Vector3.Zero;
        LookAtPlayer();

        if (PlayAudioOnActivate)
        {
            _growlAudio?.Play();
        }

        StartBreathing();
        GD.Print("EnemyPlaceholder ativado.");
    }

    public void Deactivate()
    {
        State = PlaceholderEnemyState.Dormant;
        IsActive = false;
        Visible = false;
        SetVisualVisible(false);
        Velocity = Vector3.Zero;
        StopBreathing();
        SetCollisionEnabled(false);
    }

    public void LookAtPlayer()
    {
        var player = _player ?? ResolvePlayer();
        if (player == null)
        {
            return;
        }

        var target = player.GlobalPosition;
        target.Y = GlobalPosition.Y;

        if (GlobalPosition.DistanceSquaredTo(target) > 0.01f)
        {
            LookAt(target, Vector3.Up, true);
        }
    }

    public void ApplyStun(float duration)
    {
        if (!IsActive)
        {
            return;
        }

        State = PlaceholderEnemyState.Stunned;
        _stunTimer = Mathf.Max(0.1f, duration);
        Velocity = Vector3.Zero;
        _growlAudio?.Play();
        GD.Print($"EnemyPlaceholder stunned por {_stunTimer:0.00}s");
    }

    public void ReceiveHit(int damage)
    {
        GD.Print($"EnemyPlaceholder recebeu hit: {damage}");
        PlayHitFeedback();
        ApplyStun(StunDuration);
    }

    private void PlayHitFeedback()
    {
        if (_visual == null)
        {
            return;
        }

        _hitFeedbackTween?.Kill();
        _visual.Position = _visualBasePosition;
        _visual.Scale = _visualBaseScale;

        _hitFeedbackTween = CreateTween();
        _hitFeedbackTween.TweenProperty(_visual, "scale", _visualBaseScale * 1.06f, 0.05f);
        _hitFeedbackTween.Parallel().TweenProperty(_visual, "position", _visualBasePosition + new Vector3(0.0f, 0.0f, 0.12f), 0.08f);
        _hitFeedbackTween.TweenProperty(_visual, "scale", _visualBaseScale, 0.1f);
        _hitFeedbackTween.Parallel().TweenProperty(_visual, "position", _visualBasePosition, 0.12f);
    }

    private void matchState(float dt)
    {
        switch (State)
        {
            case PlaceholderEnemyState.Idle:
                ProcessIdle(dt);
                break;
            case PlaceholderEnemyState.Alert:
                ProcessAlert(dt);
                break;
            case PlaceholderEnemyState.Chasing:
                ProcessChasing(dt);
                break;
            case PlaceholderEnemyState.Attacking:
                ProcessAttacking(dt);
                break;
            case PlaceholderEnemyState.Stunned:
                ProcessStunned(dt);
                break;
        }
    }

    private void ProcessIdle(float dt)
    {
        Velocity = ApplyGravity(Vector3.Zero, dt);
        MoveAndSlide();

        if (DistanceToPlayer() <= DetectionRange)
        {
            State = PlaceholderEnemyState.Alert;
            _alertTimer = AlertDuration;
        }
    }

    private void ProcessAlert(float dt)
    {
        Velocity = ApplyGravity(Vector3.Zero, dt);
        MoveAndSlide();
        _alertTimer -= dt;

        if (_alertTimer <= 0.0f)
        {
            State = CanChase ? PlaceholderEnemyState.Chasing : PlaceholderEnemyState.Idle;
            if (State == PlaceholderEnemyState.Chasing && !_chaseStartedLogged)
            {
                _chaseStartedLogged = true;
                GD.Print("EnemyPlaceholder iniciou perseguicao.");
            }
        }
    }

    private void ProcessChasing(float dt)
    {
        if (!CanChase)
        {
            Velocity = ApplyGravity(Vector3.Zero, dt);
            MoveAndSlide();
            return;
        }

        var distance = DistanceToPlayer();
        if (distance > DetectionRange)
        {
            _loseSightTimer += dt;
            if (_loseSightTimer >= LoseSightGraceTime)
            {
                State = PlaceholderEnemyState.Idle;
                _loseSightTimer = 0.0f;
            }

            Velocity = ApplyGravity(Vector3.Zero, dt);
            MoveAndSlide();
            return;
        }

        _loseSightTimer = 0.0f;

        if (distance <= AttackRange)
        {
            State = PlaceholderEnemyState.Attacking;
            Velocity = ApplyGravity(Vector3.Zero, dt);
            MoveAndSlide();
            return;
        }

        var direction = DirectionToPlayer();
        var horizontalVelocity = direction * ChaseSpeed;
        Velocity = ApplyGravity(horizontalVelocity, dt);
        MoveAndSlide();
        PlayStepIfNeeded(dt);
    }

    private void ProcessAttacking(float dt)
    {
        Velocity = ApplyGravity(Vector3.Zero, dt);
        MoveAndSlide();

        if (DistanceToPlayer() > AttackRange * 1.25f)
        {
            State = PlaceholderEnemyState.Chasing;
            return;
        }

        if (_attackTimer > 0.0f)
        {
            return;
        }

        _attackTimer = AttackCooldown;
        _growlAudio?.Play();
        TryDamagePlayer();
    }

    private void ProcessStunned(float dt)
    {
        Velocity = ApplyGravity(Vector3.Zero, dt);
        MoveAndSlide();
        _stunTimer -= dt;

        if (_stunTimer <= 0.0f)
        {
            State = PlaceholderEnemyState.Chasing;
            if (!_chaseStartedLogged)
            {
                _chaseStartedLogged = true;
                GD.Print("EnemyPlaceholder iniciou perseguicao.");
            }
        }
    }

    private Vector3 DirectionToPlayer()
    {
        if (_player == null)
        {
            return Vector3.Zero;
        }

        var direction = _player.GlobalPosition - GlobalPosition;
        direction.Y = 0.0f;
        return direction.LengthSquared() <= 0.001f ? Vector3.Zero : direction.Normalized();
    }

    private float DistanceToPlayer()
    {
        if (_player == null)
        {
            return float.MaxValue;
        }

        var offset = _player.GlobalPosition - GlobalPosition;
        offset.Y = 0.0f;
        return offset.Length();
    }

    private Vector3 ApplyGravity(Vector3 horizontalVelocity, float dt)
    {
        if (LockVerticalMovement)
        {
            horizontalVelocity.Y = 0.0f;
            return horizontalVelocity;
        }

        if (!IsOnFloor())
        {
            horizontalVelocity.Y = Velocity.Y - _gravity * dt;
        }
        else
        {
            horizontalVelocity.Y = -0.05f;
        }

        return horizontalVelocity;
    }

    private void TryDamagePlayer()
    {
        if (_player == null || DistanceToPlayer() > AttackRange + 0.25f)
        {
            return;
        }

        if (IsPlayerDead())
        {
            StopBecausePlayerDied();
            return;
        }

        if (GlobalPosition.Y < GroundY - 0.35f || Mathf.Abs(_player.GlobalPosition.Y - GlobalPosition.Y) > 1.6f)
        {
            return;
        }

        if (_player.GetNodeOrNull<PlayerHealth>("PlayerHealth") is { } health)
        {
            health.TakeDamage(Damage);
            GD.Print("EnemyPlaceholder atacou player.");
            return;
        }
    }

    private bool IsPlayerDead()
    {
        return _player?.GetNodeOrNull<PlayerHealth>("PlayerHealth")?.IsDead == true;
    }

    private void StopBecausePlayerDied()
    {
        State = PlaceholderEnemyState.Idle;
        Velocity = Vector3.Zero;
        StopBreathing();
    }

    private void PlayStepIfNeeded(float dt)
    {
        _stepTimer -= dt;
        if (_stepTimer > 0.0f)
        {
            return;
        }

        _stepTimer = StepInterval;
        _stepAudio?.Play();
    }

    private void ConfigureAudio()
    {
        if (_breathAudio != null)
        {
            _breathAudio.Stream = AudioResourceLoader.TryLoad(BreathAudioPath, true);
            _breathAudio.VolumeDb = -11.0f;
        }

        if (_stepAudio != null)
        {
            _stepAudio.Stream = AudioResourceLoader.TryLoad(StepAudioPath);
            _stepAudio.VolumeDb = -9.0f;
        }

        if (_growlAudio != null)
        {
            _growlAudio.Stream = AudioResourceLoader.TryLoad(GrowlAudioPath);
            _growlAudio.VolumeDb = -7.0f;
        }
    }

    private void StartBreathing()
    {
        if (_breathAudio?.Stream != null && !_breathAudio.Playing)
        {
            _breathAudio.Play();
        }
    }

    private void StopBreathing()
    {
        if (_breathAudio != null)
        {
            _breathAudio.Stop();
        }
    }

    private void SetCollisionEnabled(bool enabled)
    {
        if (_collisionShape != null)
        {
            _collisionShape.Disabled = !enabled;
        }

        if (_hurtbox != null)
        {
            _hurtbox.Monitoring = enabled;
            _hurtbox.Monitorable = enabled;
        }

        if (_hurtboxShape != null)
        {
            _hurtboxShape.Disabled = !enabled;
        }
    }

    private void SetVisualVisible(bool visible)
    {
        if (_visual != null)
        {
            _visual.Visible = visible;
        }
    }

    private void TrySnapToFloorOnce()
    {
        if (_floorRecoveryAttempted)
        {
            return;
        }

        _floorRecoveryAttempted = true;

        if (GlobalPosition.Y < GroundY)
        {
            GlobalPosition = new Vector3(GlobalPosition.X, GroundY, GlobalPosition.Z);
        }

        Velocity = Vector3.Zero;
    }

    private void KeepOnGroundPlane()
    {
        if (!LockVerticalMovement)
        {
            return;
        }

        if (!Mathf.IsEqualApprox(GlobalPosition.Y, GroundY))
        {
            GlobalPosition = new Vector3(GlobalPosition.X, GroundY, GlobalPosition.Z);
        }

        Velocity = new Vector3(Velocity.X, 0.0f, Velocity.Z);
    }

    private void TrackSafePosition()
    {
        if ((IsOnFloor() || LockVerticalMovement) && GlobalPosition.Y >= GroundY - 0.2f)
        {
            _lastSafePosition = GlobalPosition;
        }
    }

    private Node3D? ResolvePlayer()
    {
        if (GetNodeOrNull(PlayerPath) is Node3D playerFromPath)
        {
            return playerFromPath;
        }

        return GetTree().GetFirstNodeInGroup("player") as Node3D;
    }
}
