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
    public PosionType posionType;
    public PosionEffect effect;
}
