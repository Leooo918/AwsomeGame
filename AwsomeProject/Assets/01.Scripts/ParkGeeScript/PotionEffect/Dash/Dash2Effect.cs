using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash2Effect : Effect
{
    Player player;

    private float useTime = 10f;

    public override void EnterEffort(Entity target)
    {
        player = target as Player;

        player.canDash = (true);
        HasEffectManager.Instance.DashOn(1);

        target.StartDelayCallBack(useTime, () =>
        {
            player.canDash = (false);
            HasEffectManager.Instance.DashOff();
        });
    }
}
