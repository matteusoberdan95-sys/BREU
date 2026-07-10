namespace BREU.Scripts.Enemies;

/// <summary>
/// Area de dano do inimigo para ataques melee do player.
/// </summary>
public partial class EnemyHurtbox : Area3D
{
    public override void _Ready()
    {
        AddToGroup("enemy_hurtbox");
    }

    public EnemyPlaceholderAI? GetEnemy()
    {
        var current = GetParent();
        while (current != null)
        {
            if (current is EnemyPlaceholderAI enemy)
            {
                return enemy;
            }

            current = current.GetParent();
        }

        return null;
    }
}
