using UnityEngine;

public class PlayerLadderClimbFinishState : PlayerState
{
    public PlayerLadderClimbFinishState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // �������λ��Ϊ���Ӷ���
        Vector2 topPosition = player.transform.position;
        //topPosition.y += .5f; // �����ƶ�һ�������Ե���ƽ̨
        player.transform.position = topPosition;

        // ��ֹ��ɫ����
        player.rb.gravityScale = 0;
        player.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();

        // ��״̬ת��ǰ����ˮƽ�����ƶ�һ�ξ���ȷ����ɫվ��ƽ̨��
        Vector2 newPosition = player.transform.position;
        newPosition.x += player.facingDir * 0.5f; // ��ǰ�ƶ�0.5����λ
        newPosition.y += 2.65f;
        player.transform.position = newPosition;

        player.rb.gravityScale = 3.5f; // �ָ���������
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled && xInput != 0)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }
}
