namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;
using BREU.Scripts.Player;

/// <summary>Sprint 24 — ground-floor-only scripted patrol with sight and sprint hearing.</summary>
public partial class EnemyPresenceAI : Node3D
{
    private enum PresenceMode { Dormant, Patrol, Alert, Search, Lost }

    [Export] public NodePath EnemyPath { get; set; } = "../../../FirstEnemyChase/Enemy_FirstPresence";
    [Export] public NodePath PlayerPath { get; set; } = "../../../Player";
    [Export] public NodePath PatrolRootPath { get; set; } = "PatrolPoints_FirstFloor";
    [Export] public float PatrolSpeed { get; set; } = 1.35f;
    [Export] public float AlertSpeed { get; set; } = 2.35f;
    [Export] public float SightDistance { get; set; } = 6.0f;
    [Export] public float CrouchedSightDistance { get; set; } = 3.2f;
    [Export] public float SightHalfAngleDegrees { get; set; } = 28.0f;
    [Export] public float SprintHearingDistance { get; set; } = 8.0f;

    private PensaoPuzzleState? _state;
    private Node3D? _enemy;
    private CharacterBody3D? _player;
    private PlayerController? _playerController;
    private readonly List<Marker3D> _patrolPoints = new();
    private PresenceMode _mode;
    private int _patrolIndex;
    private float _waitTimer;
    private float _searchTimer;
    private float _detectionCooldown;
    private float _stepTimer;
    private Vector3 _lastKnownRoutePosition;

    public override void _Ready()
    {
        _state = GetTree().CurrentScene?.GetNodeOrNull<PensaoPuzzleState>("PuzzleState");
        _enemy = GetNodeOrNull<Node3D>(EnemyPath);
        _player = GetNodeOrNull<CharacterBody3D>(PlayerPath);
        _playerController = _player as PlayerController;

        var patrolRoot = GetNodeOrNull<Node3D>(PatrolRootPath);
        if (patrolRoot != null)
            foreach (var child in patrolRoot.GetChildren())
                if (child is Marker3D marker) _patrolPoints.Add(marker);

        if (_enemy == null || _player == null || _patrolPoints.Count < 2)
            GD.PushError("[EnemyAI] Missing enemy, player, or patrol points.");
    }

    public override void _Process(double delta)
    {
        if (_state == null || _enemy == null || _player == null || _patrolPoints.Count < 2) return;
        if (!_state.Sprint23Completed) return;

        var dt = (float)delta;
        _detectionCooldown = Mathf.Max(0f, _detectionCooldown - dt);
        _stepTimer = Mathf.Max(0f, _stepTimer - dt);

        if (_mode == PresenceMode.Dormant) StartPatrol();

        if ((_state.PlayerHidden || _state.PlayerInSafeZone) &&
            (_mode == PresenceMode.Alert || _mode == PresenceMode.Search))
        {
            LoseTarget("[EnemyAI] player hidden, losing target");
        }

        if (!_state.PlayerHidden && !_state.PlayerInSafeZone && _detectionCooldown <= 0f)
        {
            if (CanSeePlayer()) AlertToPlayer(true);
            else if (CanHearPlayerRunning()) AlertToPlayer(false);
        }

        switch (_mode)
        {
            case PresenceMode.Patrol:
                ProcessPatrol(dt);
                break;
            case PresenceMode.Alert:
                ProcessAlert(dt);
                break;
            case PresenceMode.Search:
                ProcessSearch(dt);
                break;
            case PresenceMode.Lost:
                _waitTimer -= dt;
                if (_waitTimer <= 0f) StartPatrol();
                break;
        }
    }

    private void StartPatrol()
    {
        _mode = PresenceMode.Patrol;
        _state!.ActivateEnemyPatrol();
        _enemy!.Visible = true;
        _patrolIndex = FindNearestPatrolIndex(_enemy.GlobalPosition);
        _waitTimer = 0.8f;
        GD.Print("[EnemyAI] patrol active");
    }

    private void ProcessPatrol(float dt)
    {
        if (_waitTimer > 0f)
        {
            _waitTimer -= dt;
            return;
        }

        var target = _patrolPoints[_patrolIndex].GlobalPosition;
        if (MoveEnemyToward(target, PatrolSpeed, dt))
        {
            GD.Print($"[EnemyAI] reached patrol point {_patrolPoints[_patrolIndex].Name}");
            _patrolIndex = (_patrolIndex + 1) % _patrolPoints.Count;
            _waitTimer = 1.2f + (_patrolIndex % 2) * 0.45f;
        }
    }

    private void AlertToPlayer(bool bySight)
    {
        _lastKnownRoutePosition = ProjectToCentralRoute(_player!.GlobalPosition);
        if (_mode == PresenceMode.Alert)
        {
            _state!.AlertEnemy(bySight);
            return;
        }

        _mode = PresenceMode.Alert;
        var wasAlreadyDetectedThisWay = bySight ? _state!.EnemySawPlayerOnce : _state!.EnemyHeardPlayerOnce;
        _state!.AlertEnemy(bySight);
        PensionAudioManager.Find(GetTree())?.PlayOneShot("old_house_settle_01", -18f);
        if (bySight)
        {
            if (!wasAlreadyDetectedThisWay)
                HUDController.FindActive(GetTree())?.ShowMessage("Ele viu você.", 2.5f);
            GD.Print("[EnemyAI] saw player");
        }
        else
        {
            if (!wasAlreadyDetectedThisWay)
                HUDController.FindActive(GetTree())?.ShowMessage("Ele ouviu você.", 2.5f);
            GD.Print("[EnemyAI] heard player running");
        }
    }

