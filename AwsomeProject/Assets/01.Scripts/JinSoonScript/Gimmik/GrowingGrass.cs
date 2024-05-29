using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingGrass : MonoBehaviour, IGetPortionEffect
{
    //private 

    public void GetEffort(Effect effect)
    {
        GrowthEffect growth = effect as GrowthEffect;
        if (growth == null) return;


    }
}
