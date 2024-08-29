using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatMoveState : EnemyState<GoatStateEnum>
{
    public GoatMoveState(Enemy<GoatStateEnum> enemy, EnemyStateMachine<GoatStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }
}
