using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PotionTypeEnum
{
    Drink,
    Throw
}

[Serializable]
public struct PotionInfo
{
    public EffectTypeEnum[] effectEnums;
    public int[] levels;
}

public abstract class Potion : MonoBehaviour
{
    public PotionInfo potionInfo;
    public List<Effect> effects;
    public PotionTypeEnum potionType;

    protected virtual void Start()
    {
        effects = new List<Effect>();
        foreach (var effectEnum in potionInfo.effectEnums)
        {
            effects.Add(EffectManager.GetEffect(effectEnum));
        }
        for(int i = 0; i < effects.Count; i++)
        {
            effects[i].Initialize(this, potionInfo.levels[i]);
        }
    }
    public abstract void UsePotion();
}
