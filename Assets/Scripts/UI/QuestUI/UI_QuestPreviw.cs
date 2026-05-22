using TMPro;
using UnityEngine;

public class UI_QuestPreviw : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private TextMeshProUGUI questDescription;
    [SerializeField] private TextMeshProUGUI questGoal;
    [SerializeField] private UI_QuestRewardSlot[] questReward;

    [SerializeField] private GameObject[] additionalObjects;

    public void SetupQuestPreviw(QuestDataSO questDataSO)
    {
        EnableAdditionalObjects(true);
        EnableQuestRewardObjects(false);

        questName.text = questDataSO.questName;
        questDescription.text = questDataSO.description;
        questGoal.text = questDataSO.questGoal;

        for (int i = 0; i < questDataSO.rewardItems.Length; i++)
        {
            questReward[i].gameObject.SetActive(true);
            questReward[i].UpdateSlot(questDataSO.rewardItems[i]);
        }
    }

    private void MakeQuestPreviwEmpty()
    {
        questName.text = "";
        questDescription.text = "";

        EnableAdditionalObjects(false);
        EnableQuestRewardObjects(false);
    }

    private void EnableAdditionalObjects(bool enable)
    {
        foreach (var obj in additionalObjects)
            obj.SetActive(enable);
    }

    private void EnableQuestRewardObjects(bool enable)
    {
        foreach (var obj in questReward)
            obj.gameObject.SetActive(enable);
    }
}
