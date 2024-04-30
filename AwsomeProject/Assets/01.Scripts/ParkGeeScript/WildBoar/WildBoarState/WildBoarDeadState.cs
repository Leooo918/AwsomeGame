using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBoarDeadState : EnemyState<WildBoarEnum>
{
    public WildBoarDeadState(Enemy enemy, EnemyStateMachine<WildBoarEnum> enemyStateMachine, string animBoolName) 
        : base(enemy, enemyStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        enemy.OnCompletelyDie();
    }
}
