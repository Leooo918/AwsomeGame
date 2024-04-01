using System.Collections;
using System.Collections.Generic;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public class SlimeChaseState : EnemyState<SlimeEnum>
{
    private Slime slime;
    private Transform playerTrm;
    private bool chaseStart = false;

    public SlimeChaseState(Enemy enemy, EnemyStateMachine<SlimeEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName) { slime = enemy as Slime; }

    public override void Enter()
    {
        base.Enter();

        playerTrm = PlayerManager.instance.playerTrm;
        enemy.animatorCompo.SetBool(animBoolHash, false);

        enemy.FindPlayerEvt(() =>
        {
            chaseStart = true;
            enemy.animatorCompo.SetBool(animBoolHash, true);
        });
    }

    public override void Exit()
    {
        base.Exit();
        chaseStart = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (chaseStart == false) return;

        Vector2 dir = (playerTrm.position - enemy.transform.position).normalized;

        if (Mathf.Sign(dir.x) != Mathf.Sign(enemy.FacingDir))
            enemy.Flip();

        if (slime.moveAnim == true)
            enemy.SetVelocity(dir.x * enemy.moveSpeed, 0);

        float dist = Vector3.Distance(playerTrm.position, enemy.transform.position);

        if (dist > enemy.detectingDistance + 5)
        {
            enemy.MissPlayer();
            enemyStateMachine.ChangeState(SlimeEnum.Return);
        }

        if (dist <= enemy.attackDistance)
        {
            slime.Attack();
        }
    }
}
