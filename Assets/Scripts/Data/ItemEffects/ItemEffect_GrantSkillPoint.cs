using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Grand skill point", fileName = "Item effect data - Grant Skill Point")]

public class ItemEffect_GrantSkillPoint : ItemEffect_DataSO
{
    [SerializeField] private int pointToAdd;

    public override void ExcuteEffect()
    {
        UI ui = FindFirstObjectByType<UI>();
        ui.skillTreeUI.AddSkillPoints(pointToAdd);
    }
}
