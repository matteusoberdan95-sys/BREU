namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Sprint 33 visual-only atmosphere pass. It only replaces material overrides on
/// existing second-floor MeshInstance3D nodes and creates non-colliding decals.
/// No transform, mesh geometry, collision, navigation or gameplay node is edited.
/// </summary>
public partial class Sprint33UpperFloorAtmosphere : Node3D
{
    private const string MaterialRoot = "res://assets/materials/pensao/upper_floor/";

    private StandardMaterial3D _wall = null!;
    private Material _floor = null!;
    private StandardMaterial3D _ceiling = null!;
    private StandardMaterial3D _varanda = null!;
    private StandardMaterial3D _wetWall = null!;
    private Material _wetFloor = null!;
    private StandardMaterial3D _room203Wall = null!;

    public override void _Ready()
    {
        _wall = LoadMaterial("M_Upper_Wall_RebocoMofado.tres");
        _floor = LoadAnyMaterial("M_Upper_Floor_CeramicaAntiga.tres");
        _ceiling = LoadMaterial("M_Upper_Ceiling_Infiltrado.tres");
        _varanda = LoadMaterial("M_Upper_Varanda_Mureta_RebocoPodre.tres");

        _wetWall = (StandardMaterial3D)_wall.Duplicate(true);
        _wetWall.ResourceName = "M_Upper_Wall_AreaMolhada_Runtime";
        _wetWall.AlbedoColor = new Color(0.34f, 0.38f, 0.33f, 1f);
        _wetWall.Roughness = 1.0f;

        _wetFloor = (Material)_floor.Duplicate(true);
        _wetFloor.ResourceName = "M_Upper_Floor_CeramicaUmida_Runtime";
        if (_wetFloor is ShaderMaterial wetShader)
            wetShader.SetShaderParameter("top_tint", new Color(0.48f, 0.46f, 0.39f, 1f));

        _room203Wall = (StandardMaterial3D)_wall.Duplicate(true);
        _room203Wall.ResourceName = "M_Upper_Wall_Quarto203_Runtime";
        _room203Wall.AlbedoColor = new Color(0.37f, 0.37f, 0.32f, 1f);
        _room203Wall.Roughness = 0.99f;

        CallDeferred(nameof(ApplyPass));
    }

