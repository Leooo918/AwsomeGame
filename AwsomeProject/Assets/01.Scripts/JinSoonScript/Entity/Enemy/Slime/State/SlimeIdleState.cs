using UnityEngine;

public class SlimeIdleState : EnemyState<SlimeStateEnum>
{
    public SlimeIdleState(Enemy enemy, EnemyStateMachine<SlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName) { }

    public override void UpdateState()
    {
        base.UpdateState();

        Player player = enemy.IsPlayerDetected();

        //�÷��̾ �����Ǹ� �i��
        if (player != null && enemy.IsObstacleInLine(enemy.runAwayDistance) == false)
            enemy.FindPlayerEvt(() => enemyStateMachine.ChangeState(SlimeStateEnum.Chase));

        //��ư ��������
        if (enemy.patrolEndTime + enemy.PatrolDelay < Time.time)
            enemyStateMachine.ChangeState(SlimeStateEnum.Patrol);
    }
}
