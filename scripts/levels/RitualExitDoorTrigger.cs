namespace BREU.Scripts.Levels;

/// <summary>
/// Porta de saida bloqueada da Sala dos Santos Secos.
/// </summary>
public partial class RitualExitDoorTrigger : Area3D, IInteractable
{
    [Export] public string InteractionText { get; set; } = "Abrir porta";
    [Export] public string LockedWithoutKeyMessage { get; set; } = "Esta trancada.";
    [Export] public string LockedWithKeyMessage { get; set; } = "A chave gira, mas alguma coisa segura a porta por dentro.";
    [Export] public string DoorLockedAudioPath { get; set; } = "res://assets/audio/sfx/doors/door_locked_01.ogg";
    [Export] public string DoorLockedFallbackPath { get; set; } = "res://assets/audio/sfx/doors/door_locked_rattle.ogg";
    [Export] public string DoorCreakAudioPath { get; set; } = "res://assets/audio/sfx/doors/door_creak_01.ogg";
    [Export] public string DoorCreakFallbackPath { get; set; } = "res://assets/audio/sfx/doors/door_open_old_wood.ogg";

    private AudioStreamPlayer3D? _doorAudio;
    private bool _recentBodyMessage;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        _doorAudio = GetNodeOrNull<AudioStreamPlayer3D>("DoorAudio");
        if (_doorAudio == null)
        {
            _doorAudio = new AudioStreamPlayer3D
            {
                Name = "DoorAudio",
                VolumeDb = -8.0f,
                MaxDistance = 10.0f
            };
            AddChild(_doorAudio);
        }
    }

    public string GetInteractionText()
    {
        return InteractionText;
    }

    public void Interact(PlayerController player)
    {
        ShowLockedMessage();
    }

    private async void OnBodyEntered(Node3D body)
    {
        if (_recentBodyMessage || !body.IsInGroup("player"))
        {
            return;
        }

        _recentBodyMessage = true;
        ShowLockedMessage();
        await ToSignal(GetTree().CreateTimer(2.5f), SceneTreeTimer.SignalName.Timeout);
        _recentBodyMessage = false;
    }

    private void ShowLockedMessage()
    {
        var hasKey = GetNodeOrNull<GameSession>("/root/GameSession")?.HasOldKey == true;
        var message = hasKey ? LockedWithKeyMessage : LockedWithoutKeyMessage;

        if (GetTree().GetFirstNodeInGroup("hud") is HUDController hud)
        {
            hud.ShowTemporaryMessage(message, 3.5f);
        }

        PlayDoorAudio(hasKey);
    }

    private void PlayDoorAudio(bool hasKey)
    {
        if (_doorAudio == null)
        {
            return;
        }

        _doorAudio.Stream = hasKey
            ? AudioResourceLoader.TryLoad(DoorCreakAudioPath) ?? AudioResourceLoader.TryLoad(DoorCreakFallbackPath)
            : AudioResourceLoader.TryLoad(DoorLockedAudioPath) ?? AudioResourceLoader.TryLoad(DoorLockedFallbackPath);

        if (_doorAudio.Stream != null)
        {
            _doorAudio.Play();
        }
    }
}
