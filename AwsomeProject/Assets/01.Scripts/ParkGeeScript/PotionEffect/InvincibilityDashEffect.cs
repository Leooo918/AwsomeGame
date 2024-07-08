using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityDashEffect : Effect
{
    Player player;

    private float accumulatedTime = 0f;
    private float useTime = 5f;

    public override void EnterEffort(Entity target)
    {
        player = target as Player;

        PlayerDashSkillSO dashSkillSO = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.Dash) as PlayerDashSkillSO;
        dashSkillSO.CanUseSkill = true;
        dashSkillSO.IsInvincibleWhileDash = true;

        accumulatedTime = 0f;
    }

    public override void UpdateEffort()
    {
        base.UpdateEffort();

        accumulatedTime += Time.deltaTime;

        if (accumulatedTime > useTime)
        {
            ExitEffort();
        }
    }

    public override void ExitEffort()
    {
        base.ExitEffort();

        PlayerDashSkillSO dashSkillSO = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.Dash) as PlayerDashSkillSO;
        dashSkillSO.IsInvincibleWhileDash = false;
        dashSkillSO.CanUseSkill = false;
    }
}
