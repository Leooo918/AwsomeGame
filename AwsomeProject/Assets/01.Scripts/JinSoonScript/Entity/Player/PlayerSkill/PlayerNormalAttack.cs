using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalAttack : Skill
{
    private Player player;

    public override void UseSkill()
    {
        if (player == null) player = owner as Player;

        player.StateMachine.ChangeState(PlayerStateEnum.NormalAttack);
    }
}
