namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Sprint 31C-1: PBR-only surface pass for the existing pension meshes.
/// It changes material overrides and adds thin, non-colliding visual decals.
/// No transform, structural mesh, collision, navigation or gameplay node is changed.
/// </summary>
public partial class Sprint31CPbrMaterialPass : Node3D
{
    private const string MaterialRoot = "res://assets/materials/pensao/";
    private const string DecalRoot = "res://assets/decals/pensao/";

    private StandardMaterial3D _dryPlaster = null!;
    private StandardMaterial3D _dampPlaster = null!;
    private StandardMaterial3D _oldWood = null!;
    private StandardMaterial3D _infiltratedCeiling = null!;
    private StandardMaterial3D _groundWall = null!;
    private StandardMaterial3D _groundFloor = null!;
    private StandardMaterial3D _groundCeiling = null!;
    private Material _interFloor = null!;
    private StandardMaterial3D _wetFloor = null!;
    private StandardMaterial3D _coldBalcony = null!;
    private StandardMaterial3D _stairDampWall = null!;
    private StandardMaterial3D _stairOldFloor = null!;
    private StandardMaterial3D _stairCeiling = null!;
    private StandardMaterial3D _upperLandingWall = null!;
    private StandardMaterial3D _upperLandingFloor = null!;
    private StandardMaterial3D _upperLandingCeiling = null!;

    public override void _Ready()
    {
        _dryPlaster = LoadMaterial("M_RebocoSeco_Master.tres");
        _dampPlaster = LoadMaterial("M_RebocoUmidoMofado_Master.tres");
        _oldWood = LoadMaterial("M_MadeiraVelha_Master.tres");
        _infiltratedCeiling = LoadMaterial("M_TetoInfiltrado_Master.tres");
        _groundWall = LoadMaterial("M_Terreo_RebocoMofadoBR_Master.tres");
        _groundFloor = LoadMaterial("M_Terreo_AssoalhoBR_Master.tres");
        _groundCeiling = LoadMaterial("M_Terreo_TetoRebocoInfiltrado_Master.tres");
        _interFloor = LoadAnyMaterial("M_Entrepiso_TerreoTeto_SuperiorAssoalho.tres");

        _wetFloor = (StandardMaterial3D)_oldWood.Duplicate(true);
        _wetFloor.ResourceName = "M_MadeiraVelha_Umida_Runtime";
        _wetFloor.AlbedoColor = new Color(0.48f, 0.52f, 0.46f, 1f);
        _wetFloor.Roughness = 0.99f;

        _coldBalcony = (StandardMaterial3D)_oldWood.Duplicate(true);
        _coldBalcony.ResourceName = "M_VarandaFria_Runtime";
        _coldBalcony.AlbedoColor = new Color(0.39f, 0.43f, 0.46f, 1f);
        _coldBalcony.Roughness = 0.98f;

        // Same authored PBR family, two deliberately different atmosphere
        // profiles: the stair shaft is wetter/darker, while the upper arrival
        // is drier, colder and faded. No new geometry or collision is created.
        _stairDampWall = (StandardMaterial3D)_dampPlaster.Duplicate(true);
        _stairDampWall.ResourceName = "M_Escadaria_RebocoUmido_Runtime";
        _stairDampWall.AlbedoColor = new Color(0.43f, 0.46f, 0.39f, 1f);
        _stairDampWall.Roughness = 0.99f;
        _stairDampWall.NormalScale = 2.05f;
        _stairDampWall.Uv1Scale = new Vector3(0.57f, 0.57f, 0.57f);

        _stairOldFloor = (StandardMaterial3D)_groundFloor.Duplicate(true);
        _stairOldFloor.ResourceName = "M_Escadaria_AssoalhoEscuro_Runtime";
        _stairOldFloor.AlbedoColor = new Color(0.52f, 0.43f, 0.34f, 1f);
        _stairOldFloor.Roughness = 0.99f;

        _stairCeiling = (StandardMaterial3D)_groundCeiling.Duplicate(true);
        _stairCeiling.ResourceName = "M_Escadaria_TetoUmido_Runtime";
        _stairCeiling.AlbedoColor = new Color(0.48f, 0.49f, 0.43f, 1f);
        _stairCeiling.Roughness = 0.99f;

        _upperLandingWall = (StandardMaterial3D)_dryPlaster.Duplicate(true);
        _upperLandingWall.ResourceName = "M_ChegadaSuperior_RebocoFrio_Runtime";
        _upperLandingWall.AlbedoColor = new Color(0.61f, 0.59f, 0.54f, 1f);
        _upperLandingWall.Roughness = 0.97f;
        _upperLandingWall.NormalScale = 1.55f;
        _upperLandingWall.Uv1Scale = new Vector3(0.74f, 0.74f, 0.74f);

        _upperLandingFloor = (StandardMaterial3D)_oldWood.Duplicate(true);
        _upperLandingFloor.ResourceName = "M_ChegadaSuperior_MadeiraGasta_Runtime";
        _upperLandingFloor.AlbedoColor = new Color(0.43f, 0.38f, 0.33f, 1f);
        _upperLandingFloor.Roughness = 0.98f;

        _upperLandingCeiling = (StandardMaterial3D)_infiltratedCeiling.Duplicate(true);
        _upperLandingCeiling.ResourceName = "M_ChegadaSuperior_TetoFrio_Runtime";
        _upperLandingCeiling.AlbedoColor = new Color(0.54f, 0.55f, 0.52f, 1f);
        _upperLandingCeiling.Roughness = 0.98f;

        CallDeferred(nameof(ApplyPass));
    }

