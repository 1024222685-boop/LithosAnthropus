using System.Collections;
using UnityEngine;

public class Enemy_Mage : Enemy
{
    public bool CanBeCountered { get => canBeStunned; }
    public Enemy_MageRetreatState mageRetreatState { get; private set; }
    public Enemy_MageBattleState mageBattleState { get; private set; }
    public Enemy_MageSpellCastState mageSpellCastState { get; private set; }

    [Header("Mage specifics")]
    [SerializeField] private GameObject spelPrefab;
    [SerializeField] private Transform spellStartPosition;
    [SerializeField] private int amountToCast = 3;
    [SerializeField] private float spellCastCooldown = .3f;
    public bool spellCastPerformed { get; private set; }
    [Space]
    public float retreatCooldown = 5;
    public float retreatMaxDistance = 8;
    public float retreatSpeed = 15;
    [SerializeField] private Transform behindCollisionCheck;
    [SerializeField] private bool hasRecoveryAnimation = true;

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");
        deadState = new Enemy_DeadState(this, stateMachine, "dead");

        mageSpellCastState = new Enemy_MageSpellCastState(this, stateMachine, "spellCast");
        mageRetreatState = new Enemy_MageRetreatState(this, stateMachine, "battle");
        mageBattleState = new Enemy_MageBattleState(this, stateMachine, "battle");
        battleState = mageBattleState;

        anim.SetBool("hasStunRecovery", hasRecoveryAnimation);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public void SetSpepllCastPerformed(bool performed) => spellCastPerformed = performed;

    public override void SpecialAttack()
    {
        StartCoroutine(CastSpeelCo());
    }

    private IEnumerator CastSpeelCo()
    {
        for (int i = 0; i < amountToCast; i++)
        {
            Enemy_MageProjecttile projecttile = Instantiate(spelPrefab, spellStartPosition.position, Quaternion.identity).GetComponent<Enemy_MageProjecttile>();

            projecttile.SetupProjecttile(player.transform, combat);
            yield return new WaitForSeconds(spellCastCooldown);
        }

        SetSpepllCastPerformed(true);
    }

    public void HandleCounter()
    {
        if (CanBeCountered == false)
            return;

        stateMachine.ChangeState(stunnedState);
    }

    public bool CanMoveBackwards()
    {
        bool detectedWall = Physics2D.Raycast(behindCollisionCheck.position, Vector2.right * -facingDir, 1.5f, whatIsGround);
        bool noGround = Physics2D.Raycast(behindCollisionCheck.position, Vector2.down, 1.5f, whatIsGround) == false;

        return noGround || detectedWall;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(behindCollisionCheck.position,
            new Vector3(behindCollisionCheck.position.x + (1.5f * -facingDir), behindCollisionCheck.position.y));

        Gizmos.DrawLine(behindCollisionCheck.position,
            new Vector3(behindCollisionCheck.position.x, behindCollisionCheck.position.y - 1.5f));
    }
}
