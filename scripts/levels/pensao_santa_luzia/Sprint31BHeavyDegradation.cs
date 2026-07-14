namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Heavy, visual-only degradation layer for Sprint 31B. Everything created by
/// this node is decoration: no collision, navigation, interaction or gameplay.
/// </summary>
public partial class Sprint31BHeavyDegradation : Node3D
{
    private const float GroundFloorY = 0.064f;
    private const float UpperFloorY = 2.842f;

    private StandardMaterial3D _mold = null!;
    private StandardMaterial3D _peeled = null!;
    private StandardMaterial3D _soot = null!;
    private StandardMaterial3D _ceilingCrack = null!;
    private StandardMaterial3D _ceilingDamp = null!;
    private StandardMaterial3D _waterStreak = null!;
    private StandardMaterial3D _dirtyGlass = null!;
    private StandardMaterial3D _furniturePatina = null!;
    private OmniLight3D? _flickerLight;
    private double _flickerClock;

    private static readonly Dictionary<string, PackedScene> PropCache = new();

    public override void _Ready()
    {
        BuildMaterials();
        CallDeferred(nameof(BuildPass));
    }

    public override void _Process(double delta)
    {
        if (_flickerLight == null) return;
        _flickerClock += delta;
        var pulse = Mathf.Sin((float)_flickerClock * 2.15f) * 0.012f;
        var faultyContact = Mathf.Sin((float)_flickerClock * 7.7f) > 0.94f ? -0.018f : 0f;
        _flickerLight.LightEnergy = 0.205f + pulse + faultyContact;
    }

    private void BuildMaterials()
    {
        _mold = Transparent("Decay_HeavyMold", new Color(0.035f, 0.07f, 0.045f, 0.72f));
        _peeled = Transparent("Decay_PeeledPlaster", new Color(0.36f, 0.31f, 0.23f, 0.66f));
        _soot = Transparent("Decay_SootAndGrime", new Color(0.035f, 0.03f, 0.025f, 0.54f));
        _ceilingCrack = Transparent("Decay_CeilingCrack", new Color(0.025f, 0.023f, 0.02f, 0.8f));
        _ceilingDamp = Transparent("Decay_CeilingDampHalo", new Color(0.055f, 0.058f, 0.045f, 0.48f));
        _waterStreak = Transparent("Decay_VerticalWaterStreak", new Color(0.045f, 0.072f, 0.052f, 0.58f));
        _dirtyGlass = Transparent("Window_DirtyNightGlass", new Color(0.035f, 0.05f, 0.065f, 0.7f));
        _furniturePatina = Transparent("Furniture_OldDustPatina", new Color(0.075f, 0.052f, 0.035f, 0.18f));
    }

    private void BuildPass()
    {
        var scene = GetTree().CurrentScene;
        if (scene == null) return;

        var wallOverlays = BuildWallDecay(scene);
        var floorDetails = BuildFloorDecay();
        var ceilingDetails = BuildCeilingDecay();
        var props = BuildAbandonmentDressing();
        var windows = BuildWindowGrime();
        var lights = BuildMoodLighting();
        var agedFurniture = AgeExistingVisuals(scene);

        SetMeta("wall_decay_count", wallOverlays);
        SetMeta("floor_decay_count", floorDetails);
        SetMeta("ceiling_decay_count", ceilingDetails);
        SetMeta("abandonment_prop_count", props);
        SetMeta("window_grime_count", windows);
        SetMeta("mood_light_count", lights);
        SetMeta("aged_existing_mesh_count", agedFurniture);
        GD.Print($"[Sprint31B] heavy visual degradation applied: {wallOverlays} wall overlays, {floorDetails} floor details, {ceilingDetails} ceiling details, {props} safe props, {windows} dirty windows, {lights} mood lights, {agedFurniture} existing furniture/window meshes patinated; 0 physics/gameplay nodes.");
    }

