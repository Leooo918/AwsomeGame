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
        //길열리는건 이제 딴곳에서 구독해줘야함. OnDeadevent에 아무튼 ??ㅇㅇ ㅇㅇㅇㅇㅇ:ㅇ:ㅇ::?//
    }
}
