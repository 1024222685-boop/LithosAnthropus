using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;
    private UI_EquipSlot[] uiEquipSlots;

    [SerializeField] private UI_ItemSlotParent inventorySlotsParent;
    [SerializeField] private Transform uiEquipSlotParent;

    private void Awake()
    {
        uiEquipSlots = uiEquipSlotParent.GetComponentsInChildren<UI_EquipSlot>();

        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateUI;

        UpdateUI();
    }

    private void UpdateUI()
    {
        inventorySlotsParent.UpdateSlots(inventory.itemList);
        UpdateEquipmentSlots();
    }

    private void UpdateEquipmentSlots()
    {
        List<Inventory_EquipmentSlot> playerEquipList = inventory.equipList;

        for (int i = 0; i < uiEquipSlots.Length; i++)
        {
            if (i < playerEquipList.Count)
            {
                var playerEquipSlot = playerEquipList[i];
                if (playerEquipSlot.HasItem() == false)
                    uiEquipSlots[i].UpdateSlot(null);
                else
                    uiEquipSlots[i].UpdateSlot(playerEquipSlot.equipedItem);
            }
            else
            {
                uiEquipSlots[i].UpdateSlot(null);
            }
        }
    }
}
