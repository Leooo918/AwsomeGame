using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeReadyState : EnemyState<KingSlimeStateEnum>
{
    private KingSlime kingSlime;
    private Transform playerTrm;

    public KingSlimeReadyState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        kingSlime = enemy as KingSlime;
        playerTrm = PlayerManager.Instance.PlayerTrm;
    }

    public override void UpdateState()
    {
        kingSlime.UseSkill();

        Vector2 dir = (playerTrm.position - enemy.transform.position).normalized;
        if (Mathf.Sign(dir.x) != Mathf.Sign(enemy.FacingDir)) enemy.Flip();
    }
}
