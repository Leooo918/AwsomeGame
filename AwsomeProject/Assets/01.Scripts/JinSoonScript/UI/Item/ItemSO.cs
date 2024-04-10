using UnityEngine;

public enum ItemType
{
    Ingredient = 0,
    Portion = 1
}

public class ItemSO : ScriptableObject
{
    [HideInInspector]public int id;
    [HideInInspector] public string itemName;
    [HideInInspector]public ItemType itemType;
    [HideInInspector] public int maxCarryAmountPerSlot;
    [TextArea(3,20)]
    [HideInInspector] public string itemExplain;

    [Space(20)]
    public Sprite dotImage;
    public Sprite itemImage;
    public GameObject prefab;
}
