using Godot;
using System.Collections.Generic;
using BREU.Scripts.Weapons;

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
        EmitSignal(SignalName.InventoryChanged);
    }

    public void PickupHammer(int durability)
    {
        HasHammer = true;
        EquippedWeaponName = "Martelo Enferrujado";
        EquippedWeaponDurability = durability;
        GD.Print($"Inventário: Martelo Enferrujado equipado. Durabilidade: {durability}/{durability}.");
        EmitSignal(SignalName.InventoryChanged);
    }
}
