namespace BREU.Scripts.Player;

public partial class PlayerBodyMotion : Node
{
    private enum MovementFeelState
    {
        Idle,
        Walking,
        Running,
        Crouching,
        Exhausted,
        Dead
    }

    [Export] public NodePath CameraPath { get; set; } = "CameraPivot/Camera3D";
    [Export] public NodePath FlashlightPath { get; set; } = "CameraPivot/Flashlight";
    [Export] public NodePath WeaponHolderPath { get; set; } = "CameraPivot/Camera3D/WeaponHolder";
    [Export] public NodePath BreathAudioPath { get; set; } = "BreathAudio";
    [Export] public NodePath StaminaPath { get; set; } = "PlayerStamina";
    [Export] public NodePath HealthPath { get; set; } = "PlayerHealth";

    [Export] public bool EnableBodyMotion { get; set; } = true;
    [Export] public bool EnableHeadBob { get; set; } = true;
    [Export] public bool EnableShoulderSway { get; set; } = true;
    [Export] public bool EnableWeaponSway { get; set; } = true;
    [Export] public bool EnableBreathingMotion { get; set; } = true;
    [Export] public bool EnableStepImpact { get; set; } = true;

    [Export] public float WalkStepFrequency { get; set; } = 7.2f;
    [Export] public float RunStepFrequency { get; set; } = 9.2f;
    [Export] public float CrouchStepFrequency { get; set; } = 4.2f;

    [Export] public float WalkBobVertical { get; set; } = 0.032f;
    [Export] public float WalkBobHorizontal { get; set; } = 0.016f;
    [Export] public float WalkRollAmount { get; set; } = 1.1f;
    [Export] public float RunBobVertical { get; set; } = 0.055f;
    [Export] public float RunBobHorizontal { get; set; } = 0.020f;
    [Export] public float RunRollAmount { get; set; } = 1.65f;
    [Export] public float RunPitchAmount { get; set; } = 0.85f;
    [Export] public float CrouchBobVertical { get; set; } = 0.014f;
    [Export] public float CrouchBobHorizontal { get; set; } = 0.008f;
    [Export] public float CrouchRollAmount { get; set; } = 0.5f;

    [Export] public float ShoulderSwayRunAmount { get; set; } = 0.030f;
    [Export] public float ShoulderRollRunAmount { get; set; } = 1.25f;
    [Export] public float ShoulderSwaySmooth { get; set; } = 10.0f;
    // Fallback mais leve se a corrida ainda ficar lateral demais:
    // RunBobHorizontal = 0.014, RunRollAmount = 1.2, ShoulderRollRunAmount = 0.9.

    [Export] public float AccelerationTiltAmount { get; set; } = 1.3f;
    [Export] public float StopTiltAmount { get; set; } = 1.8f;
    [Export] public float InertiaSwayAmount { get; set; } = 0.030f;
    [Export] public float InertiaSmooth { get; set; } = 8.0f;

    [Export] public float WalkStepImpact { get; set; } = 0.010f;
    [Export] public float RunStepImpact { get; set; } = 0.018f;
    [Export] public float StepImpactReturnSpeed { get; set; } = 12.0f;

    [Export] public float BreathingIdleAmount { get; set; } = 0.006f;
    [Export] public float BreathingTiredAmount { get; set; } = 0.022f;
    [Export] public float BreathingSpeed { get; set; } = 2.0f;

    [Export] public float WeaponWalkSwayAmount { get; set; } = 0.028f;
    [Export] public float WeaponRunSwayAmount { get; set; } = 0.045f;
    [Export] public float WeaponCrouchSwayAmount { get; set; } = 0.014f;
    [Export] public float TiredHandShakeAmount { get; set; } = 0.016f;

