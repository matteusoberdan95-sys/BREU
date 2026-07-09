namespace BREU.Scripts.Player;

/// <summary>
/// Passos, pulo e pouso do player. Superficie: concreto por padrao (madeira reservada).
/// </summary>
public partial class PlayerFootstepAudio : AudioStreamPlayer3D
{
    [Export] public AudioStream[] ConcreteFootsteps { get; set; } = Array.Empty<AudioStream>();
    [Export] public AudioStream[] WoodFootsteps { get; set; } = Array.Empty<AudioStream>();
    [Export] public AudioStream? JumpStartSound { get; set; }
    [Export] public AudioStream? LandSoftSound { get; set; }
    [Export] public AudioStream? LandHeavySound { get; set; }
    [Export] public float WalkStepInterval { get; set; } = 0.55f;
    [Export] public float SprintStepInterval { get; set; } = 0.36f;
    [Export] public float MinMoveSpeedForSteps { get; set; } = 0.2f;
    [Export] public float HeavyLandVelocity { get; set; } = -7.0f;
    [Export] public float FootstepVolumeDb { get; set; } = -12.0f;
    [Export] public bool UseWoodFootsteps { get; set; }

    private float _stepTimer;
    private readonly RandomNumberGenerator _rng = new();

    public override void _Ready()
    {
        VolumeDb = FootstepVolumeDb;
        _rng.Randomize();
        EnsureDefaultStreams();
    }

    public void UpdateMovementAudio(bool isMoving, bool isSprinting, bool isOnFloor, double delta)
    {
        if (!isOnFloor || !isMoving)
        {
            _stepTimer = 0.0f;
            return;
        }

        var interval = isSprinting ? SprintStepInterval : WalkStepInterval;
        _stepTimer += (float)delta;

        if (_stepTimer < interval)
        {
            return;
        }

        _stepTimer = 0.0f;
        PlayFootstep();
    }

    public void PlayJump()
    {
        PlayOneShot(JumpStartSound, "pulo");
    }

    public void PlayLand(float previousVerticalVelocity)
    {
        var stream = previousVerticalVelocity < HeavyLandVelocity ? LandHeavySound : LandSoftSound;
        var label = previousVerticalVelocity < HeavyLandVelocity ? "pouso pesado" : "pouso leve";
        PlayOneShot(stream, label);
    }

    private void PlayFootstep()
    {
        var pool = UseWoodFootsteps && WoodFootsteps.Length > 0 ? WoodFootsteps : ConcreteFootsteps;
        if (pool.Length == 0)
        {
            GD.Print("FootstepAudio: nenhum passo configurado.");
            return;
        }

        var stream = pool[_rng.RandiRange(0, pool.Length - 1)];
        PlayOneShot(stream, "passo", randomizePitch: true);
    }

    private void PlayOneShot(AudioStream? stream, string label, bool randomizePitch = false)
    {
        if (stream == null)
        {
            GD.Print($"FootstepAudio: som de {label} nao configurado.");
            return;
        }

        Stream = stream;
        VolumeDb = FootstepVolumeDb;
        PitchScale = randomizePitch ? _rng.RandfRange(0.92f, 1.08f) : 1.0f;
        Play();
    }

    private void EnsureDefaultStreams()
    {
        if (ConcreteFootsteps.Length == 0)
        {
            ConcreteFootsteps = LoadFootstepSet("footstep_concrete", 4);
        }

        if (WoodFootsteps.Length == 0)
        {
            WoodFootsteps = LoadFootstepSet("footstep_wood", 4);
        }

        JumpStartSound ??= AudioResourceLoader.TryLoad(AudioPaths.PlayerJumpStart);
        LandSoftSound ??= AudioResourceLoader.TryLoad(AudioPaths.PlayerLandSoft);
        LandHeavySound ??= AudioResourceLoader.TryLoad(AudioPaths.PlayerLandHeavy);
    }

    private static AudioStream[] LoadFootstepSet(string prefix, int count)
    {
        var streams = new List<AudioStream>();
        for (var i = 1; i <= count; i++)
        {
            var path = $"res://assets/audio/sfx/player/{prefix}_{i:D2}.ogg";
            var stream = AudioResourceLoader.TryLoad(path);
            if (stream != null)
            {
                streams.Add(stream);
            }
        }

        return streams.ToArray();
    }
}
