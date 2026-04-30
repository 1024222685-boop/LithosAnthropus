using UnityEngine;

public class SkillObject_Stuntman : SkillObject_Base
{
    [SerializeField] private float wispMoveSpeed = 15;
    [SerializeField] private GameObject onDeathvfx;
    [SerializeField] private LayerMask whatIsGround;
    private bool shouldMoveToPlayer;

    private Transform playerTransform;
    private Skill_Stuntman stuntManager;
    private TrailRenderer wispTrail;
    private Entity_Health playerhealth;
    private SkillObject_Health stuntHealth;
    private Player_SkillManager skillManager;
    private Entity_StatusHandler statusHandler;


    public int maxAttacks { get; private set; }

    public void SetupStunt(Skill_Stuntman stuntManager)
    {
        this.stuntManager = stuntManager;
        playerStats = stuntManager.player.stats;
        damageScaleData = stuntManager.damageScaleData;
        maxAttacks = stuntManager.GetMaxAttacks();
        playerTransform = stuntManager.transform.root;
        playerhealth = stuntManager.player.health;
        statusHandler = stuntManager.player.statusHandler;

        skillManager = stuntManager.skillManager;

        Invoke(nameof(HandleDeath), stuntManager.GetStuntDuration());
        FlipTotarget();

        stuntHealth = GetComponent<SkillObject_Health>();
        wispTrail = GetComponentInChildren<TrailRenderer>();
        wispTrail.gameObject.SetActive(false);

        anim.SetBool("canAttack", maxAttacks > 0);
    }

    private void Update()
    {
        if (shouldMoveToPlayer)
            HandleWispMovement();
        else
        {
            anim.SetFloat("yVelocity", rb.velocity.y);
            StopHorizontalMovement();
        }

    }

    private void HandlePlayerTouch()
    {
        float healAmount = stuntHealth.lastDamageTaken * stuntManager.GetPercentofDamageHealed();
        playerhealth.IncreaseHealth(healAmount);

        float amountInSeconds = stuntManager.GetCooldownReduceInSeconds();
        skillManager.ReduceAllSkillCooldownBy(amountInSeconds);

        if(stuntManager.CanRemoveNegativeEffects())
            statusHandler.RemoveAllNegativeEffects( );
    }

    private void HandleWispMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, wispMoveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, playerTransform.position) < .5f)
        {
            HandlePlayerTouch();
            Destroy(gameObject);
        }
    }

    private void FlipTotarget()
    {
        Transform target = FindClosestTarget();

        if (target != null && target.position.x < transform.position.x)
            transform.Rotate(0, 180, 0);
    }

    public void PerformedAttack()
    {
        DamageEnemiesInRadius(targetCheck, 1);

        if (targetGotHit == false)
            return;

        bool canDuplicate = Random.value < stuntManager.GetDuplicateChance();
        float xOffset = transform.position.x < lastTarget.position.x ? 1 : -1;

        if (canDuplicate)
            stuntManager.CreatStuntMan(lastTarget.position + new Vector3(xOffset, 0));
    }

    public void HandleDeath()
    {
        Instantiate(onDeathvfx, transform.position, Quaternion.identity);

        if (stuntManager.ShouldBeWisp())
        {
            shouldMoveToPlayer = true;

            anim.gameObject.SetActive(false);

            wispTrail.gameObject.SetActive(true);

            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            rb.isKinematic = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void TurnIntoWisp()
    {
        shouldMoveToPlayer = true;
        anim.gameObject.SetActive(false);
        wispTrail.gameObject.SetActive(true);
        rb.simulated = false;
    }

    private void StopHorizontalMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, whatIsGround);

        if (hit.collider != null)
            rb.velocity = new Vector2(0, rb.velocity.y);
    }
}
