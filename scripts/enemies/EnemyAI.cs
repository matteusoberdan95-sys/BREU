namespace BREU.Scripts.Enemies;

public enum EnemyState
{
    Idle,
    Patrol,
    Investigate,
    Chase,
    Attack,
    Stunned,
    Dead
}

public partial class EnemyAI : CharacterBody3D
{
    [Export] public float MoveSpeed { get; set; } = 2.4f;
    [Export] public float DetectionRadius { get; set; } = 8.0f;
    [Export] public float AttackRange { get; set; } = 1.35f;
    [Export] public float AttackCooldownSeconds { get; set; } = 1.2f;
    [Export] public float AttackDamage { get; set; } = 12.0f;
    [Export] public float Gravity { get; set; } = 18.0f;

    public EnemyState State { get; private set; } = EnemyState.Idle;

    private Node3D? _player;
    private EnemyHealth? _health;
    private double _attackCooldown;
    private double _stunRemaining;
    private Vector3 _knockback;

    public override void _Ready()
    {
        AddToGroup("enemy");
        _player = GetTree().GetFirstNodeInGroup("player") as Node3D;
        _health = GetNodeOrNull<EnemyHealth>("EnemyHealth");
        if (_health != null)
        {
            _health.Damaged += OnDamaged;
            _health.Died += OnDied;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (State == EnemyState.Dead)
        {
            Velocity = Vector3.Zero;
            return;
        }

        _player ??= GetTree().GetFirstNodeInGroup("player") as Node3D;
        _attackCooldown = Mathf.Max(0.0, _attackCooldown - delta);

        if (_stunRemaining > 0.0)
        {
            _stunRemaining -= delta;
            State = EnemyState.Stunned;
            MoveWithGravity(delta, _knockback);
            _knockback = _knockback.Lerp(Vector3.Zero, 8.0f * (float)delta);
            return;
        }

        if (_player == null)
        {
            State = EnemyState.Idle;
            MoveWithGravity(delta, Vector3.Zero);
            return;
        }

        var toPlayer = _player.GlobalPosition - GlobalPosition;
        var distance = toPlayer.Length();

        if (distance <= AttackRange)
        {
            State = EnemyState.Attack;
            MoveWithGravity(delta, Vector3.Zero);
            TryAttack();
            return;
        }

        if (distance <= DetectionRadius)
        {
            State = EnemyState.Chase;
            var direction = toPlayer.Normalized();
            LookAt(new Vector3(_player.GlobalPosition.X, GlobalPosition.Y, _player.GlobalPosition.Z), Vector3.Up);
            MoveWithGravity(delta, direction * MoveSpeed + _knockback);
            _knockback = _knockback.Lerp(Vector3.Zero, 5.0f * (float)delta);
            return;
        }

        State = EnemyState.Idle;
        MoveWithGravity(delta, Vector3.Zero);
    }

    public void ApplyKnockback(Vector3 force)
    {
        _knockback += force;
        _stunRemaining = 0.35;
    }

    private void TryAttack()
    {
        if (_attackCooldown > 0.0)
        {
            return;
        }

        _attackCooldown = AttackCooldownSeconds;
        GD.Print($"Enemy_Hospede atacou o jogador por {AttackDamage} de dano. TODO: vida do player.");
    }

    private void MoveWithGravity(double delta, Vector3 planarVelocity)
    {
        var velocity = planarVelocity;
        velocity.Y = Velocity.Y;
        if (!IsOnFloor())
        {
            velocity.Y -= Gravity * (float)delta;
        }
        else if (velocity.Y < 0.0f)
        {
            velocity.Y = -0.1f;
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    private void OnDamaged(float currentHealth, float damage)
    {
        if (State != EnemyState.Dead)
        {
            _stunRemaining = 0.25;
            State = EnemyState.Stunned;
        }
    }

    private void OnDied()
    {
        State = EnemyState.Dead;
        GD.Print("Enemy_Hospede caiu no chao.");
    }
}
