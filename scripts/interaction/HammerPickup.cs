namespace BREU.Scripts.Interaction;

public partial class HammerPickup : Area3D, IInteractable
{
    [Export] public string InteractionText { get; set; } = "Pegar Martelo Enferrujado";
    [Export] public int Durability { get; set; } = 10;
    [Export] public NodePath[] VisualNodesToHide { get; set; } = System.Array.Empty<NodePath>();
    [Export] public string[] FallbackVisualNodeNames { get; set; } = new[] { "hammer_handle", "hammer_head" };
    [Export] public NodePath CollisionShapePath { get; set; } = "CollisionShape3D";
    [Export] public NodePath SequenceControllerPath { get; set; } = "../../DemoRoomSequenceController";

    private bool _collected;
    private CollisionShape3D? _collisionShape;

    public override void _Ready()
    {
        _collisionShape = GetNodeOrNull<CollisionShape3D>(CollisionShapePath);
    }

    public string GetInteractionText()
    {
        return _collected ? "" : InteractionText;
    }

    public void Interact(PlayerController player)
    {
        if (_collected)
        {
            return;
        }

        _collected = true;

        var session = GetNodeOrNull<GameSession>("/root/GameSession");
        if (session != null)
        {
            session.EquipRustyHammer();
        }

        player.GetNodeOrNull<PlayerInventory>("PlayerInventory")?.PickupHammer(Durability);
        player.GetNodeOrNull<PlayerWeaponController>("PlayerWeaponController")?.RefreshWeaponFromSession();

        var hidConfiguredVisual = HideConfiguredVisuals();
        var hidFallbackVisual = hidConfiguredVisual || HideFallbackVisuals();
        if (!hidFallbackVisual)
        {
            GD.Print("Martelo coletado, mas nenhum visual foi configurado para esconder.");
        }

        if (_collisionShape != null)
        {
            _collisionShape.Disabled = true;
        }

        Monitoring = false;
        Monitorable = false;

        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.ShowTemporaryMessage("Martelo Enferrujado coletado.");
        }

        AudioManager.Find(this)?.Play2DSound(AudioResourceLoader.TryLoad(AudioPaths.PlayerPickupItem));

        NotifyHammerPicked();
        GD.Print($"Martelo Enferrujado coletado. Durabilidade: {Durability}/{Durability}.");
    }

    private void NotifyHammerPicked()
    {
        if (GetNodeOrNull(SequenceControllerPath) is DemoRoomSequenceController sequence)
        {
            sequence.OnHammerPicked();
            return;
        }

        if (GetTree().GetFirstNodeInGroup("demo_sequence") is DemoRoomSequenceController fallback)
        {
            fallback.OnHammerPicked();
        }
    }

    private bool HideConfiguredVisuals()
    {
        var hidAny = false;
        foreach (var visualPath in VisualNodesToHide)
        {
            var visual = GetNodeOrNull<Node3D>(visualPath);
            if (visual == null)
            {
                continue;
            }

            visual.Visible = false;
            hidAny = true;
        }

        return hidAny;
    }

    private bool HideFallbackVisuals()
    {
        var sceneRoot = GetTree().CurrentScene;
        if (sceneRoot == null)
        {
            return false;
        }

        var hidAny = false;
        foreach (var visualNodeName in FallbackVisualNodeNames)
        {
            hidAny = HideVisualByName(sceneRoot, visualNodeName) || hidAny;
        }

        return hidAny;
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