    [Export] public float DamageShakeAmount { get; set; } = 0.12f;
    [Export] public float DamageShakeDuration { get; set; } = 0.22f;
    [Export] public float DamageKickAmount { get; set; } = 0.035f;
    [Export] public float LeanAngle { get; set; } = 8.0f;
    [Export] public float LeanOffset { get; set; } = 0.18f;
    [Export] public float LeanSpeed { get; set; } = 8.0f;
    [Export] public float LowStaminaThreshold { get; set; } = 35.0f;
    [Export] public float Smoothing { get; set; } = 12.0f;
    [Export] public float AttackSwayDampenTime { get; set; } = 0.25f;
    [Export] public float TiredSoundCooldown { get; set; } = 3.0f;

    [Export] public string BreathLightAudioPath { get; set; } = "res://assets/audio/sfx/player/breath_light_01.ogg";
    [Export] public string BreathHeavyAudioPath { get; set; } = "res://assets/audio/sfx/player/breath_heavy_01.ogg";
    [Export] public string TiredAudioPath { get; set; } = "res://assets/audio/sfx/player/player_tired_01.ogg";

    private PlayerController? _controller;
    private PlayerStamina? _stamina;
    private PlayerHealth? _health;
    private Camera3D? _camera;
    private Node3D? _flashlight;
    private Node3D? _weaponHolder;
    private AudioStreamPlayer? _breathAudio;
    private AudioStreamPlayer? _tiredAudio;
    private AudioStream? _lightBreathStream;
    private AudioStream? _heavyBreathStream;
    private AudioStream? _tiredBreathStream;
    private Vector3 _cameraBasePosition;
    private Vector3 _cameraBaseRotation;
    private Vector3 _flashlightBasePosition;
    private Vector3 _flashlightBaseRotation;
    private Vector3 _weaponBasePosition;
    private Vector3 _weaponBaseRotation;
    private Vector2 _previousHorizontalVelocity;
    private Vector2 _inertiaSway;
    private readonly RandomNumberGenerator _random = new();
    private float _gaitPhase;
    private float _lastStepWave;
    private float _stepImpactOffset;
    private float _breathingPhase;
    private float _currentLean;
    private float _damageShakeTimer;
    private float _damageKickTimer;
    private float _attackDampenTimer;
    private float _tiredSoundCooldownRemaining;
    private float _lastStaminaPercent = 100.0f;
    private bool _missingAudioWarned;
    private bool _lowStaminaMessageShown;

    public override void _Ready()
    {
        EnsureInputMap();
        _random.Randomize();

        _controller = GetParentOrNull<PlayerController>();
        _camera = _controller?.GetNodeOrNull<Camera3D>(CameraPath);
        _flashlight = _controller?.GetNodeOrNull<Node3D>(FlashlightPath);
        _weaponHolder = _controller?.GetNodeOrNull<Node3D>(WeaponHolderPath);
        _breathAudio = _controller?.GetNodeOrNull<AudioStreamPlayer>(BreathAudioPath);
        _tiredAudio = GetNodeOrNull<AudioStreamPlayer>("TiredBreathAudio");
        if (_tiredAudio == null)
        {
            _tiredAudio = new AudioStreamPlayer { Name = "TiredBreathAudio", VolumeDb = -6.0f };
            AddChild(_tiredAudio);
        }

        _stamina = _controller?.GetNodeOrNull<PlayerStamina>(StaminaPath);
        _health = _controller?.GetNodeOrNull<PlayerHealth>(HealthPath);

        if (_camera != null)
        {
            _cameraBasePosition = _camera.Position;
            _cameraBaseRotation = _camera.Rotation;
        }

        if (_flashlight != null)
        {
            _flashlightBasePosition = _flashlight.Position;
            _flashlightBaseRotation = _flashlight.Rotation;
        }

        if (_weaponHolder != null)
        {
            _weaponBasePosition = _weaponHolder.Position;
            _weaponBaseRotation = _weaponHolder.Rotation;
        }

        LoadBreathAudio();
    }

