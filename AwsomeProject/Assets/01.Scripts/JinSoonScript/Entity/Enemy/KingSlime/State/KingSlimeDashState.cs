using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeDashState : EnemyState<KingSlimeStateEnum>
{
    public KingSlimeDashState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }
}
