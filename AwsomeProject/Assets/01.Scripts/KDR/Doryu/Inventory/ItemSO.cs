using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    [Header("ItemData")]
    public int maxMergeAmount = 5;

    public abstract string GetItemName();
    public abstract string GetItemDescription();
    public abstract int GetItemTypeNumber();

    [Space(20)]
    [Header("Sprite")]
    public Sprite image;
}