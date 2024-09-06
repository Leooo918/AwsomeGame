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

[CreateAssetMenu(menuName = "SO/Doryu/Item/PotionItem")]
public class PotionItemSO : ItemSO
{
    public PotionType potionType;
    [HideInInspector] public PotionInfo[] infos;
    [SerializeField] private PotionTypeInfo potionInfos;

    private void OnEnable()
    {
        infos = potionType == PotionType.Drink ?
        potionInfos.drinkInfos : potionInfos.throwInfos;
    }
}