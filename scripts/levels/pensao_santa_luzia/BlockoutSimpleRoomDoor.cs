namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Sprint 19 — unlocked room door: interact once to open (hide panel + disable blocker).
/// No key. One InteractionArea, one panel, one blocker.
/// </summary>
public partial class BlockoutSimpleRoomDoor : Node3D, IInteractable
{
    [Export] public string ClosedPrompt { get; set; } = "Abrir porta";
    [Export] public string OpenMessage { get; set; } = "A porta range e cede.";

    private MeshInstance3D? _panel;
    private CollisionShape3D? _blockingShape;
    private Area3D? _area;
    private bool _isOpen;

    public override void _Ready() => CallDeferred(nameof(BindNodes));

    private void BindNodes()
    {
        _panel = GetNodeOrNull<MeshInstance3D>("DoorPanel_Visual")
            ?? GetNodeOrNull<MeshInstance3D>("DoorPanel");
        _blockingShape = GetNodeOrNull<CollisionShape3D>("Door_Blocker_StaticBody/CollisionShape3D")
            ?? GetNodeOrNull<CollisionShape3D>("BlockingBody/BlockingShape");
        _area = GetNodeOrNull<Area3D>("InteractionArea");
        if (_area != null && !IsInGroup("interactable"))
        {
            AddToGroup("interactable");
        }

        ApplyState();
    }

    public string GetPromptText() => _isOpen ? string.Empty : ClosedPrompt;

    public void Interact(Node interactor)
    {
        if (_isOpen)
        {
            return;
        }

        _isOpen = true;
        ApplyState();
        var hud = HUDController.FindActive(GetTree());
        hud?.HideInteractionPrompt();
        hud?.ShowMessage(OpenMessage, 2.5f);
    }

    private void ApplyState()
    {
        if (_panel != null)
        {
            _panel.Visible = !_isOpen;
        }

        if (_blockingShape != null)
        {
            _blockingShape.Disabled = _isOpen;
        }

        if (_area != null)
        {
            _area.Monitorable = !_isOpen;
        }
    }
}
