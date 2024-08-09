using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffect : Effect
{
    Player player;

    private float accumulatedTime = 0f;
    private float useTime = 5f;

    public override void EnterEffort(Entity target)
    {
        player = target as Player;

        PlayerDashSkillSO dashSkillSO = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.Dash) as PlayerDashSkillSO;
        dashSkillSO.CanUseSkill = true;
        HasEffectManager.Instance.DashOn(0);
    }

    public override void UpdateEffort()
    {
    }

    public override void ExitEffort()
    {
        base.ExitEffort();

        PlayerDashSkillSO dashSkillSO = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.Dash) as PlayerDashSkillSO;
        dashSkillSO.CanUseSkill = false;
        HasEffectManager.Instance.DashOff();
    }
}
