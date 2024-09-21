using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBoarDeadState : EnemyState<WildBoarEnum>
{
    public WildBoarDeadState(Enemy<WildBoarEnum> enemy, EnemyStateMachine<WildBoarEnum> enemyStateMachine, string animBoolName) 
        : base(enemy, enemyStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        GameManager.Instance.killCnt++;
        enemy.colliderCompo.enabled = false;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        enemy.OnCompletelyDie();
    }
}
