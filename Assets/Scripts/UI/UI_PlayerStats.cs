using UnityEngine;

public class UI_PlayerStats : MonoBehaviour
{
    private UI_StatSlot[] uiStatsSlots;
    private Inventory_Player inventory;

    private void Awake()
    {
        uiStatsSlots = GetComponentsInChildren<UI_StatSlot>();

        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateStatsUI;
    }

    private void Start()
    {
        UpdateStatsUI();
    }

    private void UpdateStatsUI()
    {
        foreach(var statSlot in uiStatsSlots)
            statSlot.UpdateStatValue();
    }
}
