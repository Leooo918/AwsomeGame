using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeVinedState : EnemyState<KingSlimeStateEnum>
{
    public KingSlimeVinedState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        enemy.CanStateChangeable = false;
        Debug.Log("¹Ö¹Ö");
    }

    public override void Exit()
    {
        base.Exit();
    }
}
