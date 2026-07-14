namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Sprint 31 visual-only material pass. It reuses a small shared palette on
/// existing meshes and adds non-colliding finish details. Structural meshes,
/// transforms, collision, gameplay and the frozen balcony deck remain intact.
/// </summary>
public partial class Sprint31MaterialPass : Node3D
{
    private StandardMaterial3D _wallAged = null!;
    private StandardMaterial3D _wallWet = null!;
    private StandardMaterial3D _wallExterior = null!;
    private StandardMaterial3D _floorRooms = null!;
    private StandardMaterial3D _floorCorridor = null!;
    private StandardMaterial3D _floorWet = null!;
    private StandardMaterial3D _ceilingAged = null!;
    private StandardMaterial3D _ceilingWet = null!;
    private StandardMaterial3D _baseboard = null!;
    private StandardMaterial3D _dampStain = null!;
    private StandardMaterial3D _trafficStain = null!;
    private StandardMaterial3D _ceilingStain = null!;

    public override void _Ready()
    {
        BuildSharedMaterials();
        CallDeferred(nameof(ApplyVisualPass));
    }

    private void BuildSharedMaterials()
    {
        _wallAged = Material("Wall_AgedPlaster", new Color(0.43f, 0.39f, 0.32f), 0.96f);
        _wallWet = Material("Wall_DampPlaster", new Color(0.29f, 0.31f, 0.25f), 0.98f);
        _wallExterior = Material("Wall_WeatheredExterior", new Color(0.34f, 0.31f, 0.25f), 0.97f);
        _floorRooms = Material("Floor_AgedBoards", new Color(0.17f, 0.115f, 0.075f), 0.94f);
        _floorCorridor = Material("Floor_WornTraffic", new Color(0.125f, 0.078f, 0.052f), 0.97f);
        _floorWet = Material("Floor_DampService", new Color(0.17f, 0.16f, 0.13f), 0.99f);
        _ceilingAged = Material("Ceiling_Aged", new Color(0.205f, 0.19f, 0.17f), 0.98f);
        _ceilingWet = Material("Ceiling_Damp", new Color(0.145f, 0.155f, 0.13f), 1f);
        _baseboard = Material("Baseboard_DarkWood", new Color(0.105f, 0.058f, 0.032f), 0.96f);
        _dampStain = TransparentMaterial("Stain_Damp", new Color(0.075f, 0.105f, 0.075f, 0.46f));
        _trafficStain = TransparentMaterial("Stain_Traffic", new Color(0.055f, 0.042f, 0.03f, 0.36f));
        _ceilingStain = TransparentMaterial("Stain_Infiltration", new Color(0.085f, 0.078f, 0.058f, 0.4f));
    }

    private void ApplyVisualPass()
    {
        var scene = GetTree().CurrentScene;
        if (scene == null) return;

        var walls = 0;
        var floors = 0;
        var ceilings = 0;
        var wallMeshes = new List<MeshInstance3D>();

        foreach (var mesh in Enumerate(scene).OfType<MeshInstance3D>())
        {
            var path = mesh.GetPath().ToString();
            if (ShouldIgnore(path)) continue;

            if (IsCeiling(mesh, path))
            {
                mesh.MaterialOverride = IsWetArea(path) ? _ceilingWet : _ceilingAged;
                ceilings++;
                continue;
            }

            if (IsFloor(mesh, path))
            {
                mesh.MaterialOverride = IsWetArea(path)
                    ? _floorWet
                    : IsTrafficArea(path) ? _floorCorridor : _floorRooms;
                floors++;
                continue;
            }

            if (!IsWall(mesh, path)) continue;
            mesh.MaterialOverride = path.Contains("BuildingExteriorShell", StringComparison.Ordinal)
                ? _wallExterior
                : IsWetArea(path) ? _wallWet : _wallAged;
            wallMeshes.Add(mesh);
            walls++;
        }

        var baseboards = BuildBaseboards(wallMeshes);
        var dampPatches = BuildDampPatches(wallMeshes);
        BuildFloorStains();
        BuildCeilingStains();
        BuildRoom203DragMarks();

        SetMeta("materials_applied", walls + floors + ceilings);
        SetMeta("wall_material_count", walls);
        SetMeta("floor_material_count", floors);
        SetMeta("ceiling_material_count", ceilings);
        SetMeta("baseboard_count", baseboards);
        SetMeta("damp_patch_count", dampPatches);
        GD.Print($"[Sprint31Materials] visual-only pass applied: {walls} walls, {floors} floors, {ceilings} ceilings, {baseboards} baseboards, {dampPatches} damp patches; 0 collision/gameplay nodes.");
    }

