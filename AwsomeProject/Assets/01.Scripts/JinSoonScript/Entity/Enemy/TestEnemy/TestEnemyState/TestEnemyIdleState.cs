using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyIdleState : EnemyState<TestEnemyEnum>
{
    private Coroutine delayCoroutine;

    public TestEnemyIdleState(Enemy enemy, EnemyStateMachine<TestEnemyEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        if (delayCoroutine != null)
            enemy.StopCoroutine(delayCoroutine);
        enemy.StopImmediately(false);

        delayCoroutine = enemy.StartDelayCallBack(enemy.idleTime, () =>
        {
            enemyStateMachine.ChangeState(TestEnemyEnum.Run);
        });
    }

    public override void Exit()
    {
        if (delayCoroutine != null)
            enemy.StopCoroutine(delayCoroutine);

        //�տ� ���� �ְų� ���� ������ �ȵȴٸ� �����鼭 �ø��������
        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
        }
        base.Exit();
    }
}
