namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Visual-only Sprint 27 art pass. Every generated node is a MeshInstance3D, Light3D,
/// Marker3D or plain container; no physics, collision, navigation or gameplay state.
/// </summary>
public partial class Sprint27FakeWindowsLighting : Node3D
{
    private StandardMaterial3D _wood = null!;
    private StandardMaterial3D _room201Wood = null!;
    private StandardMaterial3D _glass = null!;
    private StandardMaterial3D _cloth = null!;
    private StandardMaterial3D _coldLeak = null!;
    private StandardMaterial3D _damp = null!;
    private StandardMaterial3D _shadow = null!;

    public override void _Ready()
    {
        BuildMaterials();
        var windows = GetNode<Node3D>("FakeWindows");
        var leaks = GetNode<Node3D>("LightLeaks");
        var lights = GetNode<Node3D>("LocalLights");
        var details = GetNode<Node3D>("WallDetails");
        var cloth = GetNode<Node3D>("CurtainsAndCloth");
        var markers = GetNode<Node>("DebugMarkers");

        AddFakeWindow(windows, "FakeWindow_ReceptionWest", new Vector3(-4.98f, 1.55f, -2.5f), -Mathf.Pi * 0.5f, curtain: true);
        AddFakeWindow(windows, "FakeWindow_StairHigh", new Vector3(-4.1f, 1.88f, -32.48f), Mathf.Pi, narrow: true);
        AddFakeWindow(windows, "FakeWindow_UpperCorridorEast", new Vector3(0.69f, 4.2f, -1.45f), Mathf.Pi * 0.5f);
        AddFakeWindow(windows, "FakeWindow_Room203Boarded", new Vector3(-5.08f, 4.2f, -10.8f), -Mathf.Pi * 0.5f, boarded: true);

        // Sprint 27A: fake facade windows mounted on the room-facing side of existing
        // exterior walls. They remain decorative: no hole, collision or navigation data.
        AddFakeWindow(windows, "FacadeGround_Room102WestInterior", new Vector3(-6.78f, 1.55f, -15.5f), Mathf.Pi * 0.5f, boarded: true);
        AddFakeWindow(windows, "FacadeGround_KitchenEastInterior", new Vector3(6.78f, 1.55f, -20.5f), -Mathf.Pi * 0.5f, curtain: true);
        AddFakeWindow(windows, "FacadeGround_BackCorridor", new Vector3(0f, 1.78f, -32.82f), 0f, narrow: true);

        AddFakeWindow(windows, "FacadeUpperFront_West", new Vector3(-4.7f, 4.2f, 11.78f), Mathf.Pi, curtain: true, visualBackingOnly: true);
        AddFakeWindow(windows, "FacadeUpperFront_East", new Vector3(4.7f, 4.2f, 11.78f), Mathf.Pi, visualBackingOnly: true);
        AddDeepWestWindow(windows, "FacadeUpper_Room201WestInterior", -6.66f, -14f);
        AddFakeWindow(windows, "FacadeUpper_Room202EastInterior", new Vector3(6.78f, 4.2f, -17f), -Mathf.Pi * 0.5f, boarded: true);
        // Both windows belong together on the broad west-wall panel north of the
        // stair-side opening. Keep the smaller south panel completely empty.
        AddDeepWestWindow(windows, "FacadeUpper_WestWallPairSouth", -6.86f, -26.2f);
        AddDeepWestWindow(windows, "FacadeUpper_WestWallPairNorth", -6.86f, -29.0f, curtain: true);

        AddFakeWindow(windows, "FacadeUpper_Room204East", new Vector3(14.38f, 4.2f, -0.4f), -Mathf.Pi * 0.5f, curtain: true);
        AddFakeWindow(windows, "FacadeUpper_TechnicalEast", new Vector3(14.38f, 4.35f, 3.7f), -Mathf.Pi * 0.5f, narrow: true, boarded: true);
        AddFakeWindow(windows, "FacadeUpper_LaundryWest", new Vector3(-7.38f, 4.2f, -0.5f), Mathf.Pi * 0.5f, boarded: true);
        AddFakeWindow(windows, "FacadeUpper_BathroomWest", new Vector3(-7.38f, 4.42f, 3.7f), Mathf.Pi * 0.5f, narrow: true);
        AddFakeWindow(windows, "FacadeUpper_OwnersOfficeNorth", new Vector3(-3.4f, 4.2f, 8.33f), Mathf.Pi, curtain: true);

        AddFloorLeak(leaks, "LightLeak_Reception", new Vector3(-3.95f, 0.035f, -2.5f), new Vector3(1.55f, 0.006f, 2.2f), -0.12f);
        AddFloorLeak(leaks, "LightLeak_UpperCorridor", new Vector3(0.15f, 2.825f, -1.45f), new Vector3(0.9f, 0.006f, 1.65f), 0.08f);
        AddFloorLeak(leaks, "LightLeak_Room203", new Vector3(-4.55f, 2.825f, -10.8f), new Vector3(0.72f, 0.006f, 1.25f), 0.05f);

        AddLocalSpot(lights, "NightLight_Reception", new Vector3(-4.75f, 1.75f, -2.5f), -Mathf.Pi * 0.5f, 0.16f, 3.8f);
        AddLocalSpot(lights, "NightLight_Stair", new Vector3(-4.1f, 2.0f, -32.25f), Mathf.Pi, 0.14f, 3.4f);
        AddLocalSpot(lights, "NightLight_UpperCorridor", new Vector3(0.5f, 4.35f, -1.45f), Mathf.Pi * 0.5f, 0.12f, 3.2f);
        AddLocalSpot(lights, "NightLight_Room203", new Vector3(-4.86f, 4.3f, -10.8f), -Mathf.Pi * 0.5f, 0.08f, 2.4f);

        AddWallDetail(details, "DampPatch_Reception", new Vector3(4.98f, 1.0f, -4.6f), Mathf.Pi * 0.5f, new Vector3(0.82f, 0.58f, 0.012f));
        AddWallDetail(details, "DampPatch_BackHall", new Vector3(1.18f, 1.25f, -24.1f), Mathf.Pi * 0.5f, new Vector3(0.55f, 0.9f, 0.012f));
        AddWallDetail(details, "DampPatch_UpperBathroom", new Vector3(-7.08f, 3.65f, 3.25f), -Mathf.Pi * 0.5f, new Vector3(0.62f, 0.72f, 0.012f));

        AddBox(cloth, "HangingCloth_BackHall", new Vector3(1.17f, 1.68f, -23.3f), new Vector3(0.025f, 1.15f, 0.56f), _cloth, new Vector3(0.04f, 0f, -0.06f));
        AddBox(cloth, "TornCloth_UpperLaundry", new Vector3(-7.07f, 4.25f, -0.65f), new Vector3(0.025f, 1.05f, 0.48f), _cloth, new Vector3(-0.05f, 0f, 0.08f));

        AddFloorShadow(details, "WindowShadow_Reception", new Vector3(-3.8f, 0.041f, -2.5f), new Vector3(1.2f, 0.004f, 1.7f), -0.12f);
        AddFloorShadow(details, "WindowShadow_UpperCorridor", new Vector3(0.05f, 2.831f, -1.45f), new Vector3(0.72f, 0.004f, 1.25f), 0.08f);

        foreach (var child in windows.GetChildren().OfType<Node3D>())
        {
            var marker = new Marker3D { Name = $"Marker_{child.Name}", Position = child.Position };
            markers.AddChild(marker);
        }

        GD.Print("[Sprint27Visual] fine adjustment: 4 interior + 14 exterior fake windows, 3 light leaks, 4 local lights; 0 collision nodes.");
    }

