using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBoarIdleState : EnemyState<WildBoarEnum>
{
    public WildBoarIdleState(Enemy<WildBoarEnum> enemy, EnemyStateMachine<WildBoarEnum> enemyStateMachine, string animBoolName) 
        : base(enemy, enemyStateMachine, animBoolName)
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Player player = enemy.IsPlayerDetected();

        if (player != null && enemy.IsObstacleInLine(enemy.runAwayDistance) == false)
        {
            enemy.FindPlayerEvt(() => enemyStateMachine.ChangeState(WildBoarEnum.Ready));
            Debug.Log("Ready½ÇÇà");
        }
    }
}
