namespace BREU.Scripts.Debug;

/// <summary>Debug-only failsafe so a geometry playtest never requires closing the game.</summary>
public partial class DebugFallRecovery : Node
{
    private const float MinimumValidY = -3.0f;
    private CharacterBody3D? _player;
    private Marker3D? _safeMarker;

    public override void _Ready()
    {
        if (!OS.IsDebugBuild())
        {
            SetProcess(false);
            return;
        }

        var scene = GetTree().CurrentScene;
        _player = scene?.FindChild("Player", recursive: true, owned: false) as CharacterBody3D;
        _safeMarker = scene?.FindChild("SafeMarker_SecondFloor", recursive: true, owned: false) as Marker3D;
    }

    public override void _Process(double delta)
    {
        if (_player == null || _safeMarker == null || _player.GlobalPosition.Y >= MinimumValidY) return;

        _player.GlobalPosition = _safeMarker.GlobalPosition;
        _player.Velocity = Vector3.Zero;
        GD.PushWarning("[DebugFallRecovery] Player left valid area. Returned to safe marker.");
    }
}
