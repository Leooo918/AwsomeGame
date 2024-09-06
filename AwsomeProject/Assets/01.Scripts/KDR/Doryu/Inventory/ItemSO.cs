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
    public Sprite image;
}