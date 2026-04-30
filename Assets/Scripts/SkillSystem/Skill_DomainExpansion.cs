using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_DomainExpansion : Skill_Base
{
    [SerializeField] private GameObject domainPrefab;

    [Header("Slowing Down Upgrade")]
    [SerializeField] private float slowDownPercent = .8f;
    [SerializeField] private float slowDownDomainDuration = 5;

    [Header("Speel Casting Upgrade")]
    [SerializeField] private int spellsToCast = 10;
    [SerializeField] private float spellCastingDomainSlowDown = 1;
    [SerializeField] private float spellCastingDomainDuration = 8;
    private float spellCastTimer;
    private float spellsPerSecond;

    [Header("Domain details")]
    public float maxDomainSize = 10;
    public float expandSpeed = 3;

    private List<Enemy> trappedTargets = new List<Enemy>();
    private Transform currentTarget;

    public void CreateDomain()
    {
        spellsPerSecond = spellsToCast / GetDomainDuration();

        GameObject domain = Instantiate(domainPrefab, transform.position, Quaternion.identity);
        domain.GetComponent<SkillObject_DomainExpansion>().SetupDomain(this);
    }

    public void DoSpellCasting()
    {
        spellCastTimer -= Time.deltaTime;

        if(currentTarget == null)
            currentTarget = FindTargetInDomain();

        if (currentTarget != null && spellCastTimer <= 0)
        {
            CastSpell(currentTarget);
            spellCastTimer = 1f / spellsPerSecond;
            currentTarget = null;
        }
    }

    private void CastSpell(Transform target)
    {
        if (upgradeType == SkillUpgradeType.Domain_StuntSpam)
        {
            Vector3 offset = Random.value < .5f ? new Vector2(1,0) : new Vector2(-1,0);

            skillManager.stuntman.CreatStuntMan(target.position + offset);
        }

        if (upgradeType == SkillUpgradeType.Domain_ShadowSpam)
        {
            skillManager.shadow.CreateRawShadow(target, true);
        }
    }

    private Transform FindTargetInDomain()
    {
        if(trappedTargets.Count == 0)
            return null;

        int randomIndex = Random.Range(0, trappedTargets.Count);
        Transform target = trappedTargets[randomIndex].transform;

        if (target == null)
        {
            trappedTargets.RemoveAt(randomIndex);
            return null;
        }

        return target;
    }

    public float GetDomainDuration()
    {
        if(upgradeType == SkillUpgradeType.Domain_SlowinDown)
            return slowDownDomainDuration;
        else
            return spellCastingDomainDuration;
    }

    public float GetSlowPercentage()
    {
        if (upgradeType == SkillUpgradeType.Domain_SlowinDown)
            return slowDownPercent;
        else
            return spellCastingDomainSlowDown;
    }

    public bool InstantDomain()
    {
        return upgradeType != SkillUpgradeType.Domain_StuntSpam
            && upgradeType != SkillUpgradeType.Domain_ShadowSpam;
    }

    public void AddTarget(Enemy targetToAdd)
    {
        trappedTargets.Add(targetToAdd);
    }

    public void ClearTagets()
    {
        foreach(var enemy in trappedTargets)
            enemy.StopAllCoroutines();

        trappedTargets = new List<Enemy>();
    }
}
