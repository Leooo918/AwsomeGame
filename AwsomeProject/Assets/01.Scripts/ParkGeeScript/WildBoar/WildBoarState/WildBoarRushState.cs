using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBoarRushState : EnemyState<WildBoarEnum>
{
    public WildBoarRushState(Enemy<WildBoarEnum> enemy, EnemyStateMachine<WildBoarEnum> enemyStateMachine, string animBoolName) 
        : base(enemy, enemyStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();
    }
}