    private void BuildMaterials()
    {
        _wood = Material(new Color(0.17f, 0.105f, 0.065f));
        _room201Wood = Material(new Color(0.34f, 0.19f, 0.09f), emission: new Color(0.012f, 0.006f, 0.002f));
        _glass = Material(new Color(0.018f, 0.028f, 0.055f, 0.94f), transparent: true, emission: new Color(0.018f, 0.035f, 0.075f));
        _cloth = Material(new Color(0.24f, 0.225f, 0.19f, 0.92f), transparent: true);
        _coldLeak = Material(new Color(0.14f, 0.22f, 0.36f, 0.16f), transparent: true, emission: new Color(0.035f, 0.065f, 0.12f));
        _damp = Material(new Color(0.055f, 0.065f, 0.058f, 0.72f), transparent: true);
        _shadow = Material(new Color(0.008f, 0.012f, 0.02f, 0.42f), transparent: true);
    }

    private void AddDeepWestWindow(Node3D parent, string name, float positionX, float positionZ, bool curtain = false)
    {
        // The west-side rooms and stair sector have overlapping authored wall/shell
        // bands. This casing reaches the wall face and projects toward the playable
        // area so the fake window cannot be depth-hidden by the second visual layer.
        const float width = 1.45f;
        const float height = 1.18f;
        const float frame = 0.11f;
        const float casingDepth = 0.28f;
        var root = new Node3D
        {
            Name = name,
            Position = new Vector3(positionX, 4.2f, positionZ),
            Rotation = new Vector3(0f, Mathf.Pi * 0.5f, 0f)
        };
        parent.AddChild(root);
        root.SetMeta("visual_backing_only", false);

        var frameVisual = new Node3D { Name = "Frame_Visual" };
        root.AddChild(frameVisual);
        AddBox(frameVisual, "Frame_Left", new Vector3(-width * 0.5f, 0f, 0f), new Vector3(frame, height + frame, casingDepth), _room201Wood);
        AddBox(frameVisual, "Frame_Right", new Vector3(width * 0.5f, 0f, 0f), new Vector3(frame, height + frame, casingDepth), _room201Wood);
        AddBox(frameVisual, "Frame_Top", new Vector3(0f, height * 0.5f, 0f), new Vector3(width + frame, frame, casingDepth), _room201Wood);
        AddBox(frameVisual, "Frame_Bottom", new Vector3(0f, -height * 0.5f, 0f), new Vector3(width + frame, frame, casingDepth), _room201Wood);

        var frontZ = casingDepth * 0.5f + 0.006f;
        AddBox(root, "DarkGlass_Visual", new Vector3(0f, 0f, frontZ), new Vector3(width - frame, height - frame, 0.035f), _glass);
        var crossbars = new Node3D { Name = "Optional_Crossbars_Visual", Position = new Vector3(0f, 0f, frontZ + 0.02f) };
        root.AddChild(crossbars);
        AddBox(crossbars, "Crossbar_Vertical", Vector3.Zero, new Vector3(0.055f, height - frame, 0.055f), _room201Wood);
        AddBox(crossbars, "Crossbar_Horizontal", Vector3.Zero, new Vector3(width - frame, 0.055f, 0.055f), _room201Wood);
        if (curtain)
        {
            var curtainVisual = new Node3D { Name = "Optional_Curtain_Visual", Position = new Vector3(0f, 0f, frontZ + 0.05f) };
            root.AddChild(curtainVisual);
            AddBox(curtainVisual, "Curtain_Left", new Vector3(-width * 0.28f, 0.02f, 0f), new Vector3(width * 0.38f, height * 0.94f, 0.02f), _cloth, new Vector3(0f, 0f, -0.045f));
        }
        AddBox(root, "Optional_LightLeak", new Vector3(width * 0.33f, 0f, frontZ + 0.025f), new Vector3(0.02f, height * 0.78f, 0.018f), _coldLeak);
    }

