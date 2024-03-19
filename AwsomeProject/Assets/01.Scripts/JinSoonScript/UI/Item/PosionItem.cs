using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosionItem : Item
{
    public PosionType posionType { get; protected set; }
    public PosionEffect posionEffect { get; protected set; }

    private void Awake()
    {
        itemType = ItemType.Posion;

        PosionItemSO p = itemSO as PosionItemSO;
        posionType = p.posionType;
        posionEffect = p.effect;
    }
}
