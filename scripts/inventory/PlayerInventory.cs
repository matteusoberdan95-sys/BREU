namespace BREU.Scripts.Inventory;

public partial class PlayerInventory : Node
{
    [Signal] public delegate void InventoryChangedEventHandler();

    public HashSet<string> Keys { get; } = new();
    public List<string> Documents { get; } = new();
    public WeaponData? EquippedWeapon { get; private set; }
    public bool HasHammer { get; private set; }
    public string EquippedWeaponName { get; private set; } = "";
    public int EquippedWeaponDurability { get; private set; }
    public int EquippedWeaponMaxDurability { get; private set; }

    public bool HasKey(string keyId) => string.IsNullOrWhiteSpace(keyId) || Keys.Contains(keyId);

    public void AddKey(string keyId)
    {
        if (!string.IsNullOrWhiteSpace(keyId))
        {
            Keys.Add(keyId);
            GD.Print($"Chave coletada: {keyId}");
            EmitSignal(SignalName.InventoryChanged);
        }
    }

    public void AddDocument(string documentId)
    {
        if (!string.IsNullOrWhiteSpace(documentId))
        {
            Documents.Add(documentId);
            GD.Print($"Documento coletado: {documentId}");
            EmitSignal(SignalName.InventoryChanged);
        }
    }

    public void SetEquippedWeapon(WeaponData? weapon)
    {
        EquippedWeapon = weapon;
        EquippedWeaponName = weapon?.WeaponName ?? "";
        EquippedWeaponDurability = weapon?.CurrentDurability ?? 0;
        EquippedWeaponMaxDurability = weapon?.MaxDurability ?? 0;
        HasHammer = EquippedWeaponName == "Martelo Enferrujado";
        EmitSignal(SignalName.InventoryChanged);
    }

    public void PickupHammer(int durability)
    {
        HasHammer = true;
        EquippedWeapon = null;
        EquippedWeaponName = "Martelo Enferrujado";
        EquippedWeaponDurability = durability;
        EquippedWeaponMaxDurability = durability;
        GD.Print($"Inventario: Martelo Enferrujado equipado. Durabilidade: {durability}/{durability}.");
        EmitSignal(SignalName.InventoryChanged);
    }

    public void ApplyWeaponFromSession(GameSession session)
    {
        HasHammer = session.HasRustyHammer;
        EquippedWeapon = null;
        EquippedWeaponName = session.CurrentWeaponName;
        EquippedWeaponDurability = session.CurrentWeaponDurability;
        EquippedWeaponMaxDurability = session.CurrentWeaponMaxDurability;
        EmitSignal(SignalName.InventoryChanged);
    }

    public void ClearWeapon()
    {
        HasHammer = false;
        EquippedWeapon = null;
        EquippedWeaponName = "";
        EquippedWeaponDurability = 0;
        EquippedWeaponMaxDurability = 0;
        EmitSignal(SignalName.InventoryChanged);
    }
}
