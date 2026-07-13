namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;
using BREU.Scripts.Player;

/// <summary>Sprint 24 — ground-floor-only scripted patrol with sight and sprint hearing.</summary>
public partial class EnemyPresenceAI : Node3D
{
    private enum PresenceMode { Dormant, Patrol, Alert, SecondChaseIntro, Chase, Search, SafeRoomWait, Lost }

    [Export] public NodePath EnemyPath { get; set; } = "../../../FirstEnemyChase/Enemy_FirstPresence";
    [Export] public NodePath PlayerPath { get; set; } = "../../../Player";
    [Export] public NodePath PatrolRootPath { get; set; } = "PatrolPoints_FirstFloor";
    [Export] public NodePath SecondChaseRootPath { get; set; } = "SecondChase_Path";
    [Export] public float PatrolSpeed { get; set; } = 1.35f;
    [Export] public float AlertSpeed { get; set; } = 2.35f;
    [Export] public float ChaseSpeed { get; set; } = 3.25f;
    [Export] public float SightDistance { get; set; } = 6.0f;
    [Export] public float CrouchedSightDistance { get; set; } = 3.2f;
    [Export] public float SightHalfAngleDegrees { get; set; } = 28.0f;
    [Export] public float SprintHearingDistance { get; set; } = 8.0f;

    private PensaoPuzzleState? _state;
    private Node3D? _enemy;
    private CharacterBody3D? _player;
    private PlayerController? _playerController;
    private readonly List<Marker3D> _patrolPoints = new();
    private readonly List<Marker3D> _secondChasePoints = new();
    private PresenceMode _mode;
    private int _patrolIndex;
    private float _waitTimer;
    private float _searchTimer;
    private float _detectionCooldown;
    private float _stepTimer;
    private float _lostSightTimer;
    private bool _safeRoomNearStepsPlayed;
    private bool _secondChaseSawLogged;
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

        var secondChaseRoot = GetNodeOrNull<Node3D>(SecondChaseRootPath);
        if (secondChaseRoot != null)
            foreach (var child in secondChaseRoot.GetChildren())
                if (child is Marker3D marker) _secondChasePoints.Add(marker);

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

        if (_state.MakeSecondChaseAvailable())
        {
            HUDController.FindActive(GetTree())?.ShowMessage("Objetivo: Continue até o fundo da pensão.", 5f);
            GD.Print("[SPRINT25] Second chase available");
        }

        if ((_state.PlayerHidden || _state.PlayerInSafeZone) &&
            _state.SecondChaseStarted && !_state.SecondChaseFinished &&
            (_mode == PresenceMode.Alert || _mode == PresenceMode.Chase || _mode == PresenceMode.Search))
        {
            BeginSafeRoomWait();
        }
        else if ((_state.PlayerHidden || _state.PlayerInSafeZone) &&
            (_mode == PresenceMode.Alert || _mode == PresenceMode.Search))
        {
            LoseTarget("[EnemyAI] player hidden, losing target");
        }

        if ((!_state.SecondChaseStarted || _state.SecondChaseFinished) &&
            !_state.PlayerHidden && !_state.PlayerInSafeZone && _detectionCooldown <= 0f)
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
            case PresenceMode.Chase:
                ProcessSecondChase(dt);
                break;
            case PresenceMode.Search:
                ProcessSearch(dt);
                break;
            case PresenceMode.SafeRoomWait:
                ProcessSafeRoomWait(dt);
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

    public bool TryStartSecondChase()
    {
        if (_state?.StartSecondChase() != true || _player == null || _enemy == null) return false;
        _mode = PresenceMode.SecondChaseIntro;
        _enemy.Visible = true;
        FaceToward(_player.GlobalPosition);
        PensionAudioManager.Find(GetTree())?.PlayOneShot("old_house_settle_01", -16f);
        HUDController.FindActive(GetTree())?.ShowMessage("Ele ouviu você.", 2.2f);
        GD.Print("[SPRINT25] Second chase started");
        GD.Print("[SPRINT25] Enemy heard player");
        _ = StartSecondChaseAfterDelayAsync();
        _ = FlickerCorridorAsync();
        return true;
    }

