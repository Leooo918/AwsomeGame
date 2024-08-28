using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class GoatLongDistAttack : Skill
{
    private Goat goat;
    public override void UseSkill()
    {
        goat.StateMachine.ChangeState(GoatStateEnum.LongDistAttack);
    }

    public override void SetOwner(Entity owner)
    {
        base.SetOwner(owner);
        goat = owner as Goat;
    }
}