    public override void _Process(double delta)
    {
        if (_controller == null || _camera == null)
        {
            return;
        }

        var deltaSeconds = (float)delta;
        _tiredSoundCooldownRemaining = Mathf.Max(0.0f, _tiredSoundCooldownRemaining - deltaSeconds);
        if (!EnableBodyMotion)
        {
            ReturnToNeutral(deltaSeconds);
            return;
        }

        if (Input.IsActionJustPressed("attack") || Input.IsActionJustPressed("attack_primary"))
        {
            _attackDampenTimer = AttackSwayDampenTime;
        }

        _attackDampenTimer = Mathf.Max(0.0f, _attackDampenTimer - deltaSeconds);

        var state = ResolveState();
        if (state == MovementFeelState.Dead)
        {
            ReturnToNeutral(deltaSeconds);
            StopBreathAudio();
            _previousHorizontalVelocity = Vector2.Zero;
            return;
        }

        var horizontalVelocity = new Vector2(_controller.Velocity.X, _controller.Velocity.Z);
        var horizontalSpeed = horizontalVelocity.Length();
        var maxSpeed = Mathf.Max(0.1f, _controller.SprintSpeed);
        var speedNormalized = Mathf.Clamp(horizontalSpeed / maxSpeed, 0.0f, 1.0f);
        var isMoving = _controller.IsOnFloor() && horizontalSpeed > 0.12f;

        AdvanceGait(state, isMoving, speedNormalized, deltaSeconds);
        UpdateStepImpact(state, isMoving, deltaSeconds);
        UpdateInertia(horizontalVelocity, deltaSeconds);
        UpdateBreathingMotion(deltaSeconds);

        var cameraPosition = _cameraBasePosition;
        var cameraRotation = _cameraBaseRotation;

        if (EnableHeadBob)
        {
            ApplyHeadBob(state, isMoving, ref cameraPosition, ref cameraRotation);
        }

        if (EnableShoulderSway)
        {
            ApplyShoulderSway(state, ref cameraPosition, ref cameraRotation);
        }

        ApplyLean(ref cameraPosition, ref cameraRotation, deltaSeconds);
        ApplyInertiaTilt(ref cameraPosition, ref cameraRotation, horizontalVelocity);
        ApplyBreathing(ref cameraPosition);
        ApplyDamageShake(ref cameraPosition, ref cameraRotation, deltaSeconds);

        _camera.Position = _camera.Position.Lerp(cameraPosition, GetLerpWeight(Smoothing, deltaSeconds));
        _camera.Rotation = _camera.Rotation.Lerp(cameraRotation, GetLerpWeight(Smoothing, deltaSeconds));

        UpdateWeaponAndFlashlight(state, isMoving, horizontalVelocity, deltaSeconds);
        UpdateTiredOneShot(state);
        UpdateBreathingAudio(state, isMoving);

        _lastStaminaPercent = GetStaminaPercent();
        _previousHorizontalVelocity = horizontalVelocity;
    }

    public void PlayDamageShake()
    {
        _damageShakeTimer = DamageShakeDuration;
        _damageKickTimer = DamageShakeDuration;
    }

    private MovementFeelState ResolveState()
    {
        if (_health?.IsDead == true || !_controller!.MovementEnabled)
        {
            return MovementFeelState.Dead;
        }

        var staminaPercent = GetStaminaPercent();
        var horizontalSpeed = new Vector2(_controller.Velocity.X, _controller.Velocity.Z).Length();
        var isMoving = _controller.IsOnFloor() && horizontalSpeed > 0.12f;
        if (_controller.IsCrouching)
        {
            return MovementFeelState.Crouching;
        }

        if (staminaPercent <= LowStaminaThreshold)
        {
            return MovementFeelState.Exhausted;
        }

        if (isMoving && Input.IsActionPressed("sprint"))
        {
            return MovementFeelState.Running;
        }

        return isMoving ? MovementFeelState.Walking : MovementFeelState.Idle;
    }

    private void AdvanceGait(MovementFeelState state, bool isMoving, float speedNormalized, float deltaSeconds)
    {
        if (!isMoving)
        {
            _gaitPhase = Mathf.MoveToward(_gaitPhase, 0.0f, deltaSeconds * WalkStepFrequency);
            return;
        }

        var frequency = state switch
        {
            MovementFeelState.Running => RunStepFrequency,
            MovementFeelState.Crouching => CrouchStepFrequency,
            MovementFeelState.Exhausted => WalkStepFrequency * 0.85f,
            _ => WalkStepFrequency
        };

        _gaitPhase += frequency * Mathf.Max(0.35f, speedNormalized) * deltaSeconds;
    }

