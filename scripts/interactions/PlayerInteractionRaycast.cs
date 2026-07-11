namespace BREU.Scripts.Interaction;

/// <summary>
/// Camera-forward raycast for E-key interaction. Does not modify player movement.
/// </summary>
public partial class PlayerInteractionRaycast : Node
{
    [Export] public NodePath RaycastPath { get; set; } =
        new("HeadBase/BodyMotionPivot/LeanPivot/LookBackPivot/Camera3D/InteractionRaycast");

    [Export] public float InteractionDistance { get; set; } = 2.5f;
    [Export] public bool DebugMode { get; set; }

    private RayCast3D? _raycast;
    private IInteractable? _focusedInteractable;
    private Node? _debugTarget;

    public IInteractable? FocusedInteractable => _focusedInteractable;

    public override void _Ready()
    {
        _raycast = GetNodeOrNull<RayCast3D>(RaycastPath);
        if (_raycast == null)
        {
            GD.PushError("[Interaction] InteractionRaycast node not found.");
            return;
        }

        _raycast.TargetPosition = new Vector3(0.0f, 0.0f, -InteractionDistance);
        _raycast.Enabled = true;
        _raycast.HitFromInside = false;
        _raycast.CollideWithAreas = true;
        _raycast.CollideWithBodies = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_raycast == null)
        {
            return;
        }

        _raycast.TargetPosition = new Vector3(0.0f, 0.0f, -InteractionDistance);
        _raycast.ForceRaycastUpdate();

        UpdateFocus(FindInteractable(_raycast.GetCollider() as Node));
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!@event.IsActionPressed("interact"))
        {
            return;
        }

        _focusedInteractable?.Interact(GetParent());
    }

    private void UpdateFocus(IInteractable? interactable)
    {
        if (_focusedInteractable == interactable)
        {
            return;
        }

        _focusedInteractable = interactable;
        var hud = HUDController.FindActive(GetTree());

        if (_focusedInteractable == null)
        {
            hud?.ClearInteractionPrompt();
            LogDebugTarget(null);
            return;
        }

        var prompt = _focusedInteractable.GetPromptText();
        if (string.IsNullOrWhiteSpace(prompt))
        {
            hud?.ClearInteractionPrompt();
            LogDebugTarget(null);
            return;
        }

        hud?.ShowInteractionPrompt(prompt);
        LogDebugTarget(FindInteractableNode(_raycast?.GetCollider() as Node));
    }

    private static IInteractable? FindInteractable(Node? collider)
    {
        var current = collider;
        while (current != null)
        {
            if (current is IInteractable onSelf)
            {
                return onSelf;
            }

            foreach (var child in current.GetChildren())
            {
                if (child is IInteractable onChild)
                {
                    return onChild;
                }
            }

            current = current.GetParent();
        }

        return null;
    }

    private static Node? FindInteractableNode(Node? collider)
    {
        var current = collider;
        while (current != null)
        {
            if (current is IInteractable)
            {
                return current;
            }

            foreach (var child in current.GetChildren())
            {
                if (child is IInteractable)
                {
                    return child;
                }
            }

            current = current.GetParent();
        }

        return null;
    }

    private void LogDebugTarget(Node? target)
    {
        if (!DebugMode)
        {
            return;
        }

        if (_debugTarget == target)
        {
            return;
        }

        _debugTarget = target;
        if (target == null)
        {
            GD.Print("[Interaction] No interactable in view.");
            return;
        }

        GD.Print($"Looking at interactable: {target.Name}");
    }
}
