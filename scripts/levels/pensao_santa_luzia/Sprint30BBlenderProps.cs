namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Applies the Sprint 30B visual-only replacement map after the legacy builders
/// have finished. Gameplay nodes, interaction areas and approved colliders stay
/// in their original locations; only the corresponding placeholder meshes hide.
/// </summary>
public partial class Sprint30BBlenderProps : Node3D
{
    private static readonly string[] WholeVisualPlaceholders =
    {
        "PensionGroundFloor/NarrativeReadability/ReceptionProps/Reception_Counter_Visual",
        "PensionGroundFloor/NarrativeReadability/KitchenProps/Kitchen_Counter_Visual",
        "World/VisualPolish/Sprint28_LightArtPass/Kitchen_Props/RustBucket_Body",
        "World/VisualPolish/Sprint28_LightArtPass/Kitchen_Props/RustBucket_Rim",
        "World/VisualPolish/Sprint28_LightArtPass/Bathroom_Props/DrainOuterRing",
        "World/VisualPolish/Sprint28_LightArtPass/Bathroom_Props/DrainSlat_A",
        "World/VisualPolish/Sprint28_LightArtPass/Bathroom_Props/DrainSlat_B",
        "World/VisualPolish/Sprint28_LightArtPass/Bathroom_Props/DrainSlat_C",
        "World/VisualPolish/Sprint28_LightArtPass/Bathroom_Props/BathroomBucket_Body",
        "World/VisualPolish/Sprint28_LightArtPass/Bathroom_Props/BathroomBucket_Rim",
        "World/VisualPolish/Sprint28_LightArtPass/Bathroom_Props/BrokenMirrorShard",
        "World/VisualPolish/Sprint28_LightArtPass/TechnicalRoom_Props/PanelSoot",
        "World/VisualPolish/Sprint28_LightArtPass/TechnicalRoom_Props/PanelBorderTop",
        "World/VisualPolish/Sprint28_LightArtPass/TechnicalRoom_Props/PanelBorderBottom",
        "World/VisualPolish/Sprint28_LightArtPass/TechnicalRoom_Props/FuseLabel_A",
        "World/VisualPolish/Sprint28_LightArtPass/TechnicalRoom_Props/FuseLabel_B",
        "World/VisualPolish/Sprint28_LightArtPass/UpperRooms_Props/Room201_Suitcase",
        "World/VisualPolish/Sprint28_LightArtPass/Cloths_Curtains/Room201_TornCurtain",
        "World/VisualPolish/Sprint27_FakeWindowsLighting/FakeWindows/FacadeUpper_Room201WestInterior",
        "World/VisualPolish/Sprint27_FakeWindowsLighting/FakeWindows/FacadeUpper_Room202EastInterior",
        "World/Level/SecondFloor/UpperWingRooms/Props/Prop_Bath_Mirror",
        "World/Level/SecondFloor/UpperWingRooms/SharedBathroom/Prop_Bath_Drain",
        "World/Level/SecondFloor/UpperWingRooms/Props/Prop_204_Nightstand"
    };

    private static readonly string[] PreservePhysicsHideMeshOnly =
    {
        "PensionGroundFloor/NarrativeReadability/ReceptionProps/Reception_Chair",
        "PensionGroundFloor/NarrativeReadability/KitchenProps/Kitchen_Stove",
        "PensionGroundFloor/NarrativeReadability/KitchenProps/Kitchen_Table",
        "World/Level/SecondFloor/UpperWingRooms/TechnicalRoom/TechnicalPanel/PanelVisual"
    };

    public override void _Ready()
    {
        CallDeferred(nameof(ApplyPlaceholderReplacements));
    }

    private void ApplyPlaceholderReplacements()
    {
        var scene = GetTree().CurrentScene;
        if (scene == null) return;

        var hidden = 0;
        foreach (var path in WholeVisualPlaceholders)
        {
            if (scene.GetNodeOrNull<Node3D>(path) is not { } oldVisual)
            {
                GD.PushWarning($"[Sprint30B] visual placeholder not found: {path}");
                continue;
            }

            oldVisual.Visible = false;
            hidden++;
        }

        foreach (var path in PreservePhysicsHideMeshOnly)
        {
            var oldNode = scene.GetNodeOrNull(path);
            if (oldNode == null)
            {
                GD.PushWarning($"[Sprint30B] functional placeholder not found: {path}");
                continue;
            }

            foreach (var visual in Enumerate(oldNode).OfType<GeometryInstance3D>())
                visual.Visible = false;
            hidden++;
        }

        GetNode<Node3D>("Backup_Placeholders_Sprint30B")
            .SetMeta("runtime_hidden_placeholder_count", hidden);
        GD.Print($"[Sprint30B] Blender prop replacement map applied: {hidden} placeholder visuals hidden; gameplay/colliders preserved.");
    }

    private static IEnumerable<Node> Enumerate(Node root)
    {
        yield return root;
        foreach (var child in root.GetChildren())
            foreach (var nested in Enumerate(child))
                yield return nested;
    }
}

