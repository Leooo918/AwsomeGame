using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public enum PotionType
{ 
    Drink,
    Throw
}

[Serializable]
public struct PotionTypeInfo
{
    public PotionInfo[] throwInfos;
    public PotionInfo[] drinkInfos;
}

public enum PotionItemType
{
    PoisonPotion_Throw,
    FailurePotion_Throw,
    HealPortion_Throw,
    BoomPotion_Throw,
    StonPotion_Throw,
    ShapeHornePotion_Throw,
    SpikePotion_Throw,
    SlapPotion_Throw,
    LightPotion_Throw,
    WeakPotion_Throw,
    PoisonPotion_Drink,
    FailurePotion_Drink,
    HealPortion_Drink,
    BoomPotion_Drink,
    StonPotion_Drink,
    ShapeHornePotion_Drink,
    SpikePotion_Drink,
    SlapPotion_Drink,
    LightPotion_Drink,
    WeakPotion_Drink,
}

[CreateAssetMenu(menuName = "SO/Doryu/Item/PotionItem")]
public class PotionItemSO : ItemSO
{
    [Space(20)]
    [Header("Info")]
    public PotionItemType itemType;
    public PotionType potionType;
    public int level = 0;
    public string itemName;
    [TextArea(3, 20)]
    public string[] itemDescriptions;
    [HideInInspector] public PotionInfo[] infos;
    [SerializeField] private PotionTypeInfo potionInfos;
     
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

    private void OnEnable()
    {
        infos = potionType == PotionType.Drink ?
        potionInfos.drinkInfos : potionInfos.throwInfos;
    }
}