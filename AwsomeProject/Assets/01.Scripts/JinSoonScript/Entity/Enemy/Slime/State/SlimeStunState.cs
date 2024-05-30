using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStunState : EnemyState<SlimeStateEnum>
{
    public SlimeStunState(Enemy<SlimeStateEnum> enemy, EnemyStateMachine<SlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName) { }
}
