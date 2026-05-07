using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item list", fileName = "List of items - ")]

public class ItemListDataSO : ScriptableObject
{
    public ItemDataSO[] itemList;
}
