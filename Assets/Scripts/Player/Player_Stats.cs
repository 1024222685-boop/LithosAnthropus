using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : Entity_Stats
{
    private List<string> activeBuff = new List<string>();

    public bool CanApplyBuffOf(string source)
    {
        return activeBuff.Contains(source) == false;
    }

    public void ApplyBuff(BuffEffectData[] buffsToApply, float duration, string source)
    {
        StartCoroutine(BuffCo(buffsToApply, duration, source));
    }

    private IEnumerator BuffCo(BuffEffectData[] buffsToApply, float duration, string source)
    {
        activeBuff.Add(source);

        foreach (var buff in buffsToApply)
        {
            GetStatByType(buff.type).AddModifier(buff.value, source);
        }

        yield return new WaitForSeconds(duration);

        foreach (var buff in buffsToApply)
        {
            GetStatByType(buff.type).RemoveModifier(source);
        }

        activeBuff.Remove(source);
    }
}
