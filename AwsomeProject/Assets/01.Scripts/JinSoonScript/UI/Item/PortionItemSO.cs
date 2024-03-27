using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Portion
{
    PortionForThrow,
    PortionForMyself
}

[CreateAssetMenu(menuName = "SO/Item/PosionSO")]
public class PortionItemSO : ItemSO
{
    [Space(20)]
    public Portion portionType;
    public Effect effect;

    private void Awake()
    {
        itemType = ItemType.Portion;
    }
}
