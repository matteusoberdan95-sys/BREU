namespace BREU.Scripts.Debug;

/// <summary>
/// Playtest flags: infinite lantern battery, reduced fog. Autoload or scene node in Debug/PlaytestDebug.
/// </summary>
public partial class PlaytestDebugSettings : Node
{
    public static PlaytestDebugSettings? Instance { get; private set; }

    [Export] public bool InfiniteLanternBattery { get; set; } = true;
    [Export] public bool ReducedFog { get; set; } = false;

    [Export] public float NormalFogDensity { get; set; } = 0.045f;
    [Export] public float ReducedFogDensity { get; set; } = 0.008f;

    private bool _fogDefaultsCaptured;
    private bool _storedFogEnabled;
    private float _storedFogDensity;

    public override void _EnterTree()
    {
        Instance = this;
        AddToGroup("playtest_debug_settings");
    }

    public override void _ExitTree()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public override void _Ready()
    {
        CallDeferred(nameof(ApplyFogOverride));
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("debug_toggle_infinite_lantern"))
        {
            SetInfiniteLanternBattery(!InfiniteLanternBattery);
        }

        if (@event.IsActionPressed("debug_toggle_reduced_fog"))
        {
            SetReducedFog(!ReducedFog);
        }
    }

    public void SetInfiniteLanternBattery(bool enabled)
    {
        InfiniteLanternBattery = enabled;
        GD.Print($"[PlaytestDebug] Infinite lantern battery: {(enabled ? "ON" : "OFF")}");
        HUDController.FindActive(GetTree())?.ShowMessage(
            enabled ? "Debug: lanterna infinita ON" : "Debug: lanterna infinita OFF",
            2.5f);
    }

    public void SetReducedFog(bool enabled)
    {
        ReducedFog = enabled;
        ApplyFogOverride();
        GD.Print($"[PlaytestDebug] Reduced fog: {(enabled ? "ON" : "OFF")}");
        HUDController.FindActive(GetTree())?.ShowMessage(
            enabled ? "Debug: fog reduzida ON" : "Debug: fog normal",
            2.5f);
    }

    public void ApplyFogOverride()
    {
        var worldEnvironment = FindWorldEnvironment();
        if (worldEnvironment?.Environment == null)
        {
            return;
        }

        var environment = worldEnvironment.Environment;

        if (!_fogDefaultsCaptured)
        {
            _storedFogEnabled = environment.FogEnabled;
            _storedFogDensity = environment.FogDensity;
            _fogDefaultsCaptured = true;
        }

        if (ReducedFog)
        {
            environment.FogEnabled = true;
            environment.FogDensity = ReducedFogDensity;
            return;
        }

        environment.FogEnabled = _storedFogEnabled;
        environment.FogDensity = _storedFogDensity > 0.0f ? _storedFogDensity : NormalFogDensity;
    }

    private static WorldEnvironment? FindWorldEnvironment()
    {
        var tree = Engine.GetMainLoop() as SceneTree;
        var root = tree?.CurrentScene;
        if (root == null)
        {
            return null;
        }

        return root.FindChild("WorldEnvironment", true, false) as WorldEnvironment;
    }
}
