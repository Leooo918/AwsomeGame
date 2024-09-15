using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public enum PotionItemType
{
    PoisonPotion,
    WindPotion,
    HealPortion,
    BoomPotion,
    StonePotion,
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
    Level,
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
    [TextArea(3, 20)]
    public string[] itemDescriptions;
    public PotionInfos[] potionInfos;
     
    public PotionInfo[] GetItemInfo(int level = 0)
    {
        return potionInfos[level].infos;
    }
    public override string GetItemDescription(int level = 0)
    {
        return itemDescriptions[level];
    } 

    public override string GetItemName(int level = 0)
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