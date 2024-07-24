using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeDisableState : EnemyState<KingSlimeStateEnum>
{
    public KingSlimeDisableState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }
}
