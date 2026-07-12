namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Local puzzle state for PensaoTerreoBlockout01 / Vertical (Sprint 07 + 17/17C).
/// Not a global inventory.
/// </summary>
public partial class PensaoPuzzleState : Node
{
    public bool HasDepositKey { get; private set; }
    public bool IsDepositUnlocked { get; private set; }
    public bool HasOldFuse { get; private set; }

    public bool HasReadBalconyNote { get; private set; }
    public bool HasBalconyKey { get; private set; }
    public bool IsBalconyUnlocked { get; private set; }

    /// <summary>Sprint 17C — wire hook from balcony.</summary>
    public bool HasWireHook { get; private set; }

    /// <summary>Sprint 17C — bathroom mirror examined.</summary>
    public bool HasExaminedBathroomMirror { get; private set; }

    /// <summary>Sprint 17C — key pulled from bathroom drain.</summary>
    public bool HasOwnerRoomKey { get; private set; }

    /// <summary>Sprint 17C — owner bedroom unlocked.</summary>
    public bool IsOwnerRoomUnlocked { get; private set; }

    /// <summary>Sprint 17C — owner ledger read.</summary>
    public bool HasReadOwnerLedger { get; private set; }

    public event Action? DepositKeyPickedUp;
    public event Action? OldFusePickedUp;
    public event Action? BalconyNoteRead;
    public event Action? BalconyKeyPickedUp;
    public event Action? BalconyUnlocked;
    public event Action? WireHookPickedUp;
    public event Action? BathroomMirrorExamined;
    public event Action? OwnerRoomKeyPickedUp;
    public event Action? OwnerRoomUnlocked;
    public event Action? OwnerLedgerRead;

    public void PickupDepositKey()
    {
        HasDepositKey = true;
        DepositKeyPickedUp?.Invoke();
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
        OldFusePickedUp?.Invoke();
    }

    public void ReadBalconyNote()
    {
        if (HasReadBalconyNote)
        {
            return;
        }

        HasReadBalconyNote = true;
        BalconyNoteRead?.Invoke();
    }

    public void PickupBalconyKey()
    {
        if (HasBalconyKey)
        {
            return;
        }

        HasBalconyKey = true;
        BalconyKeyPickedUp?.Invoke();
    }

    public void UnlockBalcony()
    {
        if (!HasBalconyKey || IsBalconyUnlocked)
        {
            return;
        }

        IsBalconyUnlocked = true;
        BalconyUnlocked?.Invoke();
    }

    public void PickupWireHook()
    {
        if (HasWireHook)
        {
            return;
        }

        HasWireHook = true;
        WireHookPickedUp?.Invoke();
    }

    public void ExamineBathroomMirror()
    {
        if (HasExaminedBathroomMirror)
        {
            return;
        }

        HasExaminedBathroomMirror = true;
        BathroomMirrorExamined?.Invoke();
    }

    public void PickupOwnerRoomKey()
    {
        if (HasOwnerRoomKey)
        {
            return;
        }

        HasOwnerRoomKey = true;
        OwnerRoomKeyPickedUp?.Invoke();
    }

    public void UnlockOwnerRoom()
    {
        if (!HasOwnerRoomKey || IsOwnerRoomUnlocked)
        {
            return;
        }

        IsOwnerRoomUnlocked = true;
        OwnerRoomUnlocked?.Invoke();
    }

    public void ReadOwnerLedger()
    {
        if (HasReadOwnerLedger)
        {
            return;
        }

        HasReadOwnerLedger = true;
        OwnerLedgerRead?.Invoke();
    }
}
