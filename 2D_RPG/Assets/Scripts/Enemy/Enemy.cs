using UnityEngine;

public class Enemy : Entity
{
    [Header("Stunned Info")]
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool isAttack;
    [SerializeField] protected GameObject attackWarningImage;

    [Header("Move Info")]
    public float moveSpeed;
    public float idleTime;

    [Header("Attack Info")]
    public float attackDistance;
    public float attackCooldown = 2f;
    [SerializeField] public float lastTimeAttacked;
    public float searchDistance = 10f;
    [SerializeField] protected LayerMask WhatIsPlayer;

    [Header("Enemy Type")]
    public string enemyName;

    public EnemyStateMachine  stateMachine { get; private set; }

    public bool isHit = false;
    public Vector2 lastHitDirection;
    public string lastAnimBoolName {  get; private set; }
    //public Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        attackWarningImage.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
        //Debug.Log(IsPlayerDetected().collider.gameObject.name);
    }

    public virtual void AssignLastName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, WhatIsPlayer);

    public virtual void Damage(Vector2 attackerPosition)
    {
        isHit = true;

        // 计算并保存击中方向
        lastHitDirection = (transform.position - (Vector3)attackerPosition).normalized;

        //Debug.Log(gameObject.name + " was damaged!");
    }

    public virtual void OpenAttackWarningWindow()
    {
        isAttack = true;
        attackWarningImage.SetActive(true);
    }

    public virtual void CloseAttackWarningWindow()
    {
        isAttack = false;
        attackWarningImage.SetActive(false);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,
                        new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }
}
