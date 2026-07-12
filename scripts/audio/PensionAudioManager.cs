namespace BREU.Scripts.Audio;

/// <summary>
/// Sprint 16 — pension ambience loops, zone crossfade, and narrative one-shots.
/// Missing assets log a warning and never crash.
/// </summary>
public partial class PensionAudioManager : Node
{
    public const string IdExterior = "exterior";
    public const string IdReception = "reception";
    public const string IdGroundCorridor = "ground_corridor";
    public const string IdDeposit = "deposit";
    public const string IdSecondFloor = "second_floor";
    public const string IdLampBuzz = "lamp_buzz";

    private const string AssetRoot = "res://assets/audio/pensao/";
    private const float DefaultCrossfadeSeconds = 1.6f;

    private readonly Dictionary<string, AudioStreamPlayer> _loops = new(StringComparer.Ordinal);
    private readonly Dictionary<string, float> _targetVolumesDb = new(StringComparer.Ordinal);
    private readonly Dictionary<string, string> _fileById = new(StringComparer.Ordinal);
    private readonly HashSet<AmbienceZone3D> _activeZones = new();
    private readonly List<AmbienceZone3D> _zones = new();

    private AudioStreamPlayer? _oneShotPlayer;
    private string _currentAmbienceId = string.Empty;
    private string _currentSecondaryId = string.Empty;
    private bool _crossfading;
    private float _ambienceBusOffsetDb;
    private float _sfxBusOffsetDb;

    public static PensionAudioManager? Find(SceneTree tree) =>
        tree.GetFirstNodeInGroup("pension_audio_manager") as PensionAudioManager;

    public override void _Ready()
    {
        AddToGroup("pension_audio_manager");
        RegisterCatalog();
        BuildPlayers();
        CallDeferred(nameof(DeferredSetupZones));
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
            GD.PushWarning($"[Audio] Unknown one-shot id: {soundId}");
            return;
        }

        var stream = TryLoadStream(path, loop: false);
        if (stream == null)
        {
            return;
        }

