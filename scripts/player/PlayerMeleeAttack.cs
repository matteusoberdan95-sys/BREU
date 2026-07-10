namespace BREU.Scripts.Player;

/// <summary>
/// Ataque melee simples do prototipo. Usa raycast curto da camera e GameSession.
/// </summary>
public partial class PlayerMeleeAttack : Node
{
    [Export] public float AttackRange { get; set; } = 2.0f;
    [Export] public float AttackRadius { get; set; } = 0.75f;
    [Export] public float AttackForwardOffset { get; set; } = 1.0f;
    [Export] public float AttackAngleDot { get; set; } = 0.25f;
    [Export] public float AttackCooldown { get; set; } = 0.75f;
    [Export] public int HammerDamage { get; set; } = 10;
    [Export] public int DurabilityCostPerHit { get; set; } = 1;
    [Export] public bool DebugMelee { get; set; } = true;
    [Export] public bool DebugAttackRay { get; set; } = true;
    [Export] public NodePath CameraPath { get; set; } = "../CameraPivot/Camera3D";
    [Export] public NodePath WeaponControllerPath { get; set; } = "../PlayerWeaponController";
    [Export] public NodePath HammerVisualPath { get; set; } = "../CameraPivot/Camera3D/WeaponHolder/EquippedHammerVisual";
    [Export] public NodePath MeleeAudioPath { get; set; } = "../MeleeAudio";
    [Export] public NodePath MeleeHitAudioPath { get; set; } = "../MeleeHitAudio";
    [Export] public string SwingAudioPath { get; set; } = "res://assets/audio/sfx/player/weapon_swing_01.ogg";
    [Export] public string HitAudioPath { get; set; } = "res://assets/audio/sfx/player/weapon_hit_01.ogg";
    [Export] public string HitFallbackAudioPath { get; set; } = "res://assets/audio/sfx/horror/corridor_hit_01.ogg";

    private Camera3D? _camera;
    private PlayerWeaponController? _weaponController;
    private AudioStreamPlayer3D? _meleeAudio;
    private AudioStreamPlayer3D? _hitAudio;
    private Node3D? _hammerVisual;
    private Vector3 _hammerBasePosition;
    private Vector3 _hammerBaseRotation;
    private float _cooldownRemaining;
    private Tween? _swingTween;

    public override void _Ready()
    {
        _camera = GetNodeOrNull<Camera3D>(CameraPath);
        _weaponController = GetNodeOrNull<PlayerWeaponController>(WeaponControllerPath);
        _hammerVisual = GetNodeOrNull<Node3D>(HammerVisualPath);
        _meleeAudio = GetNodeOrNull<AudioStreamPlayer3D>(MeleeAudioPath);
        _hitAudio = GetNodeOrNull<AudioStreamPlayer3D>(MeleeHitAudioPath);

        if (_hammerVisual != null)
        {
            _hammerBasePosition = _hammerVisual.Position;
            _hammerBaseRotation = _hammerVisual.Rotation;
        }

        ConfigureAudio();
    }

    public override void _Process(double delta)
    {
        _cooldownRemaining = Mathf.Max(0.0f, _cooldownRemaining - (float)delta);

        if (AttackPressed())
        {
            TryAttack();
        }
    }

    private bool AttackPressed()
    {
        return (InputMap.HasAction("attack") && Input.IsActionJustPressed("attack"))
            || (InputMap.HasAction("attack_primary") && Input.IsActionJustPressed("attack_primary"));
    }

    private void TryAttack()
    {
        if (_cooldownRemaining > 0.0f)
        {
            return;
        }

        var session = GetNodeOrNull<GameSession>("/root/GameSession");
        if (session == null || !session.HasWeapon())
        {
            ShowHudMessage("Voce esta de maos vazias.", 1.6f);
            return;
        }

        _cooldownRemaining = AttackCooldown;
        DebugAttack("MeleeAttack: swing");
        PlaySwingAudio();
        PlaySwingFeedback();

        if (TryHitEnemy(out var enemy, out var hitByVolume))
        {
            enemy.ReceiveHit(HammerDamage);
            session.ReduceWeaponDurability(DurabilityCostPerHit);
            PlayHitAudio();
            DebugAttack(hitByVolume
                ? "MeleeAttack: EnemyPlaceholder acertado por hit volume."
                : "MeleeAttack: EnemyPlaceholder acertado por raycast.");
            ShowHudMessage(session.HasWeapon() ? "Voce acertou." : "O Martelo Enferrujado quebrou.", 1.8f);
            _weaponController?.RefreshWeaponFromSession();
            return;
        }

        DebugAttack("MeleeAttack: ataque errou.");
        _weaponController?.RefreshWeaponFromSession();
    }

