namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>Oculta somente a edificacao importada quebrada; preserva trilha e exterior.</summary>
public partial class PensaoVerticalSliceVisualFilter : Node
{
    [Export] public NodePath ImportedRootPath { get; set; } = "../Base/ImportedWorld/LevelMesh";

    private static readonly string[] HiddenPrefixes =
    {
        "pension_", "interior_", "reception_", "deposit_", "room_", "stair_",
        "kitchen_", "sign_pensao_", "sign_broken_", "porch_"
    };

    public override void _Ready()
    {
        var root = GetNodeOrNull<Node>(ImportedRootPath);
        if (root is null)
        {
            GD.PushWarning("PensaoVerticalSliceVisualFilter: GLB base nao encontrado.");
            return;
        }

        var hidden = 0;
        foreach (var node in root.FindChildren("*", "Node3D", true, false))
        {
            if (node is not Node3D visual)
            {
                continue;
            }
            var normalized = visual.Name.ToString().ToLowerInvariant();
            if (!HiddenPrefixes.Any(normalized.StartsWith))
            {
                continue;
            }
            visual.Visible = false;
            hidden++;
        }
        GD.Print($"PensaoVerticalSliceVisualFilter: {hidden} partes antigas ocultas.");
    }
}