    private async System.Threading.Tasks.Task StartSecondChaseAfterDelayAsync()
    {
        await ToSignal(GetTree().CreateTimer(0.75f), SceneTreeTimer.SignalName.Timeout);
        if (!IsInsideTree() || _state?.SecondChaseStarted != true || _state.SecondChaseFinished || _player == null) return;
        _state.MarkSecondChaseTutorialShown();
        _lastKnownRoutePosition = ProjectToCentralRoute(_player.GlobalPosition);
        _lostSightTimer = 1.4f;
        _mode = PresenceMode.Chase;
        HUDController.FindActive(GetTree())?.ShowMessage("Corra. Se esconda.", 3.5f);
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

    private void ProcessSecondChase(float dt)
    {
        var canSee = CanSeePlayer();
        var canHear = CanHearPlayerRunning();
        if (canSee || canHear)
        {
            _lastKnownRoutePosition = ProjectToCentralRoute(_player!.GlobalPosition);
            _lostSightTimer = 1.4f;
            if (canSee && !_secondChaseSawLogged)
            {
                _secondChaseSawLogged = true;
                GD.Print("[SPRINT25] Enemy saw player");
            }
        }
        else
        {
            _lostSightTimer -= dt;
        }

        if (_player!.GlobalPosition.Y <= 1.7f &&
            _enemy!.GlobalPosition.DistanceTo(_player.GlobalPosition) < 1.15f)
        {
            HUDController.FindActive(GetTree())?.ShowMessage("Ela está perto demais.", 2.5f);
            PensionAudioManager.Find(GetTree())?.PlayOneShot("distant_knock_02", -15f);
            _mode = PresenceMode.Search;
            _searchTimer = 1.6f;
            _detectionCooldown = 1.5f;
            _state!.BeginEnemySearch();
            return;
        }

        if (_lostSightTimer <= 0f || MoveEnemyToward(_lastKnownRoutePosition, ChaseSpeed, dt))
        {
            _mode = PresenceMode.Search;
            _searchTimer = 2.4f;
            _state!.BeginEnemySearch();
        }
    }

    private void ProcessSearch(float dt)
    {
        if (_state?.SecondChaseStarted == true && !_state.SecondChaseFinished &&
            (CanSeePlayer() || CanHearPlayerRunning()))
        {
            _lastKnownRoutePosition = ProjectToCentralRoute(_player!.GlobalPosition);
            _lostSightTimer = 1.4f;
            _mode = PresenceMode.Chase;
            return;
        }

        _searchTimer -= dt;
        FaceToward(_lastKnownRoutePosition);
        if (_searchTimer > 0f) return;

        if (_state?.SecondChaseStarted == true && !_state.SecondChaseFinished)
        {
            _lastKnownRoutePosition = _secondChasePoints.Count > 1
                ? _secondChasePoints[1].GlobalPosition
                : ProjectToCentralRoute(_enemy!.GlobalPosition);
            _lostSightTimer = 1.4f;
            _mode = PresenceMode.Chase;
            return;
        }

        LoseTarget("[EnemyAI] returning to patrol");
    }

    private void BeginSafeRoomWait()
    {
        if (_state == null || _mode == PresenceMode.SafeRoomWait) return;
        _mode = PresenceMode.SafeRoomWait;
        _waitTimer = 4.0f;
        _safeRoomNearStepsPlayed = false;
        _lastKnownRoutePosition = ProjectToCentralRoute(_player!.GlobalPosition);
        _state.BeginSecondChaseShelterSearch();
        HUDController.FindActive(GetTree())?.ShowMessage("Não respire.", 3f);
        PensionAudioManager.Find(GetTree())?.PlayOneShot("distant_step_04", -16f);
        GD.Print("[SPRINT25] Player entered safe zone");
    }

    private void ProcessSafeRoomWait(float dt)
    {
        if (_state == null) return;
        if (!_state.PlayerInSafeZone && !_state.PlayerHidden)
        {
            _state.ResumeSecondChase();
            _lastKnownRoutePosition = ProjectToCentralRoute(_player!.GlobalPosition);
            _lostSightTimer = 1.4f;
            _mode = PresenceMode.Chase;
            HUDController.FindActive(GetTree())?.ShowMessage("Ela ainda está perto demais.", 2.5f);
            return;
        }

        MoveEnemyToward(_lastKnownRoutePosition, AlertSpeed, dt);
        _waitTimer -= dt;
        if (!_safeRoomNearStepsPlayed && _waitTimer <= 2.4f)
        {
            _safeRoomNearStepsPlayed = true;
            PensionAudioManager.Find(GetTree())?.PlayOneShot("distant_step_03", -17f);
            HUDController.FindActive(GetTree())?.ShowMessage("Fique quieto.", 2.4f);
        }

        if (_waitTimer <= 0f) FinishSecondChase();
    }

    private void FinishSecondChase()
    {
        if (_state?.FinishSecondChase() != true) return;
        _mode = PresenceMode.Lost;
        _waitTimer = 3f;
        _detectionCooldown = 6f;
        PensionAudioManager.Find(GetTree())?.PlayOneShot("distant_step_01", -19f);
        HUDController.FindActive(GetTree())?.ShowMessage("Os passos se afastaram.", 3f);
        GD.Print("[SPRINT25] Enemy lost player");
        GD.Print("[SPRINT25] Second chase finished");
        _ = ShowSecondChaseObjectiveAsync();
    }

    private async System.Threading.Tasks.Task ShowSecondChaseObjectiveAsync()
    {
        await ToSignal(GetTree().CreateTimer(3.1f), SceneTreeTimer.SignalName.Timeout);
        if (!IsInsideTree()) return;
        HUDController.FindActive(GetTree())?.ShowMessage(
            "Continue explorando com cuidado. Objetivo: Procure outra saída da pensão.", 6f);
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

    private async System.Threading.Tasks.Task FlickerCorridorAsync()
    {
        var light = GetTree().CurrentScene?.GetNodeOrNull<OmniLight3D>("Lighting/CorridorDeepLight");
        if (light == null) return;
        var energy = light.LightEnergy;
        light.LightEnergy = energy * 0.18f;
        await ToSignal(GetTree().CreateTimer(0.38f), SceneTreeTimer.SignalName.Timeout);
        if (GodotObject.IsInstanceValid(light)) light.LightEnergy = energy;
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
