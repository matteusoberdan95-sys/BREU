namespace BREU.Scripts.Player;

public partial class PlayerInteractor : Node
{
    [Signal] public delegate void FocusChangedEventHandler(string prompt);

    [Export] public NodePath RaycastPath { get; set; } = "../CameraPivot/Camera3D/InteractionRay";
    [Export] public float InteractionDistance { get; set; } = 2.5f;
    [Export(PropertyHint.Layers3DPhysics)] public uint InteractionCollisionMask { get; set; } = 2;

    private RayCast3D? _raycast;
    private IInteractable? _current;
    private string _lastPrompt = "";

    public override void _Ready()
    {
        _raycast = GetNodeOrNull<RayCast3D>(RaycastPath);
        if (_raycast != null)
        {
            _raycast.TargetPosition = new Vector3(0.0f, 0.0f, -InteractionDistance);
            _raycast.CollideWithAreas = true;
            _raycast.CollideWithBodies = false;
            _raycast.CollisionMask = InteractionCollisionMask;
            _raycast.Enabled = true;
        }
    }

    public override void _Process(double delta)
    {
        _current = FindInteractable();
        var prompt = _current?.GetInteractionText() ?? "";

        if (prompt != _lastPrompt)
        {
            _lastPrompt = prompt;
            EmitSignal(SignalName.FocusChanged, prompt);

            if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
            {
                if (string.IsNullOrWhiteSpace(prompt))
                {
                    hud.HideInteractionPrompt();
                }
                else
                {
                    hud.ShowInteractionPrompt(prompt);
                }
            }

            if (!string.IsNullOrWhiteSpace(prompt))
            {
                GD.Print($"Interacao disponivel: {prompt}");
            }
        }

        if (_current != null && Input.IsActionJustPressed("interact") && GetParent() is PlayerController player)
        {
            _current.Interact(player);
        }
    }

    private IInteractable? FindInteractable()
    {
        if (_raycast == null || !_raycast.IsColliding())
        {
            return null;
        }

        var node = _raycast.GetCollider() as Node;
        while (node != null)
        {
            if (node is IInteractable interactable)
            {
                return interactable;
            }

            node = node.GetParent();
        }

        return null;
    }
}
