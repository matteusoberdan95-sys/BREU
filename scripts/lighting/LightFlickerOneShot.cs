namespace BREU.Scripts.Lighting;

/// <summary>
/// Sprint 15 — short reversible light flicker. Always restores original energy.
/// </summary>
public partial class LightFlickerOneShot : Node
{
    private readonly HashSet<Light3D> _busy = new();

    public void Flicker(
        Light3D? light,
        float durationSeconds = 1.2f,
        float dimFactor = 0.4f,
        int pulses = 3)
    {
        if (light == null || !GodotObject.IsInstanceValid(light) || _busy.Contains(light))
        {
            return;
        }

        _ = RunFlickerAsync(light, durationSeconds, dimFactor, pulses);
    }

    public void FlickerMany(
        IEnumerable<Light3D?> lights,
        float durationSeconds = 1.2f,
        float dimFactor = 0.4f,
        int pulses = 3)
    {
        foreach (var light in lights)
        {
            Flicker(light, durationSeconds, dimFactor, pulses);
        }
    }

    private async System.Threading.Tasks.Task RunFlickerAsync(
        Light3D light,
        float durationSeconds,
        float dimFactor,
        int pulses)
    {
        _busy.Add(light);
        var original = light.LightEnergy;
        var safeDim = Mathf.Clamp(dimFactor, 0.15f, 0.85f);
        var pulseCount = Mathf.Max(1, pulses);
        var pulseDuration = Mathf.Max(0.12f, durationSeconds / pulseCount);

        try
        {
            for (var i = 0; i < pulseCount; i++)
            {
                if (!GodotObject.IsInstanceValid(light))
                {
                    return;
                }

                light.LightEnergy = original * safeDim;
                await ToSignal(GetTree().CreateTimer(pulseDuration * 0.45f), SceneTreeTimer.SignalName.Timeout);

                if (!GodotObject.IsInstanceValid(light))
                {
                    return;
                }

                light.LightEnergy = original * Mathf.Lerp(safeDim, 1f, 0.55f);
                await ToSignal(GetTree().CreateTimer(pulseDuration * 0.55f), SceneTreeTimer.SignalName.Timeout);
            }
        }
        finally
        {
            if (GodotObject.IsInstanceValid(light))
            {
                light.LightEnergy = original;
            }

            _busy.Remove(light);
        }
    }
}
