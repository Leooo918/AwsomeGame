using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum IngredientItemType
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

[CreateAssetMenu(menuName = "SO/Doryu/Item/IngredientItem")]
public class IngredientItemSO : ItemSO
{
    [Space(20)]
    [Header("Info")]
    public IngredientItemType itemType;
    public string itemName;
    [TextArea(3, 20)]
    public string itemDescription;
    public float gatheringTime;

    public override string GetItemDescription()
    {
        return itemDescription;
    }

    public override string GetItemName()
    {
        return itemName;
    }

    public override int GetItemTypeNumber()
    {
        return (int)itemType;
    }
}