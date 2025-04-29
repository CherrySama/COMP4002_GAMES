using UnityEngine;

public class WizardAttackState : EnemyState
{
    private Enemy_Wizard enemy;
    public WizardAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Wizard enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(0, 0);

        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
}
