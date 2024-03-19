using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyRunState : EnemyState<TestEnemyEnum>
{
    public TestEnemyRunState(Enemy enemy, EnemyStateMachine<TestEnemyEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();
        enemy.SetVelocity(enemy.moveSpeed * enemy.FacingDir, rigidbody.velocity.y);

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemyStateMachine.ChangeState(TestEnemyEnum.Idle);
        }
    }
}
