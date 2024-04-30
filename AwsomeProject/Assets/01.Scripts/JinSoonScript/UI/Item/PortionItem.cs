using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionItem : Item
{
    public Portion posionType { get; protected set; }
    public Effect posionEffect { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        itemType = ItemType.Portion;

        PortionItemSO p = itemSO as PortionItemSO;
        posionType = p.portionType;
        //posionEffect = EffectManager.Instance.GetEffect(p.effect);
    }
}
