using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStateMachine
{
    public PlayerState currentState {  get; private set; }
    
    public void Initialize(PlayerState _startState)
    {
        if (_startState == null)
        {
            Debug.LogError("尝试初始化状态机时传入了空状态!");
            return;
        }
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        //currentState.Exit();
        //currentState = _newState;
        //currentState.Enter();
        if (_newState == null)
        {
            Debug.LogError("尝试切换到空状态!");
            return;
        }

        if (currentState != null)
            currentState.Exit();

        currentState = _newState;
        currentState.Enter();
    }
    // 添加安全的Update方法
    public void UpdateState()
    {
        if (currentState != null)
            currentState.Update();
        else
            Debug.LogError("当前状态为空，无法更新!");
    }
}

