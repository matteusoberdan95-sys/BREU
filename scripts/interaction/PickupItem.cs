using Godot;
using BREU.Scripts.Inventory;
using BREU.Scripts.Player;
using BREU.Scripts.Weapons;

namespace BREU.Scripts.Interaction;

public partial class PickupItem : StaticBody3D, IInteractable
{
    [Export] public string ItemId { get; set; } = "item";
    [Export] public string DisplayName { get; set; } = "Item";
    [Export] public string PromptText { get; set; } = "Pegar item";
    [Export] public string KeyId { get; set; } = "";
    [Export(PropertyHint.MultilineText)] public string DocumentText { get; set; } = "";
    [Export] public WeaponData? WeaponToEquip { get; set; }

    public string GetInteractionText()
    {
        return PromptText;
    }

    public void Interact(PlayerController player)
    {
        var inventory = player.GetNodeOrNull<PlayerInventory>("PlayerInventory");

        if (!string.IsNullOrWhiteSpace(KeyId))
        {
            inventory?.AddKey(KeyId);
        }

        if (!string.IsNullOrWhiteSpace(DocumentText))
        {
            inventory?.AddDocument(ItemId);
            GD.Print($"Bilhete lido: {DocumentText}");
        }

        if (WeaponToEquip != null)
        {
            var weaponController = player.GetNodeOrNull<WeaponController>("CameraPivot/WeaponHand/WeaponController");
            weaponController?.EquipWeapon(WeaponToEquip);
            inventory?.SetEquippedWeapon(weaponController?.EquippedWeapon);
            GD.Print($"Arma coletada: {DisplayName}");
        }

        QueueFree();
    }
}