    private void UpdateStepImpact(MovementFeelState state, bool isMoving, float deltaSeconds)
    {
        _stepImpactOffset = Mathf.MoveToward(_stepImpactOffset, 0.0f, StepImpactReturnSpeed * deltaSeconds);
        if (!EnableStepImpact || !isMoving)
        {
            _lastStepWave = Mathf.Sin(_gaitPhase * 2.0f);
            return;
        }

        var stepWave = Mathf.Sin(_gaitPhase * 2.0f);
        if (_lastStepWave > 0.0f && stepWave <= 0.0f)
        {
            _stepImpactOffset = state == MovementFeelState.Running ? -RunStepImpact : -WalkStepImpact;
        }

        _lastStepWave = stepWave;
    }

    private void UpdateInertia(Vector2 horizontalVelocity, float deltaSeconds)
    {
        var velocityDelta = horizontalVelocity - _previousHorizontalVelocity;
        var targetSway = new Vector2(
            Mathf.Clamp(-velocityDelta.X * InertiaSwayAmount, -InertiaSwayAmount, InertiaSwayAmount),
            Mathf.Clamp(-velocityDelta.Y * InertiaSwayAmount, -InertiaSwayAmount, InertiaSwayAmount));
        _inertiaSway = _inertiaSway.Lerp(targetSway, GetLerpWeight(InertiaSmooth, deltaSeconds));
    }

    private void UpdateBreathingMotion(float deltaSeconds)
    {
        _breathingPhase += deltaSeconds * BreathingSpeed;
    }

    private void ApplyHeadBob(MovementFeelState state, bool isMoving, ref Vector3 cameraPosition, ref Vector3 cameraRotation)
    {
        if (!isMoving && state != MovementFeelState.Exhausted)
        {
            return;
        }

        var verticalAmount = state switch
        {
            MovementFeelState.Running => RunBobVertical,
            MovementFeelState.Crouching => CrouchBobVertical,
            MovementFeelState.Exhausted => WalkBobVertical * 0.65f,
            _ => WalkBobVertical
        };
        var horizontalAmount = state switch
        {
            MovementFeelState.Running => RunBobHorizontal,
            MovementFeelState.Crouching => CrouchBobHorizontal,
            MovementFeelState.Exhausted => WalkBobHorizontal * 0.65f,
            _ => WalkBobHorizontal
        };
        var rollAmount = state switch
        {
            MovementFeelState.Running => RunRollAmount,
            MovementFeelState.Crouching => CrouchRollAmount,
            MovementFeelState.Exhausted => WalkRollAmount * 0.7f,
            _ => WalkRollAmount
        };

        var vertical = Mathf.Abs(Mathf.Sin(_gaitPhase * 2.0f)) * verticalAmount + _stepImpactOffset;
        var horizontal = Mathf.Sin(_gaitPhase) * horizontalAmount;
        var roll = Mathf.Sin(_gaitPhase) * rollAmount;
        var pitch = state == MovementFeelState.Running ? RunPitchAmount : 0.0f;

        cameraPosition += new Vector3(horizontal, vertical, 0.0f);
        cameraRotation += new Vector3(Mathf.DegToRad(pitch), 0.0f, Mathf.DegToRad(roll));
    }

    private void ApplyShoulderSway(MovementFeelState state, ref Vector3 cameraPosition, ref Vector3 cameraRotation)
    {
        if (state != MovementFeelState.Running)
        {
            return;
        }

        var shoulder = Mathf.Sin(_gaitPhase) * ShoulderSwayRunAmount;
        var roll = Mathf.Sin(_gaitPhase) * ShoulderRollRunAmount;
        cameraPosition.X += shoulder;
        cameraRotation.Z += Mathf.DegToRad(roll);
    }

