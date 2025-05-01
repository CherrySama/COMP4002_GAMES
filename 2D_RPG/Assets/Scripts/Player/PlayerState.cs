using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected float xInput;
    private string animBoolName;
    protected float stateTimer;
    protected bool triggerCalled;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        //// 重置所有动画参数 - 这是关键修复
        //player.anim.SetBool("Idle", false);
        //player.anim.SetBool("Move", false);
        //player.anim.SetBool("Jump", false);
        //player.anim.SetBool("Attack", false);
        //player.anim.SetBool("WallSlide", false);
        //player.anim.SetBool("Dash", false);
        //player.anim.SetBool("LadderClimb", false);
        //player.anim.SetBool("LadderClimbFinish", false);
        //player.anim.SetBool("Stun", false);
        //player.anim.SetBool("Die", false);
        //player.anim.SetBool("UI", false);

        //// 强制清除动画缓存
        //player.anim.Rebind();
        //player.anim.Update(0f);

        player.anim.SetBool(animBoolName, true);
        triggerCalled = false;
        //Debug.Log("I enter " + animBoolName);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        player.anim.SetFloat("yVelocity", player.rb.linearVelocityY);

        if (player.isDead)
            stateMachine.ChangeState(player.deadState);
        if (Input.GetKeyDown(KeyCode.U))
            stateMachine.ChangeState(player.uiState);
        //Debug.Log("I'm in " + animBoolName);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
        //Debug.Log("I exit " + animBoolName);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
