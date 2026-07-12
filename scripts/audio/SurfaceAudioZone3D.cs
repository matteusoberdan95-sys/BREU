namespace BREU.Scripts.Audio;

/// <summary>
/// Sprint 16B — surface type for player footsteps. No physics blocking.
/// </summary>
public enum FootstepSurfaceType
{
    Wood = 0,
    DirtGravel = 1
}

/// <summary>
/// Area3D that tags the player's current footstep surface.
/// </summary>
public partial class SurfaceAudioZone3D : Area3D
{
    [Export] public FootstepSurfaceType SurfaceType { get; set; } = FootstepSurfaceType.Wood;
    [Export] public int ZonePriority { get; set; } = 0;

    public override void _Ready()
    {
        Monitoring = true;
        Monitorable = false;
        CollisionLayer = 0;
        CollisionMask = 16;
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body is not CharacterBody3D)
        {
            return;
        }

        PlayerFootstepAudio.Find(GetTree())?.NotifySurfaceEntered(this);
    }

    private void OnBodyExited(Node3D body)
    {
        if (body is not CharacterBody3D)
        {
            return;
        }

        PlayerFootstepAudio.Find(GetTree())?.NotifySurfaceExited(this);
    }
}
