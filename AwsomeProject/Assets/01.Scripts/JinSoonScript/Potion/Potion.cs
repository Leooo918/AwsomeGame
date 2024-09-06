using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PotionTypeEnum
{
    Drink,
    Throw
}

public abstract class Potion : MonoBehaviour
{
    public EffectTypeEnum[] effectEnums; 
    public List<Effect> effects;
    public PotionTypeEnum potionType;
    public int level;

    protected virtual void Start()
    {
        effects = new List<Effect>();
        foreach(var effectEnum in effectEnums)
        {
            effects.Add(EffectManager.GetEffect(effectEnum));
        }
        foreach (var e in effects)
        {
            e.Initialize(this);
        }
    }
    public abstract void UsePotion();
}
