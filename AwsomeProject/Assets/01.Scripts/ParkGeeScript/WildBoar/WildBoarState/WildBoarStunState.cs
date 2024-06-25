using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBoarStunState : EnemyState<WildBoarEnum>
{
    public WildBoarStunState(Enemy<WildBoarEnum> enemy, EnemyStateMachine<WildBoarEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }
}
