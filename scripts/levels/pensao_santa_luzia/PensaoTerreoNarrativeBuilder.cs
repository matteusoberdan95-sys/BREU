namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>
/// Sprint 14F — ground-floor narrative readability (minimal door frames + props).
/// </summary>
public partial class PensaoTerreoBlockout01Builder
{
    private void BuildNarrativeReadability()
    {
        var narrative = new Node3D { Name = "NarrativeReadability" };
        _interior.AddChild(narrative);

        BuildGroundFloorDoorFrames(narrative);
        BuildReceptionNarrativeProps(narrative);
        BuildRoom102NarrativeProps(narrative);
        BuildKitchenNarrativeProps(narrative);
        BuildDepositNarrativeProps(narrative);
        BuildStairNarrativeProps(narrative);
        BuildGroundFloorNarrativeInteractions();
    }

    private void BuildGroundFloorDoorFrames(Node3D parent)
    {
        const float varandaEntryZ = 9.8f;

        AddMinimalDoorFrameZWall(
            parent,
            "Door_MainEntrance_Frame",
            new Vector3(0f, -WallEmbedBelowFloor, varandaEntryZ),
            MainEntryWidth);

        AddMinimalDoorFrameXWall(
            parent,
            "Door_Room102_Frame",
            new Vector3(-CorridorWallX, -WallEmbedBelowFloor, -15.5f),
            DoorWidth,
            Mathf.Pi * 0.5f);

        AddMinimalDoorFrameXWall(
            parent,
            "Door_Kitchen_Frame",
            new Vector3(CorridorWallX, -WallEmbedBelowFloor, -20.5f),
            DoorWidth,
            Mathf.Pi * 0.5f);
    }

    private void BuildReceptionNarrativeProps(Node3D parent)
    {
        var reception = new Node3D { Name = "ReceptionProps" };
        parent.AddChild(reception);

        AddVisualProp(
            reception,
            "Reception_Counter_Visual",
            new Vector3(3.4f, 0.55f, -3.5f),
            new Vector3(2.4f, 1.1f, 0.6f),
            _matCounter);

        AddFurniture(
            reception,
            "Reception_Chair",
            new Vector3(-1.6f, 0.25f, -1.4f),
            new Vector3(0.55f, 0.5f, 0.55f),
            _matPropWood);

        AddVisualProp(
            reception,
            "Reception_KeyRack",
            new Vector3(4.6f, 1.45f, -5.4f),
            new Vector3(0.8f, 0.5f, 0.08f),
            _matPropWood);

        AddVisualProp(
            reception,
            "Reception_GuestBook",
            new Vector3(3.4f, 0.92f, -4.5f),
            new Vector3(0.42f, 0.06f, 0.3f),
            _matInteractable);
    }

    private void BuildRoom102NarrativeProps(Node3D parent)
    {
        var room = new Node3D { Name = "Room102Props" };
        parent.AddChild(room);

        AddVisualProp(
            room,
            "Room102_Bed_Visual",
            new Vector3(-5.5f, 0.35f, -15.5f),
            new Vector3(2.0f, 0.7f, 1.8f),
            _matBed);

        AddVisualProp(
            room,
            "Room102_Suitcase",
            new Vector3(-4.2f, 0.22f, -16.6f),
            new Vector3(0.75f, 0.44f, 0.48f),
            _matPropWood);
    }

    private void BuildKitchenNarrativeProps(Node3D parent)
    {
        var kitchen = new Node3D { Name = "KitchenProps" };
        parent.AddChild(kitchen);

        AddVisualProp(
            kitchen,
            "Kitchen_Counter_Visual",
            new Vector3(3.4f, 0.55f, -19.7f),
            new Vector3(1.8f, 1.1f, 0.6f),
            _matCounter);

        AddFurniture(
            kitchen,
            "Kitchen_Stove",
            new Vector3(4.6f, 0.45f, -21.6f),
            new Vector3(0.7f, 0.9f, 0.65f),
            _matPropWood);

        AddFurniture(
            kitchen,
            "Kitchen_Table",
            new Vector3(5.0f, 0.38f, -19.6f),
            new Vector3(0.9f, 0.76f, 0.9f),
            _matPropWood);

        AddFurniture(
            kitchen,
            "Kitchen_Shelf",
            new Vector3(5.3f, 1.15f, -22.1f),
            new Vector3(0.55f, 1.4f, 0.35f),
            _matPropWood);
    }

