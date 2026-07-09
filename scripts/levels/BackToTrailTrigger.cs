namespace BREU.Scripts.Levels;

public partial class BackToTrailTrigger : Area3D
{
    [Export] public string DebugMessage { get; set; } = "BackToTrailTrigger ativado.";
    [Export] public string HudMessage { get; set; } = "A trilha ficou para tras.";
    [Export] public NodePath HudPath { get; set; } = new("");
    [Export] public float HudMessageDuration { get; set; } = 3.0f;

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
        ShowHudMessage();

        // TODO: futura transicao HouseExterior -> TrailIntro.
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
}
