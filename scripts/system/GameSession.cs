namespace BREU.Scripts.Systems;

/// <summary>
/// Estado em memoria da sessao atual. Nao salva em disco ainda.
/// </summary>
public partial class GameSession : Node
{
    public bool HasRustyHammer { get; private set; }
    public bool HasOldKey { get; private set; }
    public string CurrentWeaponName { get; private set; } = "";
    public int CurrentWeaponDurability { get; private set; }
    public int CurrentWeaponMaxDurability { get; private set; }

    public void EquipRustyHammer()
    {
        EquipWeapon("Martelo Enferrujado", 10, 10);
        HasRustyHammer = true;
        GD.Print("GameSession: Martelo Enferrujado equipado 10/10");
    }

    public void EquipWeapon(string name, int durability, int maxDurability)
    {
        CurrentWeaponName = name;
        CurrentWeaponMaxDurability = Mathf.Max(0, maxDurability);
        CurrentWeaponDurability = Mathf.Clamp(durability, 0, CurrentWeaponMaxDurability);
    }

    public bool HasWeapon()
    {
        return !string.IsNullOrEmpty(CurrentWeaponName) && CurrentWeaponDurability > 0;
    }

    public void ReduceWeaponDurability(int amount)
    {
        if (amount <= 0 || !HasWeapon())
        {
            return;
        }

        CurrentWeaponDurability = Mathf.Max(0, CurrentWeaponDurability - amount);
        if (CurrentWeaponDurability <= 0)
        {
            GD.Print("O Martelo Enferrujado quebrou.");
            ClearWeapon();
        }
    }

    public void ClearWeapon()
    {
        CurrentWeaponName = "";
        CurrentWeaponDurability = 0;
        CurrentWeaponMaxDurability = 0;
    }

    public void CollectOldKey()
    {
        HasOldKey = true;
        GD.Print("GameSession: Chave Velha coletada.");
    }

    public void RestoreSnapshot(
        bool hasRustyHammer,
        bool hasOldKey,
        string weaponName,
        int weaponDurability,
        int weaponMaxDurability)
    {
        HasRustyHammer = hasRustyHammer;
        HasOldKey = hasOldKey;
        EquipWeapon(weaponName, weaponDurability, weaponMaxDurability);
        GD.Print("GameSession: snapshot de checkpoint restaurado.");
    }
}
