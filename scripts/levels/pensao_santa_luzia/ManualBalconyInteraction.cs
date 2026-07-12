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
            InteractionMode.Wire => "Pegar arame torto",
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
                _state.PickupWireHook();
                hud?.ShowMessage("Um arame torto. Talvez alcance o objeto preso no ralo do banheiro.", 4f);
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
                hud?.ShowMessage("O arame puxa uma pequena chave coberta de sujeira.", 3.5f);
                _done = true;
                break;
            case InteractionMode.Ledger:
                _state.ReadOwnerLedger();
                PensionNarrativeEvents.Find(GetTree())?.TryTrigger(PensionNarrativeEvents.EventOwnerLedgerReveal);
                _done = true;
                break;
        }
    }
}
