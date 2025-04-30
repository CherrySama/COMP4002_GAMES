using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponent<Player>();

    public void AttackFinished()
    {
        player.AnimationTrigger();
    }

    public void LadderClimbFinished()
    {
        player.AnimationTrigger();
    }

    public void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            { 
                //hit.GetComponent<Enemy>().Damage(player.transform.position);
                //hit.GetComponent<CharacterStats>().TakeDamage(player.stats.damage.GetValue());
                EnemyStat target = hit.GetComponent<EnemyStat>();
                player.stats.DoDamage(target, player.transform.position);
            }  
        }
    }
}