    private void AddFakeWindow(Node3D parent, string name, Vector3 position, float yaw, bool curtain = false, bool boarded = false, bool narrow = false, bool visualBackingOnly = false)
    {
        var root = new Node3D { Name = name, Position = position, Rotation = new Vector3(0f, yaw, 0f) };
        parent.AddChild(root);
        root.SetMeta("visual_backing_only", visualBackingOnly);
        var width = narrow ? 0.82f : 1.45f;
        var height = narrow ? 1.45f : 1.18f;
        const float frame = 0.09f;
        var frameVisual = new Node3D { Name = "Frame_Visual" };
        root.AddChild(frameVisual);
        AddBox(frameVisual, "Frame_Left", new Vector3(-width * 0.5f, 0f, 0f), new Vector3(frame, height + frame, 0.055f), _wood);
        AddBox(frameVisual, "Frame_Right", new Vector3(width * 0.5f, 0f, 0f), new Vector3(frame, height + frame, 0.055f), _wood);
        AddBox(frameVisual, "Frame_Top", new Vector3(0f, height * 0.5f, 0f), new Vector3(width + frame, frame, 0.055f), _wood);
        AddBox(frameVisual, "Frame_Bottom", new Vector3(0f, -height * 0.5f, 0f), new Vector3(width + frame, frame, 0.055f), _wood);
        AddBox(root, "DarkGlass_Visual", new Vector3(0f, 0f, -0.018f), new Vector3(width - frame, height - frame, 0.022f), _glass);

        var crossbars = new Node3D { Name = "Optional_Crossbars_Visual" };
        root.AddChild(crossbars);
        AddBox(crossbars, "Crossbar_Vertical", Vector3.Zero, new Vector3(0.045f, height - frame, 0.065f), _wood);
        AddBox(crossbars, "Crossbar_Horizontal", Vector3.Zero, new Vector3(width - frame, 0.045f, 0.065f), _wood);

        if (curtain)
        {
            var curtainVisual = new Node3D { Name = "Optional_Curtain_Visual" };
            root.AddChild(curtainVisual);
            AddBox(curtainVisual, "Curtain_Left", new Vector3(-width * 0.28f, 0.02f, 0.052f), new Vector3(width * 0.38f, height * 0.94f, 0.018f), _cloth, new Vector3(0f, 0f, -0.045f));
        }

        if (boarded)
        {
            var boards = new Node3D { Name = "Boards_Visual" };
            root.AddChild(boards);
            AddBox(boards, "Board_01", new Vector3(0f, 0.24f, 0.08f), new Vector3(width + 0.12f, 0.14f, 0.075f), _wood, new Vector3(0f, 0f, 0.12f));
            AddBox(boards, "Board_02", new Vector3(0f, -0.18f, 0.082f), new Vector3(width + 0.08f, 0.14f, 0.075f), _wood, new Vector3(0f, 0f, -0.08f));
        }

        AddBox(root, "Optional_LightLeak", new Vector3(width * 0.33f, 0f, 0.045f), new Vector3(0.018f, height * 0.78f, 0.012f), _coldLeak);
    }

