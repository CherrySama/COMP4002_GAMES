using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //AudioManager.Instance.ResumeAllAudio();
        AudioManager.Instance.PlaySFX("Move");
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.Instance.StopSFX("Move");
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed, player.rb.linearVelocityY);

        if (xInput == 0)
            stateMachine.ChangeState(player.idleState);
    }
}
