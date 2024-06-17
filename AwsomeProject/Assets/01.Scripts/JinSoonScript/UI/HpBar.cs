using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    private HpBottle[] hpBottles;

    private void Awake()
    {
        hpBottles = GetComponentsInChildren<HpBottle>();
    }

    public void TakeDamage()
    {
        for(int i = hpBottles.Length-1; i >= 0; i--)
        {
            if(hpBottles[i].IsBottleEmpty == false)
            {
                hpBottles[i].HpDown();
                break;
            }
        }
    }
}
