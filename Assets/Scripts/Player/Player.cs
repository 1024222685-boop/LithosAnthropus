using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public static event Action OnPlayerDeath;

    private UI ui;
    public PlayerInputSet input { get; private set; }
    public Player_SkillManager skillManager { get; private set; }
    public Player_VFX vfX { get; private set; }
    public Entity_Health health { get; private set; }
    public Entity_StatusHandler statusHandler { get; private set; }

    #region State Variales

    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_DeadState deadState { get; private set; }
    public Player_CounterAttackState counterAttackState { get; private set; }
    public Player_HurtState hurtState { get; private set; }
    public Player_SickleThrowState sickleThrowState { get; private set; }

    #endregion

    [Header("Attack details")]
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration = .1f;
    public float comboResetTime = 1;
    private Coroutine queuedAttackCo;

    [Header("Movement details")]//在Inseprctor里添加一个醒目的标题，形成一个视觉上的分组能看出参数变化的细节
    public float moveSpeed;
    public float jumpForce = 5;
    public Vector2 wallJumpForce;
    [Range(0, 1)]
    public float inAirMoveMultiplier = .7f;
    [Space]
    public float dashDuration = .25f;
    public float dashSpeed = 20;
    public Vector2 moveInput { get; private set; }
    public Vector2 mousePosition { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        ui = FindAnyObjectByType<UI>();
        vfX = GetComponent<Player_VFX>();
        health = GetComponent<Entity_Health>();
        skillManager = GetComponent<Player_SkillManager>();
        statusHandler = GetComponent<Entity_StatusHandler>();
        
        input = new PlayerInputSet();

        idleState = new Player_IdleState(this, stateMachine, "idle");
        moveState = new Player_MoveState(this, stateMachine, "move");
        jumpState = new Player_JumpState(this, stateMachine, "jumpFall");
        fallState = new Player_FallState(this, stateMachine, "jumpFall");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "jumpFall");
        dashState = new Player_DashState(this, stateMachine, "dash");
        basicAttackState = new Player_BasicAttackState(this, stateMachine, "basicAttack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "jumpAttack");
        deadState = new Player_DeadState(this, stateMachine, "dead");
        counterAttackState = new Player_CounterAttackState(this, stateMachine, "counterAttack");
        hurtState = new Player_HurtState(this, stateMachine, "isHurt");
        sickleThrowState = new Player_SickleThrowState(this, stateMachine, "sickleThrow");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public void SwapPlayer(Vector3 postion) => transform.position = postion;

    public override void EntityDeath()
    {
        base.EntityDeath();

        OnPlayerDeath?.Invoke();
        stateMachine.ChangeState(deadState);
    }

    public void EnterAttackStateWithDelay()
    {
        if (queuedAttackCo != null)
            StopCoroutine(queuedAttackCo);

        queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayCO());
    }

    private IEnumerator EnterAttackStateWithDelayCO()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Mouse.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();


        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;

        input.Player.ToggleSkillTreeUI.performed += ctx => ui.ToggleSkillTreeUI();
        input.Player.Spell.performed += ctx => skillManager.shadow.TryUseSkill();
    }

    public int GetInputDir()
    {
        return moveInput.x != 0 ? Mathf.RoundToInt(moveInput.x) : facingDir;
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
