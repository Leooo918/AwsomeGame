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
    //public PotionItemSO potionItemSO;
    public List<Effect> effects;

    public virtual void Init(InventorySlot slot)
    {
        //this.potionItemSO = potionItemSO;
        effects = new List<Effect>();
        PotionInfo[] potionInfos = (slot.assignedItem.itemSO as PotionItemSO).GetItemInfo();
        for (int i = 0; i < potionInfos.Length; i++)
        {
            Effect effect = EffectManager.GetEffect(potionInfos[i].effectEnum);
            effect.Initialize(this, potionInfos[i].level);
            effects.Add(effect);
        }
        slot.TrySubAmount();
    }
    public abstract void UsePotion();
}
