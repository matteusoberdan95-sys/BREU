namespace BREU.Scripts.Weapons;

public partial class WeaponController : Node3D
{
    [Signal] public delegate void WeaponChangedEventHandler(string weaponName, int durability, int maxDurability);

    [Export] public WeaponData? StartingWeapon { get; set; }
    [Export] public WeaponData? UnarmedWeapon { get; set; }
    [Export] public NodePath AttackRayPath { get; set; } = "../../Camera3D/AttackRay";
    [Export] public NodePath StaminaPath { get; set; } = "../../../PlayerStamina";

    public WeaponData? EquippedWeapon { get; private set; }

    private RayCast3D? _attackRay;
    private PlayerStamina? _stamina;
    private double _cooldownRemaining;

    public override void _Ready()
    {
        _attackRay = GetNodeOrNull<RayCast3D>(AttackRayPath);
        _stamina = GetNodeOrNull<PlayerStamina>(StaminaPath);
        EquipWeapon(StartingWeapon ?? UnarmedWeapon);
    }

    public override void _Process(double delta)
    {
        if (_cooldownRemaining > 0.0)
        {
            _cooldownRemaining -= delta;
        }

        if (Input.IsActionJustPressed("attack_primary"))
        {
            TryAttack();
        }
    }

    public void EquipWeapon(WeaponData? weapon)
    {
        EquippedWeapon = weapon?.RuntimeCopy();
        EmitWeaponChanged();
    }

    private void TryAttack()
    {
        if (EquippedWeapon == null || _cooldownRemaining > 0.0)
        {
            return;
        }

        if (_stamina != null && !_stamina.Consume(EquippedWeapon.StaminaCost))
        {
            GD.Print("Sem stamina para atacar.");
            return;
        }

        _cooldownRemaining = EquippedWeapon.AttackCooldown;
        if (_attackRay != null)
        {
            _attackRay.TargetPosition = new Vector3(0.0f, 0.0f, -EquippedWeapon.Range);
            _attackRay.ForceRaycastUpdate();
        }

        if (_attackRay?.IsColliding() == true)
        {
            TryDamageCollider(_attackRay.GetCollider() as Node);
        }
        else
        {
            GD.Print($"Ataque com {EquippedWeapon.WeaponName} errou.");
        }

        EquippedWeapon.SpendDurability();
        if (EquippedWeapon.IsBroken)
        {
            GD.Print($"{EquippedWeapon.WeaponName} quebrou.");
            EquipWeapon(UnarmedWeapon);
            return;
        }

        EmitWeaponChanged();
    }

    private void TryDamageCollider(Node? node)
    {
        while (node != null)
        {
            var health = node.GetNodeOrNull<EnemyHealth>("EnemyHealth");
            if (health != null && EquippedWeapon != null)
            {
                health.ApplyDamage(EquippedWeapon.Damage);
                if (node is EnemyAI enemy)
                {
                    enemy.ApplyKnockback(-GlobalTransform.Basis.Z * EquippedWeapon.ImpactForce);
                }
                GD.Print($"Acertou {node.Name} com {EquippedWeapon.WeaponName}.");
                return;
            }

            node = node.GetParent();
        }

        GD.Print("Ataque acertou algo sem vida.");
    }

    private void EmitWeaponChanged()
    {
        if (EquippedWeapon == null)
        {
            EmitSignal(SignalName.WeaponChanged, "Nenhuma", 0, 0);
            return;
        }

        EmitSignal(SignalName.WeaponChanged, EquippedWeapon.WeaponName, EquippedWeapon.CurrentDurability, EquippedWeapon.MaxDurability);
    }
}
