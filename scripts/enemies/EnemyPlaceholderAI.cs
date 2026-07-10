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
/// IA simples do inimigo placeholder com NavigationAgent3D opcional e fallback direto.
/// </summary>
public partial class EnemyPlaceholderAI : CharacterBody3D
{
    [Export] public float MoveSpeed { get; set; } = 1.1f;
    [Export] public float ChaseSpeed { get; set; } = 1.55f;
    [Export] public float DetectionRange { get; set; } = 7.0f;
    [Export] public float AttackRange { get; set; } = 1.25f;
    [Export] public float AttackCooldown { get; set; } = 1.9f;
    [Export] public float StunDuration { get; set; } = 1.25f;
    [Export] public int Damage { get; set; } = 15;
    [Export] public float AlertDuration { get; set; } = 1.2f;
    [Export] public float LoseSightGraceTime { get; set; } = 1.5f;
    [Export] public bool StartDormant { get; set; } = true;
    [Export] public bool CanChase { get; set; } = true;
    [Export] public bool PlayAudioOnActivate { get; set; } = true;
    [Export] public NodePath PlayerPath { get; set; } = "../../Player";
    [Export] public NodePath NavigationAgentPath { get; set; } = "NavigationAgent3D";
    [Export] public bool UseTableAvoidance { get; set; } = true;
    [Export] public float TableSideMargin { get; set; } = 0.85f;
    [Export] public float WaypointReachDistance { get; set; } = 0.45f;
    [Export] public float StuckRepathTime { get; set; } = 1.2f;
    [Export] public float AvoidanceDecisionLockTime { get; set; } = 0.8f;
    [Export] public bool DebugTableAvoidance { get; set; } = false;
    [Export] public float NavigationUpdateInterval { get; set; } = 0.2f;
    [Export] public bool DebugNavigation { get; set; } = false;
    [Export] public float StepInterval { get; set; } = 0.55f;
    [Export] public float GroundY { get; set; } = 0.05f;
    [Export] public float FallRecoveryY { get; set; } = -0.5f;
    [Export] public bool LockVerticalMovement { get; set; } = true;

    [Export] public float StuckSpeedThreshold { get; set; } = 0.12f;
    [Export] public float StuckTimeBeforeAvoid { get; set; } = 0.5f;
    [Export] public float AvoidBlend { get; set; } = 0.65f;
    [Export] public float AvoidDuration { get; set; } = 0.8f;

    [Export] public string BreathAudioPath { get; set; } = "res://assets/audio/sfx/enemies/enemy_breath_01.ogg";
    [Export] public string StepAudioPath { get; set; } = "res://assets/audio/sfx/enemies/enemy_step_01.ogg";
    [Export] public string GrowlAudioPath { get; set; } = "res://assets/audio/sfx/enemies/enemy_growl_01.ogg";

    public PlaceholderEnemyState State { get; private set; } = PlaceholderEnemyState.Dormant;
    public bool IsActive { get; private set; }

    private AudioStreamPlayer3D? _breathAudio;
    private AudioStreamPlayer3D? _stepAudio;
    private AudioStreamPlayer3D? _growlAudio;
    private NavigationAgent3D? _navigationAgent;
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
    private float _navigationTargetTimer;
    private float _gravity;
    private Vector3 _lastSafePosition;
    private Vector3 _visualBasePosition;
    private bool _floorRecoveryAttempted;
    private bool _chaseStartedLogged;
    private bool _navigationAgentConfigured;
    private bool _navigationModeLogged;
    private bool _fallbackModeLogged;
    private Tween? _hitFeedbackTween;
    private float _stuckTimer;
    private float _avoidTimer;
    private float _avoidDirection = 1f;
    private Vector3? _avoidanceTarget;
    private float _avoidanceLockTimer;
    private float _avoidanceStuckTimer;
    private Vector3 _avoidanceMoveSamplePosition;
    private Vector3 _lastPlayerPositionForAvoidance;
    private bool _avoidanceRepathLogged;
    private bool _avoidancePreferLeftSide = true;

    // Retangulo da mesa no plano X/Z (alinhado a TableCollision em Z=-0.75).
    private readonly Vector2 _tableMin = new(-1.35f, -1.35f);
    private readonly Vector2 _tableMax = new(1.35f, -0.15f);

