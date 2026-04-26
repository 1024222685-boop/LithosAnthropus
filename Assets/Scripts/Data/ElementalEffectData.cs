using UnityEngine;

public class ElementalEffectData : MonoBehaviour
{
    public float mercyDuration;
    public float mercySlowMultiplier;

    public float brutalDuration;
    public float totalbrutalDamage;

    public float cowardDuration;
    public float cowardDamage;
    public float cowardCharge;

    public ElementalEffectData(Entity_Stats entity_Stats, DamageScaleData damageScale)
    {
        mercyDuration = damageScale.mercyDuration;
        mercySlowMultiplier = damageScale.mercySlowMultiplier;

        brutalDuration = damageScale.brutalDuration;
        totalbrutalDamage = entity_Stats.offense.brutalityDamage.GetValue() * damageScale.brutalDamageScale;

        cowardDuration = damageScale.cowardDuration;
        cowardDamage = entity_Stats.offense.cowardiceDamage.GetValue() * damageScale.cowardDamageScale;
        cowardCharge = damageScale.cowardCharge;
    }
}

public class ScaleFactor
{
    public float brutalDamageScale = .5f;
}