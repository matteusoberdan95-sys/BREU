namespace BREU.Scripts.Levels;

/// <summary>
/// Remove colisao/visual de markers e decals do GLB da RitualRoom.
/// Marcadores do Blender nao devem bloquear o EnemyPlaceholder.
/// </summary>
public partial class RitualRoomEnvironmentSetup : Node
{
    [Export] public NodePath EnvironmentRootPath { get; set; } = "../Environment/sala_santos_secos_blockout";
    [Export] public bool HideDebugMarkers { get; set; } = true;
    [Export] public bool HideBloodDecals { get; set; } = true;

    private static readonly string[] MarkerNamePrefixes =
    [
        "marker_",
        "candle_01_flame_marker",
        "candle_02_flame_marker",
        "candle_03_flame_marker"
    ];

    private static readonly string[] BloodDecalPrefixes =
    [
        "floor_blood_",
        "table_blood_"
    ];

    public override void _Ready()
    {
        var root = GetNodeOrNull<Node>(EnvironmentRootPath);
        if (root == null)
        {
            GD.Print("RitualRoomEnvironmentSetup: GLB nao encontrado.");
            return;
        }

        SanitizeNodeTree(root);
    }

    private void SanitizeNodeTree(Node node)
    {
        var nodeName = node.Name.ToString().ToLowerInvariant();

        if (HideDebugMarkers && IsMarkerNode(nodeName))
        {
            DisableBlocking(node);
        }

        if (HideBloodDecals && IsBloodDecal(nodeName))
        {
            if (node is MeshInstance3D bloodMesh)
            {
                bloodMesh.Visible = false;
            }

            DisableBlocking(node);
        }

        if (node is StaticBody3D staticBody && IsMarkerNode(nodeName))
        {
            staticBody.CollisionLayer = 0;
            staticBody.CollisionMask = 0;
            foreach (var child in staticBody.GetChildren())
            {
                if (child is CollisionShape3D shape)
                {
                    shape.Disabled = true;
                }
            }
        }

        foreach (var child in node.GetChildren())
        {
            SanitizeNodeTree(child);
        }
    }

    private static bool IsMarkerNode(string nodeName)
    {
        foreach (var prefix in MarkerNamePrefixes)
        {
            if (nodeName.StartsWith(prefix, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsBloodDecal(string nodeName)
    {
        foreach (var prefix in BloodDecalPrefixes)
        {
            if (nodeName.StartsWith(prefix, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    private static void DisableBlocking(Node node)
    {
        if (node is MeshInstance3D mesh)
        {
            mesh.Visible = false;
        }

        if (node is CollisionObject3D collisionObject)
        {
            collisionObject.CollisionLayer = 0;
            collisionObject.CollisionMask = 0;
        }

        foreach (var child in node.GetChildren())
        {
            if (child is CollisionShape3D shape)
            {
                shape.Disabled = true;
            }
        }
    }
}