    private int BuildBaseboards(IEnumerable<MeshInstance3D> wallMeshes)
    {
        var root = GetNode<Node3D>("Baseboards");
        var count = 0;
        foreach (var wall in wallMeshes)
        {
            if (wall.Mesh is not BoxMesh box || box.Size.Y < 1.5f ||
                Mathf.Max(box.Size.X, box.Size.Z) < 0.65f) continue;

            var path = wall.GetPath().ToString();
            if (path.Contains("BuildingExteriorShell", StringComparison.Ordinal) ||
                path.Contains("Room205_Locked", StringComparison.Ordinal)) continue;

            var stripSize = new Vector3(box.Size.X + 0.018f, 0.14f, box.Size.Z + 0.018f);
            var strip = AddBox(root, $"Baseboard_{count:000}", stripSize, _baseboard);
            var transform = wall.GlobalTransform;
            var floorY = IsUpperFloor(path) ? 2.82f : 0.02f;
            transform.Origin = new Vector3(transform.Origin.X, floorY + 0.07f, transform.Origin.Z);
            strip.GlobalTransform = transform;
            count++;
        }

        return count;
    }

    private int BuildDampPatches(IReadOnlyList<MeshInstance3D> wallMeshes)
    {
        var root = GetNode<Node3D>("DampPatches");
        var count = 0;
        for (var i = 0; i < wallMeshes.Count; i++)
        {
            var wall = wallMeshes[i];
            var path = wall.GetPath().ToString();
            if (!IsWetArea(path) && i % 8 != 0) continue;
            if (wall.Mesh is not BoxMesh box || box.Size.Y < 1.5f) continue;

            var longX = box.Size.X >= box.Size.Z;
            var patchSize = longX
                ? new Vector3(Mathf.Min(1.35f, box.Size.X * 0.42f), 0.52f, box.Size.Z + 0.024f)
                : new Vector3(box.Size.X + 0.024f, 0.52f, Mathf.Min(1.35f, box.Size.Z * 0.42f));
            var patch = AddBox(root, $"DampPatch_{count:000}", patchSize, _dampStain);
            var transform = wall.GlobalTransform;
            var floorY = IsUpperFloor(path) ? 2.82f : 0.02f;
            transform.Origin = new Vector3(transform.Origin.X, floorY + 0.29f, transform.Origin.Z);
            patch.GlobalTransform = transform;
            count++;
        }

        return count;
    }

    private void BuildFloorStains()
    {
        var root = GetNode<Node3D>("FloorStains");
        var marks = new (string Name, Vector3 Position, Vector2 Scale)[]
        {
            ("Reception_Worn_A", new(-2.25f, 0.041f, -3.8f), new(1.35f, 0.75f)),
            ("Reception_Worn_B", new(0.85f, 0.042f, -1.2f), new(0.9f, 0.55f)),
            ("Corridor_Traffic_A", new(0.25f, 0.058f, -11.5f), new(0.8f, 1.55f)),
            ("Corridor_Traffic_B", new(-0.3f, 0.058f, -18.0f), new(0.72f, 1.7f)),
            ("Corridor_Traffic_C", new(0.35f, 0.058f, -23.7f), new(0.68f, 1.4f)),
            ("Room102_Dust", new(-4.25f, 0.041f, -16.55f), new(1.2f, 0.7f)),
            ("Kitchen_Grease_A", new(3.15f, 0.041f, -21.45f), new(1.2f, 0.68f)),
            ("Kitchen_Grease_B", new(5.75f, 0.041f, -18.9f), new(0.65f, 0.85f)),
            ("UpperCorridor_Worn_A", new(0.25f, 2.835f, -5.0f), new(0.78f, 1.45f)),
            ("UpperCorridor_Worn_B", new(-0.2f, 2.835f, -10.1f), new(0.72f, 1.2f)),
            ("Bathroom_Damp", new(-5.8f, 2.835f, 3.7f), new(1.1f, 0.8f)),
            ("Laundry_Damp", new(-5.35f, 2.835f, -0.5f), new(0.9f, 0.7f)),
            ("Technical_Grime", new(10.9f, 2.835f, 4.1f), new(1.15f, 0.65f))
        };

        foreach (var mark in marks)
            AddDisc(root, mark.Name, mark.Position, mark.Scale, _trafficStain);
    }

    private void BuildCeilingStains()
    {
        var root = GetNode<Node3D>("CeilingStains");
        AddDisc(root, "Reception_Infiltration", new Vector3(-2.6f, 2.405f, -3.4f), new Vector2(1.65f, 0.8f), _ceilingStain);
        AddDisc(root, "Kitchen_Infiltration", new Vector3(4.7f, 2.405f, -20.6f), new Vector2(1.25f, 0.7f), _ceilingStain);
        AddDisc(root, "UpperCorridor_Infiltration", new Vector3(0.35f, 5.795f, -5.6f), new Vector2(1.35f, 0.72f), _ceilingStain);
        AddDisc(root, "Bathroom_Infiltration", new Vector3(-5.1f, 5.395f, 3.7f), new Vector2(1.3f, 0.9f), _ceilingStain);
        AddDisc(root, "Laundry_Infiltration", new Vector3(-5.2f, 5.395f, -0.5f), new Vector2(1.15f, 0.72f), _ceilingStain);
    }