    private int BuildWallDecay(Node scene)
    {
        var root = GetNode<Node3D>("WallDecay");
        var candidates = Enumerate(scene).OfType<MeshInstance3D>()
            .Where(mesh => IsEligibleWall(mesh, mesh.GetPath().ToString()))
            .ToArray();
        var count = 0;

        for (var i = 0; i < candidates.Length; i++)
        {
            var wall = candidates[i];
            if (wall.Mesh is not BoxMesh box || box.Size.Y < 1.45f) continue;
            var path = wall.GetPath().ToString();
            var wet = IsWet(path);
            if (!wet && i % 3 != 0) continue;

            var longX = box.Size.X >= box.Size.Z;
            var length = Mathf.Min(wet ? 1.55f : 1.05f, Mathf.Max(box.Size.X, box.Size.Z) * 0.38f);
            var size = longX
                ? new Vector3(length, wet ? 0.72f : 0.46f, box.Size.Z + 0.032f)
                : new Vector3(box.Size.X + 0.032f, wet ? 0.72f : 0.46f, length);
            var overlay = AddBox(root, $"{(wet ? "Mold" : "Peeling")}_{count:000}", size, wet ? _mold : _peeled);
            var transform = wall.GlobalTransform;
            var floorY = IsUpper(path) ? 2.82f : 0.02f;
            transform.Origin = new Vector3(transform.Origin.X, floorY + (wet ? 0.39f : 1.22f), transform.Origin.Z);
            overlay.GlobalTransform = transform;
            count++;

            if (wet || i % 4 == 0)
            {
                var streakLength = Mathf.Min(0.22f, length * 0.22f);
                var streakSize = longX
                    ? new Vector3(streakLength, wet ? 1.18f : 0.78f, box.Size.Z + 0.038f)
                    : new Vector3(box.Size.X + 0.038f, wet ? 1.18f : 0.78f, streakLength);
                var streak = AddBox(root, $"WaterStreak_{count:000}", streakSize, _waterStreak);
                var localOffset = longX
                    ? new Vector3(((i % 5) - 2) * Mathf.Min(0.28f, box.Size.X * 0.12f), 0f, 0f)
                    : new Vector3(0f, 0f, ((i % 5) - 2) * Mathf.Min(0.28f, box.Size.Z * 0.12f));
                transform.Origin = wall.GlobalTransform.Origin + wall.GlobalTransform.Basis * localOffset;
                transform.Origin = new Vector3(transform.Origin.X, floorY + (wet ? 0.67f : 0.9f), transform.Origin.Z);
                streak.GlobalTransform = transform;
                count++;
            }

            if (!wet || Mathf.Max(box.Size.X, box.Size.Z) < 2.2f) continue;
            var upperSize = longX
                ? new Vector3(length * 0.62f, 0.28f, box.Size.Z + 0.034f)
                : new Vector3(box.Size.X + 0.034f, 0.28f, length * 0.62f);
            var upper = AddBox(root, $"Soot_{count:000}", upperSize, _soot);
            transform.Origin = new Vector3(transform.Origin.X, floorY + 1.95f, transform.Origin.Z);
            upper.GlobalTransform = transform;
            count++;
        }

        AddWallProp(root, "Bathroom_Mold", "prop_mold_stain_decal_01.glb", new(-6.765f, 3.48f, 3.75f), new(0f, -Mathf.Pi / 2f, 0f), new(1.45f, 1.35f, 1f));
        AddWallProp(root, "Kitchen_Mold", "prop_mold_stain_decal_01.glb", new(3.8f, 0.82f, -23.765f), Vector3.Zero, new(1.8f, 1.55f, 1f));
        AddWallProp(root, "UpperHall_Scratches", "prop_wall_scratch_decal_01.glb", new(-1.265f, 4.05f, -6.25f), new(0f, -Mathf.Pi / 2f, 0f), new(1.5f, 1.8f, 1f));
        return count + 3;
    }

