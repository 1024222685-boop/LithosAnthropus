using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    public Inventory_Item itemInslot { get; private set; }
    protected Inventory_Player inventory;

    [Header("UI Slot Setup")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemStacksSize;

    protected void Awake()
    {
        inventory = FindAnyObjectByType<Inventory_Player>();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (itemInslot == null)
            return;

        inventory.TryEquipItem(itemInslot);
    }

    public void UpdateSlot(Inventory_Item item)
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
        itemStacksSize.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
    }
}
