using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Craft : MonoBehaviour
{
    private UI_CraftSlot[] craftSlots;
    private UI_CraftListButton[] craftListButtons;

    private void Awake()
    {
        SetupCraftListButton();
    }

    private void SetupCraftListButton()
    {
        craftSlots = GetComponentsInChildren<UI_CraftSlot>();
        craftListButtons = GetComponentsInChildren<UI_CraftListButton>();

        foreach(var slot in craftSlots)
            slot.gameObject.SetActive(false);

        foreach(var button in craftListButtons)
            button.SetCraftSlots(craftSlots);
    }
}
