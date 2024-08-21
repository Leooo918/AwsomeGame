using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityDash3Effect : Effect
{
    Player player;

    private float useTime = 60f;

    public override void EnterEffort(Entity target)
    {
        player = target as Player;

        player.canDash = true;
        PlayerDashSkillSO dashSkillSO = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.Dash) as PlayerDashSkillSO;
        dashSkillSO.IsInvincibleWhileDash = true;
        HasEffectManager.Instance.DashOn(4);

        target.StartDelayCallBack(useTime, () =>
        {
            PlayerDashSkillSO dashSkillSO = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.Dash) as PlayerDashSkillSO;
            dashSkillSO.IsInvincibleWhileDash = false;
            player.canDash = false;
            HasEffectManager.Instance.DashOff();
        });
    }
}
