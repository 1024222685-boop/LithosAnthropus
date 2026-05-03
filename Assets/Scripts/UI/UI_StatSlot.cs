using TMPro;
using UnityEngine;

public class UI_StatSlot : MonoBehaviour
{
    private Entity_Stats playerStats;
    private RectTransform rect;
    private UI ui;

    [SerializeField] private StatType statSlotType;
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;

    private void OnValidate()
    {
        gameObject.name = "UI_Stat - " + GetStatNameByType(statSlotType);
        statName.text = GetStatNameByType(statSlotType);
    }

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        playerStats = FindFirstObjectByType<Entity_Stats>();
    }

    public void UpdateStatValue()
    {
        Stat statToUpdate = playerStats.GetStatByType(statSlotType);

        if (statToUpdate == null && statSlotType != StatType.ElementalDamage)
        {
            Debug.Log($"You do not have {statSlotType} implemented on the player!");
            return;
        }

        float value = 0;

        switch (statSlotType)
        {
            //Major stats
            case StatType.Strength:
                value = playerStats.major.strength.GetValue();
                break;
            case StatType.Agility:
                value = playerStats.major.agility.GetValue();
                break;
            case StatType.Intelligence:
                value = playerStats.major.intelligence.GetValue();
                break;
            case StatType.Vitality:
                value = playerStats.major.vitality.GetValue();
                break;

             //offense stats
            case StatType.Damage:
                value = playerStats.GetBaseDamage();
                break;
            case StatType.CritChance:
                value = playerStats.GetCritChance();
                break;
            case StatType.CritPower:
                value = playerStats.GetCritPower();
                break;
            case StatType.ArmorReduction:
                value = playerStats.GetArmorReduction() * 100;
                break;
            case StatType.AttackSpeed:
                value = playerStats.offense.attackSpeed.GetValue() * 100;
                break;

            // Defense stats
            case StatType.MaxHealth:
                value = playerStats.GetMaxHealth();
                break;
            case StatType.HealthRegen:
                value = playerStats.resources.healthRegen.GetValue();
                break;
            case StatType.Evasion:
                value = playerStats.GetEvasion();
                break;
            case StatType.Armor:
                value = playerStats.GetBaseArmor();
                break;

            // Elemental damage stats
            case StatType.MercyDamage:
                value = playerStats.offense.mercyDamage.GetValue();
                break;
            case StatType.BrutalityDamage:
                value = playerStats.offense.brutalityDamage.GetValue();
                break;
            case StatType.CowardiceDamage:
                value = playerStats.offense.cowardiceDamage.GetValue();
                break;
            case StatType.ElementalDamage:
                value = playerStats.GetElementalDamage(out ElementType element, 1);
                break;

            // Elemental resistance stats
            case StatType.MercyResistance:
                value = playerStats.GetElementalResistance(ElementType.Mercy) * 100;
                break;
            case StatType.BrutalityResistance:
                value = playerStats.GetElementalResistance(ElementType.Brutality) * 100;
                break;
            case StatType.CowardiceResistance:
                value = playerStats.GetElementalResistance(ElementType.Cowardice) * 100;
                break;
        }

        statValue.text = IsPercentagesStat(statSlotType) ? value + "%" : value.ToString();
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
            case StatType.ElementalDamage: return "Elemental Damage";

            case StatType.CowardiceDamage: return "Cowardice Damage";
            case StatType.Armor: return "Armor";
            case StatType.Evasion: return "Evasion";

            case StatType.MercyResistance: return "Mercy Resistance";
            case StatType.BrutalityResistance: return "Brutality Resistance";
            case StatType.CowardiceResistance: return "Cowardice Resistance";
            default: return "Unknown Stat";
        }
    }
}
