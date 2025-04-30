using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
    private Enemy_Wizard enemy => GetComponent<Enemy_Wizard>();
    

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                //hit.GetComponent<Player>().Damage();
                //Debug.Log(hit.GetComponent<Player>().name + " was damaged!");
                PlayerStat target = hit.GetComponent<PlayerStat>();
                enemy.stats.DoDamage(target);
            }
        }
    }

    private void GiveEXP()
    {

    }

    private void OpenWarningWindow()
    {
        enemy.OpenAttackWarningWindow();
    }
    private void CloseWarningWindow()
    {
        enemy.CloseAttackWarningWindow(); 
    }

}
