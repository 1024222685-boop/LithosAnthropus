using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>();

    private bool needToCalculator = true;
    private float finalValue;

    public float GetValue()
    {
        if (needToCalculator)
        {
            finalValue = GetFinalValue();
            needToCalculator = false;
        }

        return finalValue;
    }

    public void AddModifier(float value, string source)
    {
        StatModifier modToAdd = new StatModifier(value, source);
        modifiers.Add(modToAdd);
        needToCalculator = true;
    }

    public void RemoveModifier(string source)
    {
        modifiers.RemoveAll(modifier => modifier.source == source);
        needToCalculator = true;
    }

    private float GetFinalValue()
    {
        float finalValue = baseValue;

        foreach (var modifier in modifiers)
        {
            finalValue = finalValue + modifier.value;
        }

        return finalValue;
    }

    public void SetBaseValue(float value) => baseValue = value;
}

[Serializable]
public class StatModifier
{
    public float value;
    public string source;

    public StatModifier(float value, string source)
    {
        this.value = value;
        this.source = source;
    }
}