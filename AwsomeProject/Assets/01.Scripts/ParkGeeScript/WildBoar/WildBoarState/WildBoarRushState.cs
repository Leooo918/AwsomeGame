using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBoarRushState : EnemyState<WildBoarEnum>
{
    private WildBoar wildBoar;
    private WildBoarRushSkillSO rushSkill;
    private bool canRush = false;
    private Transform playerTrm;
    private bool rushStart = false;

    public WildBoarRushState(Enemy<WildBoarEnum> enemy, EnemyStateMachine<WildBoarEnum> enemyStateMachine, string animBoolName) 
        : base(enemy, enemyStateMachine, animBoolName)
    {
        wildBoar = enemy as WildBoar;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();


    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        canRush = wildBoar.IsGroundDetected();
        if (wildBoar.IsWallDetected() == true)
            enemyStateMachine.ChangeState(WildBoarEnum.Stun);
    }
}
