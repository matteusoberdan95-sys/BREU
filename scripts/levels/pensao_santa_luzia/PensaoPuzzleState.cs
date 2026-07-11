namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Local puzzle state for PensaoTerreoBlockout01 (Sprint 07). Not a global inventory.
/// </summary>
public partial class PensaoPuzzleState : Node
{
    public bool HasDepositKey { get; private set; }
    public bool IsDepositUnlocked { get; private set; }
    public bool HasOldFuse { get; private set; }

    public void PickupDepositKey()
    {
        HasDepositKey = true;
    }

    public void UnlockDeposit()
    {
        if (!HasDepositKey || IsDepositUnlocked)
        {
            return;
        }

        IsDepositUnlocked = true;
    }

    public void PickupOldFuse()
    {
        HasOldFuse = true;
    }
}
