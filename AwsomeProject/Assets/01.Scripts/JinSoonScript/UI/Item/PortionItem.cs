using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doryu.Inventory;

public class PortionItem : Item
{
    //public Portion portionType { get; protected set; }
    public Effect portionEffect { get; protected set; }
    public Sprite portionSprite { get; protected set; }

    //protected override void Awake()
    //{
    //    base.Awake();
    //    itemType = ItemType.Potion;

    //    PortionItemSO p = itemSO as PortionItemSO;
    //    portionType = p.portionType;
    //    portionSprite = p.dotImage;
    //    portionEffect = EffectManager.Instance.GetEffect(p.effect);
    //}
}
