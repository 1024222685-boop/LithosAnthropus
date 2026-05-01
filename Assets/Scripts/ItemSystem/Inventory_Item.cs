using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory_Item
{
   public ItemDataSO itemData;

    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;
    }
}
