namespace BREU.Scripts.Enemies;

/// <summary>
/// Inimigo silhueta placeholder — sem combate, sem IA avancada.
/// Modelo final sera feito no Blender em sprint futura.
/// </summary>
public partial class EnemyPlaceholder : Node3D
{
    [Export] public bool IsActive { get; set; }
    [Export] public bool CanChase { get; set; }
    [Export] public float MoveSpeed { get; set; } = 1.2f;
    [Export] public NodePath PlayerPath { get; set; } = "../../Player";

    public override void _Ready()
    {
        Visible = false;
        IsActive = false;
    }

    public override void _Process(double delta)
    {
        if (!IsActive || !CanChase)
        {
            return;
        }

        var player = ResolvePlayer();
        if (player == null)
        {
            return;
        }

        LookAtPlayer();
        // TODO: movimento simples / navmesh na proxima sprint.
    }

    public void Activate()
    {
        IsActive = true;
        Visible = true;
        LookAtPlayer();
        GD.Print("EnemyPlaceholder: presenca detectada no corredor.");
    }

    public void Deactivate()
    {
        IsActive = false;
        Visible = false;
        GD.Print("EnemyPlaceholder: silhueta recuou para o escuro.");
    }

    public void LookAtPlayer()
    {
        var player = ResolvePlayer();
        if (player == null)
        {
            GD.Print("EnemyPlaceholder: player nao encontrado para LookAt.");
            return;
        }

        var target = player.GlobalPosition;
        target.Y = GlobalPosition.Y;
        LookAt(target, Vector3.Up, true);
    }

    private Node3D? ResolvePlayer()
    {
        if (GetNodeOrNull(PlayerPath) is Node3D playerFromPath)
        {
            return playerFromPath;
        }

        return GetTree().GetFirstNodeInGroup("player") as Node3D;
    }
}
