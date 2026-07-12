namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;
using BREU.Scripts.Interaction;
using BREU.Scripts.Narrative;

/// <summary>Sprint 17C — balcony wing horror puzzle interactions.</summary>
public partial class PensaoBalconyWingPuzzleSetup : Node
{
    private const uint InteractableLayer = 2;

    public override void _Ready() => CallDeferred(nameof(Setup));

    private void Setup()
    {
        var state = GetNodeOrNull<PensaoPuzzleState>("../../PuzzleState");
        var interactions = GetNodeOrNull<Node3D>("../../Interactions");
        var secondFloor = GetNodeOrNull<Node3D>("../../PensionSecondFloor");
        if (state == null || interactions == null || secondFloor == null)
        {
            GD.PushError("[WingPuzzle] Missing PuzzleState / Interactions / SecondFloor.");
            return;
        }

        var host = interactions.GetNodeOrNull<Node3D>("BalconyWingPuzzleItems")
            ?? new Node3D { Name = "BalconyWingPuzzleItems" };
        if (host.GetParent() == null)
        {
            interactions.AddChild(host);
        }

        var ownerDoor = secondFloor.FindChild("Room_OwnerDoor", recursive: true, owned: false) as BlockoutOwnerBedroomDoor;
        if (ownerDoor != null)
        {
            ownerDoor.Initialize(state, ownerDoor);
        }
        else
        {
            GD.PushWarning("[WingPuzzle] Room_OwnerDoor not found.");
        }

        CreateWireHook(state, host);
        CreateBathroomDoor(host, secondFloor);
        CreateBathroomMirror(state, host, secondFloor);
        CreateBathroomDrain(state, host, secondFloor);
        CreateOwnerLedger(state, host, secondFloor);
        CreateBalconyEdgeHint(host);

        state.OwnerLedgerRead += () =>
            PensionNarrativeEvents.Find(GetTree())?.TryTrigger(PensionNarrativeEvents.EventOwnerLedgerReveal);

        GD.Print("[WingPuzzle] Wired balcony wing horror puzzle.");
    }

    private static Vector3 ResolveAnchor(Node3D secondFloor, string anchorName, Vector3 fallback)
    {
        if (secondFloor.FindChild(anchorName, recursive: true, owned: false) is Node3D marker)
        {
            return marker.GlobalPosition;
        }

        return fallback;
    }

    private static void CreateWireHook(PensaoPuzzleState state, Node3D parent)
    {
        var root = new Node3D
        {
            Name = "Interact_BalconyWireHook",
            Position = new Vector3(-0.9f, 3.55f, -3.5f)
        };
        parent.AddChild(root);

        root.AddChild(new MeshInstance3D
        {
            Mesh = new BoxMesh { Size = new Vector3(0.35f, 0.04f, 0.08f) },
            MaterialOverride = new StandardMaterial3D { AlbedoColor = new Color(0.45f, 0.42f, 0.35f) }
        });

        var area = MakeArea(new Vector3(0.4f, 0.25f, 0.35f));
        var pickup = new WingWireHookInteraction { Name = "WingWireHookInteraction" };
        area.AddChild(pickup);
        pickup.Initialize(state, root);
        root.AddChild(area);
        TagInteractable(root);
    }

    private static void CreateBathroomMirror(PensaoPuzzleState state, Node3D parent, Node3D secondFloor)
    {
        var pos = ResolveAnchor(secondFloor, "Anchor_BathroomMirror", new Vector3(2.2f, 4.15f, -4.7f));
        var root = new Node3D
        {
            Name = "Interact_BathroomMirror",
            Position = pos
        };
        parent.AddChild(root);

        root.AddChild(new MeshInstance3D
        {
            Mesh = new BoxMesh { Size = new Vector3(0.55f, 0.7f, 0.06f) },
            MaterialOverride = new StandardMaterial3D { AlbedoColor = new Color(0.12f, 0.14f, 0.16f) }
        });

        var area = MakeArea(new Vector3(0.55f, 0.7f, 0.4f));
        var interact = new BathroomMirrorInteraction { Name = "BathroomMirrorInteraction" };
        area.AddChild(interact);
        interact.Initialize(state, root);
        root.AddChild(area);
        TagInteractable(root);
    }

