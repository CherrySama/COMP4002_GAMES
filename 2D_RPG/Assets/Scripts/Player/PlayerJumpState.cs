using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.rb.linearVelocity = new Vector2(player.rb.linearVelocityX, player.jumpForce);
        AudioManager.Instance.PlaySFX("Jump");
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.Instance.StopSFX("Jump");
    }

    public override void Update()
    {
        base.Update();

        if (player.rb.linearVelocityY < 0.0f)
            stateMachine.ChangeState(player.airState);
    }
}
