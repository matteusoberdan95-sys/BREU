namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;

/// <summary>
/// Sprint 26 ambient event scheduler. Reads progression/AI state but never mutates gameplay,
/// geometry, player movement, navigation, doors or puzzle state.
/// </summary>
public partial class AmbientHorrorDirector : Node
{
    public const string DoorCreak = "AmbientEvent_DistantDoorCreak";
    public const string UpperKnock = "AmbientEvent_UpperFloorKnock";
    public const string Footsteps = "AmbientEvent_DistantFootsteps";
    public const string LightFlicker = "AmbientEvent_LightFlicker";
    public const string WallScratch = "AmbientEvent_WallScratch";
    public const string ObjectFall = "AmbientEvent_ObjectFall";
    public const string BreathBehind = "AmbientEvent_BreathBehind";
    public const string DrainWhisper = "AmbientEvent_DrainWhisper";
    public const string QuickShadow = "AmbientEvent_QuickShadow";

    private readonly RandomNumberGenerator _rng = new();
    private readonly HashSet<string> _activeZones = new(StringComparer.Ordinal);
    private readonly HashSet<string> _oneShotsPlayed = new(StringComparer.Ordinal);
    private PensaoPuzzleState? _state;
    private CharacterBody3D? _player;
    private AudioStreamPlayer3D? _emitter;
    private MeshInstance3D? _shadow;
    private double _cooldownSeconds = 6.0;
    private double _pollSeconds;
    private bool _eventPlaying;

    public bool AmbientEventsEnabled => _state?.Room203EventPlayed == true || _state?.FirstPresencePlayed == true;
    public bool AmbientEventRecentlyPlayed => _eventPlaying || _cooldownSeconds > 0.0;

    public override void _Ready()
    {
        _rng.Randomize();
        _state = GetTree().CurrentScene?.GetNodeOrNull<PensaoPuzzleState>("PuzzleState");
        _emitter = GetNodeOrNull<AudioStreamPlayer3D>("../AmbientAudioEmitters/LocalizedEmitter");
        _shadow = GetNodeOrNull<MeshInstance3D>("../AmbientVisuals/QuickShadowSilhouette");
        if (_shadow != null) _shadow.Visible = false;
        GD.Print("[AmbientHorror] Director ready; events remain gated until post-203 progression.");
    }

    public override void _Process(double delta)
    {
        if (_cooldownSeconds > 0.0) _cooldownSeconds -= delta;
        _pollSeconds -= delta;
        if (_pollSeconds > 0.0 || _eventPlaying || _activeZones.Count == 0) return;
        _pollSeconds = 2.0;
        if (_cooldownSeconds <= 0.0 && CanPlayNow()) _ = TryPlayEventAsync();
    }

    public void EnterZone(string zoneId, CharacterBody3D player)
    {
        _player = player;
        _activeZones.Add(zoneId);
        GD.Print($"[AmbientHorror] Trigger entered: {zoneId}");
        var candidates = BuildCandidates();
        GD.Print($"[AmbientHorror] Available events count: {candidates.Count}");
        if (_cooldownSeconds > 0.0) GD.Print($"[AmbientHorror] Event skipped: cooldown ({_cooldownSeconds:0.0}s)");
        else if (!CanPlayNow()) GD.Print($"[AmbientHorror] Event skipped: {BlockedReason()}");
    }

    public void ExitZone(string zoneId, CharacterBody3D player)
    {
        _activeZones.Remove(zoneId);
        if (_activeZones.Count == 0 && _player == player) _player = null;
    }

    private bool CanPlayNow()
    {
        if (!AmbientEventsEnabled || _state == null || _player == null) return false;
        var firstChaseActive = _state.FirstChaseStarted && !_state.FirstChaseFinished;
        var secondChaseActive = _state.SecondChaseStarted && !_state.SecondChaseFinished;
        return !firstChaseActive && !secondChaseActive && !_state.EnemyAlerted && !_state.EnemySearching &&
               !_state.PlayerHidden && !_state.PlayerInSafeZone;
    }

