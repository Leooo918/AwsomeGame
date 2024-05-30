using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeMucusAttackState : EnemyState<KingSlimeStateEnum>
{
    public KingSlimeMucusAttackState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }
}
