using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Reset horizontal speed to prevent sliding
        player.rb.linearVelocity = new Vector2(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (xInput != 0 && !player.isBusy)
            stateMachine.ChangeState(player.moveState);
        //if (xInput == 0)
        //    player.SetVelocity(0, player.rb.linearVelocity.y);
    }
}