    private int BuildFloorDecay()
    {
        var root = GetNode<Node3D>("FloorDecay");
        var specs = new (string Name, string Asset, Vector3 Position, Vector3 Rotation, Vector3 Scale)[]
        {
            ("Reception_DirtyCloth", "prop_dirty_floor_cloth_01.glb", new(-4.55f, GroundFloorY, -5.6f), new(0f, 0.25f, 0f), new(1.35f, 1f, 1.2f)),
            ("Reception_DragMark", "prop_drag_mark_decal_01.glb", new(-1.55f, GroundFloorY, -4.1f), new(0f, -0.32f, 0f), new(1.05f, 1f, 1.25f)),
            ("Corridor_DragMark", "prop_drag_mark_decal_01.glb", new(0.72f, GroundFloorY, -12.2f), new(0f, 0.08f, 0f), new(0.7f, 1f, 1.05f)),
            ("Kitchen_DragMark", "prop_drag_mark_decal_01.glb", new(5.45f, GroundFloorY, -20.8f), new(0f, 0.6f, 0f), new(1.0f, 1f, 1.15f)),
            ("UpperHall_DirtyCloth", "prop_dirty_floor_cloth_01.glb", new(-4.3f, UpperFloorY, -21.8f), new(0f, -0.42f, 0f), new(1.25f, 1f, 1.05f)),
            ("UpperCorridor_DragMark", "prop_drag_mark_decal_01.glb", new(0.62f, UpperFloorY, -8.1f), new(0f, -0.1f, 0f), new(0.68f, 1f, 0.92f)),
            ("Bathroom_DirtyCloth", "prop_dirty_floor_cloth_01.glb", new(-3.15f, UpperFloorY, 4.85f), new(0f, 0.45f, 0f), new(1.05f, 1f, 0.9f)),
            ("Laundry_DragMark", "prop_drag_mark_decal_01.glb", new(-5.65f, UpperFloorY, -0.15f), new(0f, 0.7f, 0f), new(0.75f, 1f, 0.9f))
        };
        foreach (var spec in specs)
            AddProp(root, spec.Name, spec.Asset, spec.Position, spec.Rotation, spec.Scale);

        var edgeGrime = new (string Name, Vector3 Position, Vector3 Size, float Angle)[]
        {
            ("Reception_EdgeGrime_West", new(-5.55f, 0.069f, -4.75f), new(0.62f, 0.008f, 1.7f), 0.03f),
            ("Reception_EdgeGrime_East", new(5.55f, 0.069f, -1.2f), new(0.48f, 0.008f, 1.35f), -0.05f),
            ("GroundCorridor_EdgeGrime", new(0.98f, 0.069f, -16.2f), new(0.32f, 0.008f, 2.1f), 0.02f),
            ("Kitchen_EdgeGrime", new(3.45f, 0.069f, -23.55f), new(2.2f, 0.008f, 0.25f), -0.02f),
            ("UpperLanding_EdgeGrime", new(-4.55f, 2.847f, -21.0f), new(0.3f, 0.008f, 1.55f), 0.02f),
            ("UpperCorridor_EdgeGrime", new(0.67f, 2.847f, -15.3f), new(0.27f, 0.008f, 2.2f), -0.015f),
            ("Bathroom_EdgeGrime", new(-5.15f, 2.847f, 5.35f), new(1.75f, 0.008f, 0.24f), 0.01f),
            ("Room201_EdgeGrime", new(-5.2f, 2.847f, -12.15f), new(1.45f, 0.008f, 0.22f), -0.02f)
        };
        foreach (var grime in edgeGrime)
            AddBox(root, grime.Name, grime.Size, _soot, grime.Position, new Vector3(0f, grime.Angle, 0f));
        return specs.Length + edgeGrime.Length;
    }

    private int BuildCeilingDecay()
    {
        var root = GetNode<Node3D>("CeilingDecay");
        var cracks = new (string Name, Vector3 Position, Vector3 Size, float Angle)[]
        {
            ("Reception_Crack_A", new(-2.1f, 2.397f, -3.9f), new(1.25f, 0.007f, 0.025f), 0.34f),
            ("Reception_Crack_B", new(-1.62f, 2.396f, -3.62f), new(0.62f, 0.007f, 0.02f), -0.55f),
            ("Kitchen_Crack", new(4.4f, 2.397f, -20.3f), new(1.05f, 0.007f, 0.024f), -0.28f),
            ("UpperHall_Crack_A", new(-0.55f, 5.787f, -5.5f), new(1.15f, 0.007f, 0.023f), 0.22f),
            ("UpperHall_Crack_B", new(-0.12f, 5.786f, -5.22f), new(0.55f, 0.007f, 0.019f), -0.68f),
            ("Bathroom_Crack", new(-4.8f, 5.387f, 3.8f), new(0.85f, 0.007f, 0.022f), 0.48f)
        };
        foreach (var crack in cracks)
            AddBox(root, crack.Name, crack.Size, _ceilingCrack, crack.Position, new Vector3(0f, crack.Angle, 0f));

        AddDisc(root, "Reception_DampHalo", new(-3.15f, 2.394f, -2.85f), new(1.55f, 0.78f), _ceilingDamp);
        AddDisc(root, "Kitchen_DampHalo", new(4.95f, 2.394f, -21.1f), new(1.28f, 0.72f), _ceilingDamp);
        AddDisc(root, "UpperHall_DampHalo", new(-2.3f, 5.784f, -21.05f), new(1.35f, 0.68f), _ceilingDamp);
        AddDisc(root, "UpperCorridor_DampHalo", new(0.2f, 5.784f, -14.8f), new(1.1f, 0.55f), _ceilingDamp);
        AddDisc(root, "Bathroom_DampHalo", new(-4.9f, 5.384f, 3.9f), new(1.25f, 0.82f), _ceilingDamp);
        return cracks.Length + 5;
    }

