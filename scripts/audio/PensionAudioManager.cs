namespace BREU.Scripts.Audio;

/// <summary>
/// Sprint 16 — pension ambience loops, zone crossfade, narrative one-shots, flashlight clicks.
/// Missing assets log a warning and never crash. Footsteps/breath catalogued for Sprint 17.
/// </summary>
public partial class PensionAudioManager : Node
{
    public const string IdExterior = "exterior";
    public const string IdReception = "reception";
    public const string IdGroundCorridor = "ground_corridor";
    public const string IdDeposit = "deposit";
    public const string IdSecondFloor = "second_floor";
    public const string IdLampBuzz = "lamp_buzz";
    public const string IdWaterDrops = "water_drops";

    private const string AssetRoot = "res://assets/audio/pensao/";
    private const float DefaultCrossfadeSeconds = 1.6f;

    private readonly Dictionary<string, AudioStreamPlayer> _loops = new(StringComparer.Ordinal);
    private readonly Dictionary<string, float> _targetVolumesDb = new(StringComparer.Ordinal);
    private readonly Dictionary<string, string> _fileById = new(StringComparer.Ordinal);
    private readonly HashSet<AmbienceZone3D> _activeZones = new();
    private readonly HashSet<string> _warnedMissing = new(StringComparer.Ordinal);
    private readonly RandomNumberGenerator _rng = new();

    private AudioStreamPlayer? _oneShotPlayer;
    private AudioStreamPlayer? _flashlightPlayer;
    private string _currentAmbienceId = string.Empty;
    private string _currentSecondaryId = string.Empty;
    private string _currentTertiaryId = string.Empty;
    private bool _crossfading;
    private float _occasionalDropTimer;
    private bool _inWetZone;
    private int _flashClickVariant;

    public static PensionAudioManager? Find(SceneTree tree) =>
        tree.GetFirstNodeInGroup("pension_audio_manager") as PensionAudioManager;

    public override void _Ready()
    {
        AddToGroup("pension_audio_manager");
        _rng.Randomize();
        RegisterCatalog();
        BuildPlayers();
        ValidateSprint17Catalog();
        CallDeferred(nameof(DeferredSetupZones));
    }

    public override void _Process(double delta)
    {
        if (!_inWetZone)
        {
            return;
        }

        _occasionalDropTimer -= (float)delta;
        if (_occasionalDropTimer > 0f)
        {
            return;
        }

        _occasionalDropTimer = _rng.RandfRange(9f, 18f);
        var dropId = _rng.RandiRange(1, 3) switch
        {
            1 => "water_drop_01",
            2 => "water_drop_02",
            _ => "water_drop_03"
        };
        PlayOneShot(dropId, -20f);
    }

    public void PlayAmbience(string ambienceId) => CrossfadeToAmbience(ambienceId, 0.35f);

    public void StopAmbience(string ambienceId)
    {
        if (!_loops.TryGetValue(ambienceId, out var player))
        {
            return;
        }

        player.Stop();
        player.VolumeDb = -80f;
        if (_currentAmbienceId == ambienceId)
        {
            _currentAmbienceId = string.Empty;
        }
    }

    public void CrossfadeToAmbience(string ambienceId, float duration)
    {
        if (string.IsNullOrWhiteSpace(ambienceId) || !_loops.ContainsKey(ambienceId))
        {
            return;
        }

        if (_currentAmbienceId == ambienceId && !_crossfading)
        {
            return;
        }

        _ = CrossfadeAsync(ambienceId, Mathf.Clamp(duration, 0.35f, 3.0f));
    }

    public void PlayOneShot(string soundId, float? volumeDb = null)
    {
        if (_oneShotPlayer == null || string.IsNullOrWhiteSpace(soundId))
        {
            return;
        }

        if (!_fileById.TryGetValue(soundId, out var path))
        {
            WarnMissing($"Unknown one-shot id: {soundId}");
            return;
        }

        var stream = TryLoadStream(path, loop: false);
        if (stream == null)
        {
            return;
        }

        _oneShotPlayer.Stream = stream;
        _oneShotPlayer.VolumeDb = volumeDb ?? -13f;
        _oneShotPlayer.Play();
        GD.Print($"[Audio] OneShot: {soundId}");
    }

    public void PlayOneShotSequence(string firstId, string secondId, float delaySeconds)
    {
        _ = PlaySequenceAsync(firstId, secondId, delaySeconds);
    }