        _oneShotPlayer.Stream = stream;
        _oneShotPlayer.VolumeDb = (volumeDb ?? -12f) + _sfxBusOffsetDb;
        _oneShotPlayer.Play();
        GD.Print($"[Audio] OneShot: {soundId}");
    }

    public void PlayOneShotSequence(string firstId, string secondId, float delaySeconds)
    {
        _ = PlaySequenceAsync(firstId, secondId, delaySeconds);
    }

    public void SetAmbienceVolume(float volumeLinear)
    {
        _ambienceBusOffsetDb = Mathf.LinearToDb(Mathf.Clamp(volumeLinear, 0.01f, 1.5f));
        RefreshPlayingVolumes();
    }

    public void SetSfxVolume(float volumeLinear)
    {
        _sfxBusOffsetDb = Mathf.LinearToDb(Mathf.Clamp(volumeLinear, 0.01f, 1.5f));
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
        // Loops
        _fileById[IdExterior] = AssetRoot + "exterior_night_wind_loop.ogg";
        _fileById[IdReception] = AssetRoot + "pension_reception_ambience_loop.ogg";
        _fileById[IdGroundCorridor] = AssetRoot + "pension_ground_corridor_loop.ogg";
        _fileById[IdDeposit] = AssetRoot + "pension_deposit_room_loop.ogg";
        _fileById[IdSecondFloor] = AssetRoot + "pension_second_floor_loop.ogg";
        _fileById[IdLampBuzz] = AssetRoot + "lamp_buzz_loop.ogg";

        // One-shots
        _fileById["wood_creak_01"] = AssetRoot + "wood_creak_01.ogg";
        _fileById["wood_creak_02"] = AssetRoot + "wood_creak_02.ogg";
        _fileById["distant_step_01"] = AssetRoot + "distant_step_01.ogg";
        _fileById["distant_step_02"] = AssetRoot + "distant_step_02.ogg";
        _fileById["distant_knock_01"] = AssetRoot + "distant_knock_01.ogg";
        _fileById["door_scratch_01"] = AssetRoot + "door_scratch_01.ogg";
        _fileById["old_house_settle_01"] = AssetRoot + "old_house_settle_01.ogg";
        _fileById["old_house_settle_02"] = AssetRoot + "old_house_settle_02.ogg";

        _targetVolumesDb[IdExterior] = -16f;
        _targetVolumesDb[IdReception] = -18f;
        _targetVolumesDb[IdGroundCorridor] = -17f;
        _targetVolumesDb[IdDeposit] = -16f;
        _targetVolumesDb[IdSecondFloor] = -15f;
        _targetVolumesDb[IdLampBuzz] = -22f;
    }

    private void BuildPlayers()
    {
        foreach (var id in new[]
                 {
                     IdExterior, IdReception, IdGroundCorridor, IdDeposit, IdSecondFloor, IdLampBuzz
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
            VolumeDb = -12f
        };
        AddChild(_oneShotPlayer);
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
            new Vector3(0f, 1.4f, -29.2f), new Vector3(4.0f, 3.2f, 7.0f));

        AddZone(host, "AudioZone_Stairwell", IdGroundCorridor, 60,
            new Vector3(-4.1f, 2.6f, -27.0f), new Vector3(6.5f, 7.0f, 9.5f));

        AddZone(host, "AudioZone_SecondFloor", IdSecondFloor, 70,
            new Vector3(0f, 4.2f, -13.5f), new Vector3(13f, 3.6f, 18f),
            secondaryLoopId: IdLampBuzz);

        // Start exterior immediately (player spawns on trail).
        CrossfadeToAmbience(IdExterior, 0.5f);
    }

    private void AddZone(
        Node3D parent,
        string name,
        string ambienceId,
        int priority,
        Vector3 position,
        Vector3 size,
        string secondaryLoopId = "")
    {
        var zone = new AmbienceZone3D
        {
            Name = name,
            AmbienceId = ambienceId,
            ZonePriority = priority,
            SecondaryLoopId = secondaryLoopId,
            Position = position
        };
        zone.AddChild(new CollisionShape3D
        {
            Shape = new BoxShape3D { Size = size }
        });
        parent.AddChild(zone);
        zone.Bind(this);
        _zones.Add(zone);
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
            // Fallback: keep last ambience, prefer exterior if nothing active.
            if (string.IsNullOrEmpty(_currentAmbienceId))
            {
                CrossfadeToAmbience(IdExterior, DefaultCrossfadeSeconds);
            }

            return;
        }

        GD.Print($"[Audio] Ambience zone: {best.AmbienceId}");
        CrossfadeToAmbience(best.AmbienceId, DefaultCrossfadeSeconds);
        UpdateSecondaryLoop(best.SecondaryLoopId);
    }

    private void UpdateSecondaryLoop(string secondaryId)
    {
        if (_currentSecondaryId == secondaryId)
        {
            return;
        }

        if (!string.IsNullOrEmpty(_currentSecondaryId) &&
            _loops.TryGetValue(_currentSecondaryId, out var oldSecondary))
        {
            _ = FadePlayerAsync(oldSecondary, oldSecondary.VolumeDb, -80f, 1.0f, stopWhenDone: true);
        }

        _currentSecondaryId = secondaryId ?? string.Empty;
        if (string.IsNullOrEmpty(_currentSecondaryId) ||
            !_loops.TryGetValue(_currentSecondaryId, out var nextSecondary))
        {
            return;
        }

        if (nextSecondary.Stream == null)
        {
            return;
        }

        var target = GetTargetDb(_currentSecondaryId);
        if (!nextSecondary.Playing)
        {
            nextSecondary.VolumeDb = -80f;
            nextSecondary.Play();
        }

        _ = FadePlayerAsync(nextSecondary, nextSecondary.VolumeDb, target, 1.2f, stopWhenDone: false);
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

            var t = i / (float)steps;
            player.VolumeDb = Mathf.Lerp(fromDb, toDb, t);
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

    private float GetTargetDb(string id)
    {
        var baseDb = _targetVolumesDb.GetValueOrDefault(id, -18f);
        return baseDb + _ambienceBusOffsetDb;
    }

    private void RefreshPlayingVolumes()
    {
        if (!string.IsNullOrEmpty(_currentAmbienceId) &&
            _loops.TryGetValue(_currentAmbienceId, out var current) &&
            current.Playing)
        {
            current.VolumeDb = GetTargetDb(_currentAmbienceId);
        }

        if (!string.IsNullOrEmpty(_currentSecondaryId) &&
            _loops.TryGetValue(_currentSecondaryId, out var secondary) &&
            secondary.Playing)
        {
            secondary.VolumeDb = GetTargetDb(_currentSecondaryId);
        }
    }

    private static AudioStream? TryLoadStream(string path, bool loop)
    {
        if (!ResourceLoader.Exists(path))
        {
            GD.PushWarning($"[Audio] Missing asset: {path}");
            return null;
        }

        var stream = GD.Load<AudioStream>(path);
        if (stream == null)
        {
            GD.PushWarning($"[Audio] Failed to load: {path}");
            return null;
        }

        switch (stream)
        {
            case AudioStreamOggVorbis ogg:
                ogg.Loop = loop;
                break;
        }

        return stream;
    }
}
