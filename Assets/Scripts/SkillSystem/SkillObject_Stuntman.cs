using UnityEngine;

public class SkillObject_Stuntman : SkillObject_Base
{
    [SerializeField] private GameObject onDeathvfx;
    [SerializeField] private LayerMask whatIsGround;
    private Skill_Stuntman stuntManager;

    public int maxAttacks { get; private set; }

    public void SetupStunt(Skill_Stuntman stuntManager)
    {
        this.stuntManager = stuntManager;
        playerStats = stuntManager.player.stats;
        damageScaleData = stuntManager.damageScaleData;
        maxAttacks = stuntManager.GetMaxAttacks();

        FlipTotarget();
        anim.SetBool("canAttack", maxAttacks > 0);
        Invoke(nameof(HandleDeath), stuntManager.GetStuntDuration());
    }

    private void Update()
    {
        anim.SetFloat("yVelocity", rb.velocity.y);
        StopHorizontalMovement();
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
        Destroy(gameObject);
    }

    private void StopHorizontalMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, whatIsGround);

        if (hit.collider != null)
            rb.velocity = new Vector2(0, rb.velocity.y);
    }
}
