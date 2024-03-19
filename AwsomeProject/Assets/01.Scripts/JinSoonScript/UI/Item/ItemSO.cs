using UnityEngine;

public enum ItemType
{
    Ingredient = 0,
    Posion = 1
}

public class ItemSO : ScriptableObject
{
    public int id;
    public string itemName;
    public ItemType itemType;
    public int maxCarryAmountPerSlot;
    public string itemExplain;

    public Sprite itemImage;
    public GameObject prefab;
}
