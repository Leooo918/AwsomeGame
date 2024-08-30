using UnityEngine;

public class SlimeIdleState : EnemyState<SlimeStateEnum>
{
    public SlimeIdleState(Enemy<SlimeStateEnum> enemy, EnemyStateMachine<SlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName) { }

    public override void UpdateState()
    {
        base.UpdateState();

        Player player = enemy.IsPlayerDetected();

        //�÷��̾ �����Ǹ� �i��
        if (player != null && enemy.IsObstacleInLine(enemy.runAwayDistance) == false)
            enemyStateMachine.ChangeState(SlimeStateEnum.Chase);

        enemyStateMachine.ChangeState(SlimeStateEnum.Patrol);
    }
}
