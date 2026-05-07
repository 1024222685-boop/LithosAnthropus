using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UI_CraftSlot : MonoBehaviour
{
    private ItemDataSO itemToCraft;

    [SerializeField] private Image craftItemIcon;
    [SerializeField] private TextMeshProUGUI craftItemName;

    public void SetupButton(ItemDataSO craftData)
    {
        this.itemToCraft = craftData;
        craftItemIcon.sprite = craftData.itemIcon;
        craftItemName.text = craftData.itemName;
    }
}
