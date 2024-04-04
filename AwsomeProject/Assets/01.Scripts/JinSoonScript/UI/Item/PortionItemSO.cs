using System.Collections;
using System.Collections.Generic;
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
    public Portion portionType;
    public Effect effect;
    public float duration;
    public bool isInfinite;


    private void OnEnable()
    {
        itemType = ItemType.Portion;
    }
}
