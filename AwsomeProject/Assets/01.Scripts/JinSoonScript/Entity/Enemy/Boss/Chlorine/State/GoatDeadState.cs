using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatDeadState : EnemyState<GoatStateEnum>
{
    public GoatDeadState(Enemy<GoatStateEnum> enemy, EnemyStateMachine<GoatStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }
}
