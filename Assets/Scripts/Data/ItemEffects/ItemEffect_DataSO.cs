using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect_DataSO : ScriptableObject
{
    [TextArea]
    public string effectDescription;
    protected Player player;

    public virtual bool CanBeUsed(Player player)
    {
        return true;
    }

    public virtual void ExcuteEffect()
    {

    }

    public virtual void Subscribe(Player player)
    {
        this.player = player;
    }

    public virtual void Unsubscribe() 
    {

    }
}
