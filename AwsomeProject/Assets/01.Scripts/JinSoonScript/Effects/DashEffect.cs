using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffect : Effect
{
    Player player;

    private float useTime = 5f;

    public override void EnterEffort(Entity target)
    {
        player = target as Player;

        player.canDash = true;

        //PlayerDashSkillSO dashSkillSO = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.Dash) as PlayerDashSkillSO;
        //dashSkillSO.CanUseSkill = true;
        //HasEffectManager.Instance.DashOn(0);

        target.StartDelayCallBack(useTime, () =>
        {
            player.canDash = false;
            //PlayerDashSkillSO dashSkillSO = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.Dash) as PlayerDashSkillSO;
            //dashSkillSO.CanUseSkill = false;
            //HasEffectManager.Instance.DashOff();
        });
    }

    public override void UpdateEffort()
    {
    }

    public override void ExitEffort()
    {
    }
}
