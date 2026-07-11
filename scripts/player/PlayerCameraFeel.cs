namespace BREU.Scripts.Player;

/// <summary>
/// Horror body camera feel — velocity-based bob, sway and inertia on BodyMotionPivot only.
/// </summary>
public partial class PlayerCameraFeel : Node3D
{
    public enum FeelPreset
    {
        Subtle,
        OutlastInspired,
        BreuDefault
    }

    [Export] public FeelPreset Preset { get; set; } = FeelPreset.BreuDefault;
    [Export] public NodePath ControllerPath { get; set; } = new NodePath("../..");
    [Export] public NodePath LookBackPath { get; set; } = new NodePath("LeanPivot/LookBackPivot");

    [Export] public float IdleBreathAmountY { get; set; } = 0.006f;
    [Export] public float IdleBreathAmountX { get; set; } = 0.003f;
    [Export] public float IdleBreathFrequency { get; set; } = 0.85f;

    [Export] public float WalkBobVertical { get; set; } = 0.030f;
    [Export] public float WalkBobHorizontal { get; set; } = 0.018f;
    [Export] public float WalkBobRollDegrees { get; set; } = 0.65f;

    [Export] public float SprintBobVertical { get; set; } = 0.050f;
    [Export] public float SprintBobHorizontal { get; set; } = 0.028f;
    [Export] public float SprintBobRollDegrees { get; set; } = 1.15f;
    [Export] public float SprintBobPitchDegrees { get; set; } = 0.75f;

    [Export] public float CrouchBobVertical { get; set; } = 0.015f;
    [Export] public float CrouchBobHorizontal { get; set; } = 0.010f;
    [Export] public float CrouchBobRollDegrees { get; set; } = 0.35f;

    [Export] public float WalkStepCycleMultiplier { get; set; } = 1.45f;
    [Export] public float SprintStepCycleMultiplier { get; set; } = 1.65f;
    [Export] public float CrouchStepCycleMultiplier { get; set; } = 1.10f;

    [Export] public float WalkShoulderSway { get; set; } = 0.018f;
    [Export] public float SprintShoulderSway { get; set; } = 0.032f;
    [Export] public float WalkTorsoRollDegrees { get; set; } = 0.5f;
    [Export] public float SprintTorsoRollDegrees { get; set; } = 0.85f;
    [Export] public float SprintForwardPitchDegrees { get; set; } = 0.5f;

    [Export] public float AccelerationPitchDegrees { get; set; } = 0.8f;
    [Export] public float StopRecoveryPitchDegrees { get; set; } = -0.5f;
    [Export] public float StrafeTiltDegrees { get; set; } = 1.2f;
    [Export] public float SprintStrafeTiltDegrees { get; set; } = 1.8f;
    [Export] public float InertiaSmooth { get; set; } = 7.5f;
    [Export] public float CameraReturnSmooth { get; set; } = 9.0f;

    [Export] public float SprintMicroShakeAmount { get; set; } = 0.0f;
    [Export] public float SprintMicroShakeFrequency { get; set; } = 0.0f;

    [Export] public float LandingBobAmount { get; set; } = 0.06f;
    [Export] public float LandingRecoverySpeed { get; set; } = 10.0f;
    [Export] public float LookBackBobReduction { get; set; } = 0.15f;

    private PlayerController? _controller;
    private PlayerLookBack? _lookBack;
    private float _gaitPhase;
    private float _idlePhase;
    private float _inertiaPitch;
    private float _landingKick;
    private float _previousSpeed;
    private Vector3 _currentOffset;
    private float _currentPitch;
    private float _currentRoll;

    public override void _Ready()
    {
        ApplyPreset(Preset);
        _controller = GetNodeOrNull<PlayerController>(ControllerPath);
        _lookBack = GetNodeOrNull<PlayerLookBack>(LookBackPath);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_controller == null)
        {
            return;
        }

