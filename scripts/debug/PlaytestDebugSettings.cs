namespace BREU.Scripts.Debug;

/// <summary>Flags globais de playtest — ativadas por cena via PensaoBlockoutPlaytestDebug.</summary>
public static class PlaytestDebugSettings
{
    public static bool InfiniteFlashlightBattery { get; set; }
    public static bool ReducedFog { get; set; }
    public static float FlashlightDrainPerSecond { get; set; } = 3.5f;
}
