using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBirdDeadState : EnemyState<AirBirdEnum>
{
    public AirBirdDeadState(Enemy<AirBirdEnum> enemy, EnemyStateMachine<AirBirdEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("deadAirBird");
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        enemy.OnCompletelyDie();
    }
}
