using UnityEngine;

public class PlayerLadderClimbState : PlayerState
{
    private float climbSpeed = 5f;
    private float yInput;
    public PlayerLadderClimbState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = 3.5f;
    }

    public override void Update()
    {
        base.Update();

        yInput = Input.GetAxisRaw("Vertical");

        if (yInput == 0 && xInput == 0)
        {
            player.anim.speed = 0;
            player.SetVelocity(0, 0);
        }
        else
        {
            player.anim.speed = 1;
            player.SetVelocity(xInput * climbSpeed, yInput * climbSpeed);
        }

        // Check whether the top of the ladder has been reached
        if (yInput > 0 && !player.IsLadderDetected())
        {
            stateMachine.ChangeState(player.ladderClimbFinishState);
            return;
        }

        // 检测是否到达地面
        if (player.IsGroundDetected() && yInput < 0)
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        // 跳离梯子
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.jumpState);
            return;
        }

        if (yInput == 0 && !player.IsLadderDetected())
        {
            stateMachine.ChangeState(player.airState);
            return;
        }
    }
}
