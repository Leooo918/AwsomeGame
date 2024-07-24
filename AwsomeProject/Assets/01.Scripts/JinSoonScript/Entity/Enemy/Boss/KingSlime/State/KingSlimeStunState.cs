using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeStunState : EnemyState<KingSlimeStateEnum>
{
    private float _stunStartTime;

    public KingSlimeStunState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    { }

    public override void Enter()
    {
        base.Enter();
        _stunStartTime = Time.time;
        enemy.CanStateChangeable = false;

        enemy.StopImmediately(false);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if(Time.time - _stunStartTime > enemy.stunDuration)
        {
            enemy.CanStateChangeable = true;
            enemyStateMachine.ChangeState(KingSlimeStateEnum.Ready);
        }
    }
}
