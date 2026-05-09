using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MerchantSlot : UI_ItemSlot
{
    private Inventory_Merchant merchant;

    public enum MerchanSlotType { MerchantSlot, PlayerSlot }
    public MerchanSlotType slotType;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(itemInslot == null)
            return;

        bool rightButton = eventData.button == PointerEventData.InputButton.Right;
        bool leftButton = eventData.button == PointerEventData.InputButton.Left;

        if (slotType == MerchanSlotType.PlayerSlot)
        {
            if (rightButton)
            {
                bool sellfullStack = Input.GetKey(KeyCode.LeftControl);
                merchant.TrySellItem(itemInslot, sellfullStack);
            }
            else if (leftButton) 
            {
                base.OnPointerDown(eventData);
            }
        }
        else if (slotType == MerchanSlotType.MerchantSlot)
        {
            if (leftButton)
                return;

            bool buyFullStack = Input.GetKey(KeyCode.LeftControl);
            merchant.TryBuyItem(itemInslot, buyFullStack);
        }

        ui.itemToolTip.ShowToolTip(false, null);
    }

    public void SetuoMerchantUI(Inventory_Merchant merchant) => this.merchant = merchant;
}
