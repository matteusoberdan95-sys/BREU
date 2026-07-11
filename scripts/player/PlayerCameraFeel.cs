namespace BREU.Scripts.Player;

public partial class PlayerCameraFeel : Node
{
    [Export] public NodePath CameraPath { get; set; } = "CameraPivot/Camera3D";
    [Export] public NodePath FlashlightPath { get; set; } = "CameraPivot/Flashlight";
    [Export] public NodePath BreathAudioPath { get; set; } = "BreathAudio";
    [Export] public NodePath StaminaPath { get; set; } = "PlayerStamina";
    [Export] public NodePath HealthPath { get; set; } = "PlayerHealth";

    [Export] public float WalkBobAmount { get; set; } = 0.030f;
    [Export] public float WalkBobSpeed { get; set; } = 7.5f;
    [Export] public float RunBobAmount { get; set; } = 0.060f;
    [Export] public float RunBobSpeed { get; set; } = 11.0f;
    [Export] public float CrouchBobAmount { get; set; } = 0.015f;
    [Export] public float CrouchBobSpeed { get; set; } = 4.5f;
    [Export] public float DamageShakeAmount { get; set; } = 0.12f;
    [Export] public float DamageShakeDuration { get; set; } = 0.22f;
    [Export] public float LeanAngle { get; set; } = 8.0f;
    [Export] public float LeanSpeed { get; set; } = 8.0f;
    [Export] public float LeanOffset { get; set; } = 0.18f;
    [Export] public float FlashlightSwayAmount { get; set; } = 0.035f;
    [Export] public float FlashlightSwaySpeed { get; set; } = 6.0f;
    [Export] public float LowStaminaThreshold { get; set; } = 35.0f;
    [Export] public string BreathLightAudioPath { get; set; } = "res://assets/audio/sfx/player/breath_light_01.ogg";
    [Export] public string BreathHeavyAudioPath { get; set; } = "res://assets/audio/sfx/player/breath_heavy_01.ogg";
    [Export] public string TiredAudioPath { get; set; } = "res://assets/audio/sfx/player/player_tired_01.ogg";

    private PlayerController? _controller;
    private PlayerStamina? _stamina;
    private PlayerHealth? _health;
    private Camera3D? _camera;
    private Node3D? _flashlight;
    private AudioStreamPlayer? _breathAudio;
    private AudioStream? _lightBreathStream;
    private AudioStream? _heavyBreathStream;
    private AudioStream? _tiredBreathStream;
    private Vector3 _cameraBasePosition;
    private Vector3 _cameraBaseRotation;
    private Vector3 _flashlightBasePosition;
    private Vector3 _flashlightBaseRotation;
    private readonly RandomNumberGenerator _random = new();
    private float _bobTimer;
    private float _currentLean;
    private float _damageShakeTimer;
    private bool _missingAudioWarned;
    private bool _lowStaminaMessageShown;

    public override void _Ready()
    {
        EnsureInputMap();
        _random.Randomize();

        _controller = GetParentOrNull<PlayerController>();
        _camera = _controller?.GetNodeOrNull<Camera3D>(CameraPath);
        _flashlight = _controller?.GetNodeOrNull<Node3D>(FlashlightPath);
        _breathAudio = _controller?.GetNodeOrNull<AudioStreamPlayer>(BreathAudioPath);
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
        else
        {
            GD.Print("PlayerCameraFeel: FlashlightPath nao encontrado; sway da lanterna desativado.");
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
        var isDead = _health?.IsDead == true || !_controller.MovementEnabled;
        if (isDead)
        {
            ReturnToNeutral(deltaSeconds);
            StopBreathAudio();
            return;
        }

        var horizontalSpeed = new Vector2(_controller.Velocity.X, _controller.Velocity.Z).Length();
        var isMoving = _controller.IsOnFloor() && horizontalSpeed > 0.12f;
        var staminaPercent = GetStaminaPercent();
        var isLowStamina = staminaPercent <= LowStaminaThreshold;
        var isSprinting = isMoving && Input.IsActionPressed("sprint") && !_controller.IsCrouching && staminaPercent > 0.0f;
        var isCrouching = _controller.IsCrouching;

        var bobAmount = isCrouching ? CrouchBobAmount : isSprinting ? RunBobAmount : WalkBobAmount;
        var bobSpeed = isCrouching ? CrouchBobSpeed : isSprinting ? RunBobSpeed : WalkBobSpeed;
        if (isLowStamina && !isCrouching)
        {
            bobAmount *= 1.18f;
        }

        var bobOffset = CalculateBobOffset(isMoving, bobAmount, bobSpeed, deltaSeconds);
        var leanOffset = CalculateLeanOffset(deltaSeconds);
        var shakeOffset = CalculateDamageShake(deltaSeconds);
        var shakeRotation = shakeOffset * 0.45f;

        var targetPosition = _cameraBasePosition + bobOffset + new Vector3(leanOffset.X, 0.0f, 0.0f) + shakeOffset;
        var targetRotation = _cameraBaseRotation + new Vector3(shakeRotation.Y, 0.0f, leanOffset.Z + shakeRotation.X);
        _camera.Position = _camera.Position.Lerp(targetPosition, Mathf.Clamp(deltaSeconds * 14.0f, 0.0f, 1.0f));
        _camera.Rotation = _camera.Rotation.Lerp(targetRotation, Mathf.Clamp(deltaSeconds * 12.0f, 0.0f, 1.0f));

        UpdateFlashlightSway(isMoving, isSprinting, isLowStamina, deltaSeconds);
        UpdateBreathing(isMoving, isSprinting, isLowStamina, staminaPercent);
    }

    public void PlayDamageShake()
    {
        _damageShakeTimer = DamageShakeDuration;
    }

    private Vector3 CalculateBobOffset(bool isMoving, float amount, float speed, float deltaSeconds)
    {
        if (!isMoving)
        {
            _bobTimer = Mathf.MoveToward(_bobTimer, 0.0f, deltaSeconds * speed);
            return Vector3.Zero;
        }

        _bobTimer += deltaSeconds * speed;
        var horizontal = Mathf.Cos(_bobTimer * 0.5f) * amount * 0.45f;
        var vertical = Mathf.Sin(_bobTimer) * amount;
        return new Vector3(horizontal, vertical, 0.0f);
    }

    private Vector3 CalculateLeanOffset(float deltaSeconds)
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
        return new Vector3(_currentLean * LeanOffset, 0.0f, Mathf.DegToRad(-_currentLean * LeanAngle));
    }

