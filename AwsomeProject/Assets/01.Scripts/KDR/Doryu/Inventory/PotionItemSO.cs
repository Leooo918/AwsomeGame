using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PotionType
{ 
    Drink,
    Throw
}

[CreateAssetMenu(menuName = "SO/Doryu/Item/PotionItem")]
public class PotionItemSO : ItemSO
{
    public PotionType potionType;
    public PotionInfo[] potionInfos;
}