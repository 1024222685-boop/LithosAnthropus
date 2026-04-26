using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat_OffenseGroup
{
    public Stat attackSpeed;

    //Physical Damage
    public Stat damage;
    public Stat critPower;
    public Stat critChance;
    public Stat armorReduction;

    // Elemental Damage
    public Stat cowardiceDamage;
    public Stat brutalityDamage;
    public Stat mercyDamage;
}