        var dt = (float)delta;
        var speed = _controller.HorizontalSpeed;
        var isMoving = _controller.IsMovingHorizontally;
        var isSprinting = _controller.IsSprinting;
        var isCrouching = _controller.IsCrouching;
        var onFloor = _controller.IsOnGround;
        var strafe = _controller.MoveInput.X;
        var forward = _controller.MoveInput.Y;
        var lookBackBlend = _lookBack?.LookBackBlend ?? 0.0f;
        var lookBackActive = lookBackBlend > 0.05f;

        UpdateInertia(dt, speed, isMoving, forward, lookBackActive);
        UpdateLanding(dt);

        var bobScale = 1.0f - lookBackBlend * LookBackBobReduction;

        Vector3 targetOffset;
        float targetPitch;
        float targetRoll;

        if (!isMoving || !onFloor)
        {
            _idlePhase += dt * IdleBreathFrequency;
            targetOffset = new Vector3(
                Mathf.Sin(_idlePhase) * IdleBreathAmountX,
                Mathf.Sin(_idlePhase * 1.65f) * IdleBreathAmountY,
                0.0f);
            targetPitch = _inertiaPitch;
            targetRoll = 0.0f;
            _gaitPhase = Mathf.Lerp(_gaitPhase, 0.0f, dt * CameraReturnSmooth);
        }
        else if (isCrouching)
        {
            AdvanceGait(dt, speed, CrouchStepCycleMultiplier);
            ComputeBobTargets(
                CrouchBobVertical, CrouchBobHorizontal, CrouchBobRollDegrees,
                WalkShoulderSway * 0.4f, WalkTorsoRollDegrees * 0.5f, bobScale,
                out targetOffset, out targetPitch, out targetRoll);
            targetPitch += _inertiaPitch;
        }
        else if (isSprinting)
        {
            AdvanceGait(dt, speed, SprintStepCycleMultiplier);
            ComputeBobTargets(
                SprintBobVertical, SprintBobHorizontal, SprintBobRollDegrees,
                SprintShoulderSway, SprintTorsoRollDegrees, bobScale,
                out targetOffset, out targetPitch, out targetRoll);

            if (!lookBackActive)
            {
                targetPitch += _inertiaPitch
                    + Mathf.DegToRad(SprintBobPitchDegrees + SprintForwardPitchDegrees);
            }
            else
            {
                targetPitch += _inertiaPitch * 0.25f;
            }
        }
        else
        {
            AdvanceGait(dt, speed, WalkStepCycleMultiplier);
            ComputeBobTargets(
                WalkBobVertical, WalkBobHorizontal, WalkBobRollDegrees,
                WalkShoulderSway, WalkTorsoRollDegrees, bobScale,
                out targetOffset, out targetPitch, out targetRoll);
            targetPitch += _inertiaPitch;
        }

        var strafeTilt = -strafe * Mathf.DegToRad(isSprinting ? SprintStrafeTiltDegrees : StrafeTiltDegrees);
        targetRoll += strafeTilt;

        if (_landingKick > 0.001f)
        {
            targetOffset.Y -= _landingKick;
        }

        var smooth = dt * (isMoving ? CameraReturnSmooth : CameraReturnSmooth * 0.85f);
        _currentOffset = _currentOffset.Lerp(targetOffset, smooth);
        _currentPitch = Mathf.LerpAngle(_currentPitch, targetPitch, smooth);
        _currentRoll = Mathf.LerpAngle(_currentRoll, targetRoll, smooth);

        Position = _currentOffset;
        Rotation = new Vector3(_currentPitch, 0.0f, _currentRoll);

