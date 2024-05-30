using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeStunState : EnemyState<KingSlimeStateEnum>
{
    public KingSlimeStunState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }
}
