using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeDeadState : EnemyState<KingSlimeStateEnum>
{
    public KingSlimeDeadState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //掩伸軒澗闇 戚薦 享員拭辞 姥偽背操醤敗. OnDeadevent拭 焼巷動 ??しし ししししし:し:し::?//
    }
}
