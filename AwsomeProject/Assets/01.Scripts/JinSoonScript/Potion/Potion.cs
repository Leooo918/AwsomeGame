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
    public EffectTypeEnum effectEnum;
    public int level;
}

public abstract class Potion : MonoBehaviour
{
    public PotionInfo[] potionInfos;
    public List<Effect> effects;
    public PotionTypeEnum potionType;

    protected virtual void Start()
    {
        effects = new List<Effect>();
        for(int i = 0; i < potionInfos.Length; i++)
        {
            Effect effect = EffectManager.GetEffect(potionInfos[i].effectEnum);
            effect.Initialize(this, potionInfos[i].level);
            effects.Add(effect);
        }
    }
    public abstract void UsePotion();
}
