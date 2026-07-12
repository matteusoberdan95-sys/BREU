namespace BREU.Scripts.Interaction;

/// <summary>
/// Camera-forward raycast for E-key interaction. Does not modify player movement.
/// </summary>
public partial class PlayerInteractionRaycast : Node
{
    public const uint WorldCollisionLayer = 1;
    public const uint InteractableCollisionLayer = 2;
    public const uint RaycastCollisionMask = WorldCollisionLayer | InteractableCollisionLayer;

    [Export] public NodePath RaycastPath { get; set; } =
        new("HeadBase/BodyMotionPivot/LeanPivot/LookBackPivot/Camera3D/InteractionRaycast");

    [Export] public float InteractionDistance { get; set; } = 3.0f;
    [Export] public bool DebugMode { get; set; }

    private Node? _player;
    private RayCast3D? _raycast;
    private IInteractable? _focusedInteractable;
    private Node? _debugTarget;

    public IInteractable? FocusedInteractable => _focusedInteractable;

    public override void _Ready()
    {
        _player = GetParent();

        if (_player == null)
        {
            GD.PushError("[Interaction] PlayerInteractionRaycast must be a child of Player.");
            return;
        }

        _raycast = _player.GetNodeOrNull<RayCast3D>(RaycastPath);
        if (_raycast == null)
        {
            GD.PushError($"[Interaction] RayCast3D not found at '{RaycastPath}' from Player.");
            return;
        }

        ApplyRaycastSettings();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_raycast == null)
        {
            return;
        }

        ApplyRaycastSettings();
        _raycast.ForceRaycastUpdate();

        UpdateFocus(FindFocusedInteractable());
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!@event.IsActionPressed("interact"))
        {
            return;
        }

        if (_focusedInteractable == null)
        {
            if (DebugMode)
            {
                GD.Print("No interactable target.");
            }

            return;
        }

        var targetName = ResolveDebugTargetName(_raycast?.GetCollider() as Node);
        if (DebugMode)
        {
            GD.Print($"Interacted with: {targetName}");
        }

        _focusedInteractable.Interact(_player!);
    }

    private void ApplyRaycastSettings()
    {
        if (_raycast == null)
        {
            return;
        }

        _raycast.TargetPosition = new Vector3(0.0f, 0.0f, -InteractionDistance);
        _raycast.Enabled = true;
        _raycast.HitFromInside = false;
        _raycast.CollideWithAreas = true;
        _raycast.CollideWithBodies = true;
        _raycast.CollisionMask = RaycastCollisionMask;
    }

    private IInteractable? FindFocusedInteractable()
    {
        if (_raycast == null)
        {
            return null;
        }

        var collider = _raycast.GetCollider() as Node;
        var interactable = FindInteractable(collider);
        if (interactable == null)
        {
            return null;
        }

        if (IsOccludedByWorldGeometry(collider as Node3D))
        {
            return null;
        }

        return interactable;
    }

    private bool IsOccludedByWorldGeometry(Node3D? targetCollider)
    {
        if (_raycast == null || targetCollider == null)
        {
            return false;
        }

        var camera = _raycast.GetParent() as Camera3D;
        if (camera == null)
        {
            return false;
        }

        var targetPosition = targetCollider.GlobalPosition;
        var origin = camera.GlobalPosition;
        var toTarget = targetPosition - origin;
        var distance = toTarget.Length();
        if (distance <= 0.01f)
        {
            return false;
        }

        var direction = toTarget / distance;
        var world = _raycast.GetWorld3D().DirectSpaceState;
        var query = PhysicsRayQueryParameters3D.Create(origin, origin + direction * (distance + 0.05f));
        query.CollisionMask = WorldCollisionLayer;
        query.CollideWithAreas = false;
        query.CollideWithBodies = true;
        query.HitBackFaces = false;

        var result = world.IntersectRay(query);
        if (result.Count == 0)
        {
            return false;
        }

        var hitPosition = (Vector3)result["position"];
        var hitDistance = origin.DistanceTo(hitPosition);
        return hitDistance < distance - 0.15f;
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
            hud?.HideInteractionPrompt();
            LogDebugTarget(null, null);
            return;
        }

        var prompt = _focusedInteractable.GetPromptText();
        if (string.IsNullOrWhiteSpace(prompt))
        {
            hud?.HideInteractionPrompt();
            LogDebugTarget(null, null);
            return;
        }

        hud?.ShowInteractionPrompt(prompt);
        LogDebugTarget(FindInteractableNode(_raycast?.GetCollider() as Node), prompt);
    }

    public static IInteractable? FindInteractable(Node? collider)
    {
        var current = collider;
        var depth = 0;

        while (current != null && depth < 8)
        {
            if (current is IInteractable onSelf)
            {
                return onSelf;
            }

            if (TryGetInteractableFromGroupMember(current, out var fromGroup))
            {
                return fromGroup;
            }

            // Only the collider itself may own an interactable child (the standard
            // Area3D/StaticBody3D pattern). Scanning children after climbing to a
            // shared level container makes unrelated sibling doors interactable.
            if (depth == 0)
            {
                foreach (var child in current.GetChildren())
                {
                    if (child is IInteractable onChild)
                    {
                        return onChild;
                    }

                    if (TryGetInteractableFromGroupMember(child, out fromGroup))
                    {
                        return fromGroup;
                    }
                }
            }

            current = current.GetParent();
            depth++;
        }

        return null;
    }

    private static bool TryGetInteractableFromGroupMember(Node node, out IInteractable? interactable)
    {
        interactable = null;
        if (!node.IsInGroup("interactable"))
        {
            return false;
        }

        foreach (var child in node.GetChildren())
        {
            if (child is IInteractable found)
            {
                interactable = found;
                return true;
            }
        }

        return false;
    }

    private static Node? FindInteractableNode(Node? collider)
    {
        var interactable = FindInteractable(collider);
        return interactable as Node;
    }

    private static string ResolveDebugTargetName(Node? collider)
    {
        var node = FindInteractableNode(collider);
        if (node == null)
        {
            return "unknown";
        }

        if (node.GetParent() != null && node.GetParent()!.Name != "Player")
        {
            return node.GetParent()!.Name;
        }

        return node.Name;
    }

    private void LogDebugTarget(Node? target, string? prompt)
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

        var name = ResolveDebugTargetName(_raycast?.GetCollider() as Node);
        GD.Print($"Interaction target: {name} | prompt: {prompt}");
    }
}
