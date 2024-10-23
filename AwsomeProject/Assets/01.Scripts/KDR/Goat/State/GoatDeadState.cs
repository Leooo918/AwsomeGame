using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatDeadState : EnemyState<GoatEnum>
{
    public GoatDeadState(Enemy<GoatEnum> enemy, EnemyStateMachine<GoatEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }
}
