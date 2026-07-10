namespace BREU.Scripts.Levels;

/// <summary>
/// Porta de saida bloqueada da Sala dos Santos Secos.
/// </summary>
public partial class RitualExitDoorTrigger : Area3D, IInteractable
{
    [Export] public string InteractionText { get; set; } = "Abrir porta";
    [Export] public string LockedMessage { get; set; } = "A porta esta trancada. Alguma coisa precisa ser feita primeiro.";

    private bool _recentBodyMessage;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    public string GetInteractionText()
    {
        return InteractionText;
    }

    public void Interact(PlayerController player)
    {
        ShowLockedMessage();
    }

    private async void OnBodyEntered(Node3D body)
    {
        if (_recentBodyMessage || !body.IsInGroup("player"))
        {
            return;
        }

        _recentBodyMessage = true;
        ShowLockedMessage();
        await ToSignal(GetTree().CreateTimer(2.0f), SceneTreeTimer.SignalName.Timeout);
        _recentBodyMessage = false;
    }

    private void ShowLockedMessage()
    {
        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.ShowTemporaryMessage(LockedMessage, 3.5f);
        }
        else
        {
            GD.Print($"HUD mensagem: {LockedMessage}");
        }

        GD.Print("Ritual exit door bloqueada.");
        // TODO: liberar esta saida quando o proximo objetivo da sala existir.
    }
}