    private bool TryHitEnemy(out EnemyPlaceholderAI enemy, out bool hitByVolume)
    {
        enemy = null!;
        hitByVolume = false;
        if (_camera == null)
        {
            return false;
        }

        if (TryHitEnemyByRaycast(out enemy))
        {
            return true;
        }

        DebugAttack("MeleeAttack: raycast errou, tentando hit volume");

        if (TryHitEnemyByVolume(out enemy))
        {
            hitByVolume = true;
            return true;
        }

        if (TryHitEnemyByGroup(out enemy))
        {
            hitByVolume = true;
            return true;
        }

        return false;
    }

    private bool TryHitEnemyByRaycast(out EnemyPlaceholderAI enemy)
    {
        enemy = null!;
        if (_camera == null)
        {
            return false;
        }

        var space = _camera.GetWorld3D().DirectSpaceState;
        var origin = _camera.GlobalPosition;
        var direction = -_camera.GlobalTransform.Basis.Z.Normalized();
        var end = origin + direction * AttackRange;
        var query = PhysicsRayQueryParameters3D.Create(origin, end);
        query.CollideWithBodies = true;
        query.CollideWithAreas = true;
        query.CollisionMask = uint.MaxValue;

        if (GetParent() is CollisionObject3D owner)
        {
            query.Exclude = new Godot.Collections.Array<Rid> { owner.GetRid() };
        }

        DebugAttack($"MeleeAttack: ray origin {origin} end {end}");

        var result = space.IntersectRay(query);
        if (result.Count == 0 || !result.TryGetValue("collider", out var colliderVariant))
        {
            DebugAttack("MeleeAttack: errou.");
            return false;
        }

        var collider = colliderVariant.AsGodotObject();
        if (collider is Node colliderNode)
        {
            DebugAttack($"MeleeAttack: acertou collider {colliderNode.Name}");
        }

        if (collider is EnemyPlaceholderAI directEnemy)
        {
            enemy = directEnemy;
            DebugAttack("MeleeAttack: EnemyPlaceholderAI encontrado");
            return true;
        }

        if (collider is Node node)
        {
            var parentEnemy = FindEnemyFromCollider(node);
            if (parentEnemy == null)
            {
                DebugAttack("MeleeAttack: EnemyPlaceholderAI NAO encontrado");
                return false;
            }

            enemy = parentEnemy;
            DebugAttack("MeleeAttack: EnemyPlaceholderAI encontrado");
            return true;
        }

        DebugAttack("MeleeAttack: EnemyPlaceholderAI NAO encontrado");
        return false;
    }

    private bool TryHitEnemyByVolume(out EnemyPlaceholderAI enemy)
    {
        enemy = null!;
        if (_camera == null)
        {
            return false;
        }

        var forward = -_camera.GlobalTransform.Basis.Z.Normalized();
        var origin = _camera.GlobalPosition;
        var attackCenter = origin + forward * AttackForwardOffset;
        var sphere = new SphereShape3D { Radius = AttackRadius };
        var query = new PhysicsShapeQueryParameters3D
        {
            Shape = sphere,
            Transform = new Transform3D(Basis.Identity, attackCenter),
            CollideWithBodies = true,
            CollideWithAreas = true,
            CollisionMask = uint.MaxValue
        };

        if (GetParent() is CollisionObject3D owner)
        {
            query.Exclude = new Godot.Collections.Array<Rid> { owner.GetRid() };
        }

        DebugAttack($"MeleeAttack: hit volume center {attackCenter} radius {AttackRadius:0.00}");
        var hits = _camera.GetWorld3D().DirectSpaceState.IntersectShape(query, 16);
        foreach (var hit in hits)
        {
            if (!hit.TryGetValue("collider", out var colliderVariant))
            {
                continue;
            }

            var collider = colliderVariant.AsGodotObject();
            if (collider is not Node colliderNode)
            {
                continue;
            }

            var candidate = ResolveEnemyTarget(colliderNode);
            if (candidate == null)
            {
                continue;
            }

            DebugAttack(colliderNode.IsInGroup("enemy_hurtbox")
                ? "MeleeAttack: hit volume encontrou enemy_hurtbox"
                : $"MeleeAttack: hit volume encontrou collider {colliderNode.Name}");

            var toTarget = candidate.GlobalPosition - origin;
            if (toTarget.LengthSquared() <= 0.001f)
            {
                enemy = candidate;
                DebugAttack("MeleeAttack: EnemyPlaceholder acertado");
                DebugAttack("MeleeAttack: EnemyPlaceholderAI encontrado");
                return true;
            }

            var dot = forward.Dot(toTarget.Normalized());
            DebugAttack($"MeleeAttack: dot com alvo = {dot:0.00}");
            if (dot < AttackAngleDot)
            {
                continue;
            }

            enemy = candidate;
            DebugAttack("MeleeAttack: EnemyPlaceholder acertado");
            DebugAttack("MeleeAttack: EnemyPlaceholderAI encontrado");
            return true;
        }

        return false;
    }

