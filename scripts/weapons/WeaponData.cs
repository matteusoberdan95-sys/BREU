namespace BREU.Scripts.Weapons;

public enum WeaponKind
{
    Hands,
    Knife,
    Hammer,
    Pipe,
    Wood,
    Bottle,
    Tool
}

public partial class WeaponData : Resource
{
    [Export] public string WeaponName { get; set; } = "Maos";
    [Export] public WeaponKind Kind { get; set; } = WeaponKind.Hands;
    [Export] public float Damage { get; set; } = 8.0f;
    [Export] public float Range { get; set; } = 1.45f;
    [Export] public int MaxDurability { get; set; } = 999;
    [Export] public int CurrentDurability { get; set; } = 999;
    [Export] public float StaminaCost { get; set; } = 12.0f;
    [Export] public float AttackCooldown { get; set; } = 0.55f;
    [Export] public float ImpactForce { get; set; } = 4.0f;

    public bool IsBroken => CurrentDurability <= 0 && Kind != WeaponKind.Hands;

    public WeaponData RuntimeCopy()
    {
        var copy = (WeaponData)Duplicate(true);
        copy.CurrentDurability = copy.MaxDurability;
        return copy;
    }

    public void SpendDurability()
    {
        if (Kind == WeaponKind.Hands)
        {
            return;
        }

        CurrentDurability = Mathf.Max(0, CurrentDurability - 1);
    }
}