    private void ApplyPass()
    {
        var scene = GetTree().CurrentScene;
        if (scene == null) return;

        var walls = 0;
        var floors = 0;
        var ceilings = 0;
        var balconyVisuals = 0;
        var groundWalls = 0;
        var groundFloors = 0;
        var groundCeilings = 0;
        var interFloorMeshes = 0;
        var stairSurfaces = 0;
        var upperLandingSurfaces = 0;

        foreach (var mesh in Enumerate(scene).OfType<MeshInstance3D>())
        {
            var path = mesh.GetPath().ToString();
            if (ShouldIgnore(path)) continue;

            if (IsCeiling(mesh, path))
            {
                if (IsStairTransitionCeiling(path))
                {
                    mesh.MaterialOverride = _stairCeiling;
                    stairSurfaces++;
                }
                else if (IsUpperLandingCeiling(path))
                {
                    mesh.MaterialOverride = _upperLandingCeiling;
                    upperLandingSurfaces++;
                }
                else if (IsGroundFloorSurface(mesh, SurfaceKind.Ceiling, path))
                {
                    mesh.MaterialOverride = _groundCeiling;
                    groundCeilings++;
                }
                else
                {
                    mesh.MaterialOverride = _infiltratedCeiling;
                }

                ceilings++;
                continue;
            }

            if (IsFloor(mesh, path))
            {
                if (IsStairwellFloor(path))
                {
                    mesh.MaterialOverride = _stairOldFloor;
                    stairSurfaces++;
                }
                else if (IsUpperLandingFloor(path))
                {
                    mesh.MaterialOverride = _upperLandingFloor;
                    upperLandingSurfaces++;
                }
                else if (IsInterFloorSlab(mesh, path))
                {
                    // One authored BoxMesh is simultaneously the upper floor and
                    // the visible downstairs ceiling. The shader keeps the
                    // already-approved wood on top and uses plaster only below.
                    mesh.MaterialOverride = _interFloor;
                    interFloorMeshes++;
                }
                else if (IsGroundFloorSurface(mesh, SurfaceKind.Floor, path))
                {
                    mesh.MaterialOverride = _groundFloor;
                    groundFloors++;
                }
                else if (path.Contains("SecondFloor_MasterSlab", StringComparison.Ordinal))
                {
                    // Visual slab only. UpperWing_CollisionDeck is explicitly excluded above.
                    mesh.MaterialOverride = _coldBalcony;
                    balconyVisuals++;
                }
                else
                {
                    mesh.MaterialOverride = IsWetArea(path) ? _wetFloor : _oldWood;
                }

                floors++;
                continue;
            }

            if (!IsWall(mesh, path)) continue;
            if (IsStairwellWall(path))
            {
                mesh.MaterialOverride = _stairDampWall;
                stairSurfaces++;
            }
            else if (IsUpperLandingWall(path))
            {
                mesh.MaterialOverride = _upperLandingWall;
                upperLandingSurfaces++;
            }
            else if (IsGroundFloorSurface(mesh, SurfaceKind.Wall, path) || IsGroundFacadeSurface(path))
            {
                // A single world-aligned material across every downstairs wall
                // removes the dry/wet material boundary that read as two images.
                mesh.MaterialOverride = _groundWall;
                groundWalls++;
            }
            else
            {
                mesh.MaterialOverride = IsWetArea(path) || IsDampWallVariation(mesh, path)
                    ? _dampPlaster
                    : _dryPlaster;
            }

            walls++;
        }

        // The reviewed prototype used thin BoxMesh overlays. In motion these
        // read as opaque plates and white strips, so no 31C decal is spawned
        // until the alpha/feathered version is rebuilt as a real decal pass.
        var decals = 0;
        var disabledLegacyOverlays = DisableLegacyHardEdgedOverlays(scene);
        TuneEnvironment(scene);

        SetMeta("materials_applied", walls + floors + ceilings);
        SetMeta("wall_material_count", walls);
        SetMeta("floor_material_count", floors);
        SetMeta("ceiling_material_count", ceilings);
        SetMeta("balcony_visual_material_count", balconyVisuals);
        SetMeta("ground_wall_material_count", groundWalls);
        SetMeta("ground_floor_material_count", groundFloors);
        SetMeta("ground_ceiling_material_count", groundCeilings);
        SetMeta("inter_floor_dual_face_material_count", interFloorMeshes);
        SetMeta("stair_surface_material_count", stairSurfaces);
        SetMeta("upper_landing_surface_material_count", upperLandingSurfaces);
        SetMeta("ground_floor_only_extension", false);
        SetMeta("second_floor_material_profile_preserved", false);
        SetMeta("stair_upper_landing_extension", true);
        SetMeta("decal_count", decals);
        SetMeta("disabled_hard_edged_overlay_count", disabledLegacyOverlays);
        GD.Print($"[Sprint31C] Pension PBR applied: {groundWalls} ground walls, {groundFloors} ground floors, {groundCeilings} ground ceiling plates, {stairSurfaces} stair-shaft surfaces, {upperLandingSurfaces} upper-arrival surfaces and {interFloorMeshes} dual-face inter-floor slabs; totals {walls}/{floors}/{ceilings}; {disabledLegacyOverlays} hard-edged overlay containers disabled; 0 decals/physics/gameplay nodes; frozen deck untouched.");
    }