        _previousSpeed = speed;
    }

    private void AdvanceGait(float dt, float horizontalSpeed, float stepMultiplier)
    {
        if (horizontalSpeed < 0.05f)
        {
            return;
        }

        _gaitPhase += dt * horizontalSpeed * stepMultiplier;
    }

    private void ComputeBobTargets(
        float vertical,
        float horizontal,
        float rollDegrees,
        float shoulder,
        float torsoRollDegrees,
        float scale,
        out Vector3 offset,
        out float pitch,
        out float roll)
    {
        // Footfall-style vertical (smooth rise/fall) + slower lateral sway (not metronome).
        var stepSin = Mathf.Sin(_gaitPhase);
        var lateralPhase = _gaitPhase * 0.5f;
        var lateralSin = Mathf.Sin(lateralPhase);

        var verticalBob = Mathf.Max(0.0f, stepSin) * vertical * scale;
        var horizontalBob = lateralSin * horizontal * scale * 0.65f;
        var shoulderBob = lateralSin * shoulder * scale * 0.5f;

        offset = new Vector3(horizontalBob + shoulderBob, verticalBob, 0.0f);
        pitch = 0.0f;
        roll = lateralSin * Mathf.DegToRad(rollDegrees + torsoRollDegrees) * scale;
    }

    private void UpdateInertia(float dt, float speed, bool isMoving, float forwardInput, bool lookBackActive)
    {
        if (lookBackActive)
        {
            _inertiaPitch = Mathf.LerpAngle(_inertiaPitch, 0.0f, dt * InertiaSmooth);
            return;
        }

        var speedDelta = speed - _previousSpeed;
        var targetPitch = 0.0f;

        if (isMoving && forwardInput > 0.1f && speedDelta > 0.05f)
        {
            targetPitch = Mathf.DegToRad(AccelerationPitchDegrees);
        }
        else if (!isMoving && _previousSpeed > 0.5f)
        {
            targetPitch = Mathf.DegToRad(StopRecoveryPitchDegrees);
        }
        else if (isMoving && forwardInput < -0.1f)
        {
            targetPitch = Mathf.DegToRad(AccelerationPitchDegrees * 0.3f);
        }

        _inertiaPitch = Mathf.LerpAngle(_inertiaPitch, targetPitch, dt * InertiaSmooth);
    }

    private void UpdateLanding(float dt)
    {
        var impact = _controller?.ConsumeLandingImpact() ?? 0.0f;
        if (impact > 0.01f)
        {
            _landingKick = LandingBobAmount * impact;
        }

        _landingKick = Mathf.MoveToward(_landingKick, 0.0f, dt * LandingRecoverySpeed * LandingBobAmount);
    }

    private void ApplyPreset(FeelPreset feelPreset)
    {
        if (feelPreset != FeelPreset.BreuDefault)
        {
            // Non-default presets keep legacy tuning for future comparison.
            return;
        }

        // Preset C — BREU Default (Sprint 02.2 hotfix): controlled horror, human gait.
        IdleBreathAmountY = 0.006f;
        IdleBreathAmountX = 0.003f;
        IdleBreathFrequency = 0.85f;
        WalkBobVertical = 0.030f;
        WalkBobHorizontal = 0.018f;
        WalkBobRollDegrees = 0.65f;
        SprintBobVertical = 0.050f;
        SprintBobHorizontal = 0.028f;
        SprintBobRollDegrees = 1.15f;
        SprintBobPitchDegrees = 0.75f;
        CrouchBobVertical = 0.015f;
        CrouchBobHorizontal = 0.010f;
        CrouchBobRollDegrees = 0.35f;
        WalkStepCycleMultiplier = 1.45f;
        SprintStepCycleMultiplier = 1.65f;
        CrouchStepCycleMultiplier = 1.10f;
        WalkShoulderSway = 0.018f;
        SprintShoulderSway = 0.032f;
        SprintForwardPitchDegrees = 0.5f;
        StrafeTiltDegrees = 1.2f;
        SprintStrafeTiltDegrees = 1.8f;
        InertiaSmooth = 7.5f;
        CameraReturnSmooth = 9.0f;
        SprintMicroShakeAmount = 0.0f;
        SprintMicroShakeFrequency = 0.0f;
    }
}
