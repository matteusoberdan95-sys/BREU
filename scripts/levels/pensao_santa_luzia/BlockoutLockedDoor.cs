namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Narrative;

/// <summary>Stable blockout door: an opaque, immovable panel with a local interaction message.</summary>
public partial class BlockoutLockedDoor : Node3D, IInteractable
{
    [Export] public string PromptText { get; set; } = "Tentar abrir";
    [Export] public string LockedMessage { get; set; } = "Está trancada por dentro.";

    /// <summary>Optional Sprint 15 one-shot narrative follow-up (e.g. scratch behind door).</summary>
    [Export] public string NarrativeFollowUpEventId { get; set; } = string.Empty;

    public string GetPromptText() => PromptText;

    public void Interact(Node interactor)
    {
        HUDController.FindActive(GetTree())?.ShowMessage(LockedMessage, 3.0f);

        if (!string.IsNullOrWhiteSpace(NarrativeFollowUpEventId))
        {
            CallDeferred(nameof(DeferredNarrativeFollowUp));
        }
    }

    private void DeferredNarrativeFollowUp()
    {
        _ = FollowUpAsync();
    }

    private async System.Threading.Tasks.Task FollowUpAsync()
    {
        await ToSignal(GetTree().CreateTimer(3.1f), SceneTreeTimer.SignalName.Timeout);
        PensionNarrativeEvents.Find(GetTree())?.TryTrigger(NarrativeFollowUpEventId);
    }
}
