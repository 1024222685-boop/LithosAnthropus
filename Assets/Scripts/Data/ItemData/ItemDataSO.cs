using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Material item", fileName = "Material data - ")]

public class ItemDataSO : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public int maxStackSize = 1;

    [Header("Item effect")]
    public ItemEffect_DataSO itemEffect;

    [Header("Craft details")]
    public Inventory_Item[] craftRecipe;
}
