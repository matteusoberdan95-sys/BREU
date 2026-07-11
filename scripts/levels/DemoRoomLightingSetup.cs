namespace BREU.Scripts.Levels;

/// <summary>
/// Alinha a luz do teto ao bulbo do GLB e aplica emissao quente no lamp_bulb.
/// </summary>
public partial class DemoRoomLightingSetup : Node
{
    [Export] public NodePath EnvironmentRootPath { get; set; } = "../Environment/quarto_07_blockout";
    [Export] public NodePath CeilingLightPath { get; set; } = "../Lighting/RoomCeilingLight";

    private static readonly Color WarmBulbColor = new(0.839f, 0.639f, 0.29f);

    public override void _Ready()
    {
        var root = GetNodeOrNull<Node>(EnvironmentRootPath);
        var ceilingLight = GetNodeOrNull<OmniLight3D>(CeilingLightPath);
        if (root == null || ceilingLight == null)
        {
            return;
        }

        var lampNode = FindNodeByName(root, "lamp_bulb");
        if (lampNode is Node3D lamp3D)
        {
            ceilingLight.GlobalPosition = lamp3D.GlobalPosition;
            ApplyEmissiveBulb(lamp3D);
        }
    }

    private static Node? FindNodeByName(Node node, string targetName)
    {
        if (node.Name.ToString().Equals(targetName, StringComparison.OrdinalIgnoreCase))
        {
            return node;
        }

        foreach (var child in node.GetChildren())
        {
            var found = FindNodeByName(child, targetName);
            if (found != null)
            {
                return found;
            }
        }

        return null;
    }

    private static void ApplyEmissiveBulb(Node3D lampNode)
    {
        if (lampNode is not MeshInstance3D meshInstance)
        {
            return;
        }

        var material = meshInstance.GetActiveMaterial(0);
        StandardMaterial3D? workingMaterial = null;

        if (material is StandardMaterial3D standard)
        {
            workingMaterial = standard.Duplicate() as StandardMaterial3D;
        }
        else
        {
            workingMaterial = new StandardMaterial3D();
        }

        if (workingMaterial == null)
        {
            return;
        }

        workingMaterial.AlbedoColor = WarmBulbColor;
        workingMaterial.EmissionEnabled = true;
        workingMaterial.Emission = WarmBulbColor;
        workingMaterial.EmissionEnergyMultiplier = 0.75f;
        meshInstance.SetSurfaceOverrideMaterial(0, workingMaterial);
    }
}
