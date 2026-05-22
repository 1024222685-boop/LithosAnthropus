using UnityEngine.EventSystems;

public class UI_QuestRewardSlot : UI_ItemSlot
{
    public override void OnPointerDown(PointerEventData eventData)
    {

    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInslot == null)
            return;

        ui.itemToolTip.ShowToolTip(true, rect, itemInslot, false, false, false);
    }
}
