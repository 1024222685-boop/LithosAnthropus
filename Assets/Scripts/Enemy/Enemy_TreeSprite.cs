using UnityEngine;

public class Enemy_TreeSprite : Enemy, ICounterable
{

    public bool CanBeCountered { get => canBeStunned; }
    public Enemy_TsDeadState tsDeadState { get; set; }

    [Header("TreeSprite specifics")]
    [SerializeField] private GameObject tsToCreatePrefab;
    [SerializeField] private int amountOfTsToCreate = 2;
    [SerializeField] private Vector2 newTsVelocity;

    [SerializeField] private bool hasRecoveryAnimation = true;

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        battleState = new Enemy_BattleState(this, stateMachine, "battle");
        deadState = new Enemy_DeadState(this, stateMachine, "idle");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");
        deadState = new Enemy_DeadState(this, stateMachine, "dead");
        tsDeadState = new Enemy_TsDeadState(this, stateMachine, "dead");

        anim.SetBool("hasStunRecovery", hasRecoveryAnimation);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public override void EntityDeath()
    {
        stateMachine.ChangeState(tsDeadState);
    }

    public void HandleCounter()
    {
        if (CanBeCountered == false)
            return;

        stateMachine.ChangeState(stunnedState);
    }

    public void CreateTsOnDeath()
    {
        if (tsToCreatePrefab == null)
            return;

        for (int i = 0; i < amountOfTsToCreate; i++)
        {
            GameObject newTs = Instantiate(tsToCreatePrefab, transform.position, Quaternion.identity);
            Enemy_TreeSprite tsScript = newTs.GetComponent<Enemy_TreeSprite>();

            tsScript.SetupTs(newTsVelocity, stats); 
        }

    }

    public void SetupTs(Vector2 velocity, Entity_Stats newStats)
    {
        float xVelocity = velocity.x * Random.Range(-2,2);
        float yVelocity = velocity.y * Random.Range(1,2);

        rb.velocity = new Vector2(xVelocity, yVelocity);

        stats.AdjustStatSetpup(stats.resources, stats.offense, stats.defense, .6f, 1.2f);
    }
}
