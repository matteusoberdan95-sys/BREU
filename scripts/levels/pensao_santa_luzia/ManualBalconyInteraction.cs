namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Narrative;

public partial class ManualBalconyInteraction : Node, IInteractable
{
    public enum InteractionMode { Wire, Mirror, Drain, Ledger }
    [Export] public InteractionMode Mode { get; set; }
    private PensaoPuzzleState? _state;
    private bool _done;

    public override void _Ready()
    {
        _state = GetTree().CurrentScene.GetNodeOrNull<PensaoPuzzleState>("PuzzleState");
        GetParent()?.GetParent()?.AddToGroup("interactable");
    }

    public string GetPromptText()
    {
        if (_done || _state == null) return string.Empty;
        return Mode switch
        {
            InteractionMode.Wire => _state.HasWireHook ? string.Empty : "Pegar arame torto",
            InteractionMode.Mirror => "Examinar espelho",
            InteractionMode.Drain => _state.HasWireHook ? "Puxar objeto do ralo" : "Examinar ralo",
            InteractionMode.Ledger => "Examinar caderno",
            _ => string.Empty
        };
    }

    public void Interact(Node interactor)
    {
        if (_done || _state == null) return;
        var hud = HUDController.FindActive(GetTree());
        switch (Mode)
        {
            case InteractionMode.Wire:
                if (_state.HasWireHook)
                {
                    _done = true;
                    GetParent()?.GetParent()?.SetDeferred(Node3D.PropertyName.Visible, false);
                    return;
                }

                _state.PickupWireHook();
                hud?.ShowMessage(
                    "Um arame torto, entre os panos da rouparia. Talvez alcance o objeto preso no ralo do banheiro da varanda.",
                    4.5f);
                _done = true;
                GetParent()?.GetParent()?.SetDeferred(Node3D.PropertyName.Visible, false);
                break;
            case InteractionMode.Mirror:
                _state.ExamineBathroomMirror();
                hud?.ShowMessage("O espelho escuro guarda marcas de dedos sob a sujeira.", 3.5f);
                _done = true;
                break;
            case InteractionMode.Drain:
                if (!_state.HasWireHook)
                {
                    hud?.ShowMessage("Há algo preso no ralo, fora do alcance da mão.", 3.5f);
                    return;
                }
                _state.PickupOwnerRoomKey();
                hud?.ShowMessage("Você puxou uma chave enferrujada do ralo. Parece servir em uma fechadura pequena.", 4f);
                _done = true;
                break;
            case InteractionMode.Ledger:
                _state.ReadOwnerLedger();
                hud?.ShowMessage("Quarto 203... preciso voltar ao corredor do segundo andar. A porta fica deste lado da pensão.", 5f);
                PensionNarrativeEvents.Find(GetTree())?.TryTrigger(PensionNarrativeEvents.EventOwnerLedgerReveal);
                _done = true;
                break;
        }
    }
}
