namespace BREU.Scripts.Visual;

using System.Linq;
using Godot;

/// <summary>
/// Sprint L — aplica materiais visuais na TrailIntro e organiza props/fachada.
/// Nao altera fog, gameplay, colisoes ou iluminacao do player.
/// </summary>
public partial class TrailIntroVisualPass : Node
{
    [Export] public NodePath TrailBlockoutPath { get; set; } = new("../Environment/trail_intro_blockout");
    [Export] public NodePath HouseExteriorPath { get; set; } = new("../Environment/HouseExteriorAtTrailEnd/pensao_santa_luzia_exterior_blockout");
    [Export] public NodePath VisualEnhancementPath { get; set; } = new("../Environment/Trail_VisualEnhancement");
    [Export] public bool SpawnExtraProps { get; set; } = true;
    [Export] public bool OrganizeHouseGroups { get; set; } = true;

    private StandardMaterial3D? _dirt;
    private StandardMaterial3D? _ground;
    private StandardMaterial3D? _wood;
    private StandardMaterial3D? _cactus;
    private StandardMaterial3D? _stone;
    private StandardMaterial3D? _plaster;
    private StandardMaterial3D? _roof;
    private StandardMaterial3D? _door;
    private StandardMaterial3D? _lantern;
    private StandardMaterial3D? _cloth;
    private StandardMaterial3D? _cross;
    private StandardMaterial3D? _window;

    public override void _Ready()
    {
        LoadMaterials();
        ApplyToNode(GetNodeOrNull(TrailBlockoutPath));
        ApplyToNode(GetNodeOrNull(HouseExteriorPath));

        if (OrganizeHouseGroups)
        {
            OrganizeHouseFacade();
        }

        if (SpawnExtraProps)
        {
            SpawnTrailPropDuplicates();
        }
    }

    private void LoadMaterials()
    {
        _dirt = LoadMat("res://materials/environment/trail/mat_trail_dirt_dark.tres");
        _ground = LoadMat("res://materials/environment/trail/mat_ground_dark.tres");
        _wood = LoadMat("res://materials/environment/trail/mat_old_wood_dark.tres");
        _cactus = LoadMat("res://materials/environment/trail/mat_cactus_dark.tres");
        _stone = LoadMat("res://materials/environment/trail/mat_stone_cold_dark.tres");
        _plaster = LoadMat("res://materials/environment/house/mat_house_plaster_dirty.tres");
        _roof = LoadMat("res://materials/environment/house/mat_roof_old_dark.tres");
        _door = LoadMat("res://materials/environment/house/mat_door_old_wood.tres");
        _lantern = LoadMat("res://materials/environment/house/mat_lantern_warm_glow.tres");
        _cloth = LoadMat("res://materials/environment/house/mat_cloth_dirty.tres");
        _cross = LoadMat("res://materials/environment/house/mat_cross_dark_wood.tres");
        _window = LoadMat("res://materials/environment/house/mat_window_dark.tres");
    }

    private static StandardMaterial3D? LoadMat(string path)
    {
        return ResourceLoader.Load<StandardMaterial3D>(path)?.Duplicate() as StandardMaterial3D;
    }

    private void ApplyToNode(Node? root)
    {
        if (root == null)
        {
            return;
        }

        ApplyRecursive(root);
    }

    private void ApplyRecursive(Node node)
    {
        if (node is MeshInstance3D meshInstance)
        {
            var name = node.Name.ToString().ToLowerInvariant();
            if (name.StartsWith("marker_"))
            {
                return;
            }

            var material = ResolveMaterial(name);
            if (material != null)
            {
                ApplyMaterial(meshInstance, material, name);
            }
        }

        foreach (var child in node.GetChildren())
        {
            ApplyRecursive(child);
        }
    }