    private static void CreateBathroomDoor(Node3D parent, Node3D secondFloor)
    {
        var pos = ResolveAnchor(secondFloor, "Anchor_BathroomDoor", new Vector3(1.45f, 4.15f, -5.35f));
        var root = new Node3D { Name = "Interact_BathroomDoor", Position = pos };
        parent.AddChild(root);

        var area = MakeArea(new Vector3(0.22f, 1.0f, 0.9f));
        var interactable = new Interactable
        {
            Name = "BathroomDoorInteraction",
            PromptText = "Entrar no banheiro",
            InteractionMessage = "O banheiro está aberto.",
            InteractionId = "bathroom_door",
            OneShot = false
        };
        area.AddChild(interactable);
        root.AddChild(area);
        TagInteractable(root);
    }

    private static void CreateBathroomDrain(PensaoPuzzleState state, Node3D parent, Node3D secondFloor)
    {
        var pos = ResolveAnchor(secondFloor, "Anchor_BathroomDrain", new Vector3(2.9f, 2.9f, -4.1f));
        var root = new Node3D
        {
            Name = "Interact_BathroomDrain",
            Position = pos
        };
        parent.AddChild(root);

        root.AddChild(new MeshInstance3D
        {
            Mesh = new BoxMesh { Size = new Vector3(0.28f, 0.04f, 0.28f) },
            MaterialOverride = new StandardMaterial3D { AlbedoColor = new Color(0.2f, 0.18f, 0.16f) }
        });

        var area = MakeArea(new Vector3(0.45f, 0.35f, 0.45f));
        var interact = new BathroomDrainInteraction { Name = "BathroomDrainInteraction" };
        area.AddChild(interact);
        interact.Initialize(state, root);
        root.AddChild(area);
        TagInteractable(root);
    }

    private static void CreateOwnerLedger(PensaoPuzzleState state, Node3D parent, Node3D secondFloor)
    {
        var pos = ResolveAnchor(secondFloor, "Anchor_OwnerLedger", new Vector3(5.0f, 3.65f, -4.8f));
        var root = new Node3D
        {
            Name = "Interact_OwnerLedger",
            Position = pos
        };
        parent.AddChild(root);

        root.AddChild(new MeshInstance3D
        {
            Mesh = new BoxMesh { Size = new Vector3(0.28f, 0.04f, 0.22f) },
            MaterialOverride = new StandardMaterial3D { AlbedoColor = new Color(0.35f, 0.22f, 0.16f) }
        });

        var area = MakeArea(new Vector3(0.4f, 0.25f, 0.35f));
        var interact = new OwnerLedgerInteraction { Name = "OwnerLedgerInteraction" };
        area.AddChild(interact);
        interact.Initialize(state, root);
        root.AddChild(area);
        TagInteractable(root);
    }

    private static void CreateBalconyEdgeHint(Node3D parent)
    {
        var root = new Node3D
        {
            Name = "Interact_BalconyLookDown",
            Position = new Vector3(0f, 3.02f, -3.42f)
        };
        parent.AddChild(root);

        // Low and shallow: the ray only catches it while looking down at the
        // actual outer edge, never from the green door or the circulation path.
        var area = MakeArea(new Vector3(1.4f, 0.22f, 0.18f));
        var interactable = new Interactable
        {
            Name = "BalconyEdgeHint",
            PromptText = "Olhar para baixo",
            InteractionMessage = "Está alto demais para descer por aqui.",
            InteractionId = "balcony_edge",
            OneShot = false
        };
        area.AddChild(interactable);
        root.AddChild(area);
        TagInteractable(root);
    }

