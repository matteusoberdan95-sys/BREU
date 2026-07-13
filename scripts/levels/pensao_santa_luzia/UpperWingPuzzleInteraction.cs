namespace BREU.Scripts.Levels.PensaoSantaLuzia;

using BREU.Scripts.Audio;

/// <summary>Sprint 18A/19 — upper wing interactions (fuse / panel / note / scares).</summary>
public partial class UpperWingPuzzleInteraction : Node, IInteractable
{
    public enum WingMode
    {
        Room204Bed,
        Room204WallMarks,
        Room204Note,
        SharedBathroomInspect,
        SharedBathroomMirror,
        LinenFuse,
        GeneratorPanel,
        Room205Blocked,
        OwnersOfficeLog
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
            WingMode.Room204Note when !_state.ReadRoom204Note => "Ler bilhete",
            WingMode.SharedBathroomInspect when !_done => "Examinar banheiro",
            WingMode.SharedBathroomMirror when !_done => "Examinar espelho",
            WingMode.LinenFuse when !_state.HasUpperFuse => "Pegar Fusível Superior",
            WingMode.GeneratorPanel when !_state.TechnicalPanelUnlocked && !_state.HasDrainKey =>
                "Examinar painel",
            WingMode.GeneratorPanel when !_state.TechnicalPanelUnlocked && _state.HasDrainKey =>
                "Destravar painel",
            WingMode.GeneratorPanel when _state.TechnicalPanelUnlocked && _state.HasOldFuse && !_state.OldFuseInstalled =>
                "Inserir Fusível Velho",
            WingMode.GeneratorPanel when _state.TechnicalPanelUnlocked && _state.HasUpperFuse && !_state.UpperFuseInstalled =>
                "Inserir Fusível Superior",
            WingMode.GeneratorPanel => "Examinar painel",
            WingMode.Room205Blocked => "Tentar abrir Quarto 205",
            WingMode.OwnersOfficeLog when !_state.ReadOwnersOfficeLog => "Examinar registros",
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

            case WingMode.Room204Note:
                if (_state.ReadRoom204Note) return;
                _state.MarkRoom204NoteRead();
                hud?.ShowMessage(
                    "Não deixem o 203 aberto depois das nove. Ele empurra os móveis sozinho.", 5f);
                audio?.PlayOneShot("old_house_settle_02", -15f);
                break;

            case WingMode.SharedBathroomInspect:
                if (_done) return;
                _done = true;
                hud?.ShowMessage("A água no ralo parece escura demais.", 3.5f);
                audio?.PlayOneShot("water_drop_01", -14f);
                if (!_state.BathroomScarePlayed)
                {
                    _state.MarkBathroomScarePlayed();
                    _ = BathroomScareAsync();
                }

                break;

            case WingMode.SharedBathroomMirror:
                if (_done) return;
                _done = true;
                hud?.ShowMessage("O espelho devolve o reflexo com atraso.", 3.5f);
                audio?.PlayOneShot("water_drop_02", -14f);
                break;

            case WingMode.LinenFuse:
                if (_state.HasUpperFuse) return;
                _state.PickupUpperFuse();
                var fuseRoot = GetParent()?.GetParent();
                if (fuseRoot?.GetNodeOrNull<MeshInstance3D>("Fuse_Upper_Visual") is { } fuseVisual)
                    fuseVisual.Visible = false;
                if (GetParent() is Area3D fuseArea)
                    fuseArea.SetDeferred(Area3D.PropertyName.Monitoring, false);
                hud?.ShowMessage("Você pegou o Fusível Superior. Leve-o ao painel da Sala Técnica.", 4f);
                _ = LaundryFuseSequenceAsync();
                break;

            case WingMode.GeneratorPanel:
                if (_state.IsUpperPowerRestored)
                {
                    hud?.ShowMessage("Os dois fusíveis vibram dentro da caixa.", 3.5f);
                    return;
                }

                if (!_state.TechnicalPanelUnlocked)
                {
                    if (!_state.HasDrainKey)
                    {
                        hud?.ShowMessage(
                            "A tampa do painel está trancada. Talvez haja uma chave pequena em algum lugar úmido. Objetivo: Encontre uma forma de abrir o painel.", 6f);
                        return;
                    }
                    _state.UnlockTechnicalPanel();
                    hud?.ShowMessage(
                        "A chave enferrujada gira com dificuldade. A tampa abre. Objetivo: Encontre os dois fusíveis.", 5f);
                    audio?.PlayOneShot("door_scratch_01", -18f);
                    return;
                }

                if (_state.HasOldFuse && !_state.OldFuseInstalled)
                {
                    _state.InstallOldFuse();
                    if (!_state.IsUpperPowerRestored)
                    {
                        hud?.ShowMessage("O fusível velho encaixa no primeiro slot. Ainda falta outro fusível.", 4.5f);
                        return;
                    }
                    hud?.ShowMessage("A caixa vibra. A energia voltou parcialmente ao andar de cima. Objetivo: Volte ao Quarto 203.", 5f);
                    audio?.PlayOneShot("old_house_settle_01", -12f);
                    _ = OnUpperPowerRestoredAsync();
                    return;
                }

                if (_state.HasUpperFuse && !_state.UpperFuseInstalled)
                {
                    _state.InstallUpperFuse();
                    if (!_state.IsUpperPowerRestored)
                    {
                        hud?.ShowMessage("O Fusível Superior encaixa no segundo slot. Ainda falta outro fusível.", 4.5f);
                        return;
                    }
                    hud?.ShowMessage("A caixa vibra. A energia voltou parcialmente ao andar de cima. Objetivo: Volte ao Quarto 203.", 5f);
                    audio?.PlayOneShot("old_house_settle_01", -12f);
                    _ = OnUpperPowerRestoredAsync();
                    return;
                }

