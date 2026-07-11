namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Sprint 14B — second-floor narrative readability (door frames + props only).
/// </summary>
public partial class PensaoVerticalBlockout01Builder
{
    private void BuildSecondFloorNarrativeReadability()
    {
        var narrative = new Node3D { Name = "SecondFloorNarrative" };
        _secondFloor.AddChild(narrative);

        var doorFrames = new Node3D
        {
            Name = "DoorFrames",
            Position = new Vector3(0f, SecondFloorTopY - WallEmbedBelowFloor, 0f)
        };
        narrative.AddChild(doorFrames);

        AddDoorFrameInXWallLocal(doorFrames, "Door_Room201_Frame", -CorridorWallX, Room201CenterZ, DoorWidth, DoorHeight);
        AddDoorFrameInXWallLocal(doorFrames, "Door_Room202_Frame", CorridorWallX, Room202CenterZ, DoorWidth, DoorHeight);

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
}
