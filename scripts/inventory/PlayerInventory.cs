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
        EmitSignal(SignalName.InventoryChanged);
    }
}