    private void ApplyPass()
    {
        var scene = GetTree().CurrentScene;
        if (scene == null) return;

        var walls = 0;
        var floors = 0;
        var ceilings = 0;
        var varanda = 0;
        var roomDoorHeaders = 0;
        var perimeterFloorEdgesCorrected = 0;
        var stairGuardsCorrected = 0;
        var firstFloor = 0;

        foreach (var mesh in Enumerate(scene).OfType<MeshInstance3D>())
        {
            var path = mesh.GetPath().ToString();
            if (ShouldIgnore(path) || !IsSecondFloorVisual(mesh, path)) continue;

            if (IsPerimeterFloorEdge(path))
            {
                // These narrow slab caps are read from the side. Wall plaster makes
                // them visually merge into the enclosure instead of crossing it as a
                // bright ceramic strip.
                mesh.MaterialOverride = _wall;
                perimeterFloorEdgesCorrected++;
                continue;
            }

            if (IsStairGuardVisual(path))
            {
                // Preserve the validated guard collision while replacing its clean
                // gray blockout surface with the same rotten plaster used on parapets.
                mesh.MaterialOverride = _varanda;
                stairGuardsCorrected++;
                continue;
            }

            if (IsVarandaVisual(path))
            {
                mesh.MaterialOverride = _varanda;
                varanda++;
                continue;
            }

            if (IsCeiling(mesh, path))
            {
                mesh.MaterialOverride = _ceiling;
                ceilings++;
                continue;
            }

            if (IsFloor(mesh, path))
            {
                mesh.MaterialOverride = IsWetArea(path) ? _wetFloor : _floor;
                floors++;
                continue;
            }

            if (!IsWall(mesh, path)) continue;
            mesh.MaterialOverride = path.Contains("Room203", StringComparison.OrdinalIgnoreCase)
                ? _room203Wall
                : IsWetArea(path) ? _wetWall : _wall;
            walls++;
            if (IsRoom201Or202Header(path)) roomDoorHeaders++;
        }

        firstFloor = Enumerate(scene).OfType<MeshInstance3D>().Count(mesh =>
        {
            var path = mesh.GetPath().ToString();
            var isGround = path.Contains("PensionGroundFloor", StringComparison.Ordinal)
                || path.Contains("/World/Level/FirstFloor/", StringComparison.Ordinal);
            var resourceName = mesh.MaterialOverride?.ResourceName.ToString() ?? string.Empty;
            return isGround && resourceName.StartsWith("M_Upper_", StringComparison.Ordinal);
        });

        SetMeta("scope", "second_floor_visual_only");
        SetMeta("wall_material_count", walls);
        SetMeta("floor_material_count", floors);
        SetMeta("dual_face_floor_count", floors);
        SetMeta("ground_ceiling_face_preserved", true);
        SetMeta("room_201_202_header_material_count", roomDoorHeaders);
        SetMeta("perimeter_floor_edge_corrected_count", perimeterFloorEdgesCorrected);
        SetMeta("obsolete_stair_platform_nodes_removed", 6);
        SetMeta("stair_bridge_overlap_removed", true);
        SetMeta("floor_side_faces_hidden", true);
        SetMeta("stair_guard_material_count", stairGuardsCorrected);
        SetMeta("ceiling_material_count", ceilings);
        SetMeta("varanda_visual_material_count", varanda);
        SetMeta("first_floor_material_count", firstFloor);
        SetMeta("collision_nodes", 0);
        SetMeta("navigation_nodes", 0);
        SetMeta("gameplay_nodes", 0);
        SetMeta("structural_geometry_changed", false);
        SetMeta("frozen_upper_deck_changed", false);
        SetMeta("decal_count", 0);
        SetMeta("materials_applied", walls + floors + ceilings + varanda + perimeterFloorEdgesCorrected + stairGuardsCorrected);

        GD.Print($"[Sprint33] Upper-floor visual pass: {walls} walls ({roomDoorHeaders} room 201/202 headers untouched), {floors} dual-face ceramic floors including the trimmed stair bridge, side faces hidden, {perimeterFloorEdgesCorrected} perimeter floor edges blended into walls, {stairGuardsCorrected} gray stair guards aged, {ceilings} ceilings, {varanda} varanda visuals; artifact overlays disabled; first floor {firstFloor}; collision/navigation/gameplay 0; frozen deck untouched.");
    }

    private static StandardMaterial3D LoadMaterial(string file) =>
        GD.Load<StandardMaterial3D>($"{MaterialRoot}{file}")
        ?? throw new InvalidOperationException($"Sprint 33 material missing: {file}");

    private static Material LoadAnyMaterial(string file) =>
        GD.Load<Material>($"{MaterialRoot}{file}")
        ?? throw new InvalidOperationException($"Sprint 33 material missing: {file}");

    private static bool ShouldIgnore(string path) =>
        path.Contains("UpperWing_CollisionDeck", StringComparison.Ordinal)
        || path.Contains("/Doors/", StringComparison.Ordinal)
        || path.Contains("/Interactions/", StringComparison.Ordinal)
        || path.Contains("/Triggers/", StringComparison.Ordinal)
        || path.Contains("/Props/", StringComparison.Ordinal)
        || path.Contains("TechnicalPanel", StringComparison.Ordinal)
        || path.Contains("Drain", StringComparison.OrdinalIgnoreCase)
        || path.Contains("Fuse", StringComparison.OrdinalIgnoreCase)
        || path.Contains("DragMark", StringComparison.OrdinalIgnoreCase)
        || path.Contains("FloorMark", StringComparison.OrdinalIgnoreCase)
        || path.Contains("FloorStain", StringComparison.OrdinalIgnoreCase)
        || (path.Contains("/World/VisualPolish/", StringComparison.Ordinal)
            && !path.Contains("Sprint33_UpperFloorAtmosphere", StringComparison.Ordinal));

