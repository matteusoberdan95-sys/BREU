namespace BREU.Scripts.Levels;

public partial class DemoEndTrigger : Area3D
{
    [Export] public string Message { get; set; } = "Fim da demo atual. Proxima sprint: porta final/transicao.";
    [Export] public string HudMessage { get; set; } = "A porta no fim do corredor esta trancada.";

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
        GD.Print(Message);

        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.ShowTemporaryMessage(HudMessage, 4.5f);
        }
    }
}
