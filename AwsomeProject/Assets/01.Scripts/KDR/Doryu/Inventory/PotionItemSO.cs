using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public enum PotionItemType
{
    PoisonPotion,
    FailurePotion,
    HealPortion,
    BoomPotion,
    StonPotion,
    ShapeHornePotion,
    SpikePotion,
    SlapPotion,
    LightPotion,
    WeakPotion,
}

public enum QuickSlotOutLine
{
    Bronze,
    Silver,
    Gold,
}

[Serializable]
public struct PotionInfos
{
    public PotionInfo[] infos;
}

public class PotionItemSO : ItemSO
{
    [Space(20)]
    [Header("Info")]
    public PotionItemType itemType;
    public QuickSlotOutLine quickSlotOutLine;
    public string itemName;
    public int level = 0;
    [TextArea(3, 20)]
    public string[] itemDescriptions;
    public PotionInfos[] potionInfos;
     
    public PotionInfo[] GetItemInfo()
    {
        return potionInfos[level].infos;
    }
    public override string GetItemDescription()
    {
        return itemDescriptions[level];
    }

    public override string GetItemName()
    {
        if (level != 0)
        {
            return $"{itemName}+{level}";
        }
        return itemName;
    }

    public override int GetItemTypeNumber()
    {
        return (int)itemType;
    }
}