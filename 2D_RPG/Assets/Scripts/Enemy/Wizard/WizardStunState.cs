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
        // Ӧ�û����� - ʼ��Զ�����
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
        // �������Ƿ���Ҫ��ת���������
        bool shouldFaceRight = enemy.lastHitDirection.x < 0; // ��X�����ʾ�����Ҳ�Ĺ���

        // ���ݵ�ǰ״̬����Ҫ�ĳ�������Ƿ�ת
        if ((shouldFaceRight && enemy.facingDir == -1) ||
            (!shouldFaceRight && enemy.facingDir == 1))
        {
            enemy.Flip();
        }
    }
}
