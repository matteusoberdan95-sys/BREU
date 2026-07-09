namespace BREU.Scripts.Levels;

/// <summary>
/// Estados simples da sequencia da DemoRoom. Nao bloqueia progressao ainda.
/// </summary>
public partial class DemoRoomSequenceController : Node
{
    public bool HasReadNote { get; private set; }
    public bool HasPickedHammer { get; private set; }
    public bool HasOpenedDoor { get; private set; }
    public bool HasTriggeredCorridorScare { get; private set; }

    public void OnNoteRead()
    {
        if (HasReadNote)
        {
            return;
        }

        HasReadNote = true;
        GD.Print("DemoSequence: bilhete lido.");
    }

    public void OnHammerPicked()
    {
        if (HasPickedHammer)
        {
            return;
        }

        HasPickedHammer = true;
        GD.Print("DemoSequence: martelo coletado.");
    }

    public void OnDoorOpened()
    {
        if (HasOpenedDoor)
        {
            return;
        }

        HasOpenedDoor = true;
        GD.Print("DemoSequence: porta aberta.");
    }

    public void OnCorridorScareTriggered()
    {
        if (HasTriggeredCorridorScare)
        {
            return;
        }

        HasTriggeredCorridorScare = true;
        GD.Print("DemoSequence: primeiro susto do corredor disparado.");
    }
}
