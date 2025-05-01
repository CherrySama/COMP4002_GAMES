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
            Debug.LogError("���Գ�ʼ��״̬��ʱ�����˿�״̬!");
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
            Debug.LogError("�����л�����״̬!");
            return;
        }

        if (currentState != null)
            currentState.Exit();

        currentState = _newState;
        currentState.Enter();
    }
    // ��Ӱ�ȫ��Update����
    public void UpdateState()
    {
        if (currentState != null)
            currentState.Update();
        else
            Debug.LogError("��ǰ״̬Ϊ�գ��޷�����!");
    }
}

