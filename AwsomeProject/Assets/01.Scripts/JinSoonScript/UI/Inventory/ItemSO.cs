using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Matarial = 0,
    Posion = 1
}

[CreateAssetMenu(menuName = "SO/ItemSO")]
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
