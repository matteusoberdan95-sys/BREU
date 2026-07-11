namespace BREU.Scripts.Levels.PensaoSantaLuzia;

/// <summary>Estado local do puzzle placeholder da vertical slice da Pensao.</summary>
public partial class PensaoVerticalSliceController : Node
{
    [Export] public NodePath DepositBlockerPath { get; set; } = "../Base/StaticGameplayCollisions/DepositDoorCollision/Shape";
    [Export] public NodePath UpstairsBlockerPath { get; set; } = "../Base/StaticGameplayCollisions/SecondFloorTemporaryBlocker/Shape";
    [Export] public NodePath FuseVisualPath { get; set; } = "../VerticalSliceInteractions/FusePickup/FuseVisual";
    [Export] public NodePath DepositVisualPath { get; set; } = "../Visual/WorldModel/deposit_door_dark";
    [Export] public NodePath UpperFloorLightPath { get; set; } = "../VerticalSliceLighting/UpperCorridorLight";

    public bool HasFuse { get; private set; }
    public bool DepositUnlocked { get; private set; }
    public bool UpstairsUnlocked { get; private set; }

    public string Interact(VerticalSliceInteractionKind kind)
    {
        return kind switch
        {
            VerticalSliceInteractionKind.GuestRegister => "Seu nome ja esta no registro.",
            VerticalSliceInteractionKind.Room102Door => "Quarto 102. Parece ter sido preparado para mim.",
            VerticalSliceInteractionKind.Room102Note => "Mandaram deixar o fusivel na cozinha. Nao mexa no deposito depois das dez.",
            VerticalSliceInteractionKind.FusePickup => CollectFuse(),
            VerticalSliceInteractionKind.Deposit => UseDeposit(),
            VerticalSliceInteractionKind.UpperCorridor => "Tem passos vindo do andar de baixo.",
            VerticalSliceInteractionKind.ManagerClue => "Esses nomes... todos vieram pela mesma proposta.",
            VerticalSliceInteractionKind.LockedUpperRoom => "A porta nao abre. Ha alguma coisa encostada do outro lado.",
            VerticalSliceInteractionKind.Bathroom => "A torneira esta seca, mas o espelho ainda esta molhado.",
            _ => "Nada aqui.",
        };
    }

    private string CollectFuse()
    {
        if (HasFuse)
        {
            return "O suporte vazio ainda cheira a fio queimado.";
        }

        HasFuse = true;
        if (GetNodeOrNull<Node3D>(FuseVisualPath) is { } visual)
        {
            visual.Visible = false;
        }
        GD.Print("PensaoVerticalSlice: fusivel coletado; evento de tensao preparado.");
        return "Fusivel velho adquirido. Alguma coisa se moveu no andar de cima.";
    }

    private string UseDeposit()
    {
        if (DepositUnlocked)
        {
            return "O deposito esta aberto. O quadro voltou a funcionar.";
        }
        if (!HasFuse)
        {
            return "Esta trancado. Preciso encontrar a chave ou alguma forma de abrir.";
        }

        DepositUnlocked = true;
        UpstairsUnlocked = true;
        SetCollisionDisabled(DepositBlockerPath, true);
        SetCollisionDisabled(UpstairsBlockerPath, true);
        if (GetNodeOrNull<Node3D>(DepositVisualPath) is { } depositVisual)
        {
            depositVisual.Visible = false;
        }
        if (GetNodeOrNull<OmniLight3D>(UpperFloorLightPath) is { } light)
        {
            light.LightEnergy = 0.62f;
            light.Visible = true;
        }
        GD.Print("PensaoVerticalSlice: deposito e acesso superior liberados.");
        return "Voce usou o fusivel. Algo destrancou no corredor.";
    }

    private void SetCollisionDisabled(NodePath path, bool disabled)
    {
        if (GetNodeOrNull<CollisionShape3D>(path) is { } shape)
        {
            shape.SetDeferred(CollisionShape3D.PropertyName.Disabled, disabled);
        }
    }
}