    private void ApplyLean(ref Vector3 cameraPosition, ref Vector3 cameraRotation, float deltaSeconds)
    {
        var targetLean = 0.0f;
        if (Input.IsActionPressed("lean_left"))
        {
            targetLean -= 1.0f;
        }

        if (Input.IsActionPressed("lean_right"))
        {
            targetLean += 1.0f;
        }

        _currentLean = Mathf.MoveToward(_currentLean, targetLean, LeanSpeed * deltaSeconds);
        cameraPosition.X += _currentLean * LeanOffset;
        cameraRotation.Z += Mathf.DegToRad(-_currentLean * LeanAngle);
    }

    private void ApplyInertiaTilt(ref Vector3 cameraPosition, ref Vector3 cameraRotation, Vector2 horizontalVelocity)
    {
        var delta = horizontalVelocity - _previousHorizontalVelocity;
        var forward2D = new Vector2(-_controller!.GlobalTransform.Basis.Z.X, -_controller.GlobalTransform.Basis.Z.Z).Normalized();
        var right2D = new Vector2(_controller.GlobalTransform.Basis.X.X, _controller.GlobalTransform.Basis.X.Z).Normalized();
        var forwardDelta = delta.Dot(forward2D);
        var sideDelta = delta.Dot(right2D);
        var pitchDegrees = forwardDelta >= 0.0f
            ? Mathf.Clamp(forwardDelta * AccelerationTiltAmount, -AccelerationTiltAmount, AccelerationTiltAmount)
            : Mathf.Clamp(forwardDelta * StopTiltAmount, -StopTiltAmount, StopTiltAmount);

        cameraRotation.X += Mathf.DegToRad(pitchDegrees);
        cameraRotation.Z += Mathf.DegToRad(Mathf.Clamp(-sideDelta * 0.7f, -1.4f, 1.4f));
        cameraPosition.X += _inertiaSway.X;
        cameraPosition.Z += _inertiaSway.Y;
    }

    private void ApplyBreathing(ref Vector3 cameraPosition)
    {
        if (!EnableBreathingMotion)
        {
            return;
        }

        var amount = GetStaminaPercent() <= LowStaminaThreshold ? BreathingTiredAmount : BreathingIdleAmount;
        cameraPosition.Y += Mathf.Sin(_breathingPhase) * amount;
    }

    private void ApplyDamageShake(ref Vector3 cameraPosition, ref Vector3 cameraRotation, float deltaSeconds)
    {
        if (_damageShakeTimer > 0.0f)
        {
            _damageShakeTimer = Mathf.Max(0.0f, _damageShakeTimer - deltaSeconds);
            var normalized = DamageShakeDuration <= 0.0f ? 0.0f : _damageShakeTimer / DamageShakeDuration;
            var amount = DamageShakeAmount * normalized;
            cameraPosition += new Vector3(
                _random.RandfRange(-amount, amount),
                _random.RandfRange(-amount * 0.45f, amount * 0.45f),
                0.0f);
            cameraRotation += new Vector3(
                Mathf.DegToRad(_random.RandfRange(-amount * 10.0f, amount * 10.0f)),
                0.0f,
                Mathf.DegToRad(_random.RandfRange(-amount * 12.0f, amount * 12.0f)));
        }

        if (_damageKickTimer > 0.0f)
        {
            _damageKickTimer = Mathf.Max(0.0f, _damageKickTimer - deltaSeconds);
            var normalized = DamageShakeDuration <= 0.0f ? 0.0f : _damageKickTimer / DamageShakeDuration;
            cameraPosition.X += DamageKickAmount * normalized;
        }
    }

