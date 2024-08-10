using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash2Effect : Effect
{
    Player player;

    private float accumulatedTime = 0f;
    private float useTime = 10f;

    public override void EnterEffort(Entity target)
    {
        player = target as Player;

        PlayerDashSkillSO dashSkillSO = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.Dash) as PlayerDashSkillSO;
        dashSkillSO.CanUseSkill = true;
        HasEffectManager.Instance.DashOn(1);

        target.StartDelayCallBack(useTime, () =>
        {
            PlayerDashSkillSO dashSkillSO = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.Dash) as PlayerDashSkillSO;
            dashSkillSO.CanUseSkill = false;
            HasEffectManager.Instance.DashOff();
        });
    }
}
