using System;
using UnityEngine;

public enum Portion
{
    PortionForThrow,
    PortionForMyself
}

[CreateAssetMenu(menuName = "SO/Item/Portion")]
public class PortionItemSO : ItemSO
{
    [Space(20)]
    [HideInInspector] public Portion portionType;
    public Effect effect;
    [HideInInspector] public float usingTime;
    [HideInInspector] public float duration;
    [HideInInspector] public bool isInfinite;


    private void OnEnable()
    {
        itemType = ItemType.Portion;
    }
}