                hud?.ShowMessage(
                    _state.OldFuseInstalled || _state.UpperFuseInstalled
                        ? "Um dos encaixes ainda está vazio. Ainda falta outro fusível."
                        : "O painel tem dois encaixes vazios.", 4f);
                break;

            case WingMode.Room205Blocked:
                hud?.ShowMessage(
                    "A maçaneta gira sozinha, mas a porta não abre.", 3.5f);
                audio?.PlayOneShot("door_scratch_01", -18f);
                break;

            case WingMode.OwnersOfficeLog:
                if (_state.ReadOwnersOfficeLog) return;
                _state.MarkOwnersOfficeLogRead();
                hud?.ShowMessage(
                    "Alguns nomes foram riscados várias vezes. O número 203 aparece repetido.", 5f);
                audio?.PlayOneShot("old_house_settle_02", -15f);
                break;
        }
    }

    private async System.Threading.Tasks.Task LaundryFuseSequenceAsync()
    {
        await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
        if (_state is { LaundryScarePlayed: false })
        {
            _state.MarkLaundryScarePlayed();
            PensionAudioManager.Find(GetTree())?.PlayOneShot("distant_knock_01", -13f);
            await FlickerCorridorLightAsync(1.0f);
            HUDController.FindActive(GetTree())?.ShowMessage(
                "Alguma coisa respondeu lá embaixo.", 3.5f);
        }
    }

    private async System.Threading.Tasks.Task BathroomScareAsync()
    {
        await ToSignal(GetTree().CreateTimer(0.35f), SceneTreeTimer.SignalName.Timeout);
        PensionAudioManager.Find(GetTree())?.PlayOneShot("door_scratch_01", -17f);
        await FlickerCorridorLightAsync(0.7f);
    }

    private async System.Threading.Tasks.Task OnUpperPowerRestoredAsync()
    {
        await FlickerUpperLightsAsync();
        EnablePoweredCorridorLight();
        await ToSignal(GetTree().CreateTimer(0.6f), SceneTreeTimer.SignalName.Timeout);
        PensionAudioManager.Find(GetTree())?.PlayOneShot("door_scratch_02", -14f);
        await ToSignal(GetTree().CreateTimer(0.9f), SceneTreeTimer.SignalName.Timeout);
        PensionAudioManager.Find(GetTree())?.PlayOneShot("distant_knock_01", -15f);
        HUDController.FindActive(GetTree())?.ShowMessage(
            "Do corredor do 203, algo respondeu com um arranhão seco.", 4f);
    }

    private void EnablePoweredCorridorLight()
    {
        var light = GetTree().CurrentScene?.GetNodeOrNull<OmniLight3D>(
            "World/Level/SecondFloor/UpperWingRooms/CorridorLight");
        if (light == null) return;
        light.LightEnergy = Mathf.Max(light.LightEnergy, 0.55f);
        light.Visible = true;
    }

    private async System.Threading.Tasks.Task FlickerCorridorLightAsync(float seconds)
    {
        var light = GetTree().CurrentScene?.GetNodeOrNull<OmniLight3D>(
            "World/Level/SecondFloor/UpperWingRooms/CorridorLight")
            ?? GetTree().CurrentScene?.GetNodeOrNull<OmniLight3D>("Lighting/UpperCorridorLight");
        if (light == null) return;

        var original = light.LightEnergy;
        var end = Time.GetTicksMsec() + (ulong)(seconds * 1000f);
        var dim = true;
        while (Time.GetTicksMsec() < end)
        {
            light.LightEnergy = dim ? original * 0.15f : original;
            dim = !dim;
            await ToSignal(GetTree().CreateTimer(0.12f), SceneTreeTimer.SignalName.Timeout);
        }

        light.LightEnergy = original;
    }

    private async System.Threading.Tasks.Task FlickerUpperLightsAsync()
    {
        var lighting = GetTree().CurrentScene.GetNodeOrNull<Node3D>("Lighting");
        var wingLight = GetTree().CurrentScene?.GetNodeOrNull<OmniLight3D>(
            "World/Level/SecondFloor/UpperWingRooms/CorridorLight");
        var lights = new List<OmniLight3D?>
        {
            lighting?.GetNodeOrNull<OmniLight3D>("UpperCorridorLight"),
            lighting?.GetNodeOrNull<OmniLight3D>("UpperCorridorFarLight"),
            wingLight
        };

        var originals = new float[lights.Count];
        for (var i = 0; i < lights.Count; i++)
        {
            originals[i] = lights[i]?.LightEnergy ?? 0f;
        }

        for (var pulse = 0; pulse < 5; pulse++)
        {
            for (var i = 0; i < lights.Count; i++)
            {
                if (lights[i] != null)
                {
                    lights[i]!.LightEnergy = pulse % 2 == 0 ? originals[i] * 0.25f : originals[i] * 1.15f;
                }
            }

            await ToSignal(GetTree().CreateTimer(0.18f), SceneTreeTimer.SignalName.Timeout);
        }

        for (var i = 0; i < lights.Count; i++)
        {
            if (lights[i] != null)
            {
                lights[i]!.LightEnergy = originals[i] > 0.01f ? originals[i] : 0.55f;
            }
        }
    }
}
