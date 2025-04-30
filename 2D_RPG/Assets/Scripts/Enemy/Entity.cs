using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
    [Header("Collision Info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask WhatIsGround;
    [SerializeField] protected Transform ladderCheck;
    [SerializeField] protected float ladderCheckDistance;
    [SerializeField] protected LayerMask whatIsLadder;
    
    #region Components
    public Animator anim { get; private set; }

    public Rigidbody2D rb { get; private set; }

    public CharacterStats stats { get; private set; }

    public CapsuleCollider2D cd { get; private set; }
    #endregion

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;
    public bool isDead { get; private set; } = false;

    public System.Action onFlipped;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {

    }

    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    //public virtual void Damage()
    //{
    //    Debug.Log(gameObject.name + " was damaged!");
    //}

    #region Collision
    public virtual bool IsGroundDetected()
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

    public virtual bool IsWallDetected()
    {
        bool result = Physics2D.Raycast(wallCheck.position,
                                        Vector2.right * facingDir,
                                        wallCheckDistance,
                                        WhatIsGround);

        return result;
    }

    public virtual bool IsLadderDetected()
    {
        bool result = Physics2D.BoxCast(ladderCheck.position,
                                        new Vector2(1f, 0.2f),
                                        0f,
                                        Vector2.up,
                                        ladderCheckDistance,
                                        whatIsLadder);
        return result;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,
                        new Vector3(groundCheck.position.x,
                                    groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,
                        new Vector3(wallCheck.position.x + wallCheckDistance,
                                    wallCheck.position.y));
        Gizmos.DrawLine(ladderCheck.position,
                new Vector3(ladderCheck.position.x,
                            ladderCheck.position.y + ladderCheckDistance));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);

        // Use different colors to display the test results
        if (Application.isPlaying)
        {
            Gizmos.color = IsGroundDetected() ? Color.green : Color.red;
            Gizmos.DrawWireCube(
                new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance / 2, 0),
                new Vector3(0.4f, groundCheckDistance, 0.1f)
            );

            Gizmos.color = IsLadderDetected() ? Color.green : Color.red;
            Gizmos.DrawWireCube(
                new Vector3(ladderCheck.position.x, ladderCheck.position.y + ladderCheckDistance / 2, 0),
                new Vector3(0.4f, ladderCheckDistance, 0.1f)
            );
        }
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
        
        onFlipped?.Invoke();
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0.0f && !facingRight)
            Flip();
        else if (_x < 0.0f && facingRight)
            Flip();
    }
    #endregion

    public virtual void Die()
    {
        isDead = true;
    }
}
