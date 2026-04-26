using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackData
{
    public float phyiscalDamage;
    public float elementalDamage;
    public bool isCrit;
    public ElementType element;

    public ElementalEffectData effectData;

    public AttackData(Entity_Stats entityStats, DamageScaleData scaleData)
    {
        phyiscalDamage = entityStats.GetPhysicalDamage(out isCrit, scaleData.physical);
        elementalDamage = entityStats.GetElementalDamage(out element, scaleData.elemental);

        effectData = new ElementalEffectData(entityStats, scaleData);
    }
}
