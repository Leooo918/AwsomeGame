using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatIdleState : EnemyState<GoatStateEnum>
{
    public GoatIdleState(Enemy<GoatStateEnum> enemy, EnemyStateMachine<GoatStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }
}
