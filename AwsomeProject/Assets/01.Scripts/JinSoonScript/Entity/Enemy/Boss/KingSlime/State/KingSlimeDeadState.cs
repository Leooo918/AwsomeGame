using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VHierarchy.Libs;

public class KingSlimeDeadState : EnemyState<KingSlimeStateEnum>
{
    public KingSlimeDeadState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    { }

    public override void AnimationFinishTrigger()
    {
        enemy.gameObject.SetActive(false);
        //여기서 풀링하게 해
    }
}