    private bool TryHitEnemyByGroup(out EnemyPlaceholderAI enemy)
    {
        enemy = null!;
        if (_camera == null)
        {
            return false;
        }

        var origin = _camera.GlobalPosition;
        var forward = -_camera.GlobalTransform.Basis.Z.Normalized();
        var maxDistance = AttackRange + 0.8f;

        foreach (var node in GetTree().GetNodesInGroup("enemies"))
        {
            if (node is not EnemyPlaceholderAI candidate)
            {
                continue;
            }

            var toTarget = candidate.GlobalPosition - origin;
            var distance = toTarget.Length();
            if (distance > maxDistance)
            {
                continue;
            }

            var dot = forward.Dot(toTarget.Normalized());
            DebugAttack($"MeleeAttack: fallback group enemy {candidate.Name} distance {distance:0.00} dot {dot:0.00}");
            if (dot < AttackAngleDot)
            {
                continue;
            }

            enemy = candidate;
            DebugAttack("MeleeAttack: EnemyPlaceholder acertado");
            DebugAttack("MeleeAttack: EnemyPlaceholderAI encontrado");
            return true;
        }

        return false;
    }

    private static EnemyPlaceholderAI? ResolveEnemyTarget(Node collider)
    {
        if (collider.IsInGroup("enemy_hurtbox"))
        {
            if (collider is EnemyHurtbox hurtbox)
            {
                return hurtbox.GetEnemy();
            }

            return FindEnemyFromCollider(collider);
        }

        return FindEnemyFromCollider(collider);
    }

    private static EnemyPlaceholderAI? FindEnemyFromCollider(Node collider)
    {
        var current = collider;
        while (current != null)
        {
            if (current is EnemyPlaceholderAI enemy)
            {
                return enemy;
            }

            if (current.HasMethod(nameof(EnemyPlaceholderAI.ReceiveHit)) && current is EnemyPlaceholderAI enemyByMethod)
            {
                return enemyByMethod;
            }

            if (current.IsInGroup("enemies"))
            {
                var childEnemy = current.GetNodeOrNull<EnemyPlaceholderAI>("EnemyPlaceholderAI");
                if (childEnemy != null)
                {
                    return childEnemy;
                }
            }

            current = current.GetParent();
        }

        return null;
    }

    private void ConfigureAudio()
    {
        if (_meleeAudio != null)
        {
            _meleeAudio.Stream = AudioResourceLoader.TryLoad(SwingAudioPath);
            _meleeAudio.VolumeDb = -10.0f;
        }

        if (_hitAudio != null)
        {
            _hitAudio.Stream = AudioResourceLoader.TryLoad(HitAudioPath)
                ?? AudioResourceLoader.TryLoad(HitFallbackAudioPath);
            _hitAudio.VolumeDb = -7.0f;
        }
    }

    private void PlaySwingAudio()
    {
        if (_meleeAudio?.Stream != null)
        {
            _meleeAudio.Play();
        }
    }

    private void PlayHitAudio()
    {
        if (_hitAudio?.Stream != null)
        {
            _hitAudio.Play();
        }
    }

    private void PlaySwingFeedback()
    {
        if (_hammerVisual == null || !_hammerVisual.Visible)
        {
            return;
        }

        _swingTween?.Kill();
        _hammerVisual.Position = _hammerBasePosition;
        _hammerVisual.Rotation = _hammerBaseRotation;

        _swingTween = CreateTween();
        _swingTween.TweenProperty(_hammerVisual, "position", _hammerBasePosition + new Vector3(0.0f, -0.04f, -0.12f), 0.08f);
        _swingTween.Parallel().TweenProperty(_hammerVisual, "rotation", _hammerBaseRotation + new Vector3(0.22f, 0.0f, -0.08f), 0.08f);
        _swingTween.TweenProperty(_hammerVisual, "position", _hammerBasePosition, 0.14f);
        _swingTween.Parallel().TweenProperty(_hammerVisual, "rotation", _hammerBaseRotation, 0.14f);
    }

    private void ShowHudMessage(string message, float duration)
    {
        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.ShowTemporaryMessage(message, duration);
            return;
        }

        GD.Print($"HUD mensagem: {message}");
    }

    private void DebugAttack(string message)
    {
        if (DebugMelee || DebugAttackRay)
        {
            GD.Print(message);
        }
    }
}
