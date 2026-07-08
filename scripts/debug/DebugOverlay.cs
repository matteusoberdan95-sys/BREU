using Godot;

namespace BREU.Scripts.Debug;

public partial class DebugOverlay : Node
{
    [Export] public bool Enabled { get; set; } = true;

    public void Log(string message)
    {
        if (Enabled)
        {
            GD.Print($"[BREU] {message}");
        }
    }
}
