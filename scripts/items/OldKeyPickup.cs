namespace BREU.Scripts.Items;

/// <summary>
/// Pickup simples da Chave Velha. Integracao com inventario fica para sprint futura.
/// </summary>
public partial class OldKeyPickup : Area3D, IInteractable
{
    [Export] public string InteractionText { get; set; } = "Pegar Chave Velha";
    [Export] public string PickupSoundPath { get; set; } = "res://assets/audio/sfx/player/pickup_item.ogg";
    [Export] public NodePath VisualRootPath { get; set; } = "VisualReference";
    [Export] public bool HasOldKey { get; set; }

    public string GetInteractionText()
    {
        return HasOldKey ? "" : InteractionText;
    }

    public void Interact(PlayerController player)
    {
        if (HasOldKey)
        {
            return;
        }

        HasOldKey = true;
        PlayPickupSound();
        ShowMessage("Chave Velha coletada.");
        GD.Print("Item coletado: Chave Velha");

        if (GetNodeOrNull<Node3D>(VisualRootPath) is { } visual)
        {
            visual.Visible = false;
        }

        Monitoring = false;
        Monitorable = false;

        if (GetNodeOrNull<CollisionShape3D>("CollisionShape3D") is { } shape)
        {
            shape.Disabled = true;
        }

        // TODO: integrar com inventario persistente quando o sistema de chaves existir.
    }

    private void PlayPickupSound()
    {
        var stream = AudioResourceLoader.TryLoad(PickupSoundPath);
        if (stream == null)
        {
            GD.Print($"OldKeyPickup: som nao encontrado: {PickupSoundPath}");
            return;
        }

        if (AudioManager.Find(this) is { } manager)
        {
            manager.Play2DSound(stream);
            return;
        }

        var player = new AudioStreamPlayer { Stream = stream };
        AddChild(player);
        player.Finished += () => player.QueueFree();
        player.Play();
    }

    private void ShowMessage(string message)
    {
        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.ShowTemporaryMessage(message, 3.0f);
            return;
        }

        GD.Print($"HUD mensagem: {message}");
    }
}
