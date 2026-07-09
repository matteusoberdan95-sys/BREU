namespace BREU.Scripts.Enemies;

/// <summary>
/// Inimigo silhueta placeholder — sem combate, sem IA avancada.
/// </summary>
public partial class EnemyPlaceholder : Node3D
{
    [Export] public bool IsActive { get; set; }
    [Export] public bool CanChase { get; set; }
    [Export] public float MoveSpeed { get; set; } = 1.2f;
    [Export] public NodePath PlayerPath { get; set; } = "../../Player";
    [Export] public bool PlayAudioOnActivate { get; set; } = true;

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
    }

    public void Activate()
    {
        IsActive = true;
        Visible = true;
        LookAtPlayer();

        if (PlayAudioOnActivate)
        {
            PlayPresenceAudio();
        }

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

    private void PlayPresenceAudio()
    {
        var manager = AudioManager.Find(this);
        if (manager == null)
        {
            GD.Print("EnemyPlaceholder: AudioManager nao encontrado.");
            return;
        }

        manager.Play3DSound(AudioResourceLoader.TryLoad(AudioPaths.EnemyBreath), GlobalPosition);

        var growl = AudioResourceLoader.TryLoad(AudioPaths.EnemyGrowl);
        if (growl != null)
        {
            GetTree().CreateTimer(0.35).Timeout += () =>
                manager.Play3DSound(growl, GlobalPosition);
        }
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
