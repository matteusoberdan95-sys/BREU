namespace BREU.Scripts.Levels;

public partial class DemoEndTrigger : Area3D
{
    [Export] public string Message { get; set; } = "Fim do corredor alcancado.";
    [Export] public string HudMessage { get; set; } = "";
    [Export] public bool ShowHudOnEnter { get; set; }

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

        if (!ShowHudOnEnter || string.IsNullOrWhiteSpace(HudMessage))
        {
            return;
        }

        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.ShowTemporaryMessage(HudMessage, 4.5f);
        }
    }
}
