using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingJumpAttackSkill : Skill
{
    private KingSlime kingSlime;
    public override void UseSkill()
    {
        kingSlime.StateMachine.ChangeState(KingSlimeStateEnum.JumpAttack);
    }

    public override void SetOwner(Entity owner)
    {
        base.SetOwner(owner);
        kingSlime = owner as KingSlime;
    }
}
