using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat_SetupSO defaultStatsetup;

    public Stat_ResourceGroup resources;
    public Stat_OffenseGroup offense;
    public Stat_DefenceGroup defense;
    public Stat_MajorGroup major;

    protected virtual void Awake()
    {
      
    }

    public void AdjustStatSetpup(Stat_ResourceGroup resourceGroup, Stat_OffenseGroup offenseGroup, Stat_DefenceGroup defenceGroup, float penalty,float increase)
    {
        offense.damage.SetBaseValue(offenseGroup.damage.GetValue() * increase);
        offense.attackSpeed.SetBaseValue(offenseGroup.attackSpeed.GetValue() * increase);
        offense.critChance.SetBaseValue(offenseGroup.critChance.GetValue() * increase);
        offense.critPower.SetBaseValue(offenseGroup.critPower.GetValue() * increase);
        offense.brutalityDamage.SetBaseValue(offenseGroup.brutalityDamage.GetValue() * increase);
        offense.mercyDamage.SetBaseValue(offenseGroup.mercyDamage.GetValue() * increase);
        offense.cowardiceDamage.SetBaseValue(offenseGroup.cowardiceDamage.GetValue() * increase);

        defense.evasion.SetBaseValue(defenceGroup.evasion.GetValue() * increase);

        resources.maxHealth.SetBaseValue(resourceGroup.maxHealth.GetValue() * penalty);
        resources.healthRegen.SetBaseValue(resourceGroup.healthRegen.GetValue() * penalty);

        defense.armor.SetBaseValue(defenceGroup.armor.GetValue() * penalty);
        defense.cowardiceRes.SetBaseValue(defenceGroup.cowardiceRes.GetValue() * penalty);
        defense.brutalityRes.SetBaseValue(defenceGroup.brutalityRes.GetValue() * penalty);
        defense.mercyRes.SetBaseValue(defenceGroup.mercyRes.GetValue() * penalty);
    }

    public AttackData GetAttackData(DamageScaleData scaleData)
    {
        return new AttackData(this, scaleData);
    }

    public float GetElementalDamage(out ElementType element, float scaleFactor = 1)
    {
        float cowardiceDamage = offense.cowardiceDamage.GetValue();
        float brutalityDamage = offense.brutalityDamage.GetValue();
        float mercyDamage = offense.mercyDamage.GetValue();
        float bonusElementalDamage = major.intelligence.GetValue();

        float highestDamage = brutalityDamage;
        element = ElementType.Brutality;

        if (cowardiceDamage > highestDamage)
        {
            highestDamage = cowardiceDamage;
            element = ElementType.Cowardice;
        }

        if (mercyDamage > highestDamage)
        {
            highestDamage = mercyDamage;
            element = ElementType.Mercy;
        }

        if (highestDamage <= 0)
        {
            element = ElementType.None;
            return 0;
        }

        float bonusBrutality = (element == ElementType.Brutality) ? 0 : brutalityDamage * .5f;
        float bonusMercy = (element == ElementType.Mercy) ? 0 : mercyDamage * .5f;
        float bonusCoward = (element == ElementType.Cowardice) ? 0 : cowardiceDamage * .5f;

        float weakerElementsDamage = bonusBrutality + bonusMercy + bonusCoward;
        float finalDamage = highestDamage + weakerElementsDamage + bonusElementalDamage;

        return finalDamage * scaleFactor;
    }

    public float GetElementalResistance(ElementType element)
    {
        float baseResistance = 0;
        float bonusResistance = major.intelligence.GetValue() * .5f;

        switch (element)
        {
            case ElementType.Brutality:
                baseResistance = defense.brutalityRes.GetValue();
                break;
            case ElementType.Mercy:
                baseResistance = defense.mercyRes.GetValue();
                break;
            case ElementType.Cowardice:
                baseResistance = defense.cowardiceRes.GetValue();
                break;
        }

        float resistance = baseResistance + bonusResistance;
        float resistanceCap = 75f;
        float finalResistance = Mathf.Clamp(resistance, 0, resistanceCap) / 100;

        return finalResistance;
    }

    public float GetPhysicalDamage(out bool isCrit, float scaleFactor = 1)
    {
        float baseDamge = GetBaseDamage();
        float critChance = GetCritChance();
        float critPower = GetCritPower() / 100;//Total crit power multiplier(e.g 150 / 100 =1.5f - multiplier)

        isCrit = Random.Range(0, 100) < critChance;
        float finalDamage = isCrit ? baseDamge * critPower : baseDamge;

        return finalDamage * scaleFactor;
    }

    public float GetBaseDamage() => offense.damage.GetValue() + major.strength.GetValue();
    public float GetCritChance() => offense.critChance.GetValue() + (major.agility.GetValue() * .3f);
    public float GetCritPower() => offense.critPower.GetValue() + (major.strength.GetValue() * .5f);

    public float GetArmorMitigation(float armorReduction)
    {
        float totalArmor = GetBaseArmor();

        float reductionMutiplier = Mathf.Clamp(1 - armorReduction, 0, 1);
        float effectiveArmor = totalArmor * reductionMutiplier;

        float mitigation = effectiveArmor / (effectiveArmor + 100);
        float mitigationCap = .85f;

        float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);

        return finalMitigation;
    }

    public float GetBaseArmor() => defense.armor.GetValue() + major.vitality.GetValue();

    public float GetArmorReduction()
    {
        float finalReduction = offense.armorReduction.GetValue();

        return finalReduction;
    }

    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * .5f;

        float totlaEvasion = baseEvasion + bonusEvasion;
        float evasionCap = 85f;//Evasion will be capped at 85%

        float finalEvasion = Mathf.Clamp(totlaEvasion, 0, evasionCap);

        return finalEvasion;
    }
    public float GetMaxHealth()
    {
        float baseMaxHealth = resources.maxHealth.GetValue();
        float bonusMaxHealth = major.vitality.GetValue() * 5;
        float finalMaxHealth = baseMaxHealth + bonusMaxHealth;

        return finalMaxHealth;
    }

    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth:
                return resources.maxHealth;
            case StatType.HealthRegen:
                return resources.healthRegen;

            case StatType.Strength:
                return major.strength;
            case StatType.Agility:
                return major.agility;
            case StatType.Intelligence:
                return major.intelligence;
            case StatType.Vitality:
                return major.vitality;

            case StatType.AttackSpeed:
                return offense.attackSpeed;
            case StatType.Damage:
                return offense.damage;
            case StatType.CritChance:
                return offense.critChance;
            case StatType.CritPower:
                return offense.critPower;
            case StatType.ArmorReduction:
                return offense.armorReduction;

            case StatType.BrutalityDamage:
                return offense.brutalityDamage;
            case StatType.MercyDamage:
                return offense.mercyDamage;
            case StatType.CowardiceDamage:
                return offense.cowardiceDamage;

            case StatType.Armor:
                return defense.armor;
            case StatType.Evasion:
                return defense.evasion;

            case StatType.MercyResistance:
                return defense.mercyRes;
            case StatType.BrutalityResistance:
                return defense.mercyRes;
            case StatType.CowardiceResistance:
                return defense.cowardiceRes;

            default:
                Debug.LogWarning($"StatType {type} not implemented yet.");
                return null;
        }
    }

    [ContextMenu("Update Default Stat Setup")]
    public void ApplyDefaultStatSetup()
    {
        if (defaultStatsetup == null)
        {
            Debug.Log("No default stat setup assigned");
            return;
        }

        resources.maxHealth.SetBaseValue(defaultStatsetup.maxHealth);
        resources.healthRegen.SetBaseValue(defaultStatsetup.healthRegen);

        major.strength.SetBaseValue(defaultStatsetup.strength);
        major.agility.SetBaseValue(defaultStatsetup.agility);
        major.intelligence.SetBaseValue(defaultStatsetup.intelligence);
        major.vitality.SetBaseValue(defaultStatsetup.vitality);

        offense.attackSpeed.SetBaseValue(defaultStatsetup.attackSpeed);
        offense.damage.SetBaseValue(defaultStatsetup.damage);
        offense.critChance.SetBaseValue(defaultStatsetup.critChance);
        offense.critPower.SetBaseValue(defaultStatsetup.critPower);
        offense.armorReduction.SetBaseValue(defaultStatsetup.armorReduction);

        offense.mercyDamage.SetBaseValue(defaultStatsetup.mercyDamage);
        offense.brutalityDamage.SetBaseValue(defaultStatsetup.brutalityDamage);
        offense.cowardiceDamage.SetBaseValue(defaultStatsetup.cowardiceDamage);

        defense.armor.SetBaseValue(defaultStatsetup.armor);
        defense.evasion.SetBaseValue(defaultStatsetup.evasion);

        defense.mercyRes.SetBaseValue(defaultStatsetup.mercyResistance);
        defense.brutalityRes.SetBaseValue(defaultStatsetup.brutalityDamage);
        defense.cowardiceRes.SetBaseValue(defaultStatsetup.cowardiceDamage);
    }
}
