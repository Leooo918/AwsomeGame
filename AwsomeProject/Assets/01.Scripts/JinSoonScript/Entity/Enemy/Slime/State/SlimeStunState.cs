using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStunState : EnemyState<SlimeEnum>
{
    public SlimeStunState(Enemy enemy, EnemyStateMachine<SlimeEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }

}
