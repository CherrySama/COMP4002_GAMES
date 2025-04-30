using UnityEngine;

public class WizardBattleState : EnemyState
{
    private Transform player;
    private Enemy_Wizard enemy;
    private int moveDir;
    public WizardBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Wizard _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Player").transform;

        //Debug.Log("111");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            //Debug.Log(Check());
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance &&
                Mathf.Abs(player.transform.position.y - enemy.transform.position.y) > 1f)
            {
                if (CanIAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                    return;
                }
                else
                {
                    enemy.SetVelocity(0, 0);
                    return;
                }
            }
        }

        //!enemy.IsGroundDetected()
        //Vector2.Distance(enemy.transform.position, player.position) > enemy.searchDistance
        if (enemy.IsWallDetected() || !enemy.IsGroundDetected() || Vector2.Distance(enemy.transform.position, player.position) > enemy.searchDistance)
        {
            //Debug.Log("222");
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        if (player.transform.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.transform.position.x < enemy.transform.position.x)
            moveDir = -1;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, enemy.rb.linearVelocityY);
    }

    private bool CanIAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        //Debug.Log("Attack is in cooldown...");
        return false;
    }

    private bool Check()
    {
        return (enemy.IsPlayerDetected().distance < enemy.attackDistance);
    }
}
