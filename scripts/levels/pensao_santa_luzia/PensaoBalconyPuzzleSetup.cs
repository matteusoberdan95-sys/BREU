namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Interaction;
using BREU.Scripts.Narrative;

/// <summary>
/// Sprint 17 — wires balcony note, key, and green door after blockout builds.
/// </summary>
public partial class PensaoBalconyPuzzleSetup : Node
{
    private const uint InteractableLayer = 2;
    /// <summary>Panel center should be ~3.9; anything above ~5.5 means ceiling bug returned.</summary>
    private const float SecondFloorSanityMaxY = 5.2f;

    public override void _Ready()
    {
        CallDeferred(nameof(SetupPuzzle));
    }

    private void SetupPuzzle()
    {
        var state = GetNodeOrNull<PensaoPuzzleState>("../../PuzzleState");
        var interactions = GetNodeOrNull<Node3D>("../../Interactions");
        var secondFloor = GetNodeOrNull<Node3D>("../../PensionSecondFloor");
        var balconyDoor = secondFloor?.GetNodeOrNull<Node3D>("Door_UpperBalcony");

        if (state == null || interactions == null || balconyDoor == null)
        {
            GD.PushError("[BalconyPuzzle] Missing PuzzleState, Interactions or Door_UpperBalcony.");
            return;
        }

        if (balconyDoor is not BlockoutBalconyDoor balcony)
        {
            GD.PushError("[BalconyPuzzle] Door_UpperBalcony is not a BlockoutBalconyDoor.");
            return;
        }

        balcony.Initialize(state, balconyDoor);

        var area = balconyDoor.GetNodeOrNull<Area3D>("InteractionArea");
        var panel = balconyDoor.GetNodeOrNull<MeshInstance3D>("Door_UpperBalcony_Blocker");
        GD.Print(
            $"[BalconyPuzzle] Door_UpperBalcony global={balconyDoor.GlobalPosition} " +
            $"panelLocal={panel?.Position} areaLocal={area?.Position} " +
            $"areaGlobal={area?.GlobalPosition}");

        if (balconyDoor.GlobalPosition.Y > SecondFloorSanityMaxY ||
            (area != null && area.GlobalPosition.Y > SecondFloorSanityMaxY))
        {
            GD.PushError(
                "[BalconyPuzzle] Door/Area still above expected second-floor height — check Y layout.");
        }

        var host = interactions.GetNodeOrNull<Node3D>("BalconyPuzzleItems")
            ?? new Node3D { Name = "BalconyPuzzleItems" };
        if (host.GetParent() == null)
        {
            interactions.AddChild(host);
        }

        CreateOwnerNote(state, host);
        CreateBalconyKey(state, host);
        CreateWingNotes(host);

        state.OldFusePickedUp += () =>
            HUDController.FindActive(GetTree())?.ShowMessage(
                "Talvez a porta da varanda tenha algum mecanismo antigo.", 3.5f);
        state.BalconyNoteRead += () =>
            HUDController.FindActive(GetTree())?.ShowMessage(
                "Dona Luzia guardou uma chave perto da recepção.", 3.0f);
        state.BalconyKeyPickedUp += () =>
            HUDController.FindActive(GetTree())?.ShowMessage(
                "Agora posso tentar a porta da varanda.", 3.0f);
        state.BalconyUnlocked += () =>
            HUDController.FindActive(GetTree())?.ShowMessage(
                "A ala da frente está acessível.", 3.0f);

        GD.Print("[BalconyPuzzle] Wired.");
    }

    private static void CreateOwnerNote(PensaoPuzzleState state, Node3D parent)
    {
        // Room 201 table area — replaces generic note interaction for this puzzle.
        var root = new Node3D
        {
            Name = "Interact_Note_OwnerBalcony",
            Position = new Vector3(-3.3f, 3.75f, -14.4f)
        };
        parent.AddChild(root);

        var noteMesh = new MeshInstance3D
        {
            Mesh = new BoxMesh { Size = new Vector3(0.22f, 0.02f, 0.16f) },
            MaterialOverride = new StandardMaterial3D { AlbedoColor = new Color(0.88f, 0.84f, 0.72f) }
        };
        root.AddChild(noteMesh);

        var area = new Area3D
        {
            Name = "InteractionArea",
            CollisionLayer = InteractableLayer,
            CollisionMask = 0,
            Monitoring = false,
            Monitorable = true
        };
        area.AddChild(new CollisionShape3D
        {
            Shape = new BoxShape3D { Size = new Vector3(0.35f, 0.2f, 0.3f) }
        });

        var note = new BalconyNoteInteraction { Name = "BalconyNoteInteraction" };
        area.AddChild(note);
        note.Initialize(state, root);
        root.AddChild(area);
    }