    private void BuildDepositNarrativeProps(Node3D parent)
    {
        var deposit = new Node3D { Name = "DepositProps" };
        parent.AddChild(deposit);

        AddFurniture(
            deposit,
            "Deposit_Shelf",
            new Vector3(-0.8f, 1.05f, -29.2f),
            new Vector3(1.2f, 1.6f, 0.4f),
            _matPropWood);

        AddVisualProp(
            deposit,
            "Deposit_BoxStack_A",
            new Vector3(0.55f, 0.35f, -29.8f),
            new Vector3(0.55f, 0.7f, 0.55f),
            _matPropWood);

        AddVisualProp(
            deposit,
            "Deposit_BoxStack_B",
            new Vector3(0.55f, 0.78f, -29.8f),
            new Vector3(0.45f, 0.45f, 0.45f),
            _matPropWood);

        AddVisualProp(
            deposit,
            "Deposit_CoverCloth",
            new Vector3(-0.2f, 0.85f, -30.4f),
            new Vector3(0.9f, 0.04f, 0.7f),
            _matBed);
    }

    private void BuildStairNarrativeProps(Node3D parent)
    {
        var stair = new Node3D { Name = "StairProps" };
        parent.AddChild(stair);

        AddVisualProp(
            stair,
            "Stair_Handrail_Visual",
            new Vector3(-5.0f, 1.0f, -28.0f),
            new Vector3(0.08f, 0.08f, 3.6f),
            _matStairRail);
    }

    private void BuildGroundFloorNarrativeInteractions()
    {
        AddInteractableArea(
            _interactions,
            "ReceptionKeyRack",
            new Vector3(4.6f, 1.35f, -5.4f),
            new Vector3(0.55f, 0.45f, 0.25f),
            "Examinar chaves",
            "Faltam várias chaves. Algumas portas parecem ter sido trancadas às pressas.",
            "reception_keys");

        AddInteractableArea(
            _interactions,
            "ReceptionCounter",
            new Vector3(3.4f, 1.05f, -3.5f),
            new Vector3(0.9f, 0.55f, 0.45f),
            "Examinar recepção",
            "Ninguém atende. A lâmpada balança como se alguém tivesse acabado de passar.",
            "reception_counter");

        AddInteractableArea(
            _interactions,
            "Room102Suitcase",
            new Vector3(-4.2f, 0.65f, -16.6f),
            new Vector3(0.55f, 0.45f, 0.45f),
            "Examinar mala",
            "Está vazia, mas há barro seco no fundo.",
            "room_102_suitcase");

        AddInteractableArea(
            _interactions,
            "KitchenStove",
            new Vector3(4.6f, 0.95f, -21.6f),
            new Vector3(0.55f, 0.65f, 0.55f),
            "Examinar fogão",
            "As cinzas estão frias, mas alguém usou isto recentemente.",
            "kitchen_stove");

        AddInteractableArea(
            _interactions,
            "KitchenCabinet",
            new Vector3(5.3f, 1.15f, -22.1f),
            new Vector3(0.45f, 0.9f, 0.35f),
            "Examinar armário",
            "Há marcas de unhas na madeira.",
            "kitchen_cabinet");

        AddInteractableArea(
            _interactions,
            "DepositShelf",
            new Vector3(-0.8f, 1.2f, -29.2f),
            new Vector3(0.75f, 0.9f, 0.35f),
            "Examinar prateleira",
            "Ferramentas enferrujadas, cordas e algo que parece sangue seco.",
            "deposit_shelf");

        AddInteractableArea(
            _interactions,
            "StairInspect",
            new Vector3(-4.8f, 1.2f, -27.0f),
            new Vector3(0.75f, 0.9f, 0.75f),
            "Examinar escada",
            "Os degraus rangem mesmo quando estou parado.",
            "stair_inspect");
    }
}