    private int BuildAbandonmentDressing()
    {
        var root = GetNode<Node3D>("AbandonmentProps");
        var specs = new (string Name, string Asset, Vector3 Position, Vector3 Rotation, Vector3 Scale)[]
        {
            ("Reception_Papers", "prop_scattered_papers_01.glb", new(-3.75f, GroundFloorY, -1.15f), new(0f, 0.4f, 0f), new(1.4f, 1f, 1.4f)),
            ("Reception_Bottle", "prop_old_bottle_01.glb", new(4.7f, 0.194f, -5.65f), new(0f, -0.35f, 0f), Vector3.One),
            ("Room102_Crate", "prop_wooden_crate_01.glb", new(-6.0f, 0.314f, -13.3f), new(0f, 0.12f, 0f), new(0.82f, 0.82f, 0.82f)),
            ("Kitchen_Papers", "prop_scattered_papers_01.glb", new(2.15f, GroundFloorY, -22.85f), new(0f, -0.55f, 0f), new(1.55f, 1f, 1.55f)),
            ("Kitchen_Bottle", "prop_old_bottle_01.glb", new(6.45f, 0.194f, -18.45f), new(0f, 0.2f, 0f), Vector3.One),
            ("Kitchen_Plank", "prop_wood_plank_01.glb", new(2.3f, 0.10f, -23.45f), new(0f, -0.06f, 0.08f), new(1.25f, 1f, 1f)),
            ("UpperHall_Papers", "prop_scattered_papers_01.glb", new(-4.18f, UpperFloorY, -20.12f), new(0f, 0.18f, 0f), new(1.5f, 1f, 1.5f)),
            ("UpperHall_Bottle", "prop_old_bottle_01.glb", new(-4.42f, 2.972f, -19.72f), new(0f, -0.2f, 0f), Vector3.One),
            ("Laundry_Crate", "prop_wooden_crate_01.glb", new(-6.05f, 3.092f, -1.65f), new(0f, -0.18f, 0f), new(0.82f, 0.82f, 0.82f)),
            ("Room204_Papers", "prop_scattered_papers_01.glb", new(9.35f, UpperFloorY, -1.65f), new(0f, 0.75f, 0f), new(1.4f, 1f, 1.4f)),
            ("Office_Bottle", "prop_old_bottle_01.glb", new(-5.75f, 2.972f, 7.75f), new(0f, 0.15f, 0f), Vector3.One)
        };
        foreach (var spec in specs)
            AddProp(root, spec.Name, spec.Asset, spec.Position, spec.Rotation, spec.Scale);

        AddWallProp(root, "Room202_TornCurtain", "prop_torn_curtain_01.glb", new(6.69f, 4.18f, -17f), new(0f, Mathf.Pi / 2f, 0f), new(0.82f, 1.05f, 1f));
        return specs.Length + 1;
    }

    private int BuildWindowGrime()
    {
        var root = GetNode<Node3D>("WindowGrime");
        AddBox(root, "Room201_DirtyGlass", new(0.014f, 0.76f, 0.88f), _dirtyGlass, new(-6.705f, 4.18f, -14f));
        AddBox(root, "Room202_DirtyGlass", new(0.014f, 0.76f, 0.88f), _dirtyGlass, new(6.705f, 4.18f, -17f));
        return 2;
    }

    private int BuildMoodLighting()
    {
        var root = GetNode<Node3D>("LightingMood");
        _flickerLight = AddLight(root, "Reception_WeakWarm_Flicker", new(-1.5f, 2.08f, -3.9f), new Color(0.78f, 0.48f, 0.23f), 0.205f, 5.2f);
        AddLight(root, "Kitchen_WeakWarm", new(3.9f, 2.05f, -20.6f), new Color(0.69f, 0.39f, 0.19f), 0.15f, 4.1f);
        AddLight(root, "UpperLanding_ColdWindow", new(-0.2f, 4.85f, -5.5f), new Color(0.22f, 0.34f, 0.55f), 0.16f, 5.4f);
        AddLight(root, "Bathroom_ColdDamp", new(-4.8f, 4.95f, 3.7f), new Color(0.22f, 0.34f, 0.42f), 0.09f, 3.4f);
        return 4;
    }

