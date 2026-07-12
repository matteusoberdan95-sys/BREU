namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;
using BREU.Scripts.Narrative;

/// <summary>Sprint 18A — upper wing interactions (fuse / generator / ambient).</summary>
public partial class UpperWingPuzzleInteraction : Node, IInteractable
{
    public enum WingMode
    {
        Room204Bed,
        Room204WallMarks,
        SharedBathroomMirror,
        LinenFuse,
        GeneratorPanel,
        Room205Blocked
    }

    [Export] public WingMode Mode { get; set; }

    private PensaoPuzzleState? _state;
    private bool _done;

    public override void _Ready()
    {
        _state = GetTree().CurrentScene.GetNodeOrNull<PensaoPuzzleState>("PuzzleState");
        GetParent()?.AddToGroup("interactable");
        if (GetParent()?.GetParent() is Node3D grand && !grand.IsInGroup("interactable"))
        {
            grand.AddToGroup("interactable");
        }
    }

    public string GetPromptText()
    {
        if (_state == null)
        {
            return string.Empty;
        }

        return Mode switch
        {
            WingMode.Room204Bed when !_done => "Examinar cama",
            WingMode.Room204WallMarks when !_done => "Examinar marcas",
            WingMode.SharedBathroomMirror when !_done => "Examinar espelho",
            WingMode.LinenFuse when !_state.HasUpperFuse =>
                "Mexer nos lençóis",
            WingMode.GeneratorPanel when !_state.IsUpperPowerRestored && !_state.HasUpperFuse =>
                "Examinar painel",
            WingMode.GeneratorPanel when !_state.IsUpperPowerRestored && _state.HasUpperFuse =>
                "Instalar fusível",
            WingMode.Room205Blocked => "Tentar abrir Quarto 205",
            _ => string.Empty
        };
    }

    public void Interact(Node interactor)
    {
        if (_state == null)
        {
            return;
        }

        var hud = HUDController.FindActive(GetTree());
        var audio = PensionAudioManager.Find(GetTree());

        switch (Mode)
        {
            case WingMode.Room204Bed:
                if (_done) return;
                _done = true;
                hud?.ShowMessage("Os lençóis estão revirados. Alguém esteve aqui recentemente.", 4f);
                audio?.PlayOneShot("old_house_settle_01", -16f);
                break;

            case WingMode.Room204WallMarks:
                if (_done) return;
                _done = true;
                hud?.ShowMessage(
                    "As marcas descem pela parede, como se alguém tivesse sido arrastado.", 4f);
                audio?.PlayOneShot("door_scratch_02", -17f);
                break;

            case WingMode.SharedBathroomMirror:
                if (_done) return;
                _done = true;
                hud?.ShowMessage(
                    "Meu reflexo demora um instante para acompanhar meu movimento.", 4f);
                audio?.PlayOneShot("old_house_settle_02", -16f);
                break;

            case WingMode.LinenFuse:
                if (_state.HasUpperFuse) return;
                hud?.ShowMessage("Há alguma coisa dura escondida entre os panos úmidos.", 2.8f);
                _state.PickupUpperFuse();
                _ = ShowFuseFoundAsync();
                break;

            case WingMode.GeneratorPanel:
                if (_state.IsUpperPowerRestored) return;
                if (!_state.HasUpperFuse)
                {
                    hud?.ShowMessage(
                        "O painel está sem fusível. A fiação sobe para o corredor do 203.", 4f);
                    return;
                }

                _state.RestoreUpperPower();
                hud?.ShowMessage("O fusível encaixa. A luz do corredor pisca.", 3.5f);
                audio?.PlayOneShot("old_house_settle_01", -12f);
                _ = OnUpperPowerRestoredAsync();
                break;

            case WingMode.Room205Blocked:
                hud?.ShowMessage(
                    "O cheiro vindo daqui é insuportável. Melhor não forçar.", 3.5f);
                audio?.PlayOneShot("door_scratch_01", -18f);
                break;
        }
    }

    private async System.Threading.Tasks.Task ShowFuseFoundAsync()
    {
        await ToSignal(GetTree().CreateTimer(2.9f), SceneTreeTimer.SignalName.Timeout);
        HUDController.FindActive(GetTree())?.ShowMessage(
            "Encontrei outro fusível. Está velho, mas talvez ainda funcione.", 4f);
        PensionAudioManager.Find(GetTree())?.PlayOneShot("water_drop_02", -14f);
    }

    private async System.Threading.Tasks.Task OnUpperPowerRestoredAsync()
    {
        await FlickerUpperLightsAsync();
        await ToSignal(GetTree().CreateTimer(0.6f), SceneTreeTimer.SignalName.Timeout);
        PensionAudioManager.Find(GetTree())?.PlayOneShot("door_scratch_02", -14f);
        await ToSignal(GetTree().CreateTimer(0.9f), SceneTreeTimer.SignalName.Timeout);
        PensionAudioManager.Find(GetTree())?.PlayOneShot("distant_knock_01", -15f);
        HUDController.FindActive(GetTree())?.ShowMessage(
            "Do corredor do 203, algo respondeu com um arranhão seco.", 4f);
    }

    private async System.Threading.Tasks.Task FlickerUpperLightsAsync()
    {
        var lighting = GetTree().CurrentScene.GetNodeOrNull<Node3D>("Lighting");
        if (lighting == null) return;

        var lights = new[]
        {
            lighting.GetNodeOrNull<OmniLight3D>("UpperCorridorLight"),
            lighting.GetNodeOrNull<OmniLight3D>("UpperCorridorFarLight"),
            lighting.GetNodeOrNull<OmniLight3D>("BalconyWingLight")
        };

        var originals = new float[lights.Length];
        for (var i = 0; i < lights.Length; i++)
        {
            originals[i] = lights[i]?.LightEnergy ?? 0f;
        }

        for (var pulse = 0; pulse < 5; pulse++)
        {
            for (var i = 0; i < lights.Length; i++)
            {
                if (lights[i] != null)
                {
                    lights[i]!.LightEnergy = pulse % 2 == 0 ? originals[i] * 0.25f : originals[i] * 1.15f;
                }
            }

            await ToSignal(GetTree().CreateTimer(0.18f), SceneTreeTimer.SignalName.Timeout);
        }

        for (var i = 0; i < lights.Length; i++)
        {
            if (lights[i] != null)
            {
                lights[i]!.LightEnergy = originals[i];
            }
        }
    }
}
