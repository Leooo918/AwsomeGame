using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeReadyState : EnemyState<KingSlimeStateEnum>
{
    public KingSlimeReadyState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }
}
