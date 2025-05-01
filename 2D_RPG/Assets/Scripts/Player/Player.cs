using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;

public class Player : Entity
{
    [Header("Attack Details")]
    public Vector2[] attackMovement;
    public float attackSpeed = 1.0f;

    [Header("Move Info")]
    public float moveSpeed = 12.0f;
    public float jumpForce = 12.0f;
    public bool canDoubleJump = true; // �����Ƿ����ö�����
    public float doubleJumpForce = 10.0f; // �����������ȣ�������С�ڵ�һ����Ծ
    private bool _hasDoubleJumped = false; // �����Ƿ��Ѿ�ʹ���˶�����

    [Header("Dash Info")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;

    [Header("Stunning Info ")]
    public float stunDuration;

    public float dashDir {  get; private set; }

    public bool isHit = false;

    public bool isBusy { get; private set; }

    #region State Lists
    public PlayerStateMachine stateMachine {  get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlideState {  get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerLadderClimbState ladderClimbState { get; private set; }
    public PlayerLadderClimbFinishState ladderClimbFinishState { get; private set; }
    public PlayerStunState stunState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    public PlayerUIState uiState { get; private set; }
    public PlayerPrimaryAttack primaryAttack { get; private set; }
    #endregion

    public UIController uiController { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState  = new PlayerAirState(this, stateMachine, "Jump");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "WallJump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        ladderClimbState = new PlayerLadderClimbState(this, stateMachine, "LadderClimb");
        ladderClimbFinishState = new PlayerLadderClimbFinishState(this, stateMachine, "LadderClimbFinish");
        stunState = new PlayerStunState(this, stateMachine, "Stun");
        deadState = new PlayerDeadState(this, stateMachine, "Die");
        uiState = new PlayerUIState(this, stateMachine, "UI");

        primaryAttack = new PlayerPrimaryAttack(this, stateMachine, "Attack");
    }

    protected override void Start()
    {
        base.Start();

        //// ������ǿ��������ϵͳ
        //StartCoroutine(ResetAnimationSystem());

        // �ȴ�һ֡�ٳ�ʼ��״̬����ȷ������������Ѽ���
        //StartCoroutine(DelayedStateInitialization());
        stateMachine.Initialize(idleState);

        uiController = FindFirstObjectByType<UIController>();
    }

    private IEnumerator ResetAnimationSystem()
    {
        // �ȴ����������ȫ��ʼ��
        yield return new WaitForEndOfFrame();

        // ��ȫ���ö���ϵͳ
        if (anim != null)
        {
            // ֹͣ��ǰ���ŵ����ж���
            anim.StopPlayback();

            // �������в���
            foreach (AnimatorControllerParameter param in anim.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Bool)
                    anim.SetBool(param.name, false);
                else if (param.type == AnimatorControllerParameterType.Float)
                    anim.SetFloat(param.name, 0f);
                else if (param.type == AnimatorControllerParameterType.Int)
                    anim.SetInteger(param.name, 0);
            }

            // ��ȫ�ذ󶨶�����
            anim.Rebind();
            anim.Update(0f);

            Debug.Log("����ϵͳ����ȫ����");
        }
    }

    private IEnumerator DelayedStateInitialization()
    {
        yield return null; // �ȴ�һ֡
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        //Debug.Log(stateMachine.currentState == null);
        stateMachine.currentState.Update();

        if (isHit)
        {
            stateMachine.ChangeState(stunState);
            return;
        }

        CheckForDashInput();
    }

    public bool hasDoubleJumped
    {
        get { return _hasDoubleJumped; }
        set { _hasDoubleJumped = value; }
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    public virtual void Damage()
    {
        isHit = true;
        //Debug.Log(gameObject.name + " was damaged!");
    }


    public void AnimationTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

    private void CheckForDashInput()
    {
        dashUsageTimer -= Time.deltaTime;

        if (IsWallDetected())
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown;
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }

    public override void Die()
    {
        base.Die();

        //stateMachine.ChangeState(deadState);
    }
}
