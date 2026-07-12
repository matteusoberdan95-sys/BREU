namespace BREU.Scripts.Debug;

/// <summary>Debug-only failsafe so a geometry playtest never requires closing the game.</summary>
public partial class DebugFallRecovery : Node
{
    private const float MinimumValidY = -3.0f;
    private CharacterBody3D? _player;
    private Marker3D? _safeMarker;
    private bool _enteredUpperWing;

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
        if (_player == null || _safeMarker == null) return;

        var position = _player.GlobalPosition;
        var insideUpperDeckXz = position.X >= -20f && position.X <= 30f
            && position.Z >= -10.8f && position.Z <= 20f;
        if (insideUpperDeckXz && position.Y >= 2.65f) _enteredUpperWing = true;

        var fellThroughUpperDeck = _enteredUpperWing && insideUpperDeckXz && position.Y < 1.9f;
        if (!fellThroughUpperDeck && position.Y >= MinimumValidY) return;

        _player.GlobalPosition = _safeMarker.GlobalPosition;
        _player.Velocity = Vector3.Zero;
        _enteredUpperWing = false;
        GD.PushWarning("[DebugFallRecovery] Player crossed below UpperWing_CollisionDeck. Returned to SafeMarker_SecondFloor.");
    }
}
