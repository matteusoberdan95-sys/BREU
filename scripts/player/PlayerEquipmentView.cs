using Godot;
using BREU.Scripts.Inventory;

namespace BREU.Scripts.Player;

public partial class PlayerEquipmentView : Node
{
    [Export] public NodePath InventoryPath { get; set; } = "../PlayerInventory";
    [Export] public NodePath HammerVisualPath { get; set; } = "../CameraPivot/Camera3D/WeaponHand/HammerVisual";

    private PlayerInventory? _inventory;
    private Node3D? _hammerVisual;

    public override void _Ready()
    {
        _inventory = GetNodeOrNull<PlayerInventory>(InventoryPath);
        _hammerVisual = GetNodeOrNull<Node3D>(HammerVisualPath);

        if (_inventory != null)
        {
            _inventory.InventoryChanged += Refresh;
        }

        Refresh();
    }

    private void Refresh()
    {
        if (_hammerVisual != null)
        {
            _hammerVisual.Visible = _inventory?.HasHammer == true;
        }
    }
}