    private int AgeExistingVisuals(Node scene)
    {
        var count = 0;
        foreach (var mesh in Enumerate(scene).OfType<MeshInstance3D>())
        {
            var path = mesh.GetPath().ToString();
            var importedFurniture = path.Contains("/Sprint30A_BlenderAssetPilot/", StringComparison.Ordinal) ||
                                    path.Contains("/Sprint30B_BlenderProps/", StringComparison.Ordinal);
            var fakeWindow = path.Contains("/Sprint27_FakeWindowsLighting/", StringComparison.Ordinal) &&
                             path.Contains("Window", StringComparison.OrdinalIgnoreCase);
            if (!importedFurniture && !fakeWindow) continue;
            if (path.Contains("Backup", StringComparison.OrdinalIgnoreCase) ||
                path.Contains("Drain", StringComparison.OrdinalIgnoreCase) ||
                path.Contains("ElectricPanel", StringComparison.OrdinalIgnoreCase) ||
                path.Contains("Mirror", StringComparison.OrdinalIgnoreCase)) continue;

            mesh.MaterialOverlay = path.Contains("Window", StringComparison.OrdinalIgnoreCase)
                ? _dirtyGlass
                : _furniturePatina;
            count++;
        }
        return count;
    }

    private static OmniLight3D AddLight(Node3D parent, string name, Vector3 position, Color color, float energy, float range)
    {
        var light = new OmniLight3D
        {
            Name = name,
            Position = position,
            LightColor = color,
            LightEnergy = energy,
            OmniRange = range,
            ShadowEnabled = false
        };
        parent.AddChild(light);
        return light;
    }

    private static void AddWallProp(Node3D parent, string name, string asset, Vector3 position, Vector3 rotation, Vector3 scale) =>
        AddProp(parent, name, asset, position, rotation, scale);

    private static Node3D AddProp(Node3D parent, string name, string asset, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        if (!PropCache.TryGetValue(asset, out var packed))
        {
            packed = GD.Load<PackedScene>($"res://assets/models/props/pensao/{asset}");
            PropCache[asset] = packed;
        }
        var instance = packed.Instantiate<Node3D>();
        instance.Name = name;
        instance.Position = position;
        instance.Rotation = rotation;
        instance.Scale = scale;
        parent.AddChild(instance);
        return instance;
    }

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

    private static bool IsEligibleWall(MeshInstance3D mesh, string path)
    {
        if (path.Contains("/VisualPolish/", StringComparison.Ordinal) ||
            path.Contains("UpperWing_CollisionDeck", StringComparison.Ordinal) ||
            path.Contains("Door", StringComparison.OrdinalIgnoreCase) ||
            path.Contains("Stair", StringComparison.OrdinalIgnoreCase) ||
            path.Contains("Varanda", StringComparison.OrdinalIgnoreCase) ||
            path.Contains("Balcony", StringComparison.OrdinalIgnoreCase) ||
            path.Contains("Room205_Locked", StringComparison.Ordinal) ||
            path.Contains("Interact", StringComparison.OrdinalIgnoreCase)) return false;
        return mesh.Name.ToString().Contains("Wall", StringComparison.OrdinalIgnoreCase) ||
               mesh.GetParent()?.Name.ToString().Contains("Wall", StringComparison.OrdinalIgnoreCase) == true ||
               path.Contains("ReceptionWalls", StringComparison.Ordinal) ||
               path.Contains("CorridorWalls", StringComparison.Ordinal) ||
               path.Contains("Room102Walls", StringComparison.Ordinal) ||
               path.Contains("KitchenWalls", StringComparison.Ordinal);
    }

    private static bool IsWet(string path) =>
        path.Contains("Kitchen", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("Bathroom", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("Laundry", StringComparison.OrdinalIgnoreCase);

    private static bool IsUpper(string path) =>
        path.Contains("PensionSecondFloor", StringComparison.Ordinal) ||
        path.Contains("UpperWingRooms", StringComparison.Ordinal) ||
        path.Contains("Room203", StringComparison.Ordinal) ||
        path.Contains("SecondFloor", StringComparison.Ordinal);

    private static StandardMaterial3D Transparent(string name, Color color) => new()
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
