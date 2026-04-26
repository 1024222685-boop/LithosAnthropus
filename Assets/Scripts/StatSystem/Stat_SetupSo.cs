using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Default Stat Setup", fileName = "default Stat Setup")]
public class Stat_SetupSO : ScriptableObject
{
    [Header("Resources")]
    public float maxHealth = 100;
    public float healthRegen;

    [Header("Offense - Physical Damage")]
    public float attackSpeed = 1;
    public float damage = 10;
    public float critChance;
    public float critPower = 150;
    public float armorReduction;

    [Header("Offense - Elemental Damage")]
    public float brutalityDamage;
    public float mercyDamage;
    public float cowardiceDamage;

    [Header("Defense - Physical Damage")]
    public float armor;
    public float evasion;

    [Header("Defense - Elemental Damage")]
    public float brutalityResistance;
    public float mercyResistance;
    public float cowardiceResistance;

    [Header("Major Stats")]
    public float strength;
    public float agility;
    public float intelligence;
    public float vitality;
}