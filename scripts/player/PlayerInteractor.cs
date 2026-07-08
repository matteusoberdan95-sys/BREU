using Godot;
using BREU.Scripts.Interaction;

namespace BREU.Scripts.Player;

public partial class PlayerInteractor : Node
{
    [Signal] public delegate void FocusChangedEventHandler(string prompt);

    [Export] public NodePath RaycastPath { get; set; } = "../CameraPivot/Camera3D/InteractionRay";

    private RayCast3D? _raycast;
    private IInteractable? _current;
    private string _lastPrompt = "";

    public override void _Ready()
    {
        _raycast = GetNodeOrNull<RayCast3D>(RaycastPath);
    }

    public override void _Process(double delta)
    {
        _current = FindInteractable();
        var prompt = _current?.Prompt ?? "";

        if (prompt != _lastPrompt)
        {
            _lastPrompt = prompt;
            EmitSignal(SignalName.FocusChanged, prompt);
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
