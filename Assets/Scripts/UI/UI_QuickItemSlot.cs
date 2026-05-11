using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_QuickItemSlot : UI_ItemSlot
{
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private int slotNumber;

    public void SetupQuickSlotItem(Inventory_Item itemToPass)
    {
        inventory.SetQuickItemInSlot(slotNumber, itemToPass);
    }

    public void UpdateQuickSlotUI(Inventory_Item currentItemInSlot)
    {
        if (currentItemInSlot == null || currentItemInSlot.itemData == null)
        {
            itemIcon.sprite = defaultSprite;
            itemStacksSize.text = "";
            return;
        }

        itemIcon.sprite = currentItemInSlot.itemData.itemIcon;
        itemStacksSize.text = currentItemInSlot.stackSize.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.inGameUI.OpenQuickItemOptions(this, rect);
    }
}