    private static int DisableLegacyHardEdgedOverlays(Node scene)
    {
        var paths = new[]
        {
            "World/VisualPolish/Sprint31_Materials/DampPatches",
            "World/VisualPolish/Sprint31_Materials/CeilingStains",
            "World/VisualPolish/Sprint31B_HeavyDegradation/WallDecay",
            "World/VisualPolish/Sprint31B_HeavyDegradation/CeilingDecay"
        };
        var disabled = 0;
        foreach (var path in paths)
        {
            var visual = scene.GetNodeOrNull<Node3D>(path);
            if (visual == null) continue;
            visual.Visible = false;
            disabled++;
        }

        return disabled;
    }

    private int BuildDecals()
    {
        var wall = GetNode<Node3D>("WallDecals");
        var floor = GetNode<Node3D>("FloorDecals");
        var ceiling = GetNode<Node3D>("CeilingDecals");
        var count = 0;

        var mold = DecalMaterial("Decal_Mofo_Rodape_01.png", "Decal_Mofo_Rodape_01_Mat");
        var dampCorner = DecalMaterial("Decal_Umidade_Canto_01.png", "Decal_Umidade_Canto_01_Mat");
        var peeled = DecalMaterial("Decal_Reboco_Descascado_01.png", "Decal_Reboco_Descascado_01_Mat");
        var streak = DecalMaterial("Decal_Infiltracao_Vertical_01.png", "Decal_Infiltracao_Vertical_01_Mat");
        var ceilingWater = DecalMaterial("Decal_Teto_Mancha_Agua_01.png", "Decal_Teto_Mancha_Agua_01_Mat");
        var floorGrime = DecalMaterial("Decal_Piso_Sujeira_Canto_01.png", "Decal_Piso_Sujeira_Canto_01_Mat");

        // Large wall damage. Offsets are 1.5-3.5 cm from the validated wall faces.
        AddDecalBox(wall, "Decal_Mofo_Rodape_01", new(2.65f, 0.95f, 0.018f), new(4.45f, 0.59f, -23.73f), mold); count++;
        AddDecalBox(wall, "Decal_Umidade_Canto_01", new(0.018f, 1.72f, 2.15f), new(-7.165f, 3.78f, 3.75f), dampCorner); count++;
        AddDecalBox(wall, "Decal_Reboco_Descascado_01", new(0.018f, 1.42f, 2.25f), new(-1.165f, 1.42f, -16.1f), peeled); count++;
        AddDecalBox(wall, "Decal_Infiltracao_Vertical_01", new(0.018f, 1.72f, 2.05f), new(-0.765f, 4.18f, -15.0f), streak); count++;
        AddDecalBox(wall, "Decal_Mofo_Rodape_01_Room201", new(0.018f, 0.88f, 1.35f), new(-6.745f, 3.38f, -15.25f), mold); count++;
        AddDecalBox(wall, "Decal_Reboco_Descascado_01_Reception", new(0.018f, 1.28f, 1.95f), new(-5.965f, 1.31f, -4.5f), peeled); count++;

        // Floor dirt is purely visual and sits millimetres above the existing walkable surface.
        AddDecalBox(floor, "Decal_Piso_Sujeira_Canto_01", new(2.6f, 0.008f, 1.35f), new(-4.6f, 0.071f, -4.65f), floorGrime); count++;
        AddDecalBox(floor, "Decal_Piso_Sujeira_Canto_01_Kitchen", new(2.45f, 0.008f, 1.3f), new(3.65f, 0.071f, -22.55f), floorGrime); count++;
        AddDecalBox(floor, "Decal_Piso_Sujeira_Canto_01_UpperHall", new(2.7f, 0.008f, 1.25f), new(-4.2f, 2.849f, -21.0f), floorGrime); count++;
        AddDecalBox(floor, "Decal_Piso_Sujeira_Canto_01_Bathroom", new(1.35f, 0.008f, 1.2f), new(-5.65f, 2.849f, 4.75f), floorGrime); count++;

        // Ceiling water halos sit just below the visible ceiling, never inside the collision deck.
        AddDecalBox(ceiling, "Decal_Teto_Mancha_Agua_01", new(2.8f, 0.008f, 1.5f), new(-2.45f, 2.394f, -3.55f), ceilingWater); count++;
        AddDecalBox(ceiling, "Decal_Teto_Mancha_Agua_01_Kitchen", new(2.45f, 0.008f, 1.35f), new(4.45f, 2.394f, -20.55f), ceilingWater); count++;
        AddDecalBox(ceiling, "Decal_Teto_Mancha_Agua_01_Upper", new(2.7f, 0.008f, 1.3f), new(0.1f, 5.784f, -14.7f), ceilingWater); count++;
        AddDecalBox(ceiling, "Decal_Teto_Mancha_Agua_01_Bathroom", new(1.85f, 0.008f, 1.35f), new(-5.1f, 5.384f, 3.75f), ceilingWater); count++;
        return count;
    }

