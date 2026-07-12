namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;

public partial class UpperWingAmbientInteraction : Node, IInteractable
{
    public enum AmbientMode { LinenCloset, Room204Bed, BathroomB }
    [Export] public AmbientMode Mode { get; set; }
    private PensaoPuzzleState? _state;

    public override void _Ready()
    {
        _state = GetTree().CurrentScene.GetNodeOrNull<PensaoPuzzleState>("PuzzleState");
        GetParent()?.GetParent()?.AddToGroup("interactable");
    }

    public string GetPromptText() => Mode switch
    {
        AmbientMode.LinenCloset => "Examinar rouparia",
        AmbientMode.Room204Bed => "Examinar cama do 204",
        _ => "Examinar banheiro antigo"
    };

    public void Interact(Node interactor)
    {
        var afterWarning = _state?.HasTriggeredRoom203Warning == true;
        var message = Mode switch
        {
            AmbientMode.LinenCloset when afterWarning => "Há marcas de unha no batente. Elas vêm do lado de fora.",
            AmbientMode.LinenCloset => "Lençóis úmidos cobrem alguma coisa no fundo. Prefiro não mexer muito nisso.",
            AmbientMode.Room204Bed when afterWarning => "As marcas na parede parecem recentes. Tem alguma coisa errada nesse andar.",
            AmbientMode.Room204Bed => "Alguém dormiu aqui há pouco tempo... ou tentou.",
            _ => "O cheiro aqui é pior do que no corredor."
        };
        HUDController.FindActive(GetTree())?.ShowMessage(message, 4f);
        PensionAudioManager.Find(GetTree())?.PlayOneShot(
            Mode == AmbientMode.LinenCloset ? "old_house_settle_01" : "door_scratch_02", -17f);
    }
}