    private void ProcessAlert(float dt)
    {
        if (_state!.PlayerHidden || _state.PlayerInSafeZone) return;
        if (CanSeePlayer() || CanHearPlayerRunning())
            _lastKnownRoutePosition = ProjectToCentralRoute(_player!.GlobalPosition);

        if (_player!.GlobalPosition.Y <= 1.7f &&
            _enemy!.GlobalPosition.DistanceTo(_player.GlobalPosition) < 1.15f)
        {
            HUDController.FindActive(GetTree())?.ShowMessage("Você escapou por pouco.", 3f);
            PensionAudioManager.Find(GetTree())?.PlayOneShot("distant_knock_02", -17f);
            _detectionCooldown = 5f;
            LoseTarget("[EnemyAI] close encounter reset");
            return;
        }

        if (MoveEnemyToward(_lastKnownRoutePosition, AlertSpeed, dt))
        {
            _mode = PresenceMode.Search;
            _searchTimer = 3.2f;
            _state.BeginEnemySearch();
            GD.Print("[EnemyAI] searching last known position");
        }
    }

    private void ProcessSearch(float dt)
    {
        _searchTimer -= dt;
        FaceToward(_lastKnownRoutePosition);
        if (_searchTimer <= 0f) LoseTarget("[EnemyAI] returning to patrol");
    }

    private void LoseTarget(string log)
    {
        if (_mode == PresenceMode.Lost) return;
        _mode = PresenceMode.Lost;
        _waitTimer = 2.2f;
        _detectionCooldown = Mathf.Max(_detectionCooldown, 3f);
        _state!.MarkEnemyLost();
        PensionAudioManager.Find(GetTree())?.PlayOneShot("distant_step_03", -19f);
        HUDController.FindActive(GetTree())?.ShowMessage("Ele perdeu você. Os passos se afastaram.", 3.5f);
        GD.Print(log);
    }

    private bool CanSeePlayer()
    {
        if (_player == null || _enemy == null || _state == null ||
            _state.PlayerHidden || _state.PlayerInSafeZone || _player.GlobalPosition.Y > 1.7f) return false;

        var sightRange = _playerController?.IsCrouching == true ? CrouchedSightDistance : SightDistance;
        var toPlayer = _player.GlobalPosition - _enemy.GlobalPosition;
        toPlayer.Y = 0f;
        var distance = toPlayer.Length();
        if (distance < 0.05f || distance > sightRange) return false;

        var forward = -_enemy.GlobalTransform.Basis.Z;
        forward.Y = 0f;
        forward = forward.Normalized();
        var minimumDot = Mathf.Cos(Mathf.DegToRad(SightHalfAngleDegrees));
        if (forward.Dot(toPlayer / distance) < minimumDot) return false;

        var from = _enemy.GlobalPosition + Vector3.Up * 1.35f;
        var to = _player.GlobalPosition + Vector3.Up * 1.05f;
        var query = PhysicsRayQueryParameters3D.Create(from, to);
        query.CollisionMask = 1;
        query.CollideWithAreas = false;
        query.CollideWithBodies = true;
        return _enemy.GetWorld3D().DirectSpaceState.IntersectRay(query).Count == 0;
    }

    private bool CanHearPlayerRunning()
    {
        if (_player == null || _enemy == null || _state == null || _playerController == null ||
            _state.PlayerHidden || _state.PlayerInSafeZone || _player.GlobalPosition.Y > 1.7f ||
            !_playerController.IsSprinting || !_playerController.IsMovingHorizontally ||
            _playerController.IsCrouching) return false;
        return _enemy.GlobalPosition.DistanceTo(_player.GlobalPosition) <= SprintHearingDistance;
    }

    private bool MoveEnemyToward(Vector3 target, float speed, float dt)
    {
        target.Y = 0f;
        FaceToward(target);
        _enemy!.GlobalPosition = _enemy.GlobalPosition.MoveToward(target, speed * dt);
        if (_stepTimer <= 0f)
        {
            PensionAudioManager.Find(GetTree())?.PlayOneShot("distant_step_02", -22f);
            _stepTimer = speed > PatrolSpeed ? 0.65f : 0.95f;
        }
        return _enemy.GlobalPosition.DistanceTo(target) < 0.08f;
    }

    private void FaceToward(Vector3 target)
    {
        if (_enemy == null) return;
        target.Y = _enemy.GlobalPosition.Y;
        if (_enemy.GlobalPosition.DistanceSquaredTo(target) > 0.001f)
            _enemy.LookAt(target, Vector3.Up);
    }

    private int FindNearestPatrolIndex(Vector3 position)
    {
        var bestIndex = 0;
        var bestDistance = float.MaxValue;
        for (var i = 0; i < _patrolPoints.Count; i++)
        {
            var distance = position.DistanceSquaredTo(_patrolPoints[i].GlobalPosition);
            if (distance >= bestDistance) continue;
            bestDistance = distance;
            bestIndex = i;
        }
        return bestIndex;
    }

    private static Vector3 ProjectToCentralRoute(Vector3 playerPosition) =>
        new(0f, 0f, Mathf.Clamp(playerPosition.Z, -23.5f, -4.5f));
}
