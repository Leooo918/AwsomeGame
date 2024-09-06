using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    RedMushroom,
    Y_BounceMushroom,
    TrunkFruit,
    ArmoryMushroom,
    HornPigHorn,
    SpikeMushroom,
    StickyGrass,
    ShinyFruit,
    SlimeCore,
    YoungBirldWing,
}

public class ItemSO : ScriptableObject
{
    [Header("ItemData")]
    //public ItemType type;
    public int maxMergeAmount = 5;
    public ItemType itemType;
    public string itemName;
    [TextArea(3, 20)]
    public string itemExplain;

    [Space(20)]
    [Header("Sprite")]
    public Sprite dotImage;
    public Sprite image;
}