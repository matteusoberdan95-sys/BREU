namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;

/// <summary>Sprint 22 — short scripted chase. Visual-only enemy; no physics or pathfinding.</summary>
public partial class FirstEnemyChaseController : Node3D
{
    [Export] public float ChaseSpeed { get; set; } = 2.65f;

    private PensaoPuzzleState? _state;
    private Area3D? _revealTrigger;
    private Area3D? _escapeTrigger;
    private Node3D? _enemy;
    private readonly List<Marker3D> _path = new();
    private int _pathIndex;
    private bool _moving;

    public override void _Ready()
    {
        _state = GetTree().CurrentScene?.GetNodeOrNull<PensaoPuzzleState>("PuzzleState");
        _revealTrigger = GetNodeOrNull<Area3D>("Trigger_FirstEnemyReveal");
        _escapeTrigger = GetNodeOrNull<Area3D>("Trigger_FirstChaseEscape");
        _enemy = GetNodeOrNull<Node3D>("Enemy_FirstPresence");
        if (_enemy != null) _enemy.Visible = false;

        var pathRoot = GetNodeOrNull<Node3D>("FirstChase_Path");
        if (pathRoot != null)
            foreach (var child in pathRoot.GetChildren())
                if (child is Marker3D marker) _path.Add(marker);

        if (_revealTrigger != null) _revealTrigger.BodyEntered += OnRevealEntered;
        if (_escapeTrigger != null) _escapeTrigger.BodyEntered += OnEscapeEntered;
    }

    public override void _Process(double delta)
    {
        if (!_moving || _enemy == null || _pathIndex >= _path.Count) return;
        var target = _path[_pathIndex].GlobalPosition;
        _enemy.GlobalPosition = _enemy.GlobalPosition.MoveToward(target, ChaseSpeed * (float)delta);
        if (_enemy.GlobalPosition.DistanceTo(target) < 0.08f)
        {
            _pathIndex++;
            if (_pathIndex >= _path.Count) _moving = false;
        }
    }

    private void OnRevealEntered(Node3D body)
    {
        if (body is not CharacterBody3D || body.GlobalPosition.Y > 1.7f ||
            _state?.Sprint21Completed != true || _state.FirstChaseStarted || _state.FirstChaseFinished) return;
        _revealTrigger?.SetDeferred(Area3D.PropertyName.Monitoring, false);
        GD.Print("[SPRINT22] Trigger reveal entered");
        _ = RevealAndChaseAsync();
    }

    private async System.Threading.Tasks.Task RevealAndChaseAsync()
    {
        var audio = PensionAudioManager.Find(GetTree());
        audio?.PlayOneShot("old_house_settle_01", -14f);
        await FlickerCorridorAsync();
        if (_enemy != null && _path.Count > 0)
        {
            _enemy.GlobalPosition = _path[0].GlobalPosition;
            _enemy.Visible = true;
        }
        HUDController.FindActive(GetTree())?.ShowMessage("Tem alguém ali.", 2.2f);
        GD.Print("[SPRINT22] First enemy seen");
        await ToSignal(GetTree().CreateTimer(1.15f), SceneTreeTimer.SignalName.Timeout);
        _state!.StartFirstChase();
        _pathIndex = Math.Min(1, _path.Count);
        _moving = _pathIndex < _path.Count;
        audio?.PlayOneShot("distant_step_04", -12f);
        HUDController.FindActive(GetTree())?.ShowMessage("Corra.", 3f);
        GD.Print("[SPRINT22] Chase started");
    }

    private void OnEscapeEntered(Node3D body)
    {
        if (body is not CharacterBody3D || body.GlobalPosition.Y > 1.7f ||
            _state?.FirstChaseStarted != true || _state.FirstChaseFinished) return;
        _state.EscapeFirstChase();
        StopForShelter();
        PensionAudioManager.Find(GetTree())?.PlayOneShot("distant_knock_02", -14f);
        HUDController.FindActive(GetTree())?.ShowMessage(
            "Ele parou. Objetivo: Algo está dentro da pensão. Procure uma forma de se esconder.", 6f);
        GD.Print("[SPRINT22] Chase escaped");
        GD.Print("[SPRINT22] Chase finished");
    }

    public void StopForShelter()
    {
        _moving = false;
        if (_enemy != null) _enemy.Visible = false;
        _escapeTrigger?.SetDeferred(Area3D.PropertyName.Monitoring, false);
    }

    private async System.Threading.Tasks.Task FlickerCorridorAsync()
    {
        var light = GetTree().CurrentScene?.GetNodeOrNull<OmniLight3D>("Lighting/CorridorDeepLight");
        if (light == null) return;
        var energy = light.LightEnergy;
        light.LightEnergy = energy * 0.1f;
        await ToSignal(GetTree().CreateTimer(0.55f), SceneTreeTimer.SignalName.Timeout);
        if (GodotObject.IsInstanceValid(light)) light.LightEnergy = energy;
    }
}
