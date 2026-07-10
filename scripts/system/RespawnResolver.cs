namespace BREU.Scripts.Systems;

/// <summary>
/// Restaura o estado basico do player quando uma cena abre apos respawn.
/// </summary>
public partial class RespawnResolver : Node
{
    public override void _Ready()
    {
        CallDeferred(nameof(ResolveRespawnState));
    }

    public void ResolveRespawnState()
    {
        if (GetTree().GetFirstNodeInGroup("player") is Node3D player)
        {
            if (player is PlayerController controller)
            {
                controller.MovementEnabled = true;
            }

            player.GetNodeOrNull<PlayerHealth>("PlayerHealth")?.ResetHealth();
        }

        if (GetTree().GetFirstNodeInGroup("death_screen") is DeathScreen deathScreen)
        {
            deathScreen.HideDeathScreen();
        }

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
}
