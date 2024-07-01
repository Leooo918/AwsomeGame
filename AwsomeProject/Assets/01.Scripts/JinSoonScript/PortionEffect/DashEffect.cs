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
    }

    public override void UpdateEffort()
    {
        accumulatedTime += Time.deltaTime;
        if (accumulatedTime > useTime)
        {
            accumulatedTime = 0;
            Debug.Log("»£√‚µ ");
            PlayerDashSkillSO dashSkillSO = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.Dash) as PlayerDashSkillSO;
            dashSkillSO.CanUseSkill = false;
        }
    }
}
