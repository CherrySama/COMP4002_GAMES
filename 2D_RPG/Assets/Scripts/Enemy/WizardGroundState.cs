using UnityEngine;

public class WizardGroundState : EnemyState
{
    protected Enemy_Wizard enemy;
    protected Transform player;

    public WizardGroundState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Wizard _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Player").transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // 背对你，你靠近会转身过来干你就用：||
        // 面向你且你靠近才会过来干你就用：&&
        if (enemy.IsPlayerDetected() && Vector2.Distance(enemy.transform.position, player.position) < enemy.searchDistance)
            stateMachine.ChangeState(enemy.battleState);
    }
}
