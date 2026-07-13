namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;

public partial class DownstairsClueAfter203 : Node, IInteractable
{
    private PensaoPuzzleState? _state;

    public override void _Ready()
    {
        _state = GetTree().CurrentScene?.GetNodeOrNull<PensaoPuzzleState>("PuzzleState");
        GetParent()?.AddToGroup("interactable");
    }

    public string GetPromptText() => _state switch
    {
        { FirstPresencePlayed: true, DownstairsClueCollected: false } => "Examinar registro rasgado",
        _ => string.Empty
    };

    public void Interact(Node interactor)
    {
        if (_state?.FirstPresencePlayed != true || _state.DownstairsClueCollected) return;
        _state.CollectDownstairsClue();
        PensionAudioManager.Find(GetTree())?.PlayOneShot("old_house_settle_02", -18f);
        HUDController.FindActive(GetTree())?.ShowMessage(
            "O nome do hóspede do 203 foi raspado do papel. Embaixo, alguém escreveu: 'não olhe se ele bater'. Objetivo: O barulho veio do fundo da pensão.", 8f);
        if (GetParent() is Area3D area) area.SetDeferred(Area3D.PropertyName.Monitoring, false);
    }
}
