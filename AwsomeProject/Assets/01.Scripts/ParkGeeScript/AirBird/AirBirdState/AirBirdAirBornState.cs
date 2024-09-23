using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBirdAirBornState : EnemyState<AirBirdEnum>
{
    public AirBirdAirBornState(Enemy<AirBirdEnum> enemy, EnemyStateMachine<AirBirdEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.CanKnockback = false;
        enemy.CanStateChangeable = false;
        enemy.StartDelayCallBack(enemy.airBornDuration, () =>
        {
            enemy.StartCoroutine(Down());
        });
    }

    private IEnumerator Down()
    {
        yield return new WaitUntil(() => enemy.rigidbodyCompo.velocity.y > -40);
        enemy.CanStateChangeable = true;
        enemyStateMachine.ChangeState(AirBirdEnum.Idle);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.CanKnockback = true;
    }
}
