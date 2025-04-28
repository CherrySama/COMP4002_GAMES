using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 当角色着地时重置二段跳状态
        player.hasDoubleJumped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.J)) 
        {
            stateMachine.ChangeState(player.primaryAttack);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
            return;
        }

        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
            return;
        }

        if (Input.GetAxisRaw("Vertical") > 0 && player.IsLadderDetected())
        {
            stateMachine.ChangeState(player.ladderClimbState);
            return;
        }
    }
}
