namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Sprint 14F — minimal open-door frames (posts + lintel only, no panel/infill/leaves).
/// </summary>
public partial class PensaoTerreoBlockout01Builder
{
    private StandardMaterial3D _matDoorFrame = null!;

    private void EnsureDoorFrameMaterial()
    {
        _matDoorFrame ??= Mat(new Color(0.28f, 0.2f, 0.14f));
    }

    protected void AddMinimalDoorFrameZWall(
        Node3D parent,
        string name,
        Vector3 position,
        float openingWidth,
        float rotationY = 0f)
    {
        EnsureDoorFrameMaterial();

        const float postWidth = 0.12f;
        const float frameDepth = 0.12f;
        const float standoffZ = 0.06f;
        const float lintelHeight = 0.12f;

        var root = new Node3D
        {
            Name = name,
            Position = position,
            Rotation = new Vector3(0f, rotationY, 0f)
        };
        parent.AddChild(root);

        var half = openingWidth * 0.5f;
        var postCenterY = DoorHeight * 0.5f;
        var lintelCenterY = DoorHeight + lintelHeight * 0.5f;

        AddVisualProp(
            root,
            $"{name}_Left",
            new Vector3(-half - postWidth * 0.5f, postCenterY, standoffZ),
            new Vector3(postWidth, DoorHeight, frameDepth),
            _matDoorFrame);

        AddVisualProp(
            root,
            $"{name}_Right",
            new Vector3(half + postWidth * 0.5f, postCenterY, standoffZ),
            new Vector3(postWidth, DoorHeight, frameDepth),
            _matDoorFrame);

        AddVisualProp(
            root,
            $"{name}_Lintel",
            new Vector3(0f, lintelCenterY, standoffZ),
            new Vector3(openingWidth + postWidth * 2f, lintelHeight, frameDepth),
            _matDoorFrame);
    }

    protected void AddMinimalDoorFrameXWall(
        Node3D parent,
        string name,
        Vector3 position,
        float openingWidth,
        float rotationY = 0f)
    {
        AddMinimalDoorFrameZWall(parent, name, position, openingWidth, rotationY);
    }
}
