namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Sprint 14 / 14A — second-floor narrative readability (doors, props, marks).
/// </summary>
public partial class PensaoVerticalBlockout01Builder
{
    private void BuildSecondFloorNarrativeReadability()
    {
        var narrative = new Node3D { Name = "SecondFloorNarrative" };
        _secondFloor.AddChild(narrative);

        AddSecondFloorDoorFrameInXWall(narrative, "Door_Room201_Frame", -CorridorWallX, Room201CenterZ, DoorWidth, DoorHeight);
        AddOpenDoorLeafXWall(narrative, "Door_Room201_Leaf", -CorridorWallX, Room201CenterZ, DoorWidth, DoorHeight, -0.45f);

        AddSecondFloorDoorFrameInXWall(narrative, "Door_Room202_Frame", CorridorWallX, Room202CenterZ, DoorWidth, DoorHeight);
        AddOpenDoorLeafXWall(narrative, "Door_Room202_Leaf", CorridorWallX, Room202CenterZ, DoorWidth, DoorHeight, 0.45f);

        BuildRoom201NarrativeProps(narrative);
        BuildRoom202NarrativeProps(narrative);
    }

    private void BuildRoom201NarrativeProps(Node3D parent)
    {
        var room = new Node3D { Name = "Room201Props" };
        parent.AddChild(room);

        AddFurniture(
            room,
            "Room201_Table",
            new Vector3(Room201CenterX - 0.8f, SecondFloorTopY + 0.38f, Room201CenterZ + 0.6f),
            new Vector3(0.8f, 0.76f, 0.55f),
            _matFurniture);

        AddVisualProp(
            room,
            "Room201_Note",
            new Vector3(Room201CenterX + 0.8f, SecondFloorTopY + 0.92f, Room201CenterZ - 0.4f),
            new Vector3(0.18f, 0.02f, 0.14f),
            _matInteractable);
    }

    private void BuildRoom202NarrativeProps(Node3D parent)
    {
        var room = new Node3D { Name = "Room202Props" };
        parent.AddChild(room);

        AddVisualProp(
            room,
            "Room202_ChairFallen",
            new Vector3(Room202CenterX + 0.4f, SecondFloorTopY + 0.18f, Room202CenterZ + 0.3f),
            new Vector3(0.45f, 0.36f, 0.45f),
            _matFurniture);

        AddVisualFloorPlate(
            room,
            "Room202_DragMark_A",
            new Vector3(Room202CenterX + 0.2f, 0f, Room202CenterZ + 0.1f),
            new Vector2(1.4f, 0.12f),
            SecondFloorVisualTopY + 0.012f,
            _matDarkMark,
            0.02f);

        AddVisualFloorPlate(
            room,
            "Room202_DragMark_B",
            new Vector3(Room202CenterX + 0.55f, 0f, Room202CenterZ - 0.15f),
            new Vector2(0.9f, 0.1f),
            SecondFloorVisualTopY + 0.012f,
            _matDarkMark,
            0.02f);
    }

    private void AddSecondFloorDoorFrameInZWall(
        Node3D parent,
        string name,
        float centerX,
        float wallZ,
        float doorCenterZ,
        float doorWidth,
        float doorHeight)
    {
        const float frameTh = 0.14f;
        const float frameDepth = 0.2f;
        var half = doorWidth * 0.5f;
        var frameCenterY = SecondFloorTopY + doorHeight * 0.5f - WallEmbedBelowFloor;
        var lintelHeight = WallHeight - doorHeight;
        var lintelCenterY = SecondFloorTopY + doorHeight + lintelHeight * 0.5f - WallEmbedBelowFloor;

        AddVisualProp(parent, $"{name}_Left", new Vector3(centerX - half - frameTh * 0.5f, frameCenterY, wallZ), new Vector3(frameTh, doorHeight, frameDepth), _matDoor);
        AddVisualProp(parent, $"{name}_Right", new Vector3(centerX + half + frameTh * 0.5f, frameCenterY, wallZ), new Vector3(frameTh, doorHeight, frameDepth), _matDoor);
        AddVisualProp(parent, $"{name}_Lintel", new Vector3(centerX, lintelCenterY, wallZ), new Vector3(doorWidth + frameTh * 2f, lintelHeight, frameDepth), _matDoor);
    }

    private void AddSecondFloorDoorFrameInXWall(
        Node3D parent,
        string name,
        float wallX,
        float doorCenterZ,
        float doorWidth,
        float doorHeight)
    {
        const float frameTh = 0.14f;
        const float frameDepth = 0.2f;
        var half = doorWidth * 0.5f;
        var frameCenterY = SecondFloorTopY + doorHeight * 0.5f - WallEmbedBelowFloor;
        var lintelHeight = WallHeight - doorHeight;
        var lintelCenterY = SecondFloorTopY + doorHeight + lintelHeight * 0.5f - WallEmbedBelowFloor;

        AddVisualProp(parent, $"{name}_South", new Vector3(wallX, frameCenterY, doorCenterZ + half + frameTh * 0.5f), new Vector3(frameDepth, doorHeight, frameTh), _matDoor);
        AddVisualProp(parent, $"{name}_North", new Vector3(wallX, frameCenterY, doorCenterZ - half - frameTh * 0.5f), new Vector3(frameDepth, doorHeight, frameTh), _matDoor);
        AddVisualProp(parent, $"{name}_Lintel", new Vector3(wallX, lintelCenterY, doorCenterZ), new Vector3(frameDepth, lintelHeight, doorWidth + frameTh * 2f), _matDoor);
    }
}
