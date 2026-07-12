namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Interaction;

/// <summary>Sprint 17 — owner note that reveals where the balcony key is hidden.</summary>
public partial class BalconyNoteInteraction : Node, IInteractable
{
    private const string Prompt = "Ler anotação";
    private const string Message =
        "A varanda emperrou de novo. Dona Luzia guardou a chave perto da recepção, onde ninguém procuraria.";

    private PensaoPuzzleState? _state;
    private bool _read;

    public void Initialize(PensaoPuzzleState state, Node3D host)
    {
        _state = state;
        if (!host.IsInGroup("interactable"))
        {
            host.AddToGroup("interactable");
        }
    }

    public string GetPromptText() => _read ? string.Empty : Prompt;

    public void Interact(Node interactor)
    {
        if (_read || _state == null)
        {
            return;
        }

        _read = true;
        _state.ReadBalconyNote();
        HUDController.FindActive(GetTree())?.ShowMessage(Message, 4.0f);
    }
}