    private StandardMaterial3D? ResolveMaterial(string name)
    {
        if (name.Contains("dirt_path") || name.Contains("front_dirt"))
        {
            return _dirt;
        }

        if (name.Contains("terrain_base") || name.Contains("exterior_ground"))
        {
            return _ground;
        }

        if (name.Contains("rock") || name.Contains("front_rock"))
        {
            return _stone;
        }

        if (name.Contains("cactus") || name.Contains("dry_bush"))
        {
            return _cactus;
        }

        if (name.Contains("roof") || name.Contains("placeholder_roof"))
        {
            return _roof;
        }

        if (name.Contains("cross"))
        {
            return _cross;
        }

        if (name.Contains("window") || name.Contains("placeholder_window"))
        {
            return _window;
        }

        if (name.Contains("front_lantern") || name.Contains("lantern_body")
            || name.Contains("distant_lamp") || name.Contains("glow_marker"))
        {
            return _lantern;
        }

        if (name.Contains("main_door") || name.Contains("door") || name.Contains("placeholder_door"))
        {
            return _door;
        }

        if (name.Contains("cloth") || name.Contains("fabric") || name.Contains("rag"))
        {
            return _cloth;
        }

        if (name.Contains("crack") || name.Contains("mold") || name.Contains("stain")
            || name.Contains("plaster") || name.Contains("dark_hole") || name.Contains("_wall")
            || name.Contains("placeholder_body"))
        {
            return _plaster;
        }

        if (name.Contains("porch_step") || name.Contains("porch_floor"))
        {
            return _stone;
        }

        if (name.Contains("fence") || name.Contains("gate") || name.Contains("porch_post")
            || name.Contains("porch_roof") || name.Contains("broken_plank") || name.Contains("crate")
            || name.Contains("door_frame") || name.Contains("ridge_beam") || name.Contains("eave"))
        {
            return _wood;
        }

        return null;
    }

    private static void ApplyMaterial(MeshInstance3D meshInstance, StandardMaterial3D source, string nodeName)
    {
        var material = (StandardMaterial3D)source.Duplicate();
        var hash = nodeName.GetHashCode();
        var variation = ((hash & 0xFF) / 255f - 0.5f) * 0.04f;
        var c = material.AlbedoColor;
        material.AlbedoColor = new Color(
            Mathf.Clamp(c.R + variation, 0f, 1f),
            Mathf.Clamp(c.G + variation * 0.8f, 0f, 1f),
            Mathf.Clamp(c.B + variation * 0.6f, 0f, 1f),
            c.A);
        meshInstance.SetSurfaceOverrideMaterial(0, material);
    }

    private void OrganizeHouseFacade()
    {
        var houseRoot = GetNodeOrNull(HouseExteriorPath);
        if (houseRoot == null)
        {
            return;
        }

        var parent = houseRoot.GetParent();
        if (parent == null)
        {
            return;
        }

        var damage = GetOrCreateGroup(parent, "House_Damage");
        var props = GetOrCreateGroup(parent, "House_Props");
        var ritual = GetOrCreateGroup(parent, "House_RitualDetails");
        var lights = GetOrCreateGroup(parent, "House_Lights");

        foreach (var child in houseRoot.GetChildren().ToArray())
        {
            if (child is not Node3D node3D)
            {
                continue;
            }

            var n = child.Name.ToString().ToLowerInvariant();
            Node target = props;

            if (n.Contains("crack") || n.Contains("mold") || n.Contains("stain")
                || n.Contains("plaster") || n.Contains("dark_hole")
                || (n.Contains("broken_plank") && n.Contains("front")))
            {
                target = damage;
            }
            else if (n.Contains("cross"))
            {
                target = ritual;
            }
            else if (n.Contains("lantern") || n.Contains("glow_marker"))
            {
                target = lights;
            }
            else if (n.Contains("crate") || n.Contains("rock") || n.Contains("dry_bush")
                     || n.Contains("cactus"))
            {
                target = props;
            }

            node3D.Reparent(target, true);
        }
    }

    private static Node3D GetOrCreateGroup(Node parent, string groupName)
    {
        var existing = parent.GetNodeOrNull<Node3D>(groupName);
        if (existing != null)
        {
            return existing;
        }

        var group = new Node3D { Name = groupName };
        parent.AddChild(group);
        return group;
    }

