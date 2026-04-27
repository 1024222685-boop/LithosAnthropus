using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Stuntman : Skill_Base
{
    [SerializeField] private GameObject stuntManPrefab;
    [SerializeField] private float stuntManDuration;

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

    public void CreatStuntMan()
    {
        GameObject stuntMan = Instantiate(stuntManPrefab, transform.position, Quaternion.identity);
        stuntMan.GetComponent<SkillObject_Stuntman>().SetupStunt(this);
    }
}
