namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Interaction;

/// <summary>
/// Sprint 07 — wires deposit key puzzle after blockout builder finishes.
/// </summary>
public partial class PensaoTerreoPuzzleSetup : Node
{
    private const uint InteractableLayer = 2;

    public override void _Ready()
    {
        CallDeferred(nameof(SetupPuzzle));
    }

    private void SetupPuzzle()
    {
        var state = GetNodeOrNull<PensaoPuzzleState>("../../PuzzleState");
        var interactions = GetNodeOrNull<Node3D>("../../Interactions");
        var depositArea = GetNodeOrNull<Node3D>("../../PensionGroundFloor/DepositArea");
        var door = depositArea?.GetNodeOrNull<StaticBody3D>("Door_Deposit_Blocked");

        if (state == null || interactions == null || door == null)
        {
            GD.PushError("[Puzzle] Missing PuzzleState, Interactions or Door_Deposit_Blocked.");
            return;
        }

        var puzzleItems = new Node3D { Name = "PuzzleItems" };
        interactions.AddChild(puzzleItems);

        SetupDepositDoor(state, door);
        CreateKeyPickup(state, puzzleItems);
        CreateFusePickup(state, puzzleItems);
        CreateDepositNote(puzzleItems);
    }

    private static void SetupDepositDoor(PensaoPuzzleState state, StaticBody3D door)
    {
        foreach (var child in door.GetChildren())
        {
            if (child is Interactable legacy)
            {
                legacy.QueueFree();
            }
        }

        var interaction = new DepositDoorInteraction { Name = "DepositDoorInteraction" };
        door.AddChild(interaction);
        interaction.Initialize(state, door);
    }

    private static void CreateKeyPickup(PensaoPuzzleState state, Node3D parent)
    {
        var root = new Node3D
        {
            Name = "Key_Deposit_Old",
            Position = new Vector3(-5.0f, 0.75f, -14.2f)
        };
        parent.AddChild(root);

        var nightstandMesh = new MeshInstance3D
        {
            Mesh = new BoxMesh { Size = new Vector3(0.45f, 0.5f, 0.35f) },
            Position = new Vector3(0, -0.25f, 0),
            MaterialOverride = new StandardMaterial3D { AlbedoColor = new Color(0.4f, 0.3f, 0.22f) }
        };
        root.AddChild(nightstandMesh);

        var keyMesh = new MeshInstance3D
        {
            Mesh = new BoxMesh { Size = new Vector3(0.14f, 0.05f, 0.22f) },
            Position = new Vector3(0, 0.08f, 0),
            MaterialOverride = new StandardMaterial3D { AlbedoColor = new Color(0.72f, 0.62f, 0.28f) }
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
            Shape = new BoxShape3D { Size = new Vector3(0.35f, 0.25f, 0.35f) }
        });

        var pickup = new PickupKeyInteraction { Name = "PickupKeyInteraction" };
        area.AddChild(pickup);
        pickup.Initialize(state, root);
        root.AddChild(area);
    }

    private static void CreateFusePickup(PensaoPuzzleState state, Node3D parent)
    {
        var root = new Node3D
        {
            Name = "Fuse_Old",
            Position = new Vector3(0, 0.55f, -30.0f)
        };
        parent.AddChild(root);

        var fuseMesh = new MeshInstance3D
        {
            Mesh = new BoxMesh { Size = new Vector3(0.18f, 0.12f, 0.35f) },
            MaterialOverride = new StandardMaterial3D { AlbedoColor = new Color(0.35f, 0.32f, 0.28f) }
        };
        root.AddChild(fuseMesh);

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
            Shape = new BoxShape3D { Size = new Vector3(0.35f, 0.25f, 0.45f) }
        });

        var pickup = new PickupFuseInteraction { Name = "PickupFuseInteraction" };
        area.AddChild(pickup);
        pickup.Initialize(state, root);
        root.AddChild(area);
    }

    private static void CreateDepositNote(Node3D parent)
    {
        var root = new Node3D
        {
            Name = "DepositNote",
            Position = new Vector3(0.65f, 1.05f, -29.6f)
        };
        parent.AddChild(root);

        var noteMesh = new MeshInstance3D
        {
            Mesh = new BoxMesh { Size = new Vector3(0.22f, 0.02f, 0.16f) },
            MaterialOverride = new StandardMaterial3D { AlbedoColor = new Color(0.85f, 0.82f, 0.72f) }
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
            Shape = new BoxShape3D { Size = new Vector3(0.28f, 0.12f, 0.22f) }
        });

        var interactable = new Interactable
        {
            Name = "DepositNoteInteractable",
            PromptText = "Ler bilhete",
            InteractionMessage = "Não deixem os novos funcionários sozinhos depois das 22h.",
            InteractionId = "deposit_note",
            OneShot = true
        };
        area.AddChild(interactable);
        root.AddChild(area);

        if (!root.IsInGroup("interactable"))
        {
            root.AddToGroup("interactable");
        }
    }
}
