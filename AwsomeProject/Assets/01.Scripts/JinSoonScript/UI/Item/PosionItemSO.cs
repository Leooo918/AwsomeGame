using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PosionType
{
    PosionForThrow,
    PosionForMyself
}

[CreateAssetMenu(menuName = "SO/PosionSO")]
public class PosionItemSO : ItemSO
{
    [Space(20)]
    public PosionType posionType;
    public PosionEffect effect;

    private void Awake()
    {
        itemType = ItemType.Posion;
    }
}