    private void BuildRoom203DragMarks()
    {
        var root = GetNode<Node3D>("Room203DragMarks");
        AddBox(root, "DragMark_A", new Vector3(0.065f, 0.006f, 1.15f), _trafficStain,
            new Vector3(-0.32f, 2.837f, -8.75f), new Vector3(0f, 0.16f, 0f));
        AddBox(root, "DragMark_B", new Vector3(0.05f, 0.006f, 0.88f), _trafficStain,
            new Vector3(0.08f, 2.838f, -8.95f), new Vector3(0f, -0.12f, 0f));
    }

    private static bool ShouldIgnore(string path) =>
        path.Contains("/VisualPolish/", StringComparison.Ordinal) ||
        path.Contains("UpperWing_CollisionDeck", StringComparison.Ordinal) ||
        path.Contains("/Doors/", StringComparison.Ordinal) ||
        path.Contains("Door_", StringComparison.Ordinal) ||
        path.Contains("Stair", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("Varanda", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("Balcony", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("/Props/", StringComparison.Ordinal) ||
        path.Contains("Furniture", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("Interact_", StringComparison.Ordinal);

    private static bool IsWall(MeshInstance3D mesh, string path) =>
        mesh.Name.ToString().Contains("Wall", StringComparison.OrdinalIgnoreCase) ||
        mesh.GetParent()?.Name.ToString().Contains("Wall", StringComparison.OrdinalIgnoreCase) == true ||
        path.Contains("ReceptionWalls", StringComparison.Ordinal) ||
        path.Contains("CorridorWalls", StringComparison.Ordinal) ||
        path.Contains("Room102Walls", StringComparison.Ordinal) ||
        path.Contains("KitchenWalls", StringComparison.Ordinal);

    private static bool IsFloor(MeshInstance3D mesh, string path) =>
        mesh.Name.ToString().StartsWith("Floor_", StringComparison.Ordinal) ||
        path.Contains("FloorsVisual", StringComparison.Ordinal) ||
        path.Contains("SecondFloor_MasterSlab", StringComparison.Ordinal);

    private static bool IsCeiling(MeshInstance3D mesh, string path) =>
        mesh.Name.ToString().Contains("Ceiling", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("PensionCeiling", StringComparison.Ordinal);

    private static bool IsWetArea(string path) =>
        path.Contains("Kitchen", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("Bathroom", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("Laundry", StringComparison.OrdinalIgnoreCase);

    private static bool IsTrafficArea(string path) =>
        path.Contains("Corridor", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("Reception", StringComparison.OrdinalIgnoreCase);

    private static bool IsUpperFloor(string path) =>
        path.Contains("PensionSecondFloor", StringComparison.Ordinal) ||
        path.Contains("UpperWingRooms", StringComparison.Ordinal) ||
        path.Contains("Room203", StringComparison.Ordinal) ||
        path.Contains("SecondFloor", StringComparison.Ordinal);

    private static MeshInstance3D AddBox(Node3D parent, string name, Vector3 size, Material material,
        Vector3? position = null, Vector3? rotation = null)
    {
        var mesh = new MeshInstance3D
        {
            Name = name,
            Mesh = new BoxMesh { Size = size },
            MaterialOverride = material,
            CastShadow = GeometryInstance3D.ShadowCastingSetting.Off,
            Position = position ?? Vector3.Zero,
            Rotation = rotation ?? Vector3.Zero
        };
        parent.AddChild(mesh);
        return mesh;
    }

    private static void AddDisc(Node3D parent, string name, Vector3 position, Vector2 scale, Material material)
    {
        var mesh = new MeshInstance3D
        {
            Name = name,
            Mesh = new CylinderMesh { TopRadius = 0.5f, BottomRadius = 0.5f, Height = 0.006f, RadialSegments = 24 },
            MaterialOverride = material,
            CastShadow = GeometryInstance3D.ShadowCastingSetting.Off,
            Position = position,
            Scale = new Vector3(scale.X, 1f, scale.Y)
        };
        parent.AddChild(mesh);
    }

    private static StandardMaterial3D Material(string name, Color color, float roughness) => new()
    {
        ResourceName = name,
        AlbedoColor = color,
        Roughness = roughness,
        Metallic = 0f,
        Transparency = BaseMaterial3D.TransparencyEnum.Disabled
    };

    private static StandardMaterial3D TransparentMaterial(string name, Color color) => new()
    {
        ResourceName = name,
        AlbedoColor = color,
        Roughness = 1f,
        Metallic = 0f,
        Transparency = BaseMaterial3D.TransparencyEnum.Alpha
    };

    private static IEnumerable<Node> Enumerate(Node root)
    {
        yield return root;
        foreach (var child in root.GetChildren())
            foreach (var nested in Enumerate(child))
                yield return nested;
    }
}
