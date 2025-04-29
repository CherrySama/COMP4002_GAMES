using UnityEngine;

public class Enemy_Wizard : Enemy
{
    #region State Lists
    public WiazrdIdleState idleState {  get; private set; }
    public WizardMoveState moveState { get; private set; }
    public WizardBattleState battleState { get; private set; }
    public WizardAttackState attackState { get; private set; }
    public WizardStunState stunState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        
        idleState = new WiazrdIdleState(this, stateMachine, "Idle", this);
        moveState = new WizardMoveState(this, stateMachine, "Move", this);
        battleState = new WizardBattleState(this, stateMachine, "Move", this);
        attackState = new WizardAttackState(this, stateMachine, "Attack", this);
        stunState = new WizardStunState(this, stateMachine, "Stun", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        if (isHit)
        {
            stateMachine.ChangeState(stunState);
            return;
        }
    }
}