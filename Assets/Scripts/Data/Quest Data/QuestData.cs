using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestData
{
    public QuestDataSO questDataSO;
    public int currentAmount;
    public bool canGetReward;

    public void AddQuestProgress(int amount = 1)
    {
        currentAmount = currentAmount + amount;
        canGetReward = CanGetReward();
    }

    public bool CanGetReward() => currentAmount >= questDataSO.requiredAmount;

    public QuestData(QuestDataSO questSO)
    {
        this.questDataSO = questSO;
    }
}