    private void UpdateWeaponAndFlashlight(MovementFeelState state, bool isMoving, Vector2 horizontalVelocity, float deltaSeconds)
    {
        if (!EnableWeaponSway)
        {
            ReturnWeaponAndFlashlightToBase(deltaSeconds);
            return;
        }

        var swayAmount = state switch
        {
            MovementFeelState.Running => WeaponRunSwayAmount,
            MovementFeelState.Crouching => WeaponCrouchSwayAmount,
            MovementFeelState.Exhausted => WeaponWalkSwayAmount * 0.8f,
            MovementFeelState.Walking => WeaponWalkSwayAmount,
            _ => WeaponWalkSwayAmount * 0.25f
        };

        if (_attackDampenTimer > 0.0f)
        {
            swayAmount *= 0.35f;
        }

        var tiredShake = GetStaminaPercent() <= LowStaminaThreshold
            ? Mathf.Sin(_breathingPhase * 4.3f) * TiredHandShakeAmount
            : 0.0f;
        var x = Mathf.Sin(_gaitPhase) * swayAmount + _inertiaSway.X * 1.8f + tiredShake;
        var y = Mathf.Abs(Mathf.Cos(_gaitPhase)) * swayAmount * -0.65f;
        var z = state == MovementFeelState.Running ? -swayAmount * 0.8f : 0.0f;
        var roll = Mathf.Sin(_gaitPhase) * swayAmount * 28.0f;
        var pitch = Mathf.Cos(_gaitPhase * 2.0f) * swayAmount * 18.0f;

        if (_weaponHolder != null)
        {
            var targetPosition = _weaponBasePosition + new Vector3(-x * 1.25f, y, z);
            var targetRotation = _weaponBaseRotation + new Vector3(Mathf.DegToRad(pitch), Mathf.DegToRad(x * 16.0f), Mathf.DegToRad(-roll));
            _weaponHolder.Position = _weaponHolder.Position.Lerp(targetPosition, GetLerpWeight(ShoulderSwaySmooth, deltaSeconds));
            _weaponHolder.Rotation = _weaponHolder.Rotation.Lerp(targetRotation, GetLerpWeight(ShoulderSwaySmooth, deltaSeconds));
        }

        if (_flashlight != null)
        {
            var targetPosition = _flashlightBasePosition + new Vector3(x * 0.65f, y * 0.45f, 0.0f);
            var targetRotation = _flashlightBaseRotation + new Vector3(Mathf.DegToRad(pitch * 0.35f), Mathf.DegToRad(-x * 12.0f), Mathf.DegToRad(roll * 0.35f));
            _flashlight.Position = _flashlight.Position.Lerp(targetPosition, GetLerpWeight(ShoulderSwaySmooth, deltaSeconds));
            _flashlight.Rotation = _flashlight.Rotation.Lerp(targetRotation, GetLerpWeight(ShoulderSwaySmooth, deltaSeconds));
        }
    }

    private void ReturnWeaponAndFlashlightToBase(float deltaSeconds)
    {
        if (_weaponHolder != null)
        {
            _weaponHolder.Position = _weaponHolder.Position.Lerp(_weaponBasePosition, GetLerpWeight(Smoothing, deltaSeconds));
            _weaponHolder.Rotation = _weaponHolder.Rotation.Lerp(_weaponBaseRotation, GetLerpWeight(Smoothing, deltaSeconds));
        }

        if (_flashlight != null)
        {
            _flashlight.Position = _flashlight.Position.Lerp(_flashlightBasePosition, GetLerpWeight(Smoothing, deltaSeconds));
            _flashlight.Rotation = _flashlight.Rotation.Lerp(_flashlightBaseRotation, GetLerpWeight(Smoothing, deltaSeconds));
        }
    }

    private void UpdateBreathingAudio(MovementFeelState state, bool isMoving)
    {
        if (_breathAudio == null)
        {
            return;
        }

        var targetStream = state == MovementFeelState.Exhausted
            ? _heavyBreathStream
            : state == MovementFeelState.Running
                ? _lightBreathStream
                : null;

        if (targetStream == null || (!isMoving && state != MovementFeelState.Exhausted))
        {
            StopBreathAudio();
            return;
        }

        if (_breathAudio.Stream != targetStream)
        {
            _breathAudio.Stream = targetStream;
        }

        _breathAudio.VolumeDb = state == MovementFeelState.Exhausted ? -7.0f : -14.0f;
        if (!_breathAudio.Playing)
        {
            _breathAudio.Play();
        }

        if (state == MovementFeelState.Exhausted && !_lowStaminaMessageShown)
        {
            _lowStaminaMessageShown = true;
            GD.Print("PlayerBodyMotion: stamina baixa, respiracao pesada.");
        }
        else if (state != MovementFeelState.Exhausted)
        {
            _lowStaminaMessageShown = false;
        }
    }

