using Godot;

namespace BREU.Scripts.Debug;

public partial class DemoEndTrigger : Area3D
{
    [Export] public string Message { get; set; } = "Fim da demo placeholder. Corredor conectado, proxima etapa: transicao/porta final.";

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
    }
}
