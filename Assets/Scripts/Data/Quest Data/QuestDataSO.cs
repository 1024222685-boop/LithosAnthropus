using UnityEditor;
using UnityEngine;

public enum RewardType { Merchant, blackSmith, None }

[CreateAssetMenu(menuName = "RPG Setup/ Quest Data/ New Quest", fileName = "Quest - ")]
public class QuestDataSO : ScriptableObject
{
    public string questSaveId;
    [Space]
    public string questName;
    [TextArea] public string description;
    [TextArea] public string questGoal;

    public string questTargetId;
    public int requiredAmount;

    [Header("Reward")]
    public RewardType rewardType;
    public Inventory_Item[] rewardItems;

    private void OnValidate()
    {

#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        questSaveId = AssetDatabase.AssetPathToGUID(path);
#endif
    }
}
