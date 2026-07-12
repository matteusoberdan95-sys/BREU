namespace BREU.Scripts.Debug;

/// <summary>
/// Debug-only void failsafe. Must NEVER teleport a first-floor player to the second floor
/// during normal corridor/reception gameplay.
/// </summary>
public partial class DebugFallRecovery : Node
{
    /// <summary>Only recover when the player is below the real world void.</summary>
    private const float KillY = -3.0f;

    /// <summary>Below this Y the player is treated as first floor / stair approach.</summary>
    private const float FirstFloorMaxY = 2.55f;

    /// <summary>At/above this Y the player is on second floor or upper wing.</summary>
    private const float SecondFloorMinY = 2.65f;

    private CharacterBody3D? _player;
    private Marker3D? _safeSecondFloor;
    private Marker3D? _safeReception;
    private string _lastFloor = "FirstFloor";
    private float _logThrottle;
    private bool _visitedUpper;

    public override void _Ready()
    {
        if (!OS.IsDebugBuild())
        {
            SetProcess(false);
            return;
        }

        var scene = GetTree().CurrentScene;
        _player = scene?.FindChild("Player", recursive: true, owned: false) as CharacterBody3D;
        _safeSecondFloor = scene?.FindChild("SafeMarker_SecondFloor", recursive: true, owned: false) as Marker3D;
        _safeReception = scene?.FindChild("SafeMarker_Reception", recursive: true, owned: false) as Marker3D;
    }

    public override void _Process(double delta)
    {
        if (_player == null) return;

        var position = _player.GlobalPosition;
        var currentFloor = EstimateFloor(position.Y);
        if (currentFloor is "SecondFloor" or "UpperWing")
        {
            _visitedUpper = true;
            _lastFloor = currentFloor;
        }
        else if (position.Y >= 0f && currentFloor == "FirstFloor")
        {
            _lastFloor = "FirstFloor";
        }

        _logThrottle -= (float)delta;
        if (_logThrottle <= 0f)
        {
            _logThrottle = 1.5f;
            GD.Print(
                $"[DebugFallRecovery] CHECK playerY={position.Y:0.00} killY={KillY:0.00} currentFloor={currentFloor}");

            // Old buggy condition treated first-floor Y < 1.9 as "fell through deck".
            if (_visitedUpper && currentFloor == "FirstFloor" && position.Y >= KillY && position.Y < 1.9f)
            {
                GD.Print("[DebugFallRecovery] ignored: player is on first floor");
            }
        }

        if (position.Y >= KillY)
        {
            return;
        }

        var destination = ChooseSafeMarker(currentFloor);
        if (destination == null)
        {
            GD.PrintErr("[DebugFallRecovery] TRIGGERED but no SafeMarker available");
            return;
        }

        var reason = $"playerY={position.Y:0.00} < killY={KillY:0.00}; lastFloor={_lastFloor}; dest={destination.Name}";
        GD.PushWarning($"[DebugFallRecovery] TRIGGERED reason={reason}");

        _player.GlobalPosition = destination.GlobalPosition;
        _player.Velocity = Vector3.Zero;
    }

    private Marker3D? ChooseSafeMarker(string currentFloor)
    {
        // Void under the level: restore to the last valid floor, never yank first-floor play to upper wing.
        var preferSecond = _lastFloor is "SecondFloor" or "UpperWing"
            || currentFloor is "SecondFloor" or "UpperWing";

        if (preferSecond && _safeSecondFloor != null)
        {
            return _safeSecondFloor;
        }

        return _safeReception ?? _safeSecondFloor;
    }

    private static string EstimateFloor(float y)
    {
        if (y >= 3.6f) return "UpperWing";
        if (y >= SecondFloorMinY) return "SecondFloor";
        if (y < FirstFloorMaxY) return "FirstFloor";
        return "Transition";
    }
}
