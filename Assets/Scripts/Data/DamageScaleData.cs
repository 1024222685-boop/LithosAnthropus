using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DamageScaleData
{
    [Header("Damage")]
    public float physical = 1;
    public float elemental = 1;

    [Header("Mercy")]
    public float mercyDuration = 3;
    public float mercySlowMultiplier = .2f;

    [Header("Brutal")]
    public float brutalDuration = 3;
    public float brutalDamageScale = 1;

    [Header("Coward")]
    public float cowardDuration = 3;
    public float cowardDamageScale = 1;
    public float cowardCharge = .4f;
}
