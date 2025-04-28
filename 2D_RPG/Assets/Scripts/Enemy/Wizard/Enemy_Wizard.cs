using UnityEngine;

public class Enemy_Wizard : Enemy
{
    #region State Lists
    public WiazrdIdleState idleState {  get; private set; }
    public WizardMoveState moveState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        
        idleState = new WiazrdIdleState(this, stateMachine, "Idle", this);
        moveState = new WizardMoveState(this, stateMachine, "Move", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }
}