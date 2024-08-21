using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityDashEffect : Effect
{
    Player player;

    private float useTime = 15f;

    public override void EnterEffort(Entity target)
    {
        player = target as Player;

        player.canDash = true;
        PlayerDashSkillSO dashSkillSO = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.Dash) as PlayerDashSkillSO;
        dashSkillSO.IsInvincibleWhileDash = true;
        HasEffectManager.Instance.DashOn(2);

        target.StartDelayCallBack(useTime, () =>
        {
            PlayerDashSkillSO dashSkillSO = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.Dash) as PlayerDashSkillSO;
            dashSkillSO.IsInvincibleWhileDash = false;
            player.canDash = false;
            HasEffectManager.Instance.DashOff();
        });
    }

    public override void UpdateEffort()
    {
        base.UpdateEffort();
    }

    public override void ExitEffort()
    {
        base.ExitEffort();
    }
}
