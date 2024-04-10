using UnityEngine;

public class SlimeIdleState : EnemyState<SlimeEnum>
{
    public SlimeIdleState(Enemy enemy, EnemyStateMachine<SlimeEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName) { }

    public override void UpdateState()
    {
        base.UpdateState();

        Player player = enemy.IsPlayerDetected();

        //플레이어가 감지되면 쫒아
        if (player != null && enemy.IsObstacleInLine(enemy.runAwayDistance) == false)
            enemy.FindPlayerEvt(() => enemyStateMachine.ChangeState(SlimeEnum.Chase));

        //암튼 순찰돌고
        if (enemy.patrolEndTime + enemy.PatrolDelay < Time.time)
            enemyStateMachine.ChangeState(SlimeEnum.Patrol);
    }
}
