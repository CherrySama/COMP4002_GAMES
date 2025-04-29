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

        // �����㣬�㿿����ת�����������ã�||
        // ���������㿿���Ż����������ã�&&
        if (enemy.IsPlayerDetected() && Vector2.Distance(enemy.transform.position, player.position) < enemy.searchDistance)
            stateMachine.ChangeState(enemy.battleState);
    }
}
