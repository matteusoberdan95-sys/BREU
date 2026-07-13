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
    public bool HasDrainKey => HasOwnerRoomKey;
    public bool TechnicalPanelUnlocked { get; private set; }
    public bool OldFuseInstalled { get; private set; }
    public bool UpperFuseInstalled { get; private set; }

    /// <summary>Sprint 17C — owner bedroom unlocked.</summary>
    public bool IsOwnerRoomUnlocked { get; private set; }

    /// <summary>Sprint 17C — owner ledger read.</summary>
    public bool HasReadOwnerLedger { get; private set; }
    public bool HasTriggeredRoom203Warning { get; private set; }
    public bool Room203Opened { get; private set; }
    public bool Room203EventPlayed { get; private set; }
    public bool Room203ObjectiveGiven { get; private set; }
    public bool FirstPresenceHintPlayed { get; private set; }
    public bool AfterRoom203DescentStarted { get; private set; }
    public bool FirstPresencePlayed { get; private set; }
    public bool DownstairsClueCollected { get; private set; }
    public bool Sprint21Completed { get; private set; }
    public bool FirstEnemySeen { get; private set; }
    public bool FirstChaseStarted { get; private set; }
    public bool FirstChaseFinished { get; private set; }
    public bool FirstChaseEscaped { get; private set; }
    public bool Room203CanBeForced => IsUpperPowerRestored;

    /// <summary>Sprint 18A — second fuse from linen closet.</summary>
    public bool HasUpperFuse { get; private set; }

    /// <summary>Sprint 18A — upper generator panel restored.</summary>
    public bool IsUpperPowerRestored { get; private set; }

    /// <summary>Sprint 19 — Room 204 owner note read.</summary>
    public bool ReadRoom204Note { get; private set; }

    /// <summary>Sprint 19B — owners office log read.</summary>
    public bool ReadOwnersOfficeLog { get; private set; }

    /// <summary>Sprint 19 — one-shot scares / intros.</summary>
    public bool CorridorIntroPlayed { get; private set; }
    public bool BathroomScarePlayed { get; private set; }
    public bool LaundryScarePlayed { get; private set; }
    public bool Room204ExitScarePlayed { get; private set; }

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
    public event Action? Room203WarningTriggered;
    public event Action? Room203OpenedChanged;
    public event Action? Room203EventTriggered;
    public event Action? FirstPresenceTriggered;
    public event Action? DownstairsClueRead;
    public event Action? FirstChaseBegan;
    public event Action? FirstChaseEnded;
    public event Action? UpperFusePickedUp;
    public event Action? UpperPowerRestored;
    public event Action? Room204NoteRead;
    public event Action? OwnersOfficeLogRead;
    public event Action? PlaytestItemsGranted;

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

    public void TriggerRoom203Warning()
    {
        if (HasTriggeredRoom203Warning) return;
        HasTriggeredRoom203Warning = true;
        Room203WarningTriggered?.Invoke();
    }

    public void OpenRoom203()
    {
        if (!Room203CanBeForced || Room203Opened) return;
        Room203Opened = true;
        Room203OpenedChanged?.Invoke();
    }

    public void TriggerRoom203Event()
    {
        if (!Room203Opened || Room203EventPlayed) return;
        Room203EventPlayed = true;
        Room203ObjectiveGiven = true;
        Room203EventTriggered?.Invoke();
    }

    public void MarkFirstPresenceHintPlayed()
    {
        if (!Room203EventPlayed || FirstPresenceHintPlayed) return;
        FirstPresenceHintPlayed = true;
    }

    public void StartFirstPresence()
    {
        if (!Room203EventPlayed || !FirstPresenceHintPlayed || FirstPresencePlayed) return;
        AfterRoom203DescentStarted = true;
        FirstPresencePlayed = true;
        FirstPresenceTriggered?.Invoke();
    }

    public void CollectDownstairsClue()
    {
        if (!FirstPresencePlayed || DownstairsClueCollected) return;
        DownstairsClueCollected = true;
        Sprint21Completed = true;
        DownstairsClueRead?.Invoke();
    }

    public void StartFirstChase()
    {
        if (!Sprint21Completed || FirstChaseStarted || FirstChaseFinished) return;
        FirstEnemySeen = true;
        FirstChaseStarted = true;
        FirstChaseBegan?.Invoke();
    }

    public void EscapeFirstChase()
    {
        if (!FirstChaseStarted || FirstChaseFinished) return;
        FirstChaseFinished = true;
        FirstChaseEscaped = true;
        FirstChaseEnded?.Invoke();
    }

    public void PickupUpperFuse()
    {
        if (HasUpperFuse)
        {
            return;
        }

        HasUpperFuse = true;
        UpperFusePickedUp?.Invoke();
    }

    public void UnlockTechnicalPanel()
    {
        if (!HasDrainKey || TechnicalPanelUnlocked) return;
        TechnicalPanelUnlocked = true;
    }

    public void InstallOldFuse()
    {
        if (!TechnicalPanelUnlocked || !HasOldFuse || OldFuseInstalled) return;
        OldFuseInstalled = true;
        RestoreUpperPower();
    }

    public void InstallUpperFuse()
    {
        if (!TechnicalPanelUnlocked || !HasUpperFuse || UpperFuseInstalled) return;
        UpperFuseInstalled = true;
        RestoreUpperPower();
    }

    public void RestoreUpperPower()
    {
        if (!TechnicalPanelUnlocked || !OldFuseInstalled || !UpperFuseInstalled || IsUpperPowerRestored)
        {
            return;
        }

        IsUpperPowerRestored = true;
        UpperPowerRestored?.Invoke();
    }

    public void MarkRoom204NoteRead()
    {
        if (ReadRoom204Note)
        {
            return;
        }

        ReadRoom204Note = true;
        Room204NoteRead?.Invoke();
    }

    public void MarkCorridorIntroPlayed()
    {
        CorridorIntroPlayed = true;
    }

    public void MarkBathroomScarePlayed()
    {
        BathroomScarePlayed = true;
    }

    public void MarkLaundryScarePlayed()
    {
        LaundryScarePlayed = true;
    }

    public void MarkRoom204ExitScarePlayed()
    {
        Room204ExitScarePlayed = true;
    }

    public void MarkOwnersOfficeLogRead()
    {
        if (ReadOwnersOfficeLog)
        {
            return;
        }

        ReadOwnersOfficeLog = true;
        OwnersOfficeLogRead?.Invoke();
    }

    /// <summary>
    /// Debug playtest helper (F4): grants keys/items and unlocks deposit + balcony + owner room.
    /// Does not auto-complete upper wing narrative (notes/scares/power) so rooms stay testable.
    /// </summary>
    public void GrantAllPuzzleItemsForPlaytest()
    {
        HasDepositKey = true;
        IsDepositUnlocked = true;
        HasOldFuse = true;
        HasReadBalconyNote = true;
        HasBalconyKey = true;
        IsBalconyUnlocked = true;
        // HasWireHook left false so laundry wire pickup remains testable (Sprint 19C).
        HasExaminedBathroomMirror = true;
        HasOwnerRoomKey = true;
        IsOwnerRoomUnlocked = true;
        HasReadOwnerLedger = true;
        HasUpperFuse = true;
        PlaytestItemsGranted?.Invoke();
    }
}
