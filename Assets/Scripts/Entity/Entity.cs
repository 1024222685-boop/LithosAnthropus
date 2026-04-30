using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour//所有实体基类（不用每个重复再写）
{
    public event Action OnFlipped;//翻转时触发的事件（所有UI会监听该事件）

    public Animator anim { get; private set; }//动画组件（设为private的目的是防止修改，保证调用时出错可首先排除这个问题）

    public Rigidbody2D rb { get; private set; }
    public Entity_Stats stats { get; private set; }
    protected StateMachine stateMachine;//让所有实体都拥有状态机

    private bool facingRight = true;
    public int facingDir { get; private set; } = 1;

    [Header("Collision detection")]
    public LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    public bool isKnocked { get; private set; }
    private Coroutine knockbackCo;
    private Coroutine slowDownCo;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();//获取子物体里的动画组件
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<Entity_Stats>();

        stateMachine = new StateMachine();//新建状态机，不会让事件重复利用导致角色动作挤在一帧上使用
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
    }

    public void CurrentStateAnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();
    }

    public virtual void EntityDeath()
    {

    }

    public virtual void SlowDownEntity(float duration, float slowMultiplier, bool canOverrideSlowEffect = false)
    {
        if (slowDownCo != null)
        {
            if (canOverrideSlowEffect)
                StopCoroutine(slowDownCo);
            else
                return;
        }

        slowDownCo = StartCoroutine(SlowDownEntityCo(duration, slowMultiplier));
    }

    protected virtual IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        yield return null;
    }

    public virtual void StopSlowDown()
    {
        slowDownCo = null;
    }

    public void ReciveKnockback(Vector2 knockback, float duration)
    {
        if (knockbackCo != null)
            StopCoroutine(knockbackCo);

        knockbackCo = StartCoroutine(KnockbackCo(knockback, duration));

        if (this is Player player)
        {
            player.stateMachine.ChangeState(player.hurtState);
        }
    }

    private IEnumerator KnockbackCo(Vector2 knockback, float duration)//击退协程
    {
        isKnocked = true;//标记：正在击退
        if (this is Player)
        {
            anim.SetBool("isHurt", true);
        }


        rb.velocity = new Vector2(knockback.x, 0);//给一个推力

        yield return new WaitForSeconds(duration);//等待击退时间

        rb.velocity = Vector2.zero;//停下
        isKnocked = false;//取消击退标记

        if (this is Player)
        {
            anim.SetBool("isHurt", false);
        }
    }

    public void SetVelocity(float xVelocity, float yVelocity)//设置角色速度（x，y轴都可用这个）
    {
        if (isKnocked)//如果正在被击退，不能移动
            return;

        rb.velocity = new Vector2(xVelocity, yVelocity);//设置速度
        HandleFlip(xVelocity);//处理翻转
    }

    public void HandleFlip(float xVelocity)//根据移动方向判断是否翻转
    {
        if (xVelocity > 0 && facingRight == false)
            Flip();
        else if (xVelocity < 0 && facingRight)
            Flip();
    }

    public void Flip()//反转角色（左右转身）
    {
        transform.Rotate(0, 180, 0);//旋转180度
        facingRight = !facingRight;//切换朝向标记
        facingDir = facingDir * -1;//方向值取反（1变-1，-1变1）

        OnFlipped?.Invoke();//触发翻转事件（血条会监听该事件）
    }

    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        Player player = this as Player;//定义只有玩家才需要检测墙壁
        if (player != null)
        {
            int inputDir = player.moveInput.x != 0 ? Mathf.RoundToInt(player.moveInput.x) : facingDir;//获取输入方向，没有输入就用当前朝向

            wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * inputDir, wallCheckDistance, whatIsGround)
                && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * inputDir, wallCheckDistance, whatIsGround);
        }
        else
        {
            wallDetected = false;
        }
    }


    protected virtual void OnDrawGizmos()//编辑器里绘制辅助线（方便观察角色或者敌人的检测范围）
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));
        Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));
    }
}