    public void PlayFlashlightClick(bool turningOn)
    {
        if (_flashlightPlayer == null)
        {
            return;
        }

        _flashClickVariant = (_flashClickVariant + 1) % 2;
        var id = turningOn
            ? (_flashClickVariant == 0 ? "flashlight_click_on_01" : "flashlight_click_on_02")
            : (_flashClickVariant == 0 ? "flashlight_click_off_01" : "flashlight_click_off_02");

        if (!_fileById.TryGetValue(id, out var path))
        {
            return;
        }

        var stream = TryLoadStream(path, loop: false);
        if (stream == null)
        {
            return;
        }

        _flashlightPlayer.Stream = stream;
        _flashlightPlayer.VolumeDb = -16f;
        _flashlightPlayer.Play();
        GD.Print($"[Audio] OneShot: {id}");
    }

    public void NotifyZoneEntered(AmbienceZone3D zone)
    {
        _activeZones.Add(zone);
        ResolveActiveZone();
    }

    public void NotifyZoneExited(AmbienceZone3D zone)
    {
        _activeZones.Remove(zone);
        ResolveActiveZone();
    }

    private void RegisterCatalog()
    {
        _fileById[IdExterior] = AssetRoot + "exterior_night_wind_loop.ogg";
        _fileById[IdReception] = AssetRoot + "pension_reception_ambience_loop.ogg";
        _fileById[IdGroundCorridor] = AssetRoot + "pension_ground_corridor_loop.ogg";
        _fileById[IdDeposit] = AssetRoot + "pension_deposit_room_loop.ogg";
        _fileById[IdSecondFloor] = AssetRoot + "pension_second_floor_loop.ogg";
        _fileById[IdLampBuzz] = AssetRoot + "lamp_buzz_loop.ogg";
        _fileById[IdWaterDrops] = AssetRoot + "pension_water_drops_loop.ogg";

        RegisterOneShot("distant_step_01");
        RegisterOneShot("distant_step_02");
        RegisterOneShot("distant_step_03");
        RegisterOneShot("distant_step_04");
        RegisterOneShot("distant_knock_01");
        RegisterOneShot("distant_knock_02");
        RegisterOneShot("door_scratch_01");
        RegisterOneShot("door_scratch_02");
        RegisterOneShot("old_house_settle_01");
        RegisterOneShot("old_house_settle_02");
        RegisterOneShot("water_drop_01");
        RegisterOneShot("water_drop_02");
        RegisterOneShot("water_drop_03");
        RegisterOneShot("flashlight_click_on_01");
        RegisterOneShot("flashlight_click_on_02");
        RegisterOneShot("flashlight_click_off_01");
        RegisterOneShot("flashlight_click_off_02");

        _targetVolumesDb[IdExterior] = -16f;
        _targetVolumesDb[IdReception] = -18f;
        _targetVolumesDb[IdGroundCorridor] = -17f;
        _targetVolumesDb[IdDeposit] = -16f;
        _targetVolumesDb[IdSecondFloor] = -15f;
        _targetVolumesDb[IdLampBuzz] = -22f;
        _targetVolumesDb[IdWaterDrops] = -22f;
    }

    private void RegisterOneShot(string id) =>
        _fileById[id] = AssetRoot + id + ".ogg";

    private void BuildPlayers()
    {
        foreach (var id in new[]
                 {
                     IdExterior, IdReception, IdGroundCorridor, IdDeposit, IdSecondFloor,
                     IdLampBuzz, IdWaterDrops
                 })
        {
            var stream = TryLoadStream(_fileById[id], loop: true);
            var player = new AudioStreamPlayer
            {
                Name = $"Loop_{id}",
                Bus = "Ambience",
                VolumeDb = -80f,
                Autoplay = false
            };
            if (stream != null)
            {
                player.Stream = stream;
            }

            AddChild(player);
            _loops[id] = player;
        }

        _oneShotPlayer = new AudioStreamPlayer
        {
            Name = "OneShotPlayer",
            Bus = "SFX",
            VolumeDb = -13f
        };
        AddChild(_oneShotPlayer);

        _flashlightPlayer = new AudioStreamPlayer
        {
            Name = "FlashlightClickPlayer",
            Bus = "SFX",
            VolumeDb = -16f
        };
        AddChild(_flashlightPlayer);
    }

