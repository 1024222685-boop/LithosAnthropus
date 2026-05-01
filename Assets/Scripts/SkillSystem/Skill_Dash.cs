using UnityEngine;

public class Skill_Dash : Skill_Base
{
    public void OnStartEffect()
    {
        if (Unlocked(SkillUpgradeType.AFewGoodMen_Resistant) || Unlocked(SkillUpgradeType.AFewGoodMen_SpeedUp))
            CreateClone();

        if (Unlocked(SkillUpgradeType.AFewGoodMen_SpeedClone) || Unlocked(SkillUpgradeType.AFewGoodMen_PowerUpMaster))
            CreateShadow();
    }

    public void OnEndEffect()
    {
        if (Unlocked(SkillUpgradeType.AFewGoodMen_SpeedUp))
            CreateClone();

        if (Unlocked(SkillUpgradeType.AFewGoodMen_PowerUpMaster))
            CreateShadow(); 
    }

    private void CreateShadow()
    {
        skillManager.shadow.CreateRawShadow();
    }

    private void CreateClone()
    {
        skillManager.stuntman.CreatStuntMan();
    }
}