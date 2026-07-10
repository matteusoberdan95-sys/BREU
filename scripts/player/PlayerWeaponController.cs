namespace BREU.Scripts.Player;

/// <summary>
/// Sincroniza a arma persistente da sessao com o Player recem-instanciado.
/// Combate completo fica para a proxima etapa.
/// </summary>
public partial class PlayerWeaponController : Node
{
    [Export] public NodePath InventoryPath { get; set; } = "../PlayerInventory";
    [Export] public NodePath HammerVisualPath { get; set; } = "../CameraPivot/Camera3D/WeaponHolder/EquippedHammerVisual";

    private PlayerInventory? _inventory;
    private Node3D? _hammerVisual;

    public override void _Ready()
    {
        _inventory = GetNodeOrNull<PlayerInventory>(InventoryPath);
        _hammerVisual = GetNodeOrNull<Node3D>(HammerVisualPath);
        RefreshWeaponFromSession();
    }

    public void RefreshWeaponFromSession()
    {
        var session = GetNodeOrNull<GameSession>("/root/GameSession");
        if (session == null)
        {
            HideWeaponVisual();
            _inventory?.ClearWeapon();
            UpdateHudEmptyHands();
            return;
        }

        if (session.HasWeapon())
        {
            _inventory?.ApplyWeaponFromSession(session);
            EquipVisualWeapon(session.CurrentWeaponName);
            UpdateHudWeapon(session.CurrentWeaponName, session.CurrentWeaponDurability, session.CurrentWeaponMaxDurability);
            return;
        }

        _inventory?.ClearWeapon();
        HideWeaponVisual();
        UpdateHudEmptyHands();
    }

    public void EquipVisualWeapon(string weaponName)
    {
        if (_hammerVisual != null)
        {
            _hammerVisual.Visible = weaponName == "Martelo Enferrujado";
        }
    }

    public void HideWeaponVisual()
    {
        if (_hammerVisual != null)
        {
            _hammerVisual.Visible = false;
        }
    }

    private void UpdateHudWeapon(string weaponName, int durability, int maxDurability)
    {
        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.SetWeapon(weaponName, durability, maxDurability);
        }
    }

    private void UpdateHudEmptyHands()
    {
        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.SetWeapon("Maos vazias", 0, 0);
        }
    }
}
