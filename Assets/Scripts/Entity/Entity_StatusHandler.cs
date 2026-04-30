using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entityvfx;
    private Entity_Stats entityStats;
    private Entity_Health entityHealth;
    private ElementType currentEffect = ElementType.None;

    [Header("coward effect details")]
    [SerializeField] private GameObject cowardiceStrikevfx;
    [SerializeField] private float currentAbsorb;
    [SerializeField] private float maximumAbsorb = 1;
    private Coroutine cowardCo;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityHealth = GetComponent<Entity_Health>();
        entityStats = GetComponent<Entity_Stats>();
        entityvfx = GetComponent<Entity_VFX>();
    }

    public void RemoveAllNegativeEffects()
    {
        StopAllCoroutines();
        currentEffect = ElementType.None;
        entityvfx.StopAllvfx();
    }

    public void ApplyStatusEffect(ElementType element,ElementalEffectData effectData)
    {
        if (element == ElementType.Mercy && CanBeApplied(ElementType.Mercy))
            ApplyMercyEffect(effectData.mercyDuration, effectData.mercySlowMultiplier);

        if (element == ElementType.Brutality && CanBeApplied(ElementType.Brutality))
            ApplyBrutalEffect(effectData.brutalDuration, effectData.totalbrutalDamage);

        if (element == ElementType.Cowardice && CanBeApplied(ElementType.Cowardice))
            ApplyCowardEffect(effectData.cowardDuration, effectData.cowardDamage, effectData.cowardCharge);
    }

    private void ApplyCowardEffect(float duration,float damage,float absorb)
    {
        float cowardiceResistance = entityStats.GetElementalResistance(ElementType.Cowardice);
        float finalAbsorb = absorb * (1 - cowardiceResistance);
        currentAbsorb = currentAbsorb + finalAbsorb;

        if (currentAbsorb >= maximumAbsorb)
        {
            DoCowardiceStrike(damage);
            StopCowardEffect();
            return;
        }

        if (cowardCo != null)
            StopCoroutine(cowardCo);

        cowardCo = StartCoroutine(CowardEffectCo(duration));
    }

    private void StopCowardEffect()
    {
        currentEffect = ElementType.None;
        currentAbsorb = 0;
        entityvfx.StopAllvfx();
    }

    private void DoCowardiceStrike(float damage)
    {
        Instantiate(cowardiceStrikevfx, transform.position, Quaternion.identity);
        entityHealth.ReduceHealth(damage);
    }

    private IEnumerator CowardEffectCo(float duration)
    {
        currentEffect = ElementType.Cowardice;
        entityvfx.PlayOnStatusVfx(duration, ElementType.Cowardice);

        yield return new WaitForSeconds(duration);
        StopCowardEffect();
    }

    private void ApplyBrutalEffect(float duration, float brutalityDamage)
    {
        float brutalityResistance = entityStats.GetElementalResistance(ElementType.Brutality);
        float finalDamage = brutalityDamage * (1 - brutalityResistance);

        StartCoroutine(BrutalEffectCo(duration, finalDamage));
    }

    private IEnumerator BrutalEffectCo(float duration, float totalDamage)
    {
        currentEffect = ElementType.Brutality;
        entityvfx.PlayOnStatusVfx(duration, ElementType.Brutality);

        int ticksPerSecond = 2;
        int tickCount = Mathf.RoundToInt(ticksPerSecond * duration);

        float damagePerTick = totalDamage / tickCount;
        float tickInterval = 1f / ticksPerSecond;

        for (int i = 0; i < tickCount; i++)
        {
            //减少实体的健康
            entityHealth.ReduceHealth(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }

        currentEffect = ElementType.None;

    }

   private void ApplyMercyEffect(float duration, float slowMultiplier)
    {
        float mercyResistance = entityStats.GetElementalResistance(ElementType.Mercy);
        float finalDuration = duration * (1 - mercyResistance);

        entity.SlowDownEntity(duration, slowMultiplier);
        StartCoroutine(MercyEffectCo(finalDuration,slowMultiplier));
    }

    private IEnumerator MercyEffectCo(float duration,float slowMutiplier)
    {
        currentEffect = ElementType.Mercy;//启用VFX
        entityvfx.PlayOnStatusVfx(duration, ElementType.Mercy);

        yield return new WaitForSeconds(duration);
        currentEffect = ElementType.None;//停止VFX
    }

    public bool CanBeApplied(ElementType element)
    {
        if(element == ElementType.Cowardice && currentEffect == ElementType.Cowardice)
            return true;

        return currentEffect == ElementType.None;
    }
}
