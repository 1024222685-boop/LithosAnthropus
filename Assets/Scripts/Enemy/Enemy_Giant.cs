using System.Collections;
using UnityEngine;

public class Enemy_Giant : Enemy, ICounterable
{
    public bool CanBeCountered { get => canBeStunned; }
    public Enemy_GiantAttackState giantAttackState { get; private set; }
    public Enemy_GiantBattleState giantBattleState { get; private set; }
    public Enemy_GiantTeleportState giantTeleportState { get; private set; }
    public Enemy_GiantSpellCastState giantSpellCastState { get; private  set; }

    [Header("Giant specifics")]
    public float maxBattleIdleTime = 5;

    [Header("Giant Spellcast")]
    [SerializeField] private DamageScaleData spellDamageScale;
    [SerializeField] private GameObject spellCastPrefab;
    [SerializeField] private int amountToCast = 6;
    [SerializeField] private float spellCastRate = 1.2f;
    [SerializeField] private float spellCastStateCooldown = 10;
    [SerializeField] private Vector2 playerOffsetPrediction;
    private float lastTimeCastedSpells = float.NegativeInfinity;
    public bool spellCastPerformed { get; private set; }
    private Player playerScript;

    [Header("Giant Teleport")]
    [SerializeField] private BoxCollider2D arenaBounds;
    [SerializeField] private float offsetCenterY = 1.725f;
    [SerializeField] private float chanceToTeleport = .25f;
    private float defaultTeleportChance;
    public bool teleportTrigger { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");
        deadState = new Enemy_DeadState(this, stateMachine, "dead");

        giantBattleState = new Enemy_GiantBattleState(this, stateMachine, "battle");
        giantAttackState = new Enemy_GiantAttackState(this, stateMachine, "attack");
        giantTeleportState = new Enemy_GiantTeleportState(this, stateMachine, "teleport");
        giantSpellCastState = new Enemy_GiantSpellCastState(this, stateMachine, "spellCast");

        battleState = giantBattleState;
    }

    protected override void Start()
    {
        base.Start();

        arenaBounds.transform.parent = null;
        defaultTeleportChance = chanceToTeleport;

        stateMachine.Initialize(idleState);
    }

    public void HandleCounter()
    {
        if (CanBeCountered == false)
            return;

        stateMachine.ChangeState(stunnedState);
    }

    public override void SpecialAttack()
    {
        StartCoroutine(CastSpeelCo());
    }

    private IEnumerator CastSpeelCo()
    {
        if (playerScript == null)
            playerScript = player.GetComponent<Player>();

        for (int i = 0; i < amountToCast; i++)
        {
            bool playerMoving = playerScript.rb.velocity.magnitude > 0;

            float xOffset = playerMoving ? playerOffsetPrediction.x * playerScript.facingDir : 0;
            Vector3 spellPosition = player.transform.position + new Vector3(xOffset, playerOffsetPrediction.y);

            Enemy_GiantSpell spell = Instantiate(spellCastPrefab, spellPosition, Quaternion.identity).GetComponent<Enemy_GiantSpell>();

            spell.SetupSpell(combat,spellDamageScale);

            yield return new WaitForSeconds(spellCastRate);
        }

        SetSpellCastPerformed(true);
    }

    public void SetSpellCastPerformed(bool spellCastStatus) => spellCastPerformed = spellCastStatus;
    public bool CanDoSpellCast() => Time.time > lastTimeCastedSpells + spellCastStateCooldown;
    public void  SetSpellCastOnCooldown() => lastTimeCastedSpells = Time.time;

    public bool ShouldTeleport()
    {
        if (Random.value < chanceToTeleport)
        {
            chanceToTeleport = defaultTeleportChance;
            return true;
        }

        chanceToTeleport = chanceToTeleport + .05f;
        return false;
    }

    public void SetTeleportTrigger(bool triggerStatus) => teleportTrigger = triggerStatus;

    public Vector3 FindTeleportPoint()
    {
        int maxAttampts = 10;
        float bossWithColliderHalfs = col.bounds.size.x / 2;

        for (int i = 0; i < maxAttampts; i++)
        {
            float randomX = Random.Range(arenaBounds.bounds.min.x + bossWithColliderHalfs, arenaBounds.bounds.max.x - bossWithColliderHalfs);

            Vector2 raycastPoint = new Vector2(randomX, arenaBounds.bounds.max.y);

            RaycastHit2D hit = Physics2D.Raycast(raycastPoint, Vector2.down, Mathf.Infinity, whatIsGround);

            if (hit.collider != null)
                return hit.point + new Vector2(0, offsetCenterY);
        }

        return transform.position;
    }
}