    private Vector3 CalculateDamageShake(float deltaSeconds)
    {
        if (_damageShakeTimer <= 0.0f)
        {
            return Vector3.Zero;
        }

        _damageShakeTimer = Mathf.Max(0.0f, _damageShakeTimer - deltaSeconds);
        var normalized = DamageShakeDuration <= 0.0f ? 0.0f : _damageShakeTimer / DamageShakeDuration;
        var amount = DamageShakeAmount * normalized;
        return new Vector3(
            _random.RandfRange(-amount, amount),
            _random.RandfRange(-amount * 0.55f, amount * 0.55f),
            0.0f);
    }

    private void UpdateFlashlightSway(bool isMoving, bool isSprinting, bool isLowStamina, float deltaSeconds)
    {
        if (_flashlight == null)
        {
            return;
        }

        var swayScale = isMoving ? isSprinting ? 1.65f : 1.0f : 0.25f;
        if (isLowStamina)
        {
            swayScale += 0.35f;
        }

        var swayX = Mathf.Sin(_bobTimer * FlashlightSwaySpeed * 0.18f) * FlashlightSwayAmount * swayScale;
        var swayY = Mathf.Cos(_bobTimer * FlashlightSwaySpeed * 0.14f) * FlashlightSwayAmount * 0.55f * swayScale;
        var targetPosition = _flashlightBasePosition + new Vector3(swayX, swayY, 0.0f);
        var targetRotation = _flashlightBaseRotation + new Vector3(swayY * 0.6f, -swayX * 0.7f, swayX * 0.35f);

        _flashlight.Position = _flashlight.Position.Lerp(targetPosition, Mathf.Clamp(deltaSeconds * 8.0f, 0.0f, 1.0f));
        _flashlight.Rotation = _flashlight.Rotation.Lerp(targetRotation, Mathf.Clamp(deltaSeconds * 8.0f, 0.0f, 1.0f));
    }

    private void UpdateBreathing(bool isMoving, bool isSprinting, bool isLowStamina, float staminaPercent)
    {
        if (_breathAudio == null)
        {
            return;
        }

        var targetStream = isLowStamina || staminaPercent <= 0.0f
            ? _heavyBreathStream ?? _tiredBreathStream
            : isSprinting
                ? _lightBreathStream
                : null;

        if (targetStream == null || (!isMoving && !isLowStamina))
        {
            StopBreathAudio();
            return;
        }

        if (_breathAudio.Stream != targetStream)
        {
            _breathAudio.Stream = targetStream;
        }

        _breathAudio.VolumeDb = isLowStamina ? -7.0f : -14.0f;
        if (!_breathAudio.Playing)
        {
            _breathAudio.Play();
        }

        if (isLowStamina && !_lowStaminaMessageShown)
        {
            _lowStaminaMessageShown = true;
            GD.Print("PlayerCameraFeel: stamina baixa, respiracao pesada.");
        }
        else if (!isLowStamina)
        {
            _lowStaminaMessageShown = false;
        }
    }

    private void StopBreathAudio()
    {
        if (_breathAudio?.Playing == true)
        {
            _breathAudio.Stop();
        }
    }

    private void ReturnToNeutral(float deltaSeconds)
    {
        if (_camera != null)
        {
            _camera.Position = _camera.Position.Lerp(_cameraBasePosition, Mathf.Clamp(deltaSeconds * 10.0f, 0.0f, 1.0f));
            _camera.Rotation = _camera.Rotation.Lerp(_cameraBaseRotation, Mathf.Clamp(deltaSeconds * 10.0f, 0.0f, 1.0f));
        }

        if (_flashlight != null)
        {
            _flashlight.Position = _flashlight.Position.Lerp(_flashlightBasePosition, Mathf.Clamp(deltaSeconds * 8.0f, 0.0f, 1.0f));
            _flashlight.Rotation = _flashlight.Rotation.Lerp(_flashlightBaseRotation, Mathf.Clamp(deltaSeconds * 8.0f, 0.0f, 1.0f));
        }

        _currentLean = Mathf.MoveToward(_currentLean, 0.0f, LeanSpeed * deltaSeconds);
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
        _tiredBreathStream = AudioResourceLoader.TryLoad(TiredAudioPath, loop: true);

        if (_lightBreathStream == null && _heavyBreathStream == null && _tiredBreathStream == null && !_missingAudioWarned)
        {
            _missingAudioWarned = true;
            GD.Print("PlayerCameraFeel: audios de respiracao ausentes; TODO adicionar breath_light/heavy/tired.");
        }
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
