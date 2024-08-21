using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBoarIdleState : EnemyState<WildBoarEnum>
{
    private Transform _playerTrm;

    public WildBoarIdleState(Enemy<WildBoarEnum> enemy, EnemyStateMachine<WildBoarEnum> enemyStateMachine, string animBoolName)
        : base(enemy, enemyStateMachine, animBoolName)
    {
        _playerTrm = PlayerManager.Instance.PlayerTrm;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Player player = enemy.IsPlayerDetected();

        float dist = Vector3.Distance(_playerTrm.position, enemy.transform.position);

        //너무 가까우면 바로 move로 이동
        if (dist <= 5)
            enemyStateMachine.ChangeState(WildBoarEnum.Move);

        if (player != null && enemy.IsObstacleInLine(enemy.runAwayDistance) == false)
            enemy.FindPlayerEvt(() => enemyStateMachine.ChangeState(WildBoarEnum.Move));
    }
}
