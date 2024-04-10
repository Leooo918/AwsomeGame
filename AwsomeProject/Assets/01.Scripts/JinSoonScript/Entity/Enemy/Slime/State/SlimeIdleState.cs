using UnityEngine;

public class SlimeIdleState : EnemyState<SlimeEnum>
{
    public SlimeIdleState(Enemy enemy, EnemyStateMachine<SlimeEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName) { }

    public override void UpdateState()
    {
        base.UpdateState();

        Player player = enemy.IsPlayerDetected();

        //�÷��̾ �����Ǹ� �i��
        if (player != null && enemy.IsObstacleInLine(enemy.runAwayDistance) == false)
            enemy.FindPlayerEvt(() => enemyStateMachine.ChangeState(SlimeEnum.Chase));

        //��ư ��������
        if (enemy.patrolEndTime + enemy.PatrolDelay < Time.time)
            enemyStateMachine.ChangeState(SlimeEnum.Patrol);
    }
}
