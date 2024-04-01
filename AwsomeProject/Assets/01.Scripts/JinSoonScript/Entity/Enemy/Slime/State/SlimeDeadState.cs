using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDeadState : EnemyState<SlimeEnum>
{
    public SlimeDeadState(Enemy enemy, EnemyStateMachine<SlimeEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }


}
