using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour
{
    public Inventory_Item itemInslot { get; private set; }

    [Header("UI Slot Setup")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemStacksSize;

    public void UpdateSLot(Inventory_Item item)
    {
        itemInslot = item;

        if (itemInslot == null)
        {
            itemStacksSize.text = "";
            itemIcon.sprite = null;
            itemIcon.color = Color.clear;
            return;
        }

        Color color = Color.white; color.a = .9f;
        itemIcon.color = color;
        itemIcon.sprite = item.itemData.itemIcon;

    }

}
