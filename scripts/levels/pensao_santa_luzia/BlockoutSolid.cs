namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>Cria caixas visuais com colisao manual para blockout navegavel.</summary>
public static class BlockoutSolid
{
    public static StaticBody3D CreateBox(
        Node3D parent,
        string name,
        Vector3 center,
        Vector3 size,
        Material? material,
        bool collision = true,
        Vector3? rotation = null)
    {
        var body = new StaticBody3D { Name = name };
        parent.AddChild(body);
        body.Position = center;
        if (rotation.HasValue)
        {
            body.Rotation = rotation.Value;
        }

        if (material != null)
        {
            var meshInstance = new MeshInstance3D
            {
                Mesh = new BoxMesh { Size = size },
                MaterialOverride = material,
            };
            body.AddChild(meshInstance);
        }

        if (collision)
        {
            var shape = new CollisionShape3D
            {
                Name = "Shape",
                Shape = new BoxShape3D { Size = size },
            };
            body.AddChild(shape);
        }

        return body;
    }

    /// <summary>Colisao invisivel sem mesh visual (gameplay only).</summary>
    public static StaticBody3D CreateCollisionOnly(
        Node3D parent,
        string name,
        Vector3 center,
        Vector3 size,
        Vector3? rotation = null)
    {
        return CreateBox(parent, name, center, size, null, collision: true, rotation: rotation);
    }

    public static MeshInstance3D CreateVisualOnly(
        Node3D parent,
        string name,
        Vector3 center,
        Vector3 size,
        Material? material,
        Vector3? rotation = null)
    {
        var visual = new MeshInstance3D
        {
            Name = name,
            Mesh = new BoxMesh { Size = size },
            MaterialOverride = material,
        };
        parent.AddChild(visual);
        visual.Position = center;
        if (rotation.HasValue)
        {
            visual.Rotation = rotation.Value;
        }

        return visual;
    }

    public static Label3D CreateSign(
        Node3D parent,
        string name,
        Vector3 center,
        string text,
        int fontSize = 32)
    {
        var label = new Label3D
        {
            Name = name,
            Text = text,
            FontSize = fontSize,
            OutlineSize = 4,
            Modulate = new Color(0.78f, 0.68f, 0.45f),
            Position = center,
            Billboard = BaseMaterial3D.BillboardModeEnum.Disabled,
        };
        parent.AddChild(label);
        return label;
    }
}
