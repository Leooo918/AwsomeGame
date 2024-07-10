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
    }

    public override void UpdateState()
    {
        Debug.Log("นึ");
        kingSlime.UseSkill();

        playerTrm = PlayerManager.Instance.PlayerTrm;
        Vector2 dir = (playerTrm.position - enemy.transform.position).normalized;
        if (Mathf.Sign(dir.x) != Mathf.Sign(enemy.FacingDir)) enemy.Flip();
    }
}
