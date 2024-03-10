using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HerbEnum
{
    Ming,
    MingMing,
    MingMingMing
}


[CreateAssetMenu(menuName = "SO/Herb")]
public class HerbSO : ScriptableObject
{
    public HerbEnum herb;
    public float gatheringTime;
}
