namespace BREU.Scripts.Weapons;

public partial class UnarmedWeapon : WeaponData
{
    public UnarmedWeapon()
    {
        WeaponName = "Soco";
        Kind = WeaponKind.Hands;
        Damage = 6.0f;
        Range = 1.15f;
        MaxDurability = 999;
        CurrentDurability = 999;
        StaminaCost = 9.0f;
        AttackCooldown = 0.48f;
        ImpactForce = 2.4f;
    }
}
