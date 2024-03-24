using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private int baseValue;

    public List<int> modifiers = new List<int>();

    public int GetValue()
    {
        int finalValue = baseValue;
        foreach (int value in modifiers)
        {
            finalValue += value;
        }

        return finalValue;
    }

    public void RemoveModifier(int value)
    {
        if (value != 0)
            modifiers.Remove(value);
    }

    public void AddModifier(int value)
    {
        if (value != 0)
            modifiers.Add(value);
    }

    public void SetDefaultValue(int value)
    {
        baseValue = value;
    }
}
