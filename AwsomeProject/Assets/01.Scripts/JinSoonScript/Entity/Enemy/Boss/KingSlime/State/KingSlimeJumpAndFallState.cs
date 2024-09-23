using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeJumpAndFallState : EnemyState<KingSlimeStateEnum>
{
    public KingSlimeJumpAndFallState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }
}
