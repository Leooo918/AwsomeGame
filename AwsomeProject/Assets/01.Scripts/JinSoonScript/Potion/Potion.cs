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
    public int[] level;
    public List<Effect> effects;
    public PotionTypeEnum potionType;

    protected virtual void Start()
    {
        effects = new List<Effect>();
        foreach(var effectEnum in effectEnums)
        {
            effects.Add(EffectManager.GetEffect(effectEnum));
        }
        for(int i = 0; i < effectEnums.Length; i++)
        {
            effects[i].Initialize(this, level[i]);
        }
    }
    public abstract void UsePotion();
}
