using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeJumpAttackState : EnemyState<KingSlimeStateEnum>
{
    public KingSlimeJumpAttackState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }
}
