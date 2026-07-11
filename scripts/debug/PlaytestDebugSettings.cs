namespace BREU.Scripts.Debug;

/// <summary>
/// Playtest flags: infinite lantern battery, fog debug cycle (F11).
/// Autoload or scene node in Debug/PlaytestDebug.
/// </summary>
public partial class PlaytestDebugSettings : Node
{
    public enum FogDebugMode
    {
        Normal,
        Off,
        Strong
    }

    public static PlaytestDebugSettings? Instance { get; private set; }

    [Export] public bool InfiniteLanternBattery { get; set; } = true;
    [Export] public FogDebugMode FogMode { get; set; } = FogDebugMode.Normal;

    [Export] public float NormalFogDensity { get; set; } = 0.026f;
    [Export] public float StrongFogDensity { get; set; } = 0.072f;

    private bool _fogDefaultsCaptured;
    private bool _storedFogEnabled;
    private float _storedFogDensity;

    public string FogModeDisplayName => FogMode switch
    {
        FogDebugMode.Normal => "fog normal",
        FogDebugMode.Off => "fog off",
        FogDebugMode.Strong => "fog debug",
        _ => "fog normal"
    };

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
            CycleFogMode();
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

    public void CycleFogMode()
    {
        FogMode = (FogDebugMode)(((int)FogMode + 1) % 3);
        ApplyFogOverride();
        GD.Print($"[PlaytestDebug] Fog mode: {FogModeDisplayName}");
        HUDController.FindActive(GetTree())?.ShowMessage(
            $"Debug: {FogModeDisplayName}",
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

        switch (FogMode)
        {
            case FogDebugMode.Off:
                environment.FogEnabled = false;
                break;
            case FogDebugMode.Strong:
                environment.FogEnabled = true;
                environment.FogDensity = StrongFogDensity;
                break;
            default:
                environment.FogEnabled = _storedFogEnabled;
                environment.FogDensity = _storedFogDensity > 0.0f ? _storedFogDensity : NormalFogDensity;
                break;
        }
    }

    private static WorldEnvironment? FindWorldEnvironment()
    {
        var tree = Engine.GetMainLoop() as SceneTree;
        var root = tree?.CurrentScene;
        if (root == null)
        {
            return null;
        }

        return root.FindChild("Pension_WorldEnvironment", true, false) as WorldEnvironment
            ?? root.FindChild("WorldEnvironment", true, false) as WorldEnvironment;
    }
}
