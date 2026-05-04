using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatToolTip : UI_ToolTip
{
    private Player_Stats playerStats;
    private TextMeshProUGUI statToolTipText;

    protected override void Awake()
    {
        base.Awake();
        playerStats = FindFirstObjectByType<Player_Stats>();
        statToolTipText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowToolTip(bool show, RectTransform targetRect,StatType statType)
    {
        base.ShowToolTip(show, targetRect);
        statToolTipText.text = GetStatTextByType(statType);
    }

    public string GetStatTextByType(StatType type)
    {
        switch (type)
        {
            // Major stats
            case StatType.Strength:
                return "Increases physical damage by 1 per point." +
                       "\n Increases critical power by 0.5% per point.";
            case StatType.Agility:
                return "Increases critical chance by 0.3% per point." +
                       "\n Increases evasion by 0.5% per point.";
            case StatType.Intelligence:
                return "Increases elemental resistances by 0.5% per point." +
                       "\n Adds 1 elemental damage per point as a bonus." +
                       "\n If all elements have 0 damage, the bonus will not be applied.";
            case StatType.Vitality:
                return "Increases maximum health by 5 per point" +
                       "\n Increases armor by 1 per point.";

            // Physical Damage Stats
            case StatType.Damage:
                return "Determines the physical damage of your attacks.";
            case StatType.CritChance:
                return "Chance for your attacks to critically strike.";
            case StatType.CritPower:
                return "Increases the damage dealt by critical strikes.";
            case StatType.ArmorReduction:
                return "Percent of armor that will be ignored by your attacks.";
            case StatType.AttackSpeed:
                return "Determines how quickly you can attack.";

            // Defense Stats
            case StatType.MaxHealth:
                return "Determines how much total health you have.";
            case StatType.HealthRegen:
                return "Amount of health restored per second.";
            case StatType.Armor:
                return "Reduces incoming physical damage."
                    + "\n Armor mitigation is Limited at 85%."
                    + "Current mitigation is: " + playerStats.GetArmorMitigation(0) * 100 + "%.";
            case StatType.Evasion:
                return "Chance to completely avoid attacks." + "\n Limited at 85%.";

            // Elemental Damage
            case StatType.MercyDamage:
                return "Determines the mercy damage of your attacks.";
            case StatType.BrutalityDamage:
                return "Determines the brutality damage of your attacks.";
            case StatType.CowardiceDamage:
                return "Determines the cowardice damage of your attacks.";
            case StatType.ElementalDamage:
                return
                    "Elemental damage combines all three elements. " +
                    "\n The highest element applies corresponding element status effect and full damage." +
                    "\n The other two elements contribute 50% of their damage as a bonus.";

            // Elemental Resistances
            case StatType.MercyResistance:
                return "Reduces mercy damage taken.";
            case StatType.BrutalityResistance:
                return "Reduces brutality damage taken.";
            case StatType.CowardiceResistance:
                return "Reduces cowardice damage taken.";

            default:
                return "No tooltip available for this stat.";
        }
    }
}
