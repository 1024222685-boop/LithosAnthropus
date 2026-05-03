using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Inventory_Item itemInslot { get; private set; }
    protected Inventory_Player inventory;
    protected UI ui;
    protected RectTransform rect;

    [Header("UI Slot Setup")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemStacksSize;

    protected void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        inventory = FindAnyObjectByType<Inventory_Player>();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (itemInslot == null || itemInslot.itemData.itemType == ItemType.Material)
            return;

        inventory.TryEquipItem(itemInslot);

        if (itemInslot == null)
            ui.itemToolTip.ShowToolTip(false, null);
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInslot == null)
            return;

        ui.itemToolTip.ShowToolTip(true, rect, itemInslot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.itemToolTip.ShowToolTip(false, null);
    }
}
