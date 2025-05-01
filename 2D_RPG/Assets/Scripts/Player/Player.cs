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
    public bool canDoubleJump = true; // 控制是否启用二段跳
    public float doubleJumpForce = 10.0f; // 二段跳的力度，可以略小于第一段跳跃
    private bool _hasDoubleJumped = false; // 跟踪是否已经使用了二段跳

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

        //// 新增：强制清理动画系统
        //StartCoroutine(ResetAnimationSystem());

        // 等待一帧再初始化状态机，确保所有组件都已加载
        //StartCoroutine(DelayedStateInitialization());
        stateMachine.Initialize(idleState);

        uiController = FindFirstObjectByType<UIController>();
    }

    private IEnumerator ResetAnimationSystem()
    {
        // 等待所有组件完全初始化
        yield return new WaitForEndOfFrame();

        // 完全重置动画系统
        if (anim != null)
        {
            // 停止当前播放的所有动画
            anim.StopPlayback();

            // 重置所有参数
            foreach (AnimatorControllerParameter param in anim.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Bool)
                    anim.SetBool(param.name, false);
                else if (param.type == AnimatorControllerParameterType.Float)
                    anim.SetFloat(param.name, 0f);
                else if (param.type == AnimatorControllerParameterType.Int)
                    anim.SetInteger(param.name, 0);
            }

            // 完全重绑定动画器
            anim.Rebind();
            anim.Update(0f);

            Debug.Log("动画系统已完全重置");
        }
    }

    private IEnumerator DelayedStateInitialization()
    {
        yield return null; // 等待一帧
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