    private static void CreateBalconyKey(PensaoPuzzleState state, Node3D parent)
    {
        // Behind / under reception counter (east side of reception).
        var root = new Node3D
        {
            Name = "Interact_BalconyKey",
            Position = new Vector3(3.55f, 0.28f, -4.05f)
        };
        parent.AddChild(root);

        var shelf = new MeshInstance3D
        {
            Mesh = new BoxMesh { Size = new Vector3(0.4f, 0.12f, 0.28f) },
            MaterialOverride = new StandardMaterial3D { AlbedoColor = new Color(0.32f, 0.26f, 0.2f) }
        };
        root.AddChild(shelf);

        var keyMesh = new MeshInstance3D
        {
            Name = "KeyMesh",
            Mesh = new BoxMesh { Size = new Vector3(0.12f, 0.04f, 0.2f) },
            Position = new Vector3(0f, 0.1f, 0f),
            MaterialOverride = new StandardMaterial3D { AlbedoColor = new Color(0.55f, 0.48f, 0.28f) }
        };
        root.AddChild(keyMesh);

        var area = new Area3D
        {
            Name = "InteractionArea",
            CollisionLayer = InteractableLayer,
            CollisionMask = 0,
            Monitoring = false,
            Monitorable = true
        };
        area.AddChild(new CollisionShape3D
        {
            Shape = new BoxShape3D { Size = new Vector3(0.45f, 0.35f, 0.4f) }
        });

        var pickup = new PickupBalconyKeyInteraction { Name = "PickupBalconyKeyInteraction" };
        area.AddChild(pickup);
        pickup.Initialize(state, root);
        root.AddChild(area);
    }

    private static void CreateWingNotes(Node3D parent)
    {
        CreateSimpleNote(
            parent,
            "Interact_Room203_Note",
            new Vector3(3.35f, 3.85f, -6.25f),
            "Ler bilhete",
            "Ele disse que ouviu alguém andando no teto. Ninguém acreditou.",
            "room_203_note",
            PensionNarrativeEvents.EventRoom203Note);

        CreateSimpleNote(
            parent,
            "Interact_OwnerLedger",
            new Vector3(4.4f, 3.9f, -7.05f),
            "Examinar caderno",
            "Os nomes dos hóspedes terminam no mesmo dia. Depois disso, só há páginas rasgadas.",
            "owner_ledger",
            PensionNarrativeEvents.EventOwnerLedgerRead);
    }

    private static void CreateSimpleNote(
        Node3D parent,
        string name,
        Vector3 position,
        string prompt,
        string message,
        string interactionId,
        string narrativeEventId)
    {
        var root = new Node3D { Name = name, Position = position };
        parent.AddChild(root);

        root.AddChild(new MeshInstance3D
        {
            Mesh = new BoxMesh { Size = new Vector3(0.24f, 0.02f, 0.18f) },
            MaterialOverride = new StandardMaterial3D { AlbedoColor = new Color(0.82f, 0.78f, 0.68f) }
        });

        var area = new Area3D
        {
            Name = "InteractionArea",
            CollisionLayer = InteractableLayer,
            CollisionMask = 0,
            Monitoring = false,
            Monitorable = true
        };
        area.AddChild(new CollisionShape3D
        {
            Shape = new BoxShape3D { Size = new Vector3(0.35f, 0.2f, 0.3f) }
        });

        var interactable = new WingNoteInteractable
        {
            Name = $"{name}_Interactable",
            PromptText = prompt,
            InteractionMessage = message,
            InteractionId = interactionId,
            OneShot = true,
            NarrativeEventId = narrativeEventId
        };
        area.AddChild(interactable);
        root.AddChild(area);
        if (!root.IsInGroup("interactable"))
        {
            root.AddToGroup("interactable");
        }
    }
}

/// <summary>One-shot note that also fires a Sprint 17 narrative event.</summary>
public partial class WingNoteInteractable : Node, IInteractable
{
    [Export] public string PromptText { get; set; } = "Interagir";
    [Export] public string InteractionMessage { get; set; } = string.Empty;
    [Export] public string InteractionId { get; set; } = string.Empty;
    [Export] public bool OneShot { get; set; } = true;
    [Export] public string NarrativeEventId { get; set; } = string.Empty;

    private bool _used;

    public override void _Ready()
    {
        var host = GetParent()?.GetParent() ?? GetParent();
        if (host != null && !host.IsInGroup("interactable"))
        {
            host.AddToGroup("interactable");
        }
    }

    public string GetPromptText() => _used && OneShot ? string.Empty : PromptText;

    public void Interact(Node interactor)
    {
        if (_used && OneShot)
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(InteractionMessage))
        {
            HUDController.FindActive(GetTree())?.ShowMessage(InteractionMessage, 3.5f);
        }

        if (OneShot)
        {
            _used = true;
        }

        if (!string.IsNullOrWhiteSpace(NarrativeEventId))
        {
            PensionNarrativeEvents.Find(GetTree())?.TryTrigger(NarrativeEventId);
        }
    }
}
