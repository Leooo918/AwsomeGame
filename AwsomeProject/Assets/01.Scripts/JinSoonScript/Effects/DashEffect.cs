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
        HasEffectManager.Instance.DashOn(0);// 체력바 하단에 효과를 띄어주는 거임 HasEffectManager참고

        //PlayerDashSkillSO dashSkillSO = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.Dash) as PlayerDashSkillSO;
        //dashSkillSO.CanUseSkill = true;
        //HasEffectManager.Instance.DashOn(0);

        target.StartDelayCallBack(useTime, () =>
        {
            player.canDash = false;
            HasEffectManager.Instance.DashOff();
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
