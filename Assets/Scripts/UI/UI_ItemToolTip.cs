using System.Text;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemInfo;

    public void ShowToolTip(bool show, RectTransform targetRect, Inventory_Item itemToShow)
    {
        base.ShowToolTip(show, targetRect);

        itemName.text = itemToShow.itemData.itemName;
        itemType.text = itemToShow.itemData.itemType.ToString();
        itemInfo.text = GetItemInfo(itemToShow);
    }

    public string GetItemInfo(Inventory_Item item)
    {
        if (item.itemData.itemType == ItemType.Material)
            return "Used for crafting.";

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("");

        foreach (var mod in item.modifiers)
        {
            string modType = GetStatNameByType(mod.statType);
            string modValue = IsPercentagesStat(mod.statType) ? mod.value.ToString() + "%" : mod.value.ToString();
            sb.AppendLine("+ " + modValue + " " + modType);
        }
        return sb.ToString();
    }

    private string GetStatNameByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return "Max Health";
            case StatType.HealthRegen: return "Health Regeneration";
            case StatType.Strength: return "Strength";
            case StatType.Agility: return "Agility";

            case StatType.Intelligence: return "Intelligence";
            case StatType.Vitality: return "Vitality";
            case StatType.AttackSpeed: return "Attack Speed";
            case StatType.Damage: return "Damage";

            case StatType.CritChance: return "Critical Chance";
            case StatType.CritPower: return "Critical Power";
            case StatType.ArmorReduction: return "Armor Reduction";
            case StatType.BrutalityDamage: return "Brutality Damage";
            case StatType.MercyDamage: return "Mercy Damage";

            case StatType.CowardiceDamage: return "Cowardice Damage";
            case StatType.Armor: return "Armor";
            case StatType.Evasion: return "Evasion";

            case StatType.MercyResistance: return "Mercy Resistance";
            case StatType.BrutalityResistance: return "Brutality Resistance";
            case StatType.CowardiceResistance: return "Cowardice Resistance";
            default: return "Unknown Stat";
        }
    }

    private bool IsPercentagesStat(StatType type)
    {
        switch (type)
        {
            case StatType.CritChance:
            case StatType.CritPower:
            case StatType.ArmorReduction:
            case StatType.MercyResistance:
            case StatType.BrutalityResistance:
            case StatType.CowardiceResistance:
            case StatType.AttackSpeed:
            case StatType.Evasion:
                return true;
            default:
                return false;
        }
    }
}
