namespace BREU.Scripts.Debug;

/// <summary>
/// Debug-only locator for reporting exact upper-wing problem spots from screenshots.
/// F7 prints the current position and nearest authored wall to the Output.
/// </summary>
public partial class PlayerCoordinateOverlay : CanvasLayer
{
    private Label? _label;
    private CharacterBody3D? _player;
    private Node3D? _upperWing;

    public override void _Ready()
    {
        if (!OS.IsDebugBuild())
        {
            QueueFree();
            return;
        }

        Layer = 100;
        _label = new Label
        {
            Name = "CoordinateLabel",
            Position = new Vector2(730, 126),
            Size = new Vector2(410, 92),
            HorizontalAlignment = HorizontalAlignment.Right,
            Text = "Localizador carregando..."
        };
        _label.AddThemeColorOverride("font_color", new Color(1f, 0.9f, 0.35f));
        _label.AddThemeColorOverride("font_shadow_color", Colors.Black);
        _label.AddThemeConstantOverride("shadow_offset_x", 2);
        _label.AddThemeConstantOverride("shadow_offset_y", 2);
        _label.AddThemeFontSizeOverride("font_size", 17);
        AddChild(_label);

        CallDeferred(nameof(BindScene));
    }

    public override void _Process(double delta)
    {
        if (_label == null) return;
        if (!GodotObject.IsInstanceValid(_player)) BindScene();
        if (_player == null)
        {
            _label.Text = "LOCAL: player não encontrado";
            return;
        }

        var position = _player.GlobalPosition;
        var wall = FindNearestWall(position, out var wallDistance);
        var room = FindContainingRoom(position);
        var forward = -_player.GlobalTransform.Basis.Z;
        _label.Text =
            $"LOCAL X {position.X:0.00}  Y {position.Y:0.00}  Z {position.Z:0.00}\n" +
            $"ÁREA {room} | PAREDE {wall?.Name.ToString() ?? "nenhuma"} ({wallDistance:0.00}m)\n" +
            $"DIREÇÃO X {forward.X:0.00} Z {forward.Z:0.00} | F7 registrar";
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventKey { Pressed: true, Echo: false, Keycode: Key.F7 } || _player == null) return;
        var position = _player.GlobalPosition;
        var wall = FindNearestWall(position, out var distance);
        var report = $"[LOCALIZADOR] X={position.X:0.000} Y={position.Y:0.000} Z={position.Z:0.000} " +
                     $"area={FindContainingRoom(position)} nearest={wall?.GetPath().ToString() ?? "none"} distance={distance:0.000}m";
        GD.Print(report);
        DisplayServer.ClipboardSet(report);
        HUDController.FindActive(GetTree())?.ShowMessage("Coordenada copiada e registrada no Output.", 2.5f);
    }

    private void BindScene()
    {
        var scene = GetTree().CurrentScene;
        _player = scene?.FindChild("Player", true, false) as CharacterBody3D;
        _upperWing = scene?.GetNodeOrNull<Node3D>("World/Level/SecondFloor/UpperWingRooms");
    }

    private Node3D? FindNearestWall(Vector3 position, out float distance)
    {
        Node3D? nearest = null;
        distance = float.PositiveInfinity;
        if (_upperWing == null) return null;
        foreach (var node in Enumerate(_upperWing))
        {
            if (node is not Node3D wall || !wall.Name.ToString().StartsWith("Wall_")) continue;
            var candidate = wall.GlobalPosition.DistanceTo(position);
            if (candidate >= distance) continue;
            distance = candidate;
            nearest = wall;
        }
        return nearest;
    }

    private string FindContainingRoom(Vector3 position)
    {
        if (_upperWing == null) return "fora da ala";
        var nearest = _upperWing.GetChildren().OfType<Node3D>()
            .Where(node => node.Name.ToString() is "Room204_Bedroom" or "Room205_Locked" or "SharedBathroom" or "LaundryStorage" or "TechnicalRoom" or "OwnersOffice" or "Corridor_Main")
            .OrderBy(node => node.GlobalPosition.DistanceSquaredTo(position))
            .FirstOrDefault();
        return nearest?.Name.ToString() ?? "fora da ala";
    }

    private static IEnumerable<Node> Enumerate(Node root)
    {
        yield return root;
        foreach (var child in root.GetChildren())
            foreach (var descendant in Enumerate(child))
                yield return descendant;
    }
}
