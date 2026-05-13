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
    [SerializeField] protected GameObject defaultIcon;
    [SerializeField] protected Image itemIcon;
    [SerializeField] protected TextMeshProUGUI itemStacksSize;

    protected virtual void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        inventory = FindAnyObjectByType<Inventory_Player>();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (itemInslot == null || itemInslot.itemData.itemType == ItemType.Material)
            return;

        bool alternativeInput = Input.GetKey(KeyCode.LeftControl);

        if (alternativeInput)
        {
            inventory.RemoveOneItem(itemInslot);
        }
        else
        {
            if (itemInslot.itemData.itemType == ItemType.Consumable)
            {


                inventory.TryUseItem(itemInslot);
            }
            else
                inventory.TryEquipItem(itemInslot);
        }

        if (itemInslot == null)
            ui.itemToolTip.ShowToolTip(false, null);
    }

    public void UpdateSlot(Inventory_Item item)
    {
        itemInslot = item;

        if (defaultIcon != null)
            defaultIcon.gameObject.SetActive(itemInslot == null);

        if (itemInslot == null)
        {
            itemStacksSize.text = "";
            itemIcon.color = Color.clear;
            return;
        }

        Color color = Color.white; color.a = .9f;
        itemIcon.color = color;
        itemIcon.sprite = item.itemData.itemIcon;
        itemStacksSize.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
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
