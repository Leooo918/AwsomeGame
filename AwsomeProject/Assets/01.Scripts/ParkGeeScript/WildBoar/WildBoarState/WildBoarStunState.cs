using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBoarStunState : EnemyState<WildBoarEnum>
{
    private float _stunStartTime;
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
        _stunStartTime = Time.time;
        enemy.CanStateChangeable = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if(Time.time > enemy.stunDuration + _stunStartTime)
        {
            enemy.CanStateChangeable = true;
            enemyStateMachine.ChangeState(WildBoarEnum.Idle);
        }
    }
}
