using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash dash { get; private set; }
    public Skill_Shadow shadow { get; private set; }
    public Skill_SickleThrow sickleThrow { get; private set; }
    public Skill_Stuntman stuntman { get; private set; }

    private void Awake()
    {
        dash = GetComponentInChildren<Skill_Dash>();
        shadow = GetComponentInChildren<Skill_Shadow>();
        sickleThrow = GetComponentInChildren<Skill_SickleThrow>();
        stuntman = GetComponentInChildren<Skill_Stuntman>();
    }

    public Skill_Base GetSkillByType(SkillType type)
    {
        switch (type)
        {
            case SkillType.AFewGoodMen: return dash;
            case SkillType.ShadowClone: return shadow;
            case SkillType.SickleThrow:return sickleThrow;
            case SkillType.Stuntman:return stuntman;

            default:
                Debug.Log($"Skill type {type} is not implemented yet.");
                return null;
        }
    }
}
