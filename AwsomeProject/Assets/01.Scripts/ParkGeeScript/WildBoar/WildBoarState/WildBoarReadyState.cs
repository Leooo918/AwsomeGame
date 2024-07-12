using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBoarReadyState : EnemyState<WildBoarEnum>
{
    private WildBoar wildBoar;
    private bool isReady = false;
    private float time;
    //private float 

    public WildBoarReadyState(Enemy<WildBoarEnum> enemy, EnemyStateMachine<WildBoarEnum> enemyStateMachine, string animBoolName) 
        : base(enemy, enemyStateMachine, animBoolName)
    {
        wildBoar = enemy as WildBoar;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Ready");
    }

    public override void Exit()
    {
        base.Exit();
        isReady = true;

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