    public override void _Ready()
    {
        AddToGroup("enemies");
        GetNodeOrNull<Area3D>("EnemyHurtbox")?.AddToGroup("enemy_hurtbox");
        _breathAudio = GetNodeOrNull<AudioStreamPlayer3D>("Audio/BreathAudio");
        _stepAudio = GetNodeOrNull<AudioStreamPlayer3D>("Audio/StepAudio");
        _growlAudio = GetNodeOrNull<AudioStreamPlayer3D>("Audio/GrowlAudio");
        _navigationAgent = GetNodeOrNull<NavigationAgent3D>(NavigationAgentPath);
        _collisionShape = GetNodeOrNull<CollisionShape3D>("CollisionShape3D");
        _hurtbox = GetNodeOrNull<Area3D>("EnemyHurtbox");
        _hurtboxShape = GetNodeOrNull<CollisionShape3D>("EnemyHurtbox/CollisionShape3D");
        _visual = GetNodeOrNull<Node3D>("Visual");
        _visualBaseScale = _visual?.Scale ?? Vector3.One;
        _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
        _lastSafePosition = GlobalPosition;
        _visualBasePosition = _visual?.Position ?? Vector3.Zero;

        ConfigureAudio();
        ConfigureNavigationAgent();

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
        _stuckTimer = 0.0f;
        _avoidTimer = 0.0f;
        _avoidDirection = 1f;
        _navigationTargetTimer = 0.0f;
        _avoidanceTarget = null;
        _avoidanceLockTimer = 0.0f;
        _avoidanceStuckTimer = 0.0f;
        _avoidanceRepathLogged = false;
        _avoidancePreferLeftSide = true;
        TrySnapToFloorOnce();
        KeepOnGroundPlane();
        _lastSafePosition = GlobalPosition;
        SetVisualVisible(true);
        SetCollisionEnabled(true);
        _player = ResolvePlayer();
        _lastPlayerPositionForAvoidance = _player?.GlobalPosition ?? Vector3.Zero;
        _alertTimer = AlertDuration;
        _loseSightTimer = 0.0f;
        State = PlaceholderEnemyState.Alert;
        Velocity = Vector3.Zero;
        LookAtPlayer();
        SyncNavigationTarget(true);

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

    private void ConfigureNavigationAgent()
    {
        if (_navigationAgent == null)
        {
            LogFallbackModeOnce();
            return;
        }

        _navigationAgent.PathDesiredDistance = 0.45f;
        _navigationAgent.TargetDesiredDistance = 1.25f;
        _navigationAgent.Radius = 0.35f;
        _navigationAgent.Height = 1.8f;
        _navigationAgent.AvoidanceEnabled = false;
        _navigationAgentConfigured = true;
        LogNavigationModeOnce();
    }

    private void SyncNavigationTarget(bool force)
    {
        if (_navigationAgent == null || _player == null)
        {
            return;
        }

        if (!force && _navigationTargetTimer > 0.0f)
        {
            return;
        }

        _navigationAgent.TargetPosition = _player.GlobalPosition;
        _navigationTargetTimer = NavigationUpdateInterval;
    }

    private bool CanUseNavigation()
    {
        if (_navigationAgent == null || !_navigationAgentConfigured)
        {
            return false;
        }

        var map = _navigationAgent.GetNavigationMap();
        return map.IsValid && NavigationServer3D.MapGetIterationId(map) > 0;
    }

    private Vector3 GetChaseDirection(float dt)
    {
        if (TryGetTableAvoidanceDirection(dt, out var avoidanceDirection))
        {
            return avoidanceDirection;
        }

        return GetNavigationOrDirectDirection();
    }

    private Vector3 GetNavigationOrDirectDirection()
    {
        var directDirection = DirectionToPlayer();
        if (!CanUseNavigation() || _player == null)
        {
            LogFallbackModeOnce();
            return ApplyDirectAvoidance(directDirection);
        }

        LogNavigationModeOnce();

        var nextPathPosition = _navigationAgent!.GetNextPathPosition();
        var direction = nextPathPosition - GlobalPosition;
        direction.Y = 0.0f;

        if (direction.LengthSquared() <= 0.04f)
        {
            DebugNav($"fallback curto; distancia player {DistanceToPlayer():0.00}");
            return ApplyDirectAvoidance(directDirection);
        }

        DebugNav($"next={nextPathPosition} finished={_navigationAgent.IsNavigationFinished()} dist={DistanceToPlayer():0.00}");
        return direction.Normalized();
    }

    private bool TryGetTableAvoidanceDirection(float dt, out Vector3 direction)
    {
        direction = Vector3.Zero;

        if (!UseTableAvoidance || _player == null)
        {
            return false;
        }

        var enemyPosition = GlobalPosition;
        var playerPosition = _player.GlobalPosition;

        if (!IsTableBetweenEnemyAndPlayer(enemyPosition, playerPosition))
        {
            if (_avoidanceTarget.HasValue)
            {
                LogTableAvoidance("Voltando a perseguir player.");
            }

            _avoidanceTarget = null;
            _avoidanceStuckTimer = 0.0f;
            return false;
        }

        LogTableAvoidance("Mesa bloqueando caminho direto.");

        _avoidanceLockTimer -= dt;
        UpdateAvoidanceStuckRecovery(dt);

        if (_avoidanceTarget.HasValue)
        {
            var target = _avoidanceTarget.Value;
            var toTarget = target - enemyPosition;
            toTarget.Y = 0.0f;

            if (toTarget.Length() <= WaypointReachDistance)
            {
                LogTableAvoidance($"Waypoint alcançado: {target}");
                _avoidanceTarget = null;
                _avoidanceLockTimer = 0.0f;
                return false;
            }

            direction = toTarget.Normalized();
            if (_avoidTimer > 0.0f)
            {
                direction = BlendWithAvoidDirection(direction);
            }

            return true;
        }

        var playerMovedFar = playerPosition.DistanceSquaredTo(_lastPlayerPositionForAvoidance) > 1.5f;
        var shouldPickTarget = _avoidanceLockTimer <= 0.0f || playerMovedFar || _avoidanceStuckTimer >= StuckRepathTime;
        if (!shouldPickTarget)
        {
            return false;
        }

        var avoidanceTarget = GetTableAvoidanceTarget(enemyPosition, playerPosition);
        _avoidanceTarget = avoidanceTarget;
        _avoidanceLockTimer = AvoidanceDecisionLockTime;
        _avoidanceStuckTimer = 0.0f;
        _avoidanceMoveSamplePosition = enemyPosition;
        _lastPlayerPositionForAvoidance = playerPosition;
        _avoidanceRepathLogged = false;

        LogTableAvoidance($"Waypoint escolhido: {avoidanceTarget}");

        var initialDirection = avoidanceTarget - enemyPosition;
        initialDirection.Y = 0.0f;
        direction = initialDirection.LengthSquared() <= 0.001f ? DirectionToPlayer() : initialDirection.Normalized();
        if (_avoidTimer > 0.0f)
        {
            direction = BlendWithAvoidDirection(direction);
        }

        return true;
    }

    private bool IsTableBetweenEnemyAndPlayer(Vector3 enemyPos, Vector3 playerPos)
    {
        var start = new Vector2(enemyPos.X, enemyPos.Z);
        var end = new Vector2(playerPos.X, playerPos.Z);
        if (SegmentIntersectsRect(start, end, _tableMin, _tableMax))
        {
            return true;
        }

        var origin = enemyPos + Vector3.Up * 0.9f;
        var destination = playerPos + Vector3.Up * 0.9f;
        var space = GetWorld3D().DirectSpaceState;
        var query = PhysicsRayQueryParameters3D.Create(origin, destination);
        query.CollideWithBodies = true;
        query.CollideWithAreas = false;
        query.CollisionMask = uint.MaxValue;
        query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };

        var result = space.IntersectRay(query);
        if (result.Count == 0 || !result.TryGetValue("collider", out var colliderVariant))
        {
            return false;
        }

        return colliderVariant.AsGodotObject() is Node node && node.IsInGroup("enemy_path_blocker");
    }

