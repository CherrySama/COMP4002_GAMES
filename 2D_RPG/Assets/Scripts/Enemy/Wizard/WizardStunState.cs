using UnityEngine;

public class WizardStunState : EnemyState
{
    private Enemy_Wizard enemy;

    public WizardStunState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Wizard _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {        
        base.Enter();

        stateTimer = enemy.stunDuration;
        FacePlayer();
        // 应用击退力 - 始终远离玩家
        enemy.rb.linearVelocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.isHit = false;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }

    private void FacePlayer()
    {
        // 检查敌人是否需要翻转以面向玩家
        bool shouldFaceRight = enemy.lastHitDirection.x < 0; // 负X方向表示来自右侧的攻击

        // 根据当前状态和需要的朝向决定是否翻转
        if ((shouldFaceRight && enemy.facingDir == -1) ||
            (!shouldFaceRight && enemy.facingDir == 1))
        {
            enemy.Flip();
        }
    }
}
