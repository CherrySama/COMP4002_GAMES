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
        player.anim.SetBool(animBoolName, true);
        triggerCalled = false;
        //Debug.Log("I enter " + animBoolName);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        player.anim.SetFloat("yVelocity", player.rb.linearVelocityY);
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
