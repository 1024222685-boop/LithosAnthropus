using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int gold;

    public List<Inventory_Item> itemList;
    public SerializableDictionary<string,int>inventory;
    public SerializableDictionary<string, int> storageItems;
    public SerializableDictionary<string, int> storageMaterials;

    public SerializableDictionary<string,ItemType> equipedItems;

    public int skillPoints;
    public SerializableDictionary<string, bool> skillTreeUI;
    public SerializableDictionary<SkillType, SkillUpgradeType> skillUpgrades;

    public SerializableDictionary<string, bool> unlockedCheckpoints;
    public SerializableDictionary<string, Vector3> inScenePortals;

    public string portalDestinationSceneName;
    public bool returningFromPlace;

    public string lastScenePlayed;
    public Vector3 lastPlayerPosition;


    public GameData()
    {
        inventory = new SerializableDictionary<string,int>();
        storageItems = new SerializableDictionary<string,int>();
        storageMaterials = new SerializableDictionary<string,int>();

        equipedItems = new SerializableDictionary<string,ItemType>();

        skillTreeUI = new SerializableDictionary<string, bool>();
        skillUpgrades = new SerializableDictionary<SkillType, SkillUpgradeType>();

        unlockedCheckpoints = new SerializableDictionary<string,bool>();
        inScenePortals = new SerializableDictionary<string,Vector3>();
    }
}
