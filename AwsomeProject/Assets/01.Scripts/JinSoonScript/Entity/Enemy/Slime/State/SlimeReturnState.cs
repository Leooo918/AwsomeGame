using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeReturnState : EnemyState<SlimeEnum>
{
    public SlimeReturnState(Enemy enemy, EnemyStateMachine<SlimeEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }


}
