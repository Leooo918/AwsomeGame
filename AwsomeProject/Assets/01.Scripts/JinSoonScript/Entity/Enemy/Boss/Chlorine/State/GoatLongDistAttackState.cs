using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatLongDistAttackState : EnemyState<GoatStateEnum>
{
    public GoatLongDistAttackState(Enemy<GoatStateEnum> enemy, EnemyStateMachine<GoatStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }
}
