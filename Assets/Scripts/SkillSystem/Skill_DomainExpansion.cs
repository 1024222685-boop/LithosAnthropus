using System.Collections.Generic;
using UnityEngine;

public class Skill_DomainExpansion : Skill_Base
{
    [SerializeField] private GameObject domainPrefab;

    [Header("Slowing Down Upgrade")]
    [SerializeField] private float slowDownPercent = .8f;
    [SerializeField] private float slowDownDomainDuration = 5;

    [Header("Shadow Cast Upgrade")]
    [SerializeField] private int shaowToCast = 10;
    [SerializeField] private float shadowCastDomainSlow = 1;
    [SerializeField] private float shadowCastDomainDuration = 8;
    private float spellCastTimer;
    private float spellsPerSecond;

    [Header("Sunt man cast Upgrade")]
    [SerializeField] private int manToCast = 8;
    [SerializeField] private float manCastDomainSlow = 1;
    [SerializeField] private float manCastDomainDuration = 6;
    [SerializeField] private float healthToRestoredWithMan = .05f;

    [Header("Domain details")]
    public float maxDomainSize = 10;
    public float expandSpeed = 3;

    private List<Enemy> trappedTargets = new List<Enemy>();
    private Transform currentTarget;

    public void CreateDomain()
    {
        spellsPerSecond = GetSpellsToCast() / GetDomainDuration();

        GameObject domain = Instantiate(domainPrefab, transform.position, Quaternion.identity);
        domain.GetComponent<SkillObject_DomainExpansion>().SetupDomain(this);
    }

    public void DoSpellCasting()
    {
        spellCastTimer -= Time.deltaTime;

        if (currentTarget == null)
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
            Vector3 offset = Random.value < .5f ? new Vector2(1, 0) : new Vector2(-1, 0);

            skillManager.stuntman.CreatStuntMan(target.position + offset);
        }

        if (upgradeType == SkillUpgradeType.Domain_ShadowSpam)
        {
            skillManager.shadow.CreateRawShadow(target, true);
        }
    }

    private Transform FindTargetInDomain()
    {
        trappedTargets.RemoveAll(target => target == null || target.health.isDead);

        if(trappedTargets.Count == 0)
            return null;

        int randomIndex = Random.Range(0, trappedTargets.Count);
        return trappedTargets[randomIndex].transform;
    }

    public float GetDomainDuration()
    {
        if (upgradeType == SkillUpgradeType.Domain_SlowinDown)
            return slowDownDomainDuration;
        else if (upgradeType == SkillUpgradeType.Domain_ShadowSpam)
            return shadowCastDomainDuration;
        else if (upgradeType == SkillUpgradeType.Domain_StuntSpam)
            return shadowCastDomainDuration;

        return 0;
    }

    public float GetSlowPercentage()
    {
        if (upgradeType == SkillUpgradeType.Domain_SlowinDown)
            return slowDownPercent;
        else if (upgradeType == SkillUpgradeType.Domain_ShadowSpam)
            return shadowCastDomainSlow;
        else if (upgradeType == SkillUpgradeType.Domain_StuntSpam)
            return manCastDomainSlow;

        return 0;
    }

    public int GetSpellsToCast()
    {
        if (upgradeType == SkillUpgradeType.Domain_ShadowSpam)
            return shaowToCast;
        else if (upgradeType == SkillUpgradeType.Domain_StuntSpam)
            return manToCast;

        return 0;
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
        foreach (var enemy in trappedTargets)
            enemy.StopAllCoroutines();

        trappedTargets = new List<Enemy>();
    }
}
