namespace BREU.Scripts.Interaction;

public partial class DoorInteractable : Area3D, IInteractable
{
    [Export] public string ClosedInteractionText { get; set; } = "Abrir porta";
    [Export] public string OpenInteractionText { get; set; } = "Porta aberta";
    [Export] public bool IsLocked { get; set; }
    [Export] public NodePath DoorAudioPath { get; set; } = "DoorAudio";
    [Export] public NodePath SequenceControllerPath { get; set; } = "../../DemoRoomSequenceController";
    [Export] public NodePath[] CollisionShapesToDisable { get; set; } = Array.Empty<NodePath>();
    [Export] public string[] VisualNodeNamesToHide { get; set; } = new[] { "door_01" };

    private bool _isOpen;
    private DoorAudioController? _doorAudio;

    public override void _Ready()
    {
        _doorAudio = GetNodeOrNull<DoorAudioController>(DoorAudioPath);
    }

    public string GetInteractionText()
    {
        return _isOpen ? OpenInteractionText : ClosedInteractionText;
    }

    public void Interact(PlayerController player)
    {
        if (IsLocked)
        {
            _doorAudio?.PlayLocked();
            GD.Print("A porta esta trancada.");
            return;
        }

        if (_isOpen)
        {
            GD.Print("A porta ja esta aberta.");
            return;
        }

        _isOpen = true;
        DisableDoorCollisions();
        HideDoorVisuals();
        _doorAudio?.PlayOpen();
        NotifyDoorOpened();
        GD.Print("Porta aberta. Corredor placeholder liberado.");
    }

    public void CloseDoor()
    {
        if (!_isOpen)
        {
            return;
        }

        _isOpen = false;
        _doorAudio?.PlayClose();
        GD.Print("Porta fechada.");
    }

    private void NotifyDoorOpened()
    {
        if (GetNodeOrNull(SequenceControllerPath) is DemoRoomSequenceController sequence)
        {
            sequence.OnDoorOpened();
            return;
        }

        if (GetTree().GetFirstNodeInGroup("demo_sequence") is DemoRoomSequenceController fallback)
        {
            fallback.OnDoorOpened();
        }
    }

    private void DisableDoorCollisions()
    {
        foreach (var collisionPath in CollisionShapesToDisable)
        {
            var collisionShape = GetNodeOrNull<CollisionShape3D>(collisionPath);
            if (collisionShape != null)
            {
                collisionShape.Disabled = true;
            }
        }
    }

    private void HideDoorVisuals()
    {
        var sceneRoot = GetTree().CurrentScene;
        if (sceneRoot == null)
        {
            return;
        }

        foreach (var visualNodeName in VisualNodeNamesToHide)
        {
            HideVisualByName(sceneRoot, visualNodeName);
        }
    }

    private static bool HideVisualByName(Node node, string visualNodeName)
    {
        var hidAny = false;
        if (node.Name == visualNodeName && node is Node3D visual)
        {
            visual.Visible = false;
            hidAny = true;
        }

        foreach (var child in node.GetChildren())
        {
            hidAny = HideVisualByName(child, visualNodeName) || hidAny;
        }

        return hidAny;
    }
}
