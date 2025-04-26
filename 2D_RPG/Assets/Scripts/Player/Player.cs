using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move Info")]
    public float moveSpeed = 12.0f;
    public float jumpForce = 12.0f;

    [Header("Collision Info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask WhatIsGround;

    public int facingDir { get; private set; } = 1;
    private bool facingRight = true;

    #region Components
    public Animator anim {  get; private set; }
    
    public Rigidbody2D rb { get; private set; }
    #endregion

    #region State Lists
    public PlayerStateMachine stateMachine {  get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlideState {  get; private set; }
    #endregion

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState  = new PlayerAirState(this, stateMachine, "Jump");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    public bool IsGroundDetected()
    {
        //bool result = Physics2D.Raycast(groundCheck.position,
        //                                Vector2.down,
        //                                groundCheckDistance,
        //                                WhatIsGround);
        bool result = Physics2D.BoxCast(groundCheck.position,
                                        new Vector2(0.5f, 0.2f), 
                                        0f, 
                                        Vector2.down,
                                        groundCheckDistance,
                                        WhatIsGround);
        //Debug.Log("Ground Detected: " + result);
        return result;
    }

    public bool IsWallDetected()
    {
        bool result = Physics2D.Raycast(wallCheck.position,
                                        Vector2.right * facingDir,
                                        wallCheckDistance,
                                        WhatIsGround);

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, 
                        new Vector3(groundCheck.position.x, 
                                    groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,
                        new Vector3(wallCheck.position.x + wallCheckDistance,
                                    wallCheck.position.y));

        // 使用不同颜色显示检测结果
        if (Application.isPlaying)
        {
            Gizmos.color = IsGroundDetected() ? Color.green : Color.red;
            Gizmos.DrawWireCube(
                new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance / 2, 0),
                new Vector3(0.4f, groundCheckDistance, 0.1f)
            );
        }
    }

    public void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public void FlipController(float _x)
    {
        if (_x > 0.0f && !facingRight)
            Flip();
        else if (_x < 0.0f && facingRight)
            Flip();
    }
}
