using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashSkill : Skill
{
    public float dashPower;
    public float dashTime;

    public override void UseSkill()
    {
        Player player = owner as Player;
        player.dashPower = dashPower;
        player.dashTime = dashTime;

        player.StateMachine.ChangeState(PlayerStateEnum.Dash);
    }

    public void Init(float dashPower, float dashTime)
    {
        this.dashPower = dashPower;
        this.dashTime = dashTime;
    }
}
