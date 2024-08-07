using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBirdChaseState : EnemyState<AirBirdEnum>
{
    private AirBird airbird;
    private AirBirdShootSkillSO shootSkill;
    private bool canJump = false;
    private Transform playerTrm;
    private bool chaseStart = false;

    public AirBirdChaseState(Enemy<AirBirdEnum> enemy, EnemyStateMachine<AirBirdEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        airbird = enemy as AirBird;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        playerTrm = PlayerManager.Instance.PlayerTrm;
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


        float dist = Vector3.Distance(playerTrm.position, enemy.transform.position);

        if (dist <= enemy.attackDistance) airbird.Attack();

        Vector2 dir = (playerTrm.position - enemy.transform.position).normalized;
        if (Mathf.Sign(dir.x) != Mathf.Sign(enemy.FacingDir)) enemy.Flip();

        if (airbird.moveAnima == true)
            enemy.SetVelocity(dir.x * enemy.moveSpeed, enemy.rigidbodyCompo.velocity.y);
    }
}
