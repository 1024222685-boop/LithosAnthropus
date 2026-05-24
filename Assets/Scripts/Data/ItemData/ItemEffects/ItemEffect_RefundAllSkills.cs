using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Refund all skills", fileName = "Item effect data - Refund all skills")]

public class ItemEffect_RefundAllSkills : ItemEffect_DataSO
{
    public override void ExcuteEffect()
    {
        UI ui = FindFirstObjectByType<UI>();
        ui.skillTreeUI.RefundAllSkills();
    }
}
