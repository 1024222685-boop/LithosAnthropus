using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour
{
    protected Player player;
    public event Action OnInventoryChange;

    public int maxInventorySize = 12;
    public List<Inventory_Item> itemList = new List<Inventory_Item>();

    protected virtual void Awake()
    {
        player = GetComponent<Player>();
    }

    public void TryUseItem(Inventory_Item itemToUse)
    {
        Inventory_Item consumable = itemList.Find(item => item == itemToUse);

        if (consumable == null)
            return;

        if (consumable.itemEffect.CanBeUsed(player) == false)
            return;

        consumable.itemEffect.ExcuteEffect();

        if (consumable.stackSize > 1)
            consumable.RemoveStack();
        else
            RemoveOneItem(consumable);

        OnInventoryChange?.Invoke();
    }

    public bool CanAddItem(Inventory_Item itemToAdd)
    {
        bool hasStackable = FindStackable(itemToAdd) != null;
        return hasStackable || itemList.Count < maxInventorySize;
    }
    public Inventory_Item FindStackable(Inventory_Item itemToAdd)
    {
        List<Inventory_Item> stackableItems = itemList.FindAll(item => item.itemData == itemToAdd.itemData);

        foreach (var stackableItem in stackableItems)
        {
            if (stackableItem.CanAddStack())
                return stackableItem;
        }

        return null;
    }

    public void AddItem(Inventory_Item itemToAdd)
    {
        Inventory_Item itemInInventory = FindStackable(itemToAdd);

        if (itemInInventory != null)
            itemInInventory.AddStack();
        else
            itemList.Add(itemToAdd);

        OnInventoryChange?.Invoke();
    }

    public void RemoveOneItem(Inventory_Item itemToRemove)
    {
        if (itemToRemove == null) return;

        Inventory_Item itemInventory = itemList.Find(item => item == itemToRemove);

        if (itemInventory == null) return;

        if (itemInventory.stackSize > 1)
            itemInventory.RemoveStack();
        else
            itemList.Remove(itemInventory);

        OnInventoryChange?.Invoke();
    }

    public void RemoveFullStack(Inventory_Item itemToRemove)
    {
        for (int i = 0; i < itemToRemove.stackSize; i++)
        {
            RemoveOneItem(itemToRemove);
        }
    }

    public Inventory_Item FindItem(Inventory_Item itemToFind)
    {
        return itemList.Find(item => item == itemToFind);
    }

    public Inventory_Item FindSameItem(Inventory_Item itemToFind)
    {
        return itemList.Find(item => item.itemData == itemToFind.itemData);
    }

    public void TriggerUpdateUI() => OnInventoryChange?.Invoke();
}