    private Vector3 GetTableAvoidanceTarget(Vector3 enemyPos, Vector3 playerPos, bool? forceLeft = null)
    {
        var leftSideX = _tableMin.X - TableSideMargin;
        var rightSideX = _tableMax.X + TableSideMargin;
        var targetZ = playerPos.Z;

        if (targetZ >= _tableMin.Y && targetZ <= _tableMax.Y)
        {
            targetZ = enemyPos.Z < _tableMin.Y
                ? _tableMin.Y - 0.55f
                : _tableMax.Y + 0.55f;
        }

        var leftTarget = new Vector3(leftSideX, enemyPos.Y, targetZ);
        var rightTarget = new Vector3(rightSideX, enemyPos.Y, targetZ);
        var leftCost = enemyPos.DistanceTo(leftTarget) + leftTarget.DistanceTo(playerPos);
        var rightCost = enemyPos.DistanceTo(rightTarget) + rightTarget.DistanceTo(playerPos);

        var pickLeft = forceLeft ?? leftCost <= rightCost;
        if (forceLeft == null)
        {
            _avoidancePreferLeftSide = pickLeft;
        }

        return pickLeft ? leftTarget : rightTarget;
    }

    private void UpdateAvoidanceStuckRecovery(float dt)
    {
        if (!_avoidanceTarget.HasValue || _player == null)
        {
            _avoidanceStuckTimer = 0.0f;
            return;
        }

        var moved = new Vector2(
            GlobalPosition.X - _avoidanceMoveSamplePosition.X,
            GlobalPosition.Z - _avoidanceMoveSamplePosition.Z).Length();

        if (moved < 0.05f)
        {
            _avoidanceStuckTimer += dt;
        }
        else
        {
            _avoidanceStuckTimer = 0.0f;
            _avoidanceMoveSamplePosition = GlobalPosition;
        }

        if (_avoidanceStuckTimer < StuckRepathTime)
        {
            return;
        }

        if (!_avoidanceRepathLogged)
        {
            _avoidanceRepathLogged = true;
            GD.Print("EnemyPlaceholder: recalculando contorno da mesa.");
        }

        _avoidancePreferLeftSide = !_avoidancePreferLeftSide;
        _avoidanceTarget = GetTableAvoidanceTarget(GlobalPosition, _player.GlobalPosition, _avoidancePreferLeftSide);
        _avoidanceLockTimer = AvoidanceDecisionLockTime;
        _avoidanceStuckTimer = 0.0f;
        _avoidanceMoveSamplePosition = GlobalPosition;
        _avoidDirection *= -1.0f;
        _avoidTimer = AvoidDuration;

        LogTableAvoidance($"Waypoint alternativo: {_avoidanceTarget.Value}");
    }

