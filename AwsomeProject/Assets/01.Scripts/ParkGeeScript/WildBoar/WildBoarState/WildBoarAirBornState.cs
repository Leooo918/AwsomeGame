using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBoarAirBornState : EnemyState<WildBoarEnum>
{
    public WildBoarAirBornState(Enemy<WildBoarEnum> enemy, EnemyStateMachine<WildBoarEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.CanStateChangeable = false;
        enemy.StartDelayCallBack(enemy.airBornDuration, () =>
        {
            enemy.CanStateChangeable = true;
            enemyStateMachine.ChangeState(WildBoarEnum.Idle);
        });
    }
}
