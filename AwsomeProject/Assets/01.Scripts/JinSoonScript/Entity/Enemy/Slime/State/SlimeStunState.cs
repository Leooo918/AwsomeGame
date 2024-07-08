using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStunState : EnemyState<SlimeStateEnum>
{
    public SlimeStunState(Enemy<SlimeStateEnum> enemy, EnemyStateMachine<SlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        enemy.CanStateChangeable = false;
        enemy.StartDelayCallBack(enemy.stunDuration, () =>
        {
            enemy.CanStateChangeable = true;
            enemyStateMachine.ChangeState(SlimeStateEnum.Chase);
            if (enemy.healthCompo.curHp < 0)
            {
                enemyStateMachine.ChangeState(SlimeStateEnum.Dead);
            }
        });
    }
}
