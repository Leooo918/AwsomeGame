using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : Health
{
    public int maxPhase = 2;

    public List<float> phaseChangeHealth = new List<float>();
    public int currentPhase { get; private set; } = 0;
    //0 -> 1페이지로 전환되는 순간이라면 1이 넘어감
    public Action<int> OnChangePhase;

    public override bool TakeDamage(int damage, Vector2 knockPower, Entity dealer, bool isPersent = false)
    {
        base.TakeDamage(damage, knockPower, dealer);

        if (currentPhase < maxPhase - 1 && curHp <= phaseChangeHealth[currentPhase])
            GoToNextPhase();

        return true;
    }

    private void GoToNextPhase()
    {
        currentPhase++;

        if(currentPhase >= maxPhase)
        {
            currentPhase--;
            return;
        }

        OnChangePhase?.Invoke(currentPhase);
    }
}
