using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StorageSlot : UI_ItemSlot
{
    private Inventory_Storage storage;

    public enum StorageSlotType { StorageSlot,PlayerInventorySlot }
    public StorageSlotType slotType;
    public void SetStorage(Inventory_Storage storage) => this.storage = storage;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(itemInslot == null)
            return;

        if (slotType == StorageSlotType.StorageSlot)
            storage.FromStorageToPlayer(itemInslot);

        if (slotType == StorageSlotType.PlayerInventorySlot)
            storage.FromPlayerToStorage(itemInslot);

        ui.itemToolTip.ShowToolTip(false, null);
    }
}
