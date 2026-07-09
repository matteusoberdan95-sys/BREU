namespace BREU.Scripts.Audio;

/// <summary>
/// Caminhos canonicos de audio. Arquivos opcionais — ver AUDIO_ASSETS_NEEDED.md.
/// </summary>
public static class AudioPaths
{
    public const string DoorsOpen = "res://assets/audio/sfx/doors/door_open_old_wood.ogg";
    public const string DoorsClose = "res://assets/audio/sfx/doors/door_close_old_wood.ogg";
    public const string DoorsLocked = "res://assets/audio/sfx/doors/door_locked_rattle.ogg";

    public const string RadioStatic = "res://assets/audio/sfx/radio/radio_static_loop.ogg";
    public const string RadioWhisper = "res://assets/audio/sfx/radio/radio_whisper_01.ogg";

    public const string HorrorScareStinger = "res://assets/audio/sfx/horror/scare_stinger_01.ogg";
    public const string HorrorCorridorHit = "res://assets/audio/sfx/horror/corridor_hit_01.ogg";
    public const string HorrorDistantKnock = "res://assets/audio/sfx/horror/distant_knock_01.ogg";

    public const string PlayerFlashlightClick = "res://assets/audio/sfx/player/flashlight_click.ogg";
    public const string PlayerPickupItem = "res://assets/audio/sfx/player/pickup_item.ogg";
    public const string PlayerHeartbeatLow = "res://assets/audio/sfx/player/heartbeat_low.ogg";
    public const string PlayerJumpStart = "res://assets/audio/sfx/player/jump_start.ogg";
    public const string PlayerLandSoft = "res://assets/audio/sfx/player/land_soft.ogg";
    public const string PlayerLandHeavy = "res://assets/audio/sfx/player/land_heavy.ogg";

    public static string PlayerFootstepConcrete(int index) =>
        $"res://assets/audio/sfx/player/footstep_concrete_{index:D2}.ogg";

    public static string PlayerFootstepWood(int index) =>
        $"res://assets/audio/sfx/player/footstep_wood_{index:D2}.ogg";

    public const string EnemyBreath = "res://assets/audio/sfx/enemies/enemy_breath_01.ogg";
    public const string EnemyStep = "res://assets/audio/sfx/enemies/enemy_step_01.ogg";
    public const string EnemyGrowl = "res://assets/audio/sfx/enemies/enemy_growl_01.ogg";

    public const string AmbienceRoomTone = "res://assets/audio/ambience/room_tone_01.ogg";
    public const string AmbienceCorridorTone = "res://assets/audio/ambience/corridor_tone_01.ogg";
    public const string AmbienceWind = "res://assets/audio/ambience/wind_old_house_01.ogg";
}
