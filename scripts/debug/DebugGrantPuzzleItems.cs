namespace BREU.Scripts.Debug;

using BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// F4 — grants all puzzle keys/items and unlocks deposit, balcony and owner room for playtest.
/// Does not open Room 203 or restore upper power narrative state beyond having the upper fuse.
/// </summary>
public partial class DebugGrantPuzzleItems : Node
{
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventKey { Pressed: true, Echo: false, Keycode: Key.F4 })
        {
            return;
        }

        var scene = GetTree().CurrentScene;
        var state = scene?.GetNodeOrNull<PensaoPuzzleState>("PuzzleState");
        if (state == null)
        {
            GD.PushWarning("[DebugGrant] PuzzleState missing.");
            return;
        }

        state.GrantAllPuzzleItemsForPlaytest();
        RefreshDoors(scene!);

        var hud = HUDController.FindActive(GetTree());
        hud?.ShowMessage(
            "[DEBUG F4] Chaves e itens liberados. Depósito, varanda e quarto da dona abertos. Fusível Superior no inventário.",
            5f);
        GD.Print("[DebugGrant] Playtest items granted; balcony/deposit/owner unlocked.");
        GetViewport().SetInputAsHandled();
    }

    private static void RefreshDoors(Node scene)
    {
        foreach (var node in scene.GetChildren())
        {
            RefreshRecursive(node);
        }
    }

    private static void RefreshRecursive(Node node)
    {
        switch (node)
        {
            case BlockoutBalconyDoor balcony:
                balcony.ApplyState();
                break;
            case BlockoutUnlockHideDoor deposit:
                deposit.ApplyState();
                break;
            case BlockoutOwnerBedroomDoor owner:
                owner.ApplyState();
                break;
        }

        foreach (var child in node.GetChildren())
        {
            RefreshRecursive(child);
        }
    }
}
