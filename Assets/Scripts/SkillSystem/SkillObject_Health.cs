using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_Health : Entity_Health
{
    protected override void Die()
    {
        SkillObject_Stuntman stuntMan = GetComponent<SkillObject_Stuntman>();
        stuntMan.HandleDeath();
    }
}
