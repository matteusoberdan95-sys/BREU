namespace BREU.Scripts.Levels;

public partial class HouseEntryTrigger : Area3D
{
    [Export] public string TargetScenePath { get; set; } = "res://scenes/levels/house_exterior/HouseExterior.tscn";
    [Export] public string DebugMessage { get; set; } = "Transicao: TrailIntro -> HouseExterior";
    [Export] public string HudMessage { get; set; } = "A Pensao Santa Luzia.";
    [Export] public NodePath HudPath { get; set; } = new("");
    [Export] public float HudMessageDuration { get; set; } = 4.0f;

    private bool _triggered;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (_triggered || !body.IsInGroup("player"))
        {
            return;
        }

        _triggered = true;
        GD.Print(DebugMessage);
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
            GD.PrintErr($"HouseEntryTrigger: cena nao encontrada: {TargetScenePath}");
            return;
        }

        if (SceneTransitionController.Instance != null)
        {
            SceneTransitionController.Instance.ChangeSceneWithFade(TargetScenePath, HudMessage);
            return;
        }

        ShowHudMessage();

        var error = GetTree().ChangeSceneToFile(TargetScenePath);
        if (error != Error.Ok)
        {
            GD.PrintErr($"HouseEntryTrigger: falha ao trocar para {TargetScenePath} ({error}).");
        }
    }
}
