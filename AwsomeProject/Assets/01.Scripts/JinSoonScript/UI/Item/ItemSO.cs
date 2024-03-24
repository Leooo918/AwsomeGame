using UnityEngine;

public enum ItemType
{
    Ingredient = 0,
    Portion = 1
}

public class ItemSO : ScriptableObject
{
    public int id;
    public string itemName;
    [HideInInspector]public ItemType itemType;
    public int maxCarryAmountPerSlot;
    [TextArea(3,20)]
    public string itemExplain;

    [Space(20)]
    public Sprite itemImage;
    public GameObject prefab;
}