    private static Area3D MakeArea(Vector3 size)
    {
        var area = new Area3D
        {
            Name = "InteractionArea",
            CollisionLayer = InteractableLayer,
            CollisionMask = 0,
            Monitoring = false,
            Monitorable = true
        };
        area.AddChild(new CollisionShape3D { Shape = new BoxShape3D { Size = size } });
        return area;
    }

    private static void TagInteractable(Node3D root)
    {
        if (!root.IsInGroup("interactable"))
        {
            root.AddToGroup("interactable");
        }
    }
}

public partial class WingWireHookInteraction : Node, IInteractable
{
    private PensaoPuzzleState? _state;
    private Node3D? _root;
    private bool _taken;

    public void Initialize(PensaoPuzzleState state, Node3D root)
    {
        _state = state;
        _root = root;
    }

    public string GetPromptText() => _taken ? string.Empty : "Pegar arame torto";

    public void Interact(Node interactor)
    {
        if (_taken || _state == null)
        {
            return;
        }

        _taken = true;
        _state.PickupWireHook();
        HUDController.FindActive(GetTree())?.ShowMessage(
            "Um arame torto. Talvez alcance o objeto preso no ralo do banheiro.", 4.0f);
        if (_root != null)
        {
            _root.Visible = false;
        }
    }
}

public partial class BathroomMirrorInteraction : Node, IInteractable
{
    private PensaoPuzzleState? _state;
    private bool _done;

    public void Initialize(PensaoPuzzleState state, Node3D root)
    {
        _state = state;
        if (!root.IsInGroup("interactable"))
        {
            root.AddToGroup("interactable");
        }
    }

    public string GetPromptText() => _done ? string.Empty : "Examinar espelho";

    public void Interact(Node interactor)
    {
        if (_done || _state == null)
        {
            return;
        }

        _done = true;
        _state.ExamineBathroomMirror();
        HUDController.FindActive(GetTree())?.ShowMessage(
            "O espelho está coberto por uma camada escura. Por baixo dela, há marcas de dedos.", 4.0f);
        PensionAudioManager.Find(GetTree())?.PlayOneShot("door_scratch_01", -16f);
    }
}

public partial class BathroomDrainInteraction : Node, IInteractable
{
    private PensaoPuzzleState? _state;
    private bool _gotKey;

    public void Initialize(PensaoPuzzleState state, Node3D root)
    {
        _state = state;
        if (!root.IsInGroup("interactable"))
        {
            root.AddToGroup("interactable");
        }
    }

    public string GetPromptText()
    {
        if (_gotKey || _state == null)
        {
            return string.Empty;
        }

        return _state.HasWireHook ? "Puxar objeto do ralo" : "Examinar ralo";
    }

    public void Interact(Node interactor)
    {
        if (_gotKey || _state == null)
        {
            return;
        }

        var hud = HUDController.FindActive(GetTree());
        if (!_state.HasWireHook)
        {
            hud?.ShowMessage("Há algo preso lá dentro, mas não consigo alcançar com a mão.", 3.5f);
            return;
        }

        _gotKey = true;
        _state.PickupOwnerRoomKey();
        hud?.ShowMessage("Consegui puxar uma chave pequena, coberta de sujeira.", 3.5f);
        PensionAudioManager.Find(GetTree())?.PlayOneShot("water_drop_02", -12f);
    }
}

public partial class OwnerLedgerInteraction : Node, IInteractable
{
    private PensaoPuzzleState? _state;
    private bool _read;

    public void Initialize(PensaoPuzzleState state, Node3D root)
    {
        _state = state;
        if (!root.IsInGroup("interactable"))
        {
            root.AddToGroup("interactable");
        }
    }

    public string GetPromptText() => _read ? string.Empty : "Examinar caderno";

    public void Interact(Node interactor)
    {
        if (_read || _state == null)
        {
            return;
        }

        _read = true;
        // Flag + event; narrative queue shows ledger lines then tension beat.
        _state.ReadOwnerLedger();
    }
}
