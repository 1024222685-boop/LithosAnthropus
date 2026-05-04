using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect_DataSO : ScriptableObject
{
    [TextArea]
    public string effectDescription;

    public virtual void ExcuteEffect()
    {

    }
}
