namespace BREU.Scripts.Levels;

public partial class EnterHouseTrigger : Area3D, IInteractable
{
    [Export] public string TargetScenePath { get; set; } = "res://scenes/levels/demo_room/DemoRoom.tscn";
    [Export] public string DebugMessage { get; set; } = "Transicao: HouseExterior -> DemoRoom";
    [Export] public string HudMessage { get; set; } = "Entrando na Pensao Santa Luzia...";
    [Export] public string TransitionMessage { get; set; } = "A porta range como se ja estivesse aberta por dentro.";
    [Export] public string InteractionText { get; set; } = "Entrar na Pensao Santa Luzia";
    [Export] public NodePath HudPath { get; set; } = new("");
    [Export] public NodePath DoorAudioPath { get; set; } = "DoorAudio";
    [Export] public float HudMessageDuration { get; set; } = 3.0f;

    private DoorAudioController? _doorAudio;
    private bool _triggered;

    public override void _Ready()
    {
        _doorAudio = GetNodeOrNull<DoorAudioController>(DoorAudioPath);
    }

    public string GetInteractionText()
    {
        return InteractionText;
    }

    public void Interact(PlayerController player)
    {
        if (_triggered)
        {
            return;
        }

        _triggered = true;
        GD.Print(DebugMessage);
        _doorAudio?.PlayOpen();
        ShowHudMessage();
        ChangeScene();
    }

    private void ShowHudMessage()
    {
        var hud = HudPath.IsEmpty
            ? GetTree().GetFirstNodeInGroup("hud")
            : GetNodeOrNull(HudPath);

        if (hud == null)
        {
            GD.Print($"HUD mensagem: {HudMessage}");
            return;
        }

        hud.Call("ShowTemporaryMessage", HudMessage, HudMessageDuration);
    }

    private void ChangeScene()
    {
        if (!ResourceLoader.Exists(TargetScenePath))
        {
            GD.PrintErr($"EnterHouseTrigger: cena nao encontrada: {TargetScenePath}");
            return;
        }

        if (SceneTransitionController.Instance != null)
        {
            SceneTransitionController.Instance.ChangeSceneWithFade(TargetScenePath, TransitionMessage);
            return;
        }

        var error = GetTree().ChangeSceneToFile(TargetScenePath);
        if (error != Error.Ok)
        {
            GD.PrintErr($"EnterHouseTrigger: falha ao trocar para {TargetScenePath} ({error}).");
        }
    }
}
