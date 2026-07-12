namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;

public partial class UpperWingAmbientInteraction : Node, IInteractable
{
    public enum AmbientMode { LinenCloset, Room204 }
    [Export] public AmbientMode Mode { get; set; }
    private PensaoPuzzleState? _state;

    public override void _Ready()
    {
        _state = GetTree().CurrentScene.GetNodeOrNull<PensaoPuzzleState>("PuzzleState");
        GetParent()?.GetParent()?.AddToGroup("interactable");
    }

    public string GetPromptText() => Mode == AmbientMode.LinenCloset
        ? "Examinar rouparia"
        : "Tentar abrir Quarto 204";

    public void Interact(Node interactor)
    {
        var afterWarning = _state?.HasTriggeredRoom203Warning == true;
        var message = Mode switch
        {
            AmbientMode.LinenCloset when afterWarning => "Há marcas de unha no batente. Elas vêm do lado de fora.",
            AmbientMode.LinenCloset => "Lençóis úmidos cobrem alguma coisa no fundo. Prefiro não mexer muito nisso.",
            AmbientMode.Room204 when afterWarning => "Por um segundo, ouvi algo respirando do outro lado.",
            _ => "A maçaneta não se move. Há um cheiro forte vindo de dentro."
        };
        HUDController.FindActive(GetTree())?.ShowMessage(message, 4f);
        PensionAudioManager.Find(GetTree())?.PlayOneShot(
            Mode == AmbientMode.LinenCloset ? "old_house_settle_01" : "door_scratch_02", -17f);
    }
}
