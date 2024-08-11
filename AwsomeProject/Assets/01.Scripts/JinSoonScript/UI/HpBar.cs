using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] private HpBottle _hpBottlePf;
    private int _currentHealth;
    private HpBottle[] hpBottles;
    private Health _playerHealth;

    private void Start()
    {
        _playerHealth = PlayerManager.Instance.Player.healthCompo;
    }

    public void TakeDamage()
    {
        int healthTmp = _currentHealth;
        _currentHealth = (int)_playerHealth.curHp;
        for (int i = hpBottles.Length - 1; i >= 0; i--)
        {
            if (hpBottles[i].IsBottleEmpty == false)
            {
                for (int j = 0; j < healthTmp - _currentHealth; j++)
                {
                    hpBottles[i].HpDown();
                    if(hpBottles[i].IsBottleEmpty) i--;
                }
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
        _currentHealth = hp;
    }
}
