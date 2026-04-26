using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_SicklePeirce : SkillObject_Sickle
{
    private int amountToPierce;

    public override void SetupSickle(Skill_SickleThrow sickleManager, Vector2 direction)
    {
        base.SetupSickle(sickleManager, direction);
        amountToPierce = sickleManager.amountToPierce;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        bool groundHit = collision.gameObject.layer == LayerMask.NameToLayer("Ground");

        if (amountToPierce <= 0 || groundHit)
        {
            DamageEnemiesInRadius(transform, .3f);
            StopSickle(collision);
            return;
        }

        amountToPierce--;
        DamageEnemiesInRadius(transform, .3f);
    }
}
