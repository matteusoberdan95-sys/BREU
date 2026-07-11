namespace BREU.Scripts.Levels.PensaoSantaLuzia;

public enum PensaoBlockoutInteractionKind
{
    JobOfferSign,
    GuestRegister,
    Room102,
    Room102Note,
    FusePickup,
    Deposit,
    ManagerClue,
    LockedUpperRoom,
    Bathroom,
}

/// <summary>Estado local do puzzle placeholder da pensao blockout limpa.</summary>
public partial class PensaoBlockoutCleanController : Node
{
    [Export] public NodePath DepositBlockerPath { get; set; } = "../StaticGameplayCollisions/DepositDoorCollision/Shape";
    [Export] public NodePath UpstairsBlockerPath { get; set; } = "../StaticGameplayCollisions/StairAccessBlocker/Shape";
    [Export] public NodePath FuseVisualPath { get; set; } = "../Interactions/FusePickup/FuseVisual";
    [Export] public NodePath UpperFloorLightPath { get; set; } = "../Lighting/UpperFloorLight";

    public bool HasFuse { get; private set; }
    public bool DepositUnlocked { get; private set; }
    public bool ManagerRoomClueFound { get; private set; }

    public string Interact(PensaoBlockoutInteractionKind kind)
    {
        return kind switch
        {
            PensaoBlockoutInteractionKind.JobOfferSign =>
                "OFERTA DE TRABALHO - MINERACAO - PENSAO SANTA LUZIA.",
            PensaoBlockoutInteractionKind.GuestRegister =>
                "Seu nome ja esta no registro.",
            PensaoBlockoutInteractionKind.Room102 =>
                "Quarto 102. Parece ter sido preparado para mim.",
            PensaoBlockoutInteractionKind.Room102Note =>
                "Mandaram deixar o fusivel na cozinha. Nao mexa no deposito depois das dez.",
            PensaoBlockoutInteractionKind.FusePickup => CollectFuse(),
            PensaoBlockoutInteractionKind.Deposit => UseDeposit(),
            PensaoBlockoutInteractionKind.ManagerClue => ReadManagerClue(),
            PensaoBlockoutInteractionKind.LockedUpperRoom =>
                "Tem algo se mexendo do outro lado.",
            PensaoBlockoutInteractionKind.Bathroom =>
                "A torneira esta seca, mas o espelho ainda esta molhado.",
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

        return "Fusivel velho adquirido.";
    }

    private string UseDeposit()
    {
        if (DepositUnlocked)
        {
            return "O deposito esta aberto. Algo destrancou no corredor.";
        }

        if (!HasFuse)
        {
            return "Esta trancado.";
        }

        DepositUnlocked = true;
        SetCollisionDisabled(DepositBlockerPath, true);
        SetCollisionDisabled(UpstairsBlockerPath, true);
        if (GetNodeOrNull<OmniLight3D>(UpperFloorLightPath) is { } light)
        {
            light.LightEnergy = 0.62f;
            light.Visible = true;
        }

        return "Voce usou o fusivel. Algo destrancou no corredor.";
    }

    private string ReadManagerClue()
    {
        ManagerRoomClueFound = true;
        return "Esses nomes... todos vieram pela mesma proposta.";
    }

    private void SetCollisionDisabled(NodePath path, bool disabled)
    {
        if (GetNodeOrNull<CollisionShape3D>(path) is { } shape)
        {
            shape.SetDeferred(CollisionShape3D.PropertyName.Disabled, disabled);
        }
    }
}
