namespace BREU.Scripts.Visual;

/// <summary>
/// Aplicador opcional de leitura visual. Por padrao nao altera a cena;
/// habilite apenas os exports necessarios para evitar sobrescrever ajustes manuais.
/// </summary>
public partial class VisualProfileApplier : Node
{
    [Export] public bool ApplyOnReady { get; set; } = false;

    [ExportGroup("World Environment")]
    [Export] public NodePath WorldEnvironmentPath { get; set; } = new("");
    [Export] public bool ApplyWorldEnvironment { get; set; } = false;
    [Export] public bool DuplicateEnvironmentResource { get; set; } = true;
    [Export] public bool ApplyBackgroundColor { get; set; } = false;
    [Export] public Color BackgroundColor { get; set; } = new(0.063f, 0.09f, 0.133f);
    [Export] public bool ApplyAmbientEnergy { get; set; } = false;
    [Export(PropertyHint.Range, "0,1,0.01")] public float AmbientEnergy { get; set; } = 0.1f;

    [ExportGroup("Main Light")]
    [Export] public NodePath MainLightPath { get; set; } = new("");
    [Export] public bool ApplyMainLight { get; set; } = false;
    [Export] public bool ApplyMainLightColor { get; set; } = false;
    [Export] public Color MainLightColor { get; set; } = new(0.686f, 0.769f, 0.839f);
    [Export] public bool ApplyMainLightEnergy { get; set; } = false;
    [Export(PropertyHint.Range, "0,8,0.05")] public float MainLightEnergy { get; set; } = 1.0f;

    private bool _warnedMissingWorldEnvironment;
    private bool _warnedMissingMainLight;

    public override void _Ready()
    {
        if (ApplyOnReady)
        {
            ApplyProfile();
        }
    }

    public void ApplyProfile()
    {
        if (ApplyWorldEnvironment)
        {
            ApplyEnvironmentSettings();
        }

        if (ApplyMainLight)
        {
            ApplyLightSettings();
        }
    }

    private void ApplyEnvironmentSettings()
    {
        var worldEnvironment = ResolveWorldEnvironment();
        if (worldEnvironment == null)
        {
            WarnOnce(ref _warnedMissingWorldEnvironment, "VisualProfileApplier: WorldEnvironment nao encontrado; perfil visual ignorado.");
            return;
        }

        if (worldEnvironment.Environment == null)
        {
            worldEnvironment.Environment = new Godot.Environment();
        }
        else if (DuplicateEnvironmentResource)
        {
            worldEnvironment.Environment = worldEnvironment.Environment.Duplicate() as Godot.Environment;
        }

        var environment = worldEnvironment.Environment;
        if (environment == null)
        {
            return;
        }

        if (ApplyBackgroundColor)
        {
            environment.BackgroundMode = Godot.Environment.BGMode.Color;
            environment.BackgroundColor = BackgroundColor;
        }

        if (ApplyAmbientEnergy)
        {
            environment.AmbientLightEnergy = AmbientEnergy;
        }
    }

    private void ApplyLightSettings()
    {
        var light = ResolveMainLight();
        if (light == null)
        {
            WarnOnce(ref _warnedMissingMainLight, "VisualProfileApplier: luz principal nao encontrada; ajuste de luz ignorado.");
            return;
        }

        if (ApplyMainLightColor)
        {
            light.LightColor = MainLightColor;
        }

        if (ApplyMainLightEnergy)
        {
            light.LightEnergy = MainLightEnergy;
        }
    }

    private WorldEnvironment? ResolveWorldEnvironment()
    {
        if (!WorldEnvironmentPath.IsEmpty)
        {
            return GetNodeOrNull<WorldEnvironment>(WorldEnvironmentPath);
        }

        return GetTree().CurrentScene?.FindChild("WorldEnvironment", true, false) as WorldEnvironment;
    }

    private Light3D? ResolveMainLight()
    {
        if (!MainLightPath.IsEmpty)
        {
            return GetNodeOrNull<Light3D>(MainLightPath);
        }

        return GetTree().CurrentScene?.FindChild("MoonLight", true, false) as Light3D
            ?? GetTree().CurrentScene?.FindChild("CandleLightMain", true, false) as Light3D
            ?? GetTree().CurrentScene?.FindChild("RoomLightPoint", true, false) as Light3D;
    }

    private static void WarnOnce(ref bool warned, string message)
    {
        if (warned)
        {
            return;
        }

        warned = true;
        GD.PushWarning(message);
    }
}