    private void ValidateSprint17Catalog()
    {
        // Footsteps / breath: validate presence only — do not wire to player this sprint.
        var files = new[]
        {
            "player_footstep_wood_01.ogg", "player_footstep_wood_02.ogg", "player_footstep_wood_03.ogg",
            "player_footstep_wood_04.ogg", "player_footstep_wood_05.ogg", "player_footstep_wood_06.ogg",
            "player_footstep_wood_07.ogg", "player_footstep_wood_08.ogg",
            "player_footsteps_wood_sequence.ogg",
            "player_footstep_dirt_gravel_01.ogg", "player_footstep_dirt_gravel_02.ogg",
            "player_footstep_dirt_gravel_03.ogg", "player_footstep_dirt_gravel_04.ogg",
            "player_footstep_dirt_gravel_05.ogg", "player_footstep_dirt_gravel_06.ogg",
            "player_footstep_dirt_gravel_07.ogg", "player_footstep_dirt_gravel_08.ogg",
            "player_footstep_dirt_gravel_09.ogg", "player_footstep_dirt_gravel_10.ogg",
            "player_footstep_dirt_gravel_11.ogg", "player_footstep_dirt_gravel_12.ogg",
            "player_footsteps_dirt_gravel_sequence.ogg",
            "player_run_step_01.ogg", "player_run_step_02.ogg", "player_run_step_03.ogg",
            "player_run_step_04.ogg", "player_run_step_05.ogg", "player_run_step_06.ogg",
            "player_run_step_07.ogg", "player_run_step_08.ogg", "player_run_step_09.ogg",
            "player_run_step_10.ogg", "player_run_step_11.ogg", "player_run_step_12.ogg",
            "player_running_sequence.ogg",
            "player_breath_heavy_loop.ogg",
            "player_breath_heavy_01.ogg", "player_breath_heavy_02.ogg",
            "player_breath_heavy_03.ogg", "player_breath_heavy_04.ogg",
            "player_panting_loop.ogg"
        };

        var present = 0;
        foreach (var file in files)
        {
            var path = AssetRoot + file;
            if (ResourceLoader.Exists(path))
            {
                present++;
            }
            else
            {
                WarnMissing(path);
            }
        }

        GD.Print($"[Audio] Sprint17 catalog ready: {present}/{files.Length} footstep/breath assets present (not wired).");
    }

    private void DeferredSetupZones()
    {
        var host = new Node3D { Name = "AmbienceZones" };
        AddChild(host);

        // Priority: deposit > second_floor > stairwell > ground_corridor > reception > exterior
        AddZone(host, "AudioZone_ExteriorTrail", IdExterior, 10,
            new Vector3(0f, 1.5f, 33f), new Vector3(10f, 5f, 36f));

        AddZone(host, "AudioZone_Reception", IdReception, 40,
            new Vector3(0f, 1.4f, -1.5f), new Vector3(11f, 3.2f, 12f),
            secondaryLoopId: IdLampBuzz);

        AddZone(host, "AudioZone_GroundCorridor", IdGroundCorridor, 50,
            new Vector3(0f, 1.4f, -17.5f), new Vector3(4.2f, 3.2f, 20f));

        AddZone(host, "AudioZone_Room102", IdGroundCorridor, 45,
            new Vector3(-4.2f, 1.4f, -15.5f), new Vector3(5.5f, 3.2f, 5.2f));

        AddZone(host, "AudioZone_Kitchen", IdGroundCorridor, 45,
            new Vector3(4.2f, 1.4f, -20.5f), new Vector3(5.5f, 3.2f, 5.2f));

        AddZone(host, "AudioZone_Deposit", IdDeposit, 80,
            new Vector3(0f, 1.4f, -29.2f), new Vector3(4.0f, 3.2f, 7.0f),
            secondaryLoopId: IdWaterDrops);

        AddZone(host, "AudioZone_Stairwell", IdGroundCorridor, 60,
            new Vector3(-4.1f, 2.6f, -27.0f), new Vector3(6.5f, 7.0f, 9.5f));

        AddZone(host, "AudioZone_SecondFloor", IdSecondFloor, 70,
            new Vector3(0f, 4.2f, -13.5f), new Vector3(13f, 3.6f, 18f),
            secondaryLoopId: IdLampBuzz);

        CrossfadeToAmbience(IdExterior, 0.5f);
    }

    private void AddZone(
        Node3D parent,
        string name,
        string ambienceId,
        int priority,
        Vector3 position,
        Vector3 size,
        string secondaryLoopId = "",
        string tertiaryLoopId = "")
    {
        var zone = new AmbienceZone3D
        {
            Name = name,
            AmbienceId = ambienceId,
            ZonePriority = priority,
            SecondaryLoopId = secondaryLoopId,
            TertiaryLoopId = tertiaryLoopId,
            Position = position
        };
        zone.AddChild(new CollisionShape3D
        {
            Shape = new BoxShape3D { Size = size }
        });
        parent.AddChild(zone);
        zone.Bind(this);
    }

    private void ResolveActiveZone()
    {
        AmbienceZone3D? best = null;
        foreach (var zone in _activeZones)
        {
            if (!GodotObject.IsInstanceValid(zone))
            {
                continue;
            }

            if (best == null || zone.ZonePriority > best.ZonePriority)
            {
                best = zone;
            }
        }

        if (best == null)
        {
            _inWetZone = false;
            if (string.IsNullOrEmpty(_currentAmbienceId))
            {
                CrossfadeToAmbience(IdExterior, DefaultCrossfadeSeconds);
            }

            UpdateExtraLoops(string.Empty, string.Empty);
            return;
        }

        GD.Print($"[Audio] Zone: {best.AmbienceId}");
        CrossfadeToAmbience(best.AmbienceId, DefaultCrossfadeSeconds);
        UpdateExtraLoops(best.SecondaryLoopId, best.TertiaryLoopId);
        _inWetZone = best.AmbienceId == IdDeposit || best.Name.ToString().Contains("Kitchen");
        if (_inWetZone && _occasionalDropTimer <= 0f)
        {
            _occasionalDropTimer = _rng.RandfRange(6f, 12f);
        }
    }

