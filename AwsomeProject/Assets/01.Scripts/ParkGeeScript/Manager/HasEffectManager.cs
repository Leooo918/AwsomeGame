using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HasEffectManager : Singleton<HasEffectManager>
{
    [SerializeField] private Image _dashEffectImg;
    [SerializeField] private Image _hardenEffectImg;

    public void DashOn()
    {
        _dashEffectImg.enabled = true;
    }

    public void DashOff()
    {
        _dashEffectImg.enabled = false;
    }

    public void HardenOn()
    {
        _hardenEffectImg.enabled = true;
    }

    public void HardenOff()
    {
        _hardenEffectImg.enabled = false;
    }
}
