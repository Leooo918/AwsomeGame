using UnityEngine;

public class NormalHealPortionEffect : Effect
{
    private int healAmount = 50;

    public override void EnterEffort(Entity target)
    {
        target.healthCompo.GetHeal(healAmount);
    }
}

