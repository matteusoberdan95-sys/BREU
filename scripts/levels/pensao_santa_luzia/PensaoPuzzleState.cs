namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Local puzzle state for PensaoTerreoBlockout01 / Vertical (Sprint 07 + 17).
/// Not a global inventory.
/// </summary>
public partial class PensaoPuzzleState : Node
{
    public bool HasDepositKey { get; private set; }
    public bool IsDepositUnlocked { get; private set; }
    public bool HasOldFuse { get; private set; }

    /// <summary>Sprint 17 — balcony access puzzle.</summary>
    public bool HasReadBalconyNote { get; private set; }

    /// <summary>Sprint 17 — balcony access puzzle.</summary>
    public bool HasBalconyKey { get; private set; }

    /// <summary>Sprint 17 — balcony access puzzle.</summary>
    public bool IsBalconyUnlocked { get; private set; }

    /// <summary>Sprint 15 — narrative observers only; does not change puzzle rules.</summary>
    public event Action? DepositKeyPickedUp;

    /// <summary>Sprint 15 — narrative observers only; does not change puzzle rules.</summary>
    public event Action? OldFusePickedUp;

    /// <summary>Sprint 17 — narrative / HUD observers.</summary>
    public event Action? BalconyNoteRead;

    /// <summary>Sprint 17 — narrative / HUD observers.</summary>
    public event Action? BalconyKeyPickedUp;

    /// <summary>Sprint 17 — narrative / HUD observers.</summary>
    public event Action? BalconyUnlocked;

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
}
