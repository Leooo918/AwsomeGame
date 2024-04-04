using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffect : Effect
{
    Player player;
    public override void EnterEffort(Entity target)
    {
        player = target as Player;

        PlayerDashSkillSO dashSkillSO = player.playerStatus.GetSkillByEnum(PlayerSkill.Dash) as PlayerDashSkillSO;
        dashSkillSO.CanUseSkill = true;
    }
}