    private async System.Threading.Tasks.Task TryPlayEventAsync()
    {
        if (!CanPlayNow()) return;
        var candidates = BuildCandidates();
        if (candidates.Count == 0) return;
        var eventId = candidates[_rng.RandiRange(0, candidates.Count - 1)];
        _eventPlaying = true;
        GD.Print($"[AmbientHorror] Event started: {eventId}");
        GD.Print($"[AmbientHorror] Playing: {eventId} in {CurrentZone()}");
        try
        {
            await PlayEventAsync(eventId);
            if (IsOneShot(eventId)) _oneShotsPlayed.Add(eventId);
        }
        finally
        {
            _eventPlaying = false;
            _cooldownSeconds = _rng.RandfRange(25f, 45f);
            GD.Print($"[AmbientHorror] Event finished: {eventId}");
            GD.Print($"[AmbientHorror] Cooldown {_cooldownSeconds:0.0}s");
        }
    }

    private List<string> BuildCandidates()
    {
        var zone = CurrentZone();
        var result = new List<string>();
        if (_state?.Room203EventPlayed == true) result.Add(WallScratch);
        if (_state?.Sprint21Completed == true || _state?.FirstPresencePlayed == true) result.Add(Footsteps);
        if (_state?.FirstPresencePlayed == true && zone is "Reception" or "BackHall" or "UpperCorridor") result.Add(DoorCreak);
        if (zone == "Reception" && _state?.Room203EventPlayed == true) result.Add(UpperKnock);
        if (_state?.IsUpperPowerRestored == true) result.Add(LightFlicker);
        if (_state?.FirstChaseFinished == true) { result.Add(ObjectFall); result.Add(BreathBehind); }
        if (zone == "Bathroom" && _state?.HasDrainKey == true) result.Add(DrainWhisper);
        if (_state?.Sprint22Completed == true && zone is "Reception" or "BackHall" or "UpperCorridor") result.Add(QuickShadow);
        result.RemoveAll(id => IsOneShot(id) && _oneShotsPlayed.Contains(id));
        return result;
    }

    private async System.Threading.Tasks.Task PlayEventAsync(string eventId)
    {
        switch (eventId)
        {
            case DoorCreak:
                await PlaySpatialAsync("old_house_settle_01", EventPosition(5f), -12f, 2.2f);
                ShowRareMessage("Uma porta rangeu em algum lugar da pensão.");
                break;
            case UpperKnock:
                await PlaySpatialAsync("distant_knock_01", new Vector3(0, 4.3f, -13.5f), -10f, 0.55f);
                await PlaySpatialAsync("distant_knock_02", new Vector3(0, 4.3f, -13.5f), -12f, 1.2f);
                break;
            case Footsteps:
                for (var i = 0; i < 3; i++)
                    await PlaySpatialAsync($"distant_step_{_rng.RandiRange(1, 4):00}", EventPosition(6f), -14f, 0.65f);
                break;
            case LightFlicker:
                await FlickerLocalLightAsync();
                break;
            case WallScratch:
                await PlaySpatialAsync("door_scratch_02", EventPosition(2.5f), -14f, 2.0f);
                break;
            case ObjectFall:
                await PlaySpatialAsync("distant_knock_02", EventPosition(5f), -9f, 0.45f);
                await PlaySpatialAsync("old_house_settle_02", EventPosition(5f), -15f, 1.2f);
                break;
            case BreathBehind:
                await PlaySpatialAsync($"player_breath_heavy_{_rng.RandiRange(1, 4):00}", BehindPlayer(), -9f, 2.0f);
                ShowRareMessage("Havia uma respiração perto demais.");
                break;
            case DrainWhisper:
                await PlaySpatialAsync("water_drop_03", new Vector3(-4.5f, 3.0f, 3.5f), -10f, 0.8f);
                await PlaySpatialAsync("player_breath_heavy_02", new Vector3(-4.5f, 3.0f, 3.5f), -17f, 1.5f);
                break;
            case QuickShadow:
                await ShowQuickShadowAsync();
                break;
        }
    }

