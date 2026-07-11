namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>Debug de playtest da Pensao blockout: lanterna longa e fog reduzida.</summary>
public partial class PensaoBlockoutPlaytestDebug : Node
{
    [Export] public bool PlaytestInfiniteFlashlight { get; set; } = true;
    [Export] public bool PlaytestReducedFog { get; set; } = true;
    [Export] public float PlaytestBatteryMinutes { get; set; } = 10f;
    [Export] public NodePath PlayerPath { get; set; } = "../../Player";
    [Export] public NodePath WorldEnvironmentPath { get; set; } = "../../Atmosphere/WorldEnvironment";
    [Export] public NodePath DepthFogPath { get; set; } = "../../Atmosphere/PostProcess/DepthFogPostProcess";

    public override void _Ready()
    {
        BREU.Scripts.Debug.PlaytestDebugSettings.InfiniteFlashlightBattery = PlaytestInfiniteFlashlight;
        BREU.Scripts.Debug.PlaytestDebugSettings.ReducedFog = PlaytestReducedFog;
        if (PlaytestInfiniteFlashlight)
        {
            BREU.Scripts.Debug.PlaytestDebugSettings.FlashlightDrainPerSecond = 0f;
        }

        CallDeferred(MethodName.ApplySettings);
    }

    private void ApplySettings()
    {
        ApplyFlashlightSettings();
        if (PlaytestReducedFog)
        {
            ApplyReducedFog();
        }
    }

    private void ApplyFlashlightSettings()
    {
        BREU.Scripts.Debug.PlaytestDebugSettings.InfiniteFlashlightBattery = PlaytestInfiniteFlashlight;
        BREU.Scripts.Debug.PlaytestDebugSettings.ReducedFog = PlaytestReducedFog;

        var flashlight = GetNodeOrNull(PlayerPath)?
            .GetNodeOrNull<BREU.Scripts.Player.FlashlightController>("CameraPivot/Flashlight");
        if (flashlight == null)
        {
            if (PlaytestInfiniteFlashlight)
            {
                GD.Print("Playtest: lanterna sem descarga (debug).");
            }
            else
            {
                GD.PushWarning("PensaoBlockoutPlaytestDebug: Flashlight nao encontrada; usando drain padrao.");
            }
            return;
        }

        if (PlaytestInfiniteFlashlight)
        {
            BREU.Scripts.Debug.PlaytestDebugSettings.FlashlightDrainPerSecond = 0f;
            GD.Print("Playtest: lanterna sem descarga (debug).");
            return;
        }

        var minutes = Mathf.Max(PlaytestBatteryMinutes, 5f);
        var drain = flashlight.MaxBattery / (minutes * 60f);
        BREU.Scripts.Debug.PlaytestDebugSettings.FlashlightDrainPerSecond = drain;
        GD.Print($"Playtest: bateria da lanterna ~{minutes:0} min.");
    }

    private void ApplyReducedFog()
    {
        if (GetNodeOrNull(WorldEnvironmentPath) is WorldEnvironment { Environment: { } env })
        {
            env.FogDensity = 0.004f;
            env.FogDepthBegin = 28f;
            env.FogDepthEnd = 120f;
            env.FogHeightDensity = 0.004f;
        }

        if (GetNodeOrNull(DepthFogPath) is MeshInstance3D fogMesh
            && fogMesh.MaterialOverride is ShaderMaterial shader)
        {
            shader.SetShaderParameter("fog_strength", 0.38f);
            shader.SetShaderParameter("fog_density", 0.045f);
            shader.SetShaderParameter("fog_start", 4.5f);
            shader.SetShaderParameter("fog_power", 0.9f);
        }

        GD.Print("Playtest: fog reduzida para validar layout.");
    }
}
