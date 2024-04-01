using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalAttack : Skill
{
    private Player player;

    private Stat firstAttack;
    private Stat secondAttack;

    public override void UseSkill()
    {
        if (player == null) player = owner as Player;

        player.StateMachine.ChangeState(PlayerStateEnum.NormalAttack);
    }

    public void Init(Stat firstAttack, Stat secondAttack)
    {
        this.firstAttack = firstAttack;
        this.secondAttack = secondAttack;
    }
}
