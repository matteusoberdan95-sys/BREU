using Godot;
using BREU.Scripts.Inventory;
using BREU.Scripts.Player;
using BREU.Scripts.Weapons;

namespace BREU.Scripts.Ui;

public partial class HUDController : CanvasLayer
{
    [Export] public NodePath InteractionLabelPath { get; set; } = "Root/InteractionLabel";
    [Export] public NodePath StaminaLabelPath { get; set; } = "Root/Stats/StaminaLabel";
    [Export] public NodePath BatteryLabelPath { get; set; } = "Root/Stats/BatteryLabel";
    [Export] public NodePath WeaponLabelPath { get; set; } = "Root/Stats/WeaponLabel";

    private Label? _interactionLabel;
    private Label? _staminaLabel;
    private Label? _batteryLabel;
    private Label? _weaponLabel;

    public override void _Ready()
    {
        _interactionLabel = GetNodeOrNull<Label>(InteractionLabelPath);
        _staminaLabel = GetNodeOrNull<Label>(StaminaLabelPath);
        _batteryLabel = GetNodeOrNull<Label>(BatteryLabelPath);
        _weaponLabel = GetNodeOrNull<Label>(WeaponLabelPath);
        SetInteractionPrompt("");
        SetWeapon("Mãos vazias", 0, 0);
        CallDeferred(nameof(BindToPlayer));
    }

    public void BindToPlayer()
    {
        var player = GetTree().GetFirstNodeInGroup("player") as PlayerController;
        if (player == null)
        {
            return;
        }

        player.GetNodeOrNull<PlayerInteractor>("PlayerInteractor")?.Connect(
            PlayerInteractor.SignalName.FocusChanged,
            Callable.From<string>(SetInteractionPrompt));

        player.GetNodeOrNull<PlayerStamina>("PlayerStamina")?.Connect(
            PlayerStamina.SignalName.StaminaChanged,
            Callable.From<float, float>(SetStamina));

        player.GetNodeOrNull<FlashlightController>("CameraPivot/Flashlight")?.Connect(
            FlashlightController.SignalName.BatteryChanged,
            Callable.From<float, float>(SetBattery));

        player.GetNodeOrNull<WeaponController>("CameraPivot/WeaponHand/WeaponController")?.Connect(
            WeaponController.SignalName.WeaponChanged,
            Callable.From<string, int, int>(SetWeapon));

        var inventory = player.GetNodeOrNull<PlayerInventory>("PlayerInventory");
        if (inventory != null)
        {
            inventory.InventoryChanged += () => SetInventoryWeapon(inventory);
            SetInventoryWeapon(inventory);
        }
    }

    private void SetInteractionPrompt(string prompt)
    {
        if (_interactionLabel != null)
        {
            _interactionLabel.Text = string.IsNullOrWhiteSpace(prompt) ? "" : $"[E] {prompt}";
        }
    }

    private void SetStamina(float current, float maximum)
    {
        if (_staminaLabel != null)
        {
            _staminaLabel.Text = $"Stamina {current:0}/{maximum:0}";
        }
    }

    private void SetBattery(float current, float maximum)
    {
        if (_batteryLabel != null)
        {
            _batteryLabel.Text = $"Lanterna {current:0}/{maximum:0}";
        }
    }

    private void SetWeapon(string weaponName, int durability, int maxDurability)
    {
        if (_weaponLabel != null)
        {
            _weaponLabel.Text = maxDurability <= 0
                ? $"Arma: {weaponName}"
                : $"Arma: {weaponName} {durability}/{maxDurability}";
        }
    }

    private void SetInventoryWeapon(PlayerInventory inventory)
    {
        if (_weaponLabel == null)
        {
            return;
        }

        _weaponLabel.Text = inventory.HasHammer
            ? $"Arma: {inventory.EquippedWeaponName} {inventory.EquippedWeaponDurability}/10"
            : "Arma: Mãos vazias";
    }
}
