using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeVinedState : EnemyState<KingSlimeStateEnum>
{
    public KingSlimeVinedState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }
}
