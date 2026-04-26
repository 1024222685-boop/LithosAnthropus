using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_SickleSpin : SkillObject_Sickle
{
    private int maxDistance;
    private float attackPerSecond;
    private float attackTimer;

    public override void SetupSickle(Skill_SickleThrow sickleManager, Vector2 direction)
    {
        base.SetupSickle(sickleManager, direction);

        anim?.SetTrigger("spin");

        maxDistance = sickleManager.maxDistance;
        attackPerSecond = sickleManager.attackPerSecond;

        Invoke(nameof(GetSickleBackToPlayer),sickleManager.maxSpinDuration);
    }

    protected override void Update()
    {
        HandleAttack();
        HandleStopping();
        HandleComeback();
    }

    private void HandleStopping()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if(distanceToPlayer > maxDistance && rb.simulated == true)
            rb.simulated = false;
    }

    private void HandleAttack()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer < 0)
        {
            DamageEnemiesInRadius(transform, 1);
            attackTimer = 1 / attackPerSecond;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        rb.simulated = false;
    }
}
