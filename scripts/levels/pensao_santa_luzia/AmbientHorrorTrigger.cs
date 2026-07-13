namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>Small, floor-localized sensor. It never blocks or moves the player.</summary>
public partial class AmbientHorrorTrigger : Area3D
{
    [Export] public string ZoneId { get; set; } = "Corridor";

    private AmbientHorrorDirector? _director;

    public override void _Ready()
    {
        CollisionLayer = 0;
        CollisionMask = 16;
        Monitoring = true;
        Monitorable = false;
        _director = GetNodeOrNull<AmbientHorrorDirector>("../../AmbientHorrorDirector");
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body is CharacterBody3D player)
            _director?.EnterZone(ZoneId, player);
    }

    private void OnBodyExited(Node3D body)
    {
        if (body is CharacterBody3D player)
            _director?.ExitZone(ZoneId, player);
    }
}
