using UnityEngine;

public class PlayerLadderClimbFinishState : PlayerState
{
    public PlayerLadderClimbFinishState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 设置玩家位置为梯子顶部
        Vector2 topPosition = player.transform.position;
        //topPosition.y += .5f; // 向上移动一定距离以到达平台
        player.transform.position = topPosition;

        // 防止角色下落
        player.rb.gravityScale = 0;
        player.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();

        // 在状态转换前，向水平方向移动一段距离确保角色站在平台上
        Vector2 newPosition = player.transform.position;
        newPosition.x += player.facingDir * 0.5f; // 向前移动0.5个单位
        newPosition.y += 2.65f;
        player.transform.position = newPosition;

        player.rb.gravityScale = 3.5f; // 恢复正常重力
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
