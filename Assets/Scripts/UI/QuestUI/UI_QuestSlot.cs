using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private Image[] rewardQuickPreviewSlots;

    public QuestDataSO questInSlot { get; private set; }
    private UI_QuestPreviw questPreviw;

    public void SetupQuestSlot(QuestDataSO questDataSO)
    {
        questPreviw = transform.root.GetComponentInChildren<UI_Quest>().GetQuestPreviw();

        questInSlot = questDataSO;
        questName.text = questDataSO.questName;

        foreach (var previwIcon in rewardQuickPreviewSlots)
        {
            previwIcon.gameObject.SetActive(false);
        }

        for(int i = 0;i < questInSlot.rewardItems.Length;i++) 
        {
            if (questDataSO.rewardItems[i] == null || questDataSO.rewardItems[i].itemData == null) continue;

            Image slot = rewardQuickPreviewSlots[i];

            slot.gameObject.SetActive(true);
            slot.sprite = questDataSO.rewardItems[i].itemData.itemIcon;
            slot.GetComponentInChildren<TextMeshProUGUI>().text = questDataSO.rewardItems[i].stackSize.ToString();
        }
    }

    public void UpdateQuestPreviw()
    {
        questPreviw.SetupQuestPreviw(questInSlot);
    }
}
