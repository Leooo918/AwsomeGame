using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PotionType
{
    Drink = 31,
    Throw = 37
}

[Serializable]
public struct PotionInfo
{
    public EffectTypeEnum effectEnum;
    public int level;
}

public abstract class Potion : MonoBehaviour
{
    public PotionItemSO potionItemSO;
    public List<Effect> effects;
    public int level;

    public virtual void Init(QuickSlot slot)
    {
        potionItemSO = slot.assignedItem.itemSO as PotionItemSO;
        effects = new List<Effect>();
        level = slot.assignedItem.level;
        PotionInfo[] potionInfos = potionItemSO.GetItemInfo(level);
        for (int i = 0; i < potionInfos.Length; i++)
        {
            Effect effect = EffectManager.GetEffect(potionInfos[i].effectEnum);
            effect.Initialize(this, potionInfos[i].level);
            effects.Add(effect);
        }
        slot.TryUsePotion();
    }
    public abstract void UsePotion();
}
