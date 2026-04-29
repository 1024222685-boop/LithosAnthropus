using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_AnimationTriggers : MonoBehaviour
{
    private SkillObject_Stuntman stuntman;

    private void Awake()
    {
        stuntman = GetComponentInParent<SkillObject_Stuntman>();
    }

    private void AttackTrigger()
    {
        stuntman.PerformedAttack();
    }

    private void TryTerminate(int currentAttackIndex)
    {
        if(currentAttackIndex == stuntman.maxAttacks)
            stuntman.HandleDeath();
    }
}
