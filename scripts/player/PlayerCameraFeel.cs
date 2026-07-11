namespace BREU.Scripts.Player;

/// <summary>
/// Horror body camera feel — bob, sway, inertia, sprint shake and landing response.
/// Applies only to BodyMotionPivot local transform. Does not move CharacterBody3D.
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

    // --- Idle breath ---
    [Export] public float IdleBreathAmountY { get; set; } = 0.012f;
    [Export] public float IdleBreathAmountX { get; set; } = 0.006f;
    [Export] public float IdleBreathFrequency { get; set; } = 1.1f;

    // --- Walk bob ---
    [Export] public float WalkBobVertical { get; set; } = 0.045f;
    [Export] public float WalkBobHorizontal { get; set; } = 0.030f;
    [Export] public float WalkBobRollDegrees { get; set; } = 1.2f;
    [Export] public float WalkBobFrequency { get; set; } = 7.5f;

    // --- Sprint bob ---
    [Export] public float SprintBobVertical { get; set; } = 0.095f;
    [Export] public float SprintBobHorizontal { get; set; } = 0.075f;
    [Export] public float SprintBobRollDegrees { get; set; } = 3.2f;
    [Export] public float SprintBobPitchDegrees { get; set; } = 1.8f;
    [Export] public float SprintBobFrequency { get; set; } = 11.5f;

    // --- Crouch bob ---
    [Export] public float CrouchBobVertical { get; set; } = 0.022f;
    [Export] public float CrouchBobHorizontal { get; set; } = 0.018f;
    [Export] public float CrouchBobRollDegrees { get; set; } = 0.8f;
    [Export] public float CrouchBobFrequency { get; set; } = 5.2f;

    // --- Shoulder / torso ---
    [Export] public float WalkShoulderSway { get; set; } = 0.025f;
    [Export] public float SprintShoulderSway { get; set; } = 0.070f;
    [Export] public float WalkTorsoRollDegrees { get; set; } = 1.0f;
    [Export] public float SprintTorsoRollDegrees { get; set; } = 3.0f;
    [Export] public float SprintForwardPitchDegrees { get; set; } = 2.0f;

    // --- Inertia ---
    [Export] public float AccelerationPitchDegrees { get; set; } = 1.2f;
    [Export] public float StopRecoveryPitchDegrees { get; set; } = -0.8f;
    [Export] public float StrafeTiltDegrees { get; set; } = 2.0f;
    [Export] public float SprintStrafeTiltDegrees { get; set; } = 3.5f;
    [Export] public float InertiaSmooth { get; set; } = 8.0f;

    // --- Sprint horror ---
    [Export] public float SprintMicroShakeAmount { get; set; } = 0.010f;
    [Export] public float SprintMicroShakeFrequency { get; set; } = 23.0f;
    [Export] public float SprintPanicMultiplier { get; set; } = 1.0f;

    // --- Landing ---
    [Export] public float LandingBobAmount { get; set; } = 0.06f;
    [Export] public float LandingRecoverySpeed { get; set; } = 10.0f;

    [Export] public float LookBackBobReduction { get; set; } = 0.15f;
    [Export] public float EffectBlendSpeed { get; set; } = 12.0f;

    private PlayerController? _controller;
    private PlayerLookBack? _lookBack;
    private float _gaitPhase;
    private float _idlePhase;
    private float _shakePhase;
    private float _inertiaPitch;
    private float _landingKick;
    private float _previousSpeed;

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
        var speedNorm = Mathf.Clamp(speed / _controller.WalkSpeed, 0.0f, 2.2f);
        var isMoving = _controller.IsMovingHorizontally;
        var isSprinting = _controller.IsSprinting;
        var isCrouching = _controller.IsCrouching;
        var onFloor = _controller.IsOnGround;
        var strafe = _controller.MoveInput.X;
        var forward = _controller.MoveInput.Y;
        var lookBackBlend = _lookBack?.LookBackBlend ?? 0.0f;

        UpdateInertia(dt, speed, isMoving, forward);
        UpdateLanding(dt);

        var bobScale = 1.0f - lookBackBlend * LookBackBobReduction;
        var panic = isSprinting ? SprintPanicMultiplier : 1.0f;

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
            _gaitPhase = Mathf.Lerp(_gaitPhase, 0.0f, dt * EffectBlendSpeed);
        }
        else if (isCrouching)
        {
            AdvanceGait(dt, CrouchBobFrequency, speedNorm);
            targetOffset = ComputeBob(CrouchBobVertical, CrouchBobHorizontal, WalkShoulderSway * 0.5f, bobScale);
            targetPitch = _inertiaPitch;
            targetRoll = Mathf.Sin(_gaitPhase) * Mathf.DegToRad(CrouchBobRollDegrees);
        }
        else if (isSprinting)
        {
            AdvanceGait(dt, SprintBobFrequency, speedNorm);
            _shakePhase += dt * SprintMicroShakeFrequency;
            var microShake = new Vector3(
                (Mathf.Sin(_shakePhase * 1.7f) + Mathf.Sin(_shakePhase * 2.3f)) * 0.5f * SprintMicroShakeAmount,
                (Mathf.Sin(_shakePhase * 2.1f) + Mathf.Cos(_shakePhase * 1.9f)) * 0.5f * SprintMicroShakeAmount,
                0.0f) * panic;

            targetOffset = (ComputeBob(
                SprintBobVertical,
                SprintBobHorizontal,
                SprintShoulderSway,
                bobScale * panic) + microShake);
            targetPitch = _inertiaPitch + Mathf.DegToRad(SprintBobPitchDegrees + SprintForwardPitchDegrees) * panic;
            targetRoll = Mathf.Sin(_gaitPhase) * Mathf.DegToRad(SprintBobRollDegrees + SprintTorsoRollDegrees) * panic;
        }
        else
        {
            AdvanceGait(dt, WalkBobFrequency, speedNorm);
            targetOffset = ComputeBob(WalkBobVertical, WalkBobHorizontal, WalkShoulderSway, bobScale);
            targetPitch = _inertiaPitch;
            targetRoll = Mathf.Sin(_gaitPhase) * Mathf.DegToRad(WalkBobRollDegrees + WalkTorsoRollDegrees);
        }

        var strafeTilt = -strafe * Mathf.DegToRad(isSprinting ? SprintStrafeTiltDegrees : StrafeTiltDegrees);
        targetRoll += strafeTilt;

        if (_landingKick > 0.001f)
        {
            targetOffset.Y -= _landingKick;
        }

        var smooth = dt * EffectBlendSpeed;
        Position = Position.Lerp(targetOffset, smooth);
        var currentPitch = Rotation.X;
        var blendedPitch = Mathf.LerpAngle(currentPitch, targetPitch, smooth);
        Rotation = new Vector3(blendedPitch, 0.0f, Mathf.LerpAngle(Rotation.Z, targetRoll, smooth));

        _previousSpeed = speed;
    }

    private void AdvanceGait(float dt, float frequency, float speedNorm)
    {
        _gaitPhase += dt * frequency * Mathf.Max(0.4f, speedNorm);
    }

    private Vector3 ComputeBob(float vertical, float horizontal, float shoulder, float scale)
    {
        var sin = Mathf.Sin(_gaitPhase);
        var cos = Mathf.Cos(_gaitPhase * 0.5f);
        return new Vector3(
            cos * horizontal * scale + sin * shoulder * scale * 0.6f,
            sin * vertical * scale,
            0.0f);
    }

    private void UpdateInertia(float dt, float speed, bool isMoving, float forwardInput)
    {
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
            targetPitch = Mathf.DegToRad(AccelerationPitchDegrees * 0.35f);
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
        switch (feelPreset)
        {
            case FeelPreset.Subtle:
                // Preset A — Resident Evil 7: heavy, controlled.
                IdleBreathAmountY = 0.008f;
                IdleBreathAmountX = 0.004f;
                WalkBobVertical = 0.032f;
                WalkBobHorizontal = 0.018f;
                WalkBobRollDegrees = 0.8f;
                SprintBobVertical = 0.055f;
                SprintBobHorizontal = 0.040f;
                SprintBobRollDegrees = 1.8f;
                SprintShoulderSway = 0.040f;
                SprintMicroShakeAmount = 0.004f;
                SprintForwardPitchDegrees = 1.2f;
                break;

            case FeelPreset.OutlastInspired:
                // Preset B — Outlast: nervous sprint, strong shoulders.
                IdleBreathAmountY = 0.014f;
                WalkBobVertical = 0.040f;
                SprintBobVertical = 0.110f;
                SprintBobHorizontal = 0.090f;
                SprintBobRollDegrees = 4.0f;
                SprintShoulderSway = 0.085f;
                SprintTorsoRollDegrees = 3.8f;
                SprintForwardPitchDegrees = 2.8f;
                SprintMicroShakeAmount = 0.014f;
                SprintPanicMultiplier = 1.15f;
                break;

            case FeelPreset.BreuDefault:
                // Preset C — BREU default: weighted walk, desperate sprint, playable.
                break;
        }
    }
}