    private bool HasAttackLineOfSightToPlayer()
    {
        if (_player == null)
        {
            return false;
        }

        var origin = GlobalPosition + Vector3.Up * 0.9f;
        var destination = _player.GlobalPosition + Vector3.Up * 0.9f;
        var space = GetWorld3D().DirectSpaceState;
        var query = PhysicsRayQueryParameters3D.Create(origin, destination);
        query.CollideWithBodies = true;
        query.CollideWithAreas = false;
        query.CollisionMask = uint.MaxValue;
        query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };

        var result = space.IntersectRay(query);
        if (result.Count == 0 || !result.TryGetValue("collider", out var colliderVariant))
        {
            return true;
        }

        if (colliderVariant.AsGodotObject() is not Node colliderNode)
        {
            return true;
        }

        if (colliderNode.IsInGroup("enemy_path_blocker"))
        {
            return false;
        }

        return IsPlayerCollider(colliderNode);
    }

    private bool IsPlayerCollider(Node node)
    {
        var current = node;
        while (current != null)
        {
            if (current == _player)
            {
                return true;
            }

            current = current.GetParent();
        }

        return false;
    }

    private static bool SegmentIntersectsRect(Vector2 start, Vector2 end, Vector2 rectMin, Vector2 rectMax)
    {
        if (PointInRect(start, rectMin, rectMax) || PointInRect(end, rectMin, rectMax))
        {
            return true;
        }

        var topLeft = new Vector2(rectMin.X, rectMin.Y);
        var topRight = new Vector2(rectMax.X, rectMin.Y);
        var bottomLeft = new Vector2(rectMin.X, rectMax.Y);
        var bottomRight = new Vector2(rectMax.X, rectMax.Y);

        return SegmentsIntersect(start, end, topLeft, topRight)
            || SegmentsIntersect(start, end, topRight, bottomRight)
            || SegmentsIntersect(start, end, bottomRight, bottomLeft)
            || SegmentsIntersect(start, end, bottomLeft, topLeft);
    }

    private static bool PointInRect(Vector2 point, Vector2 rectMin, Vector2 rectMax)
    {
        return point.X >= rectMin.X && point.X <= rectMax.X
            && point.Y >= rectMin.Y && point.Y <= rectMax.Y;
    }

    private static bool SegmentsIntersect(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        var d1 = Direction(b1, b2, a1);
        var d2 = Direction(b1, b2, a2);
        var d3 = Direction(a1, a2, b1);
        var d4 = Direction(a1, a2, b2);

        if (((d1 > 0.0f && d2 < 0.0f) || (d1 < 0.0f && d2 > 0.0f))
            && ((d3 > 0.0f && d4 < 0.0f) || (d3 < 0.0f && d4 > 0.0f)))
        {
            return true;
        }

        return Mathf.IsZeroApprox(d1) && OnSegment(b1, b2, a1)
            || Mathf.IsZeroApprox(d2) && OnSegment(b1, b2, a2)
            || Mathf.IsZeroApprox(d3) && OnSegment(a1, a2, b1)
            || Mathf.IsZeroApprox(d4) && OnSegment(a1, a2, b2);
    }

    private static float Direction(Vector2 a, Vector2 b, Vector2 c)
    {
        return (c.X - a.X) * (b.Y - a.Y) - (c.Y - a.Y) * (b.X - a.X);
    }

    private static bool OnSegment(Vector2 a, Vector2 b, Vector2 c)
    {
        return c.X <= Mathf.Max(a.X, b.X) && c.X >= Mathf.Min(a.X, b.X)
            && c.Y <= Mathf.Max(a.Y, b.Y) && c.Y >= Mathf.Min(a.Y, b.Y);
    }

    private Vector3 ApplyDirectAvoidance(Vector3 directDirection)
    {
        if (_avoidTimer <= 0.0f || directDirection.LengthSquared() <= 0.001f)
        {
            return directDirection;
        }

        return BlendWithAvoidDirection(directDirection);
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

        _avoidTimer = Mathf.Max(0.0f, _avoidTimer - dt);

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

        if (distance <= AttackRange && HasAttackLineOfSightToPlayer())
        {
            State = PlaceholderEnemyState.Attacking;
            Velocity = ApplyGravity(Vector3.Zero, dt);
            MoveAndSlide();
            return;
        }

        _navigationTargetTimer -= dt;
        if (_navigationTargetTimer <= 0.0f)
        {
            SyncNavigationTarget(true);
        }

        var direction = GetChaseDirection(dt);
        var beforeMove = GlobalPosition;
        var horizontalVelocity = direction * ChaseSpeed;
        Velocity = ApplyGravity(horizontalVelocity, dt);
        MoveAndSlide();

        if (!CanUseNavigation() && !_avoidanceTarget.HasValue && direction.LengthSquared() > 0.001f)
        {
            UpdateDirectStuckRecovery(dt, beforeMove);
        }

        PlayStepIfNeeded(dt);
    }

    private void UpdateDirectStuckRecovery(float dt, Vector3 beforeMove)
    {
        var moved = new Vector2(
            GlobalPosition.X - beforeMove.X,
            GlobalPosition.Z - beforeMove.Z).Length();

        if (moved < StuckSpeedThreshold * dt)
        {
            _stuckTimer += dt;
        }
        else
        {
            _stuckTimer = 0.0f;
        }

        if (_stuckTimer >= StuckTimeBeforeAvoid)
        {
            _avoidDirection *= -1.0f;
            _avoidTimer = AvoidDuration;
            _stuckTimer = 0.0f;
        }
    }

    private Vector3 BlendWithAvoidDirection(Vector3 toPlayerDirection)
    {
        if (toPlayerDirection.LengthSquared() <= 0.001f)
        {
            return toPlayerDirection;
        }

        var lateral = new Vector3(-toPlayerDirection.Z, 0.0f, toPlayerDirection.X) * _avoidDirection;
        var blended = (toPlayerDirection * (1.0f - AvoidBlend)) + (lateral.Normalized() * AvoidBlend);
        return blended.LengthSquared() <= 0.001f ? toPlayerDirection : blended.Normalized();
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

        if (!HasAttackLineOfSightToPlayer())
        {
            State = PlaceholderEnemyState.Chasing;
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

        if (!HasAttackLineOfSightToPlayer())
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

    private void LogNavigationModeOnce()
    {
        if (_navigationModeLogged)
        {
            return;
        }

        _navigationModeLogged = true;
        GD.Print("EnemyPlaceholder: usando NavigationAgent3D.");
    }

    private void LogFallbackModeOnce()
    {
        if (_fallbackModeLogged)
        {
            return;
        }

        _fallbackModeLogged = true;
        GD.Print("EnemyPlaceholder: fallback para perseguicao direta.");
    }

    private void DebugNav(string message)
    {
        if (DebugNavigation)
        {
            GD.Print($"EnemyPlaceholder nav: {message}");
        }
    }

    private void LogTableAvoidance(string message)
    {
        if (DebugTableAvoidance)
        {
            GD.Print($"EnemyPlaceholder table: {message}");
        }
    }
}
