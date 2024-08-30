using UnityEngine;

public class SlimeIdleState : EnemyState<SlimeStateEnum>
{
    public SlimeIdleState(Enemy<SlimeStateEnum> enemy, EnemyStateMachine<SlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName) { }

    public override void UpdateState()
    {
        base.UpdateState();

        Player player = enemy.IsPlayerDetected();

        //플레이어가 감지되면 쫒아
        if (player != null && enemy.IsObstacleInLine(enemy.runAwayDistance) == false)
            enemyStateMachine.ChangeState(SlimeStateEnum.Chase);

        enemyStateMachine.ChangeState(SlimeStateEnum.Patrol);
    }
}