    private void AddFloorLeak(Node3D parent, string name, Vector3 position, Vector3 size, float yaw) =>
        AddBox(parent, name, position, size, _coldLeak, new Vector3(0f, yaw, 0f));

    private void AddFloorShadow(Node3D parent, string name, Vector3 position, Vector3 size, float yaw)
    {
        var root = new Node3D { Name = name, Position = position, Rotation = new Vector3(0f, yaw, 0f) };
        parent.AddChild(root);
        AddBox(root, "ShadowPane", Vector3.Zero, size, _shadow);
        AddBox(root, "ShadowBar_A", new Vector3(-size.X * 0.18f, 0.004f, 0f), new Vector3(0.045f, 0.006f, size.Z), _shadow);
        AddBox(root, "ShadowBar_B", new Vector3(size.X * 0.18f, 0.004f, 0f), new Vector3(0.045f, 0.006f, size.Z), _shadow);
    }

    private void AddWallDetail(Node3D parent, string name, Vector3 position, float yaw, Vector3 size) =>
        AddBox(parent, name, position, size, _damp, new Vector3(0f, yaw, 0.07f));

    private static void AddLocalSpot(Node3D parent, string name, Vector3 position, float yaw, float energy, float range)
    {
        var light = new SpotLight3D
        {
            Name = name,
            Position = position,
            Rotation = new Vector3(0f, yaw, 0f),
            LightColor = new Color(0.34f, 0.43f, 0.62f),
            LightEnergy = energy,
            LightIndirectEnergy = 0f,
            SpotRange = range,
            SpotAngle = 32f,
            ShadowEnabled = false
        };
        parent.AddChild(light);
    }

    private static MeshInstance3D AddBox(Node3D parent, string name, Vector3 position, Vector3 size, Material material, Vector3? rotation = null)
    {
        var mesh = new MeshInstance3D
        {
            Name = name,
            Position = position,
            Rotation = rotation ?? Vector3.Zero,
            Mesh = new BoxMesh { Size = size, Material = material },
            CastShadow = GeometryInstance3D.ShadowCastingSetting.Off
        };
        parent.AddChild(mesh);
        return mesh;
    }

    private static StandardMaterial3D Material(Color color, bool transparent = false, Color? emission = null)
    {
        var material = new StandardMaterial3D
        {
            AlbedoColor = color,
            Roughness = 0.88f,
            Metallic = 0f,
            Transparency = transparent ? BaseMaterial3D.TransparencyEnum.Alpha : BaseMaterial3D.TransparencyEnum.Disabled
        };
        if (emission.HasValue)
        {
            material.EmissionEnabled = true;
            material.Emission = emission.Value;
        }
        return material;
    }
}