    private static void TuneEnvironment(Node scene)
    {
        var worldEnvironment = Enumerate(scene).OfType<WorldEnvironment>().FirstOrDefault();
        var environment = worldEnvironment?.Environment;
        if (environment == null) return;

        // Keep the decayed PBR response without crushing the reception into black.
        // The previous pass compounded dark albedo, low ambient energy and low exposure.
        environment.BackgroundEnergyMultiplier = 1.0f;
        environment.AmbientLightEnergy = 0.34f;
        environment.TonemapExposure = 0.9f;
    }

    private static StandardMaterial3D LoadMaterial(string file) =>
        ResourceLoader.Load<StandardMaterial3D>(MaterialRoot + file)
        ?? throw new InvalidOperationException($"Sprint31C material not found: {file}");

    private static Material LoadAnyMaterial(string file) =>
        ResourceLoader.Load<Material>(MaterialRoot + file)
        ?? throw new InvalidOperationException($"Sprint31C material not found: {file}");

    private static StandardMaterial3D DecalMaterial(string file, string name)
    {
        var texture = ResourceLoader.Load<Texture2D>(DecalRoot + file)
            ?? throw new InvalidOperationException($"Sprint31C decal not found: {file}");
        return new StandardMaterial3D
        {
            ResourceName = name,
            AlbedoColor = Colors.White,
            AlbedoTexture = texture,
            Roughness = 1f,
            Metallic = 0f,
            Transparency = BaseMaterial3D.TransparencyEnum.Alpha,
            ShadingMode = BaseMaterial3D.ShadingModeEnum.PerPixel,
            Uv1Triplanar = true,
            Uv1TriplanarSharpness = 2f
        };
    }

