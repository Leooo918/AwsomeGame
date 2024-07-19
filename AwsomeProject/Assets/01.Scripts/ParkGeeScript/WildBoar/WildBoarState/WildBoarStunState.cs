using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBoarStunState : EnemyState<WildBoarEnum>
{
    public WildBoarStunState(Enemy<WildBoarEnum> enemy, EnemyStateMachine<WildBoarEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        enemy.CanStateChangeable = false;
        enemy.StartDelayCallBack(enemy.stunDuration, () =>
        {
            enemy.CanStateChangeable = true;
            enemyStateMachine.ChangeState(WildBoarEnum.Idle);
            if (enemy.healthCompo.curHp < 0)
            {
                enemyStateMachine.ChangeState(WildBoarEnum.Dead);
            }
        });
    }
}