    private void SpawnTrailPropDuplicates()
    {
        var root = GetNodeOrNull(VisualEnhancementPath) as Node3D;
        if (root == null || _stone == null || _wood == null || _cactus == null)
        {
            return;
        }

        var rocks = GetOrCreateGroup(root, "Trail_Rocks");
        var dryVeg = GetOrCreateGroup(root, "Trail_DryVegetation");
        var fenceDamage = GetOrCreateGroup(root, "Trail_Fence_Damage");
        var props = GetOrCreateGroup(root, "Trail_Props");

        AddRock(rocks, "RockSmall_A", new Vector3(-3.2f, 0.12f, 8.5f), new Vector3(0.9f, 0.55f, 1.1f));
        AddRock(rocks, "RockSmall_B", new Vector3(3.4f, 0.1f, 5.0f), new Vector3(1.2f, 0.45f, 0.85f));
        AddRock(rocks, "RockSmall_A2", new Vector3(-3.6f, 0.08f, -1.0f), new Vector3(0.7f, 0.4f, 0.9f));
        AddRock(rocks, "RockSmall_B2", new Vector3(3.1f, 0.11f, -6.5f), new Vector3(1.0f, 0.5f, 1.3f));

        AddPropBox(dryVeg, "DryBranch_A", new Vector3(-2.8f, 0.05f, 2.0f), new Vector3(0.08f, 0.05f, 0.9f), _wood, new Vector3(0, 0.4f, 0));
        AddPropBox(dryVeg, "DryBranch_A2", new Vector3(2.9f, 0.04f, -3.5f), new Vector3(0.07f, 0.05f, 0.75f), _wood, new Vector3(0, -0.5f, 0.2f));

        AddPropBox(fenceDamage, "BrokenPlank_A", new Vector3(-2.2f, 0.35f, 4.5f), new Vector3(0.9f, 0.06f, 0.14f), _wood, new Vector3(0, 0.2f, -0.3f));
        AddPropBox(fenceDamage, "FencePost_Damaged_A", new Vector3(2.15f, 0.45f, -0.5f), new Vector3(0.12f, 0.9f, 0.12f), _wood, Vector3.Zero);

        AddPropBox(props, "CrateOld_A", new Vector3(-2.6f, 0.25f, -10.5f), new Vector3(0.55f, 0.45f, 0.5f), _wood, new Vector3(0, 0.15f, 0));
        AddPropBox(props, "CrateOld_A2", new Vector3(2.5f, 0.2f, -12.0f), new Vector3(0.45f, 0.35f, 0.42f), _wood, new Vector3(0, -0.25f, 0));

        AddPropBox(dryVeg, "CactusSmall_A", new Vector3(-3.0f, 0.35f, -8.0f), new Vector3(0.25f, 0.55f, 0.25f), _cactus, Vector3.Zero);
        AddPropBox(dryVeg, "CactusTall_A", new Vector3(3.2f, 0.55f, -14.0f), new Vector3(0.3f, 1.1f, 0.3f), _cactus, Vector3.Zero);
    }

    private void AddRock(Node3D parent, string name, Vector3 pos, Vector3 scale)
    {
        if (_stone == null)
        {
            return;
        }

        var meshInstance = new MeshInstance3D
        {
            Name = name,
            Mesh = new SphereMesh { Radius = 0.35f, RadialSegments = 8, Rings = 6 },
            Position = pos,
            Scale = scale,
        };
        meshInstance.SetSurfaceOverrideMaterial(0, (StandardMaterial3D)_stone.Duplicate());
        parent.AddChild(meshInstance);
    }

    private static void AddPropBox(Node3D parent, string name, Vector3 pos, Vector3 size, StandardMaterial3D mat, Vector3 rot)
    {
        var meshInstance = new MeshInstance3D
        {
            Name = name,
            Mesh = new BoxMesh { Size = size },
            Position = pos,
            Rotation = rot,
        };
        meshInstance.SetSurfaceOverrideMaterial(0, (StandardMaterial3D)mat.Duplicate());
        parent.AddChild(meshInstance);
    }
}
