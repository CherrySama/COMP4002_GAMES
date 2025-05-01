using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 1.5f;

    public PlayerPrimaryAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //xInput = 0; // need this to fix the bug on attacking

        if (comboCounter > 3 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter);
        player.anim.speed = player.attackSpeed;

        player.SetVelocity(player.attackMovement[comboCounter].x * Input.GetAxisRaw("Horizontal"), player.attackMovement[comboCounter].y);

        AudioManager.Instance.PlaySFX("Attack");

        stateTimer = .1f;
        //Debug.Log(comboCounter);
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.Instance.StopSFX("Attack");

        player.StartCoroutine("BusyFor", .2f);
        player.anim.speed = 1.0f;

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.rb.linearVelocity = new Vector2(0, 0);

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);      
    }
}
