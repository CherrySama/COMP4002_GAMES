using UnityEngine;

public class PlayerAirState : PlayerState
{
    // Protection time after takeoff, ignoring ground detection
    protected float airStateCooldown = 0.1f; 
    //protected float airStateTimer;
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // Reset the counter
        stateTimer = airStateCooldown;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // Added airState horizontal movement control
        if (xInput != 0)
            player.SetVelocity(xInput * player.moveSpeed, player.rb.linearVelocityY);

        //airStateTimer -= Time.deltaTime;
        // Only detect ground after the guard time has passed
        if (stateTimer <= 0 && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        if (stateTimer <= 0 && player.IsWallDetected() && !player.IsLadderDetected())
        { 
            stateMachine.ChangeState(player.wallSlideState);
            return;
        }

        if (Input.GetAxisRaw("Vertical") != 0 && player.IsLadderDetected())
        {
            stateMachine.ChangeState(player.ladderClimbState);
            return;
        }

        // 二段跳检测
        if (Input.GetKeyDown(KeyCode.Space) && !player.hasDoubleJumped && player.canDoubleJump)
        {
            // 执行二段跳
            player.hasDoubleJumped = true;

            // 先重置垂直速度，然后应用新的跳跃力
            player.rb.linearVelocity = new Vector2(player.rb.linearVelocityX, 0);
            player.rb.linearVelocity = new Vector2(player.rb.linearVelocityX, player.doubleJumpForce);

            // 可以在这里添加二段跳的特效或声音
            return;
        }
    }
}
