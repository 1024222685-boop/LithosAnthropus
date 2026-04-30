using UnityEngine;

public class Skill_Stuntman : Skill_Base
{
    [SerializeField] private GameObject stuntManPrefab;
    [SerializeField] private float stuntManDuration;

    [Header("Attack Upgrades")]
    [SerializeField] private int maxAttacks = 3;
    [SerializeField] private float duplocationChance = .3f;

    [Header("Heal Upgrades")]
    [SerializeField] private float damagePercentHealed = .3f;
    [SerializeField] private float cooldownReducedInSeconds;

    public float GetPercentofDamageHealed()
    {
        if(ShouldBeWisp() == false)
            return 0;

        return damagePercentHealed;
    }

    public float GetCooldownReduceInSeconds()
    {
        if(upgradeType != SkillUpgradeType.StuntMan_Cooldown)
            return 0;

        return cooldownReducedInSeconds;
    }

    public bool CanRemoveNegativeEffects()
    {
        return upgradeType == SkillUpgradeType.StuntMan_Clean;
    }

    public bool ShouldBeWisp()
    {
        return upgradeType == SkillUpgradeType.StuntMan_Heal
            || upgradeType == SkillUpgradeType.StuntMan_Clean
            || upgradeType == SkillUpgradeType.StuntMan_Cooldown;
    }

    public float GetDuplicateChance()
    {
        if (upgradeType != SkillUpgradeType.StuntMan_ChanceToDuplicate)
            return 0;

        return duplocationChance;
    }

    public int GetMaxAttacks()
    {
        if (upgradeType == SkillUpgradeType.StuntMan_SingleAttack || upgradeType == SkillUpgradeType.StuntMan_ChanceToDuplicate)
            return 1;
        if (upgradeType == SkillUpgradeType.StuntMan_MultiAttack)
            return maxAttacks;

        return 0;
    }

    public float GetStuntDuration()
    {
        return stuntManDuration;
    }

    public override void TryUseSkill()
    {
        if (CanUseSkill() == false)
            return;

        CreatStuntMan();
    }

    public void CreatStuntMan(Vector3? targetposition = null)
    {
        Vector3 position = targetposition ?? transform.position;

        GameObject stuntMan = Instantiate(stuntManPrefab, position, Quaternion.identity);
        stuntMan.GetComponent<SkillObject_Stuntman>().SetupStunt(this);
    }
}