    private async System.Threading.Tasks.Task PlaySpatialAsync(string soundId, Vector3 position, float volumeDb, float fallbackSeconds)
    {
        if (_emitter == null) { PensionAudioManager.Find(GetTree())?.PlayOneShot(soundId, volumeDb); return; }
        var stream = GD.Load<AudioStream>($"res://assets/audio/pensao/{soundId}.ogg");
        if (stream == null) { GD.PushWarning($"[AmbientHorror] Missing audio {soundId}"); return; }
        _emitter.GlobalPosition = position;
        _emitter.Stream = stream;
        _emitter.VolumeDb = volumeDb;
        _emitter.Play();
        await ToSignal(GetTree().CreateTimer(fallbackSeconds), SceneTreeTimer.SignalName.Timeout);
    }

    private async System.Threading.Tasks.Task FlickerLocalLightAsync()
    {
        var path = CurrentZone() switch
        {
            "Reception" => "Lighting/ReceptionLight",
            "BackHall" => "Lighting/CorridorDeepLight",
            "Stair" => "Lighting/StairShaftUpperLight",
            _ => "Lighting/UpperCorridorLight"
        };
        var light = GetTree().CurrentScene?.GetNodeOrNull<Light3D>(path);
        if (light == null) return;
        var original = light.LightEnergy;
        for (var i = 0; i < 4; i++)
        {
            light.LightEnergy = i % 2 == 0 ? original * 0.08f : original * 0.7f;
            await ToSignal(GetTree().CreateTimer(0.12f), SceneTreeTimer.SignalName.Timeout);
        }
        if (GodotObject.IsInstanceValid(light)) light.LightEnergy = original;
    }

    private async System.Threading.Tasks.Task ShowQuickShadowAsync()
    {
        if (_shadow == null || _player == null) return;
        _shadow.GlobalPosition = EventPosition(5.5f) + Vector3.Up * 0.9f;
        _shadow.LookAt(_player.GlobalPosition + Vector3.Up * 0.9f, Vector3.Up);
        _shadow.Visible = true;
        await ToSignal(GetTree().CreateTimer(0.55f), SceneTreeTimer.SignalName.Timeout);
        if (GodotObject.IsInstanceValid(_shadow)) _shadow.Visible = false;
    }

    private Vector3 EventPosition(float distance)
    {
        if (_player == null) return Vector3.Zero;
        var side = _rng.Randf() < 0.5f ? -1f : 1f;
        return _player.GlobalPosition + _player.GlobalBasis.X * side * 2f - _player.GlobalBasis.Z * distance + Vector3.Up * 0.8f;
    }

    private Vector3 BehindPlayer() => _player == null ? Vector3.Zero : _player.GlobalPosition + _player.GlobalBasis.Z * 1.15f + Vector3.Up * 1.25f;
    private string CurrentZone() => _activeZones.FirstOrDefault() ?? "Corridor";
    private static bool IsOneShot(string id) => id is DoorCreak or UpperKnock or ObjectFall or BreathBehind or DrainWhisper or QuickShadow;
    private void ShowRareMessage(string text) => HUDController.FindActive(GetTree())?.ShowMessage(text, 2.8f);

    private string BlockedReason()
    {
        if (!AmbientEventsEnabled) return "progression locked";
        if (_state == null) return "puzzle state unavailable";
        if ((_state.FirstChaseStarted && !_state.FirstChaseFinished) ||
            (_state.SecondChaseStarted && !_state.SecondChaseFinished)) return "chase active";
        if (_state.EnemyAlerted) return "enemy alerted";
        if (_state.EnemySearching) return "enemy searching";
        if (_state.PlayerHidden) return "player hidden";
        if (_state.PlayerInSafeZone) return "safe zone active";
        return "no eligible event";
    }
}
