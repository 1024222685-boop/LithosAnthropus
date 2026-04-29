using UnityEngine;

public class Skill_Stuntman : Skill_Base
{
    [SerializeField] private GameObject stuntManPrefab;
    [SerializeField] private float stuntManDuration;

    [Header("Attack Upgrades")]
    [SerializeField] private int maxAttacks = 3;
    [SerializeField] private float duplocationChance = .3f;

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
