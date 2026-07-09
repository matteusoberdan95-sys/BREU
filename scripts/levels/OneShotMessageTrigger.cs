namespace BREU.Scripts.Levels;

/// <summary>
/// Trigger narrativo simples que mostra uma mensagem uma unica vez.
/// </summary>
public partial class OneShotMessageTrigger : Area3D
{
    [Export] public string Message { get; set; } = "";
    [Export] public float Duration { get; set; } = 3.0f;
    [Export] public NodePath HudPath { get; set; } = new("");

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
        ShowMessage();
    }

    private void ShowMessage()
    {
        if (string.IsNullOrWhiteSpace(Message))
        {
            return;
        }

        var hud = HudPath.IsEmpty
            ? GetTree().GetFirstNodeInGroup("hud")
            : GetNodeOrNull(HudPath);

        if (hud != null)
        {
            hud.Call("ShowTemporaryMessage", Message, Duration);
            return;
        }

        GD.Print($"HUD mensagem: {Message}");
    }
}
