using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBoarReadyState : EnemyState<WildBoarEnum>
{
    private WildBoar wildBoar;
    private Transform playerTrm;
    private bool isReady = false;

    public WildBoarReadyState(Enemy enemy, EnemyStateMachine<WildBoarEnum> enemyStateMachine, string animBoolName) 
        : base(enemy, enemyStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        playerTrm = PlayerManager.Instance.PlayerTrm;

        enemy.FindPlayerEvt(() =>
        {
            isReady = true;

        });
    }

    public override void Exit()
    {
        base.Exit();
        isReady = false;

        enemyStateMachine.ChangeState(WildBoarEnum.Rush);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (!isReady)
            return;

        if (wildBoar.IsGroundDetected() == false)
        {
            wildBoar.Flip();
        }

    }
}