    private static void AddDecalBox(Node3D parent, string name, Vector3 size, Vector3 position, Material material)
    {
        parent.AddChild(new MeshInstance3D
        {
            Name = name,
            Mesh = new BoxMesh { Size = size },
            Position = position,
            MaterialOverride = material,
            CastShadow = GeometryInstance3D.ShadowCastingSetting.Off
        });
    }

    private static bool ShouldIgnore(string path) =>
        path.Contains("/VisualPolish/", StringComparison.Ordinal) ||
        path.Contains("UpperWing_CollisionDeck", StringComparison.Ordinal) ||
        path.Contains("/Doors/", StringComparison.Ordinal) ||
        path.Contains("Door_", StringComparison.Ordinal) ||
        (path.Contains("Stair", StringComparison.OrdinalIgnoreCase) && !IsApprovedStairSurface(path)) ||
        path.Contains("/Props/", StringComparison.Ordinal) ||
        path.Contains("Furniture", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("Interaction", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("Interact_", StringComparison.Ordinal) ||
        path.Contains("TechnicalPanel", StringComparison.Ordinal) ||
        path.Contains("Drain", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("Fuse", StringComparison.OrdinalIgnoreCase);

    private static bool IsWall(MeshInstance3D mesh, string path) =>
        mesh.Name.ToString().Contains("Wall", StringComparison.OrdinalIgnoreCase) ||
        mesh.GetParent()?.Name.ToString().Contains("Wall", StringComparison.OrdinalIgnoreCase) == true ||
        path.Contains("ReceptionWalls", StringComparison.Ordinal) ||
        path.Contains("CorridorWalls", StringComparison.Ordinal) ||
        path.Contains("Room102Walls", StringComparison.Ordinal) ||
        path.Contains("KitchenWalls", StringComparison.Ordinal) ||
        path.Contains("BuildingExteriorShell", StringComparison.Ordinal) ||
        path.Contains("Shell_FacadeUpper", StringComparison.Ordinal) ||
        IsStairwellWall(path) ||
        IsUpperLandingWall(path);

    private static bool IsFloor(MeshInstance3D mesh, string path) =>
        mesh.Name.ToString().StartsWith("Floor_", StringComparison.Ordinal) ||
        mesh.Name.ToString().Contains("Floor", StringComparison.OrdinalIgnoreCase) ||
        (mesh.GetParent()?.Name.ToString().StartsWith("Floor_", StringComparison.Ordinal) == true &&
         mesh.GlobalPosition.Y < 1.35f) ||
        path.Contains("FloorsVisual", StringComparison.Ordinal) ||
        path.Contains("SecondFloor_MasterSlab", StringComparison.Ordinal) ||
        IsStairwellFloor(path) ||
        IsUpperLandingFloor(path);

    private static bool IsCeiling(MeshInstance3D mesh, string path) =>
        mesh.Name.ToString().Contains("Ceiling", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("PensionCeiling", StringComparison.Ordinal) ||
        IsStairTransitionCeiling(path) ||
        IsUpperLandingCeiling(path);

    private static bool IsApprovedStairSurface(string path) =>
        IsStairwellWall(path) || IsStairwellFloor(path) || IsStairTransitionCeiling(path) ||
        IsUpperLandingWall(path) || IsUpperLandingFloor(path) || IsUpperLandingCeiling(path);

    private static bool IsStairwellWall(string path) =>
        path.Contains("StairWell_Wall_West", StringComparison.Ordinal) ||
        path.Contains("UpperStair_BackClosureWall", StringComparison.Ordinal) ||
        path.Contains("UpperStair_NorthEastSeal", StringComparison.Ordinal);

    private static bool IsStairwellFloor(string path) =>
        path.Contains("StairWell_FloorVisual", StringComparison.Ordinal);

    private static bool IsStairTransitionCeiling(string path) =>
        path.Contains("Ceiling_Transition", StringComparison.Ordinal);

    private static bool IsUpperLandingWall(string path) =>
        path.Contains("UpperLanding_BackSeal", StringComparison.Ordinal) ||
        path.Contains("Wall_Transition_West", StringComparison.Ordinal) ||
        path.Contains("Wall_Transition_East", StringComparison.Ordinal);

    private static bool IsUpperLandingFloor(string path) =>
        path.Contains("UpperLanding_Main", StringComparison.Ordinal) ||
        path.Contains("UpperLanding_StairBridge", StringComparison.Ordinal);

    private static bool IsUpperLandingCeiling(string path) =>
        path.Contains("UpperLanding", StringComparison.Ordinal) &&
        path.Contains("Ceiling", StringComparison.OrdinalIgnoreCase);

    private static bool IsWetArea(string path) =>
        path.Contains("Kitchen", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("Bathroom", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("Bath", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("Laundry", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("Varanda", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("Balcony", StringComparison.OrdinalIgnoreCase);

    private static bool IsDampWallVariation(MeshInstance3D mesh, string path)
    {
        if (path.Contains("Exterior", StringComparison.OrdinalIgnoreCase)) return true;
        var hash = StringComparer.Ordinal.GetHashCode(mesh.Name.ToString() + path);
        return Math.Abs(hash % 4) == 0;
    }

    private enum SurfaceKind
    {
        Wall,
        Floor,
        Ceiling
    }

    private static bool IsGroundFloorSurface(MeshInstance3D mesh, SurfaceKind kind, string path)
    {
        if (IsExplicitUpperPath(path)) return false;
        if (path.Contains("PensionGroundFloor", StringComparison.Ordinal) ||
            mesh.IsInGroup("level_first_floor"))
            return true;

        var y = mesh.GlobalPosition.Y;
        return kind switch
        {
            SurfaceKind.Floor => y < 1.35f,
            SurfaceKind.Wall => y < 2.70f,
            SurfaceKind.Ceiling => y < 4.0f,
            _ => false
        };
    }

    private static bool IsExplicitUpperPath(string path) =>
        path.Contains("PensionSecondFloor", StringComparison.Ordinal) ||
        path.Contains("/SecondFloor/", StringComparison.Ordinal) ||
        path.Contains("UpperWing", StringComparison.Ordinal) ||
        path.Contains("BalconyWing", StringComparison.Ordinal) ||
        path.Contains("Room203", StringComparison.Ordinal) ||
        path.Contains("Room204", StringComparison.Ordinal) ||
        path.Contains("Room205", StringComparison.Ordinal);

    private static bool IsGroundFacadeSurface(string path) =>
        path.Contains("/Exterior/Shell_FacadeUpper_Front", StringComparison.Ordinal) ||
        path.Contains("/Exterior/Shell_FacadeUpper_Parapet", StringComparison.Ordinal);

    private static bool IsInterFloorSlab(MeshInstance3D mesh, string path)
    {
        if (path.Contains("SecondFloor_MasterSlab", StringComparison.Ordinal)) return true;
        if (!path.Contains("PensionSecondFloor", StringComparison.Ordinal)) return false;
        return mesh.GlobalPosition.Y < 3.2f &&
               (mesh.Name.ToString().Contains("Floor", StringComparison.OrdinalIgnoreCase) ||
                mesh.GetParent()?.Name.ToString().Contains("Floor", StringComparison.OrdinalIgnoreCase) == true);
    }

    private static IEnumerable<Node> Enumerate(Node root)
    {
        yield return root;
        foreach (var child in root.GetChildren())
            foreach (var nested in Enumerate(child))
                yield return nested;
    }
}
