using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] private HpBottle _hpBottlePf;
    private HpBottle[] hpBottles;

    public void TakeDamage()
    {
        for (int i = hpBottles.Length - 1; i >= 0; i--)
        {
            if (hpBottles[i].IsBottleEmpty == false)
            {
                hpBottles[i].HpDown();
                break;
            }
        }
    }

    public void Init(int hp)
    {
        for (int i = 0; i < hp; i += 2)
        {
            HpBottle bottle = Instantiate(_hpBottlePf, transform);
            if (i % 2 == 1)
                bottle.SetAsHalfHp();
        }

        hpBottles = GetComponentsInChildren<HpBottle>();
    }
}