    private void UpdateExtraLoops(string secondaryId, string tertiaryId)
    {
        FadeExtraLoop(ref _currentSecondaryId, secondaryId);
        FadeExtraLoop(ref _currentTertiaryId, tertiaryId);
    }

    private void FadeExtraLoop(ref string currentId, string nextId)
    {
        nextId ??= string.Empty;
        if (currentId == nextId)
        {
            return;
        }

        if (!string.IsNullOrEmpty(currentId) && _loops.TryGetValue(currentId, out var oldLoop))
        {
            _ = FadePlayerAsync(oldLoop, oldLoop.VolumeDb, -80f, 1.0f, stopWhenDone: true);
        }

        currentId = nextId;
        if (string.IsNullOrEmpty(currentId) || !_loops.TryGetValue(currentId, out var nextLoop))
        {
            return;
        }

        if (nextLoop.Stream == null)
        {
            return;
        }

        var target = GetTargetDb(currentId);
        if (!nextLoop.Playing)
        {
            nextLoop.VolumeDb = -80f;
            nextLoop.Play();
        }

        _ = FadePlayerAsync(nextLoop, nextLoop.VolumeDb, target, 1.2f, stopWhenDone: false);
    }

    private async System.Threading.Tasks.Task CrossfadeAsync(string nextId, float duration)
    {
        if (!_loops.TryGetValue(nextId, out var next) || next.Stream == null)
        {
            return;
        }

        _crossfading = true;
        var previousId = _currentAmbienceId;
        _currentAmbienceId = nextId;

        var targetDb = GetTargetDb(nextId);
        if (!next.Playing)
        {
            next.VolumeDb = -80f;
            next.Play();
        }

        var fadeIn = FadePlayerAsync(next, next.VolumeDb, targetDb, duration, stopWhenDone: false);
        System.Threading.Tasks.Task fadeOut = System.Threading.Tasks.Task.CompletedTask;
        if (!string.IsNullOrEmpty(previousId) &&
            previousId != nextId &&
            _loops.TryGetValue(previousId, out var previous))
        {
            fadeOut = FadePlayerAsync(previous, previous.VolumeDb, -80f, duration, stopWhenDone: true);
        }

        await fadeIn;
        await fadeOut;
        _crossfading = false;
    }

    private async System.Threading.Tasks.Task FadePlayerAsync(
        AudioStreamPlayer player,
        float fromDb,
        float toDb,
        float duration,
        bool stopWhenDone)
    {
        if (!GodotObject.IsInstanceValid(player))
        {
            return;
        }

        var steps = Mathf.Max(1, Mathf.CeilToInt(duration / 0.05f));
        for (var i = 1; i <= steps; i++)
        {
            if (!GodotObject.IsInstanceValid(player))
            {
                return;
            }

            player.VolumeDb = Mathf.Lerp(fromDb, toDb, i / (float)steps);
            await ToSignal(GetTree().CreateTimer(0.05f), SceneTreeTimer.SignalName.Timeout);
        }

        if (stopWhenDone && GodotObject.IsInstanceValid(player))
        {
            player.Stop();
            player.VolumeDb = -80f;
        }
    }

    private async System.Threading.Tasks.Task PlaySequenceAsync(string firstId, string secondId, float delay)
    {
        PlayOneShot(firstId, -13f);
        await ToSignal(GetTree().CreateTimer(Mathf.Max(0.2f, delay)), SceneTreeTimer.SignalName.Timeout);
        PlayOneShot(secondId, -13f);
    }

    private float GetTargetDb(string id) =>
        _targetVolumesDb.GetValueOrDefault(id, -18f);

    private AudioStream? TryLoadStream(string path, bool loop)
    {
        if (!ResourceLoader.Exists(path))
        {
            WarnMissing(path);
            return null;
        }

        var stream = GD.Load<AudioStream>(path);
        if (stream == null)
        {
            WarnMissing(path);
            return null;
        }

        if (stream is AudioStreamOggVorbis ogg)
        {
            ogg.Loop = loop;
        }

        return stream;
    }

    private void WarnMissing(string pathOrId)
    {
        if (!_warnedMissing.Add(pathOrId))
        {
            return;
        }

        GD.PushWarning($"[Audio] Missing asset: {pathOrId}");
    }
}