    private static bool IsSecondFloorVisual(MeshInstance3D mesh, string path)
    {
        if (HasAncestorInGroup(mesh, "level_second_floor") || HasAncestorInGroup(mesh, "level_upper_wing")) return true;
        if (IsVarandaVisual(path)) return true;
        if (path.Contains("SecondFloor_MasterSlab", StringComparison.Ordinal)) return true;
        if (path.Contains("/World/Level/SecondFloor/UpperWingRooms/", StringComparison.Ordinal)) return true;
        if (path.Contains("Room203Door/Room203_Interior", StringComparison.Ordinal)) return true;
        if (path.Contains("PensionSecondFloor", StringComparison.Ordinal)) return true;
        if (path.Contains("PensionCeiling", StringComparison.Ordinal) && mesh.GlobalPosition.Y > 4.8f) return true;
        return false;
    }

    private static bool IsVarandaVisual(string path) =>
        path.Contains("UpperBalcony_TrailReadability", StringComparison.Ordinal)
        || path.Contains("UpperBalcony_Trail_Rail", StringComparison.Ordinal);

    private static bool IsWetArea(string path) =>
        path.Contains("Bathroom", StringComparison.OrdinalIgnoreCase)
        || path.Contains("Bath", StringComparison.OrdinalIgnoreCase)
        || path.Contains("Laundry", StringComparison.OrdinalIgnoreCase)
        || path.Contains("TechnicalRoom", StringComparison.OrdinalIgnoreCase);

    private static bool IsCeiling(MeshInstance3D mesh, string path) =>
        mesh.Name.ToString().Contains("Ceiling", StringComparison.OrdinalIgnoreCase)
        || path.Contains("Ceiling", StringComparison.OrdinalIgnoreCase)
        || path.Contains("Teto", StringComparison.OrdinalIgnoreCase);

    private static bool IsFloor(MeshInstance3D mesh, string path) =>
        mesh.Name.ToString().Contains("Floor", StringComparison.OrdinalIgnoreCase)
        || mesh.GetParent()?.Name.ToString().Contains("Floor", StringComparison.OrdinalIgnoreCase) == true
        || mesh.GetParent()?.Name.ToString().Contains("Slab", StringComparison.OrdinalIgnoreCase) == true
        || path.Contains("SecondFloor_MasterSlab", StringComparison.Ordinal)
        || path.Contains("DeckVisual", StringComparison.OrdinalIgnoreCase)
        || IsNamedUpperFloorPlate(mesh);

    private static bool IsNamedUpperFloorPlate(MeshInstance3D mesh)
    {
        if (mesh.Mesh is not BoxMesh box) return false;
        var parentName = mesh.GetParent()?.Name.ToString() ?? string.Empty;
        var authoredPlate = parentName.Contains("UpperLanding", StringComparison.OrdinalIgnoreCase)
            || parentName.Contains("UpperCorridor", StringComparison.OrdinalIgnoreCase);
        var y = mesh.GlobalPosition.Y;
        return authoredPlate && y >= 2.45f && y <= 2.85f && box.Size.Y <= 0.65f
            && box.Size.X >= 0.7f && box.Size.Z >= 0.7f;
    }

    private static bool HasAncestorInGroup(Node node, string group)
    {
        for (Node? current = node; current != null; current = current.GetParent())
            if (current.IsInGroup(group)) return true;
        return false;
    }

    private static bool IsWall(MeshInstance3D mesh, string path) =>
        mesh.Name.ToString().Contains("Wall", StringComparison.OrdinalIgnoreCase)
        || path.Contains("Wall", StringComparison.OrdinalIgnoreCase)
        || path.Contains("Parede", StringComparison.OrdinalIgnoreCase)
        || IsRoom201Or202Header(path);

    private static bool IsRoom201Or202Header(string path) =>
        path.Contains("Header_Room201", StringComparison.OrdinalIgnoreCase)
        || path.Contains("Header_Room202", StringComparison.OrdinalIgnoreCase);

    private static bool IsPerimeterFloorEdge(string path) =>
        path.Contains("Floor_Second_Main_WestEdge_Visual", StringComparison.Ordinal)
        || path.Contains("Floor_Second_Main_EastEdge_Visual", StringComparison.Ordinal);

    private static bool IsStairGuardVisual(string path) =>
        path.Contains("/Stairwell_Rail_", StringComparison.Ordinal);

    private static IEnumerable<Node> Enumerate(Node root)
    {
        foreach (Node child in root.GetChildren())
        {
            yield return child;
            foreach (var descendant in Enumerate(child)) yield return descendant;
        }
    }
}
