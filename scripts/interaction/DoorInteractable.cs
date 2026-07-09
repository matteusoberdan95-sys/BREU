using Godot;
using BREU.Scripts.Player;

namespace BREU.Scripts.Interaction;

public partial class DoorInteractable : Area3D, IInteractable
{
    [Export] public string ClosedInteractionText { get; set; } = "Abrir porta";
    [Export] public string OpenInteractionText { get; set; } = "Porta aberta";
    [Export] public NodePath[] CollisionShapesToDisable { get; set; } = System.Array.Empty<NodePath>();
    [Export] public string[] VisualNodeNamesToHide { get; set; } = new[] { "door_01" };

    private bool _isOpen;

    public string GetInteractionText()
    {
        return _isOpen ? OpenInteractionText : ClosedInteractionText;
    }

    public void Interact(PlayerController player)
    {
        if (_isOpen)
        {
            GD.Print("A porta ja esta aberta.");
            return;
        }

        _isOpen = true;
        DisableDoorCollisions();
        HideDoorVisuals();
        GD.Print("Porta aberta. Corredor placeholder liberado.");
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