    private void UpdateTiredOneShot(MovementFeelState state)
    {
        if (_tiredAudio == null || _tiredBreathStream == null || _health?.IsDead == true)
        {
            return;
        }

        var staminaPercent = GetStaminaPercent();
        var reachedZero = _lastStaminaPercent > 0.0f && staminaPercent <= 0.0f;
        var triedSprintWithoutStamina = staminaPercent <= 0.0f && Input.IsActionPressed("sprint");
        if (state == MovementFeelState.Dead || (!reachedZero && !triedSprintWithoutStamina))
        {
            return;
        }

        if (_tiredSoundCooldownRemaining > 0.0f || _tiredAudio.Playing)
        {
            return;
        }

        _tiredAudio.Stream = _tiredBreathStream;
        _tiredAudio.VolumeDb = -6.0f;
        _tiredAudio.Play();
        _tiredSoundCooldownRemaining = TiredSoundCooldown;
    }

    private void ReturnToNeutral(float deltaSeconds)
    {
        if (_camera != null)
        {
            _camera.Position = _camera.Position.Lerp(_cameraBasePosition, GetLerpWeight(Smoothing, deltaSeconds));
            _camera.Rotation = _camera.Rotation.Lerp(_cameraBaseRotation, GetLerpWeight(Smoothing, deltaSeconds));
        }

        ReturnWeaponAndFlashlightToBase(deltaSeconds);
        _currentLean = Mathf.MoveToward(_currentLean, 0.0f, LeanSpeed * deltaSeconds);
        _stepImpactOffset = Mathf.MoveToward(_stepImpactOffset, 0.0f, StepImpactReturnSpeed * deltaSeconds);
    }

    private float GetStaminaPercent()
    {
        if (_stamina == null || _stamina.MaxStamina <= 0.0f)
        {
            return 100.0f;
        }

        return (_stamina.Current / _stamina.MaxStamina) * 100.0f;
    }

    private void LoadBreathAudio()
    {
        _lightBreathStream = AudioResourceLoader.TryLoad(BreathLightAudioPath, loop: true);
        _heavyBreathStream = AudioResourceLoader.TryLoad(BreathHeavyAudioPath, loop: true);
        _tiredBreathStream = AudioResourceLoader.TryLoad(TiredAudioPath);

        if (_lightBreathStream == null && _heavyBreathStream == null && _tiredBreathStream == null && !_missingAudioWarned)
        {
            _missingAudioWarned = true;
            GD.Print("PlayerBodyMotion: audios de respiracao ausentes; TODO adicionar breath_light/heavy/tired.");
        }
    }

    private void StopBreathAudio()
    {
        if (_breathAudio?.Playing == true)
        {
            _breathAudio.Stop();
        }

        if (_tiredAudio?.Playing == true)
        {
            _tiredAudio.Stop();
        }
    }

    private static float GetLerpWeight(float speed, float deltaSeconds)
    {
        return Mathf.Clamp(deltaSeconds * speed, 0.0f, 1.0f);
    }

    private static void EnsureInputMap()
    {
        EnsureKeyAction("lean_left", Key.Q);
        EnsureKeyAction("lean_right", Key.R);
    }

    private static void EnsureKeyAction(string actionName, Key key)
    {
        if (!InputMap.HasAction(actionName))
        {
            InputMap.AddAction(actionName);
        }

        foreach (var existingEvent in InputMap.ActionGetEvents(actionName))
        {
            if (existingEvent is InputEventKey existingKey &&
                (existingKey.Keycode == key || existingKey.PhysicalKeycode == key))
            {
                return;
            }
        }

        InputMap.ActionAddEvent(actionName, new InputEventKey
        {
            Keycode = key,
            PhysicalKeycode = key
        });
    }
}
