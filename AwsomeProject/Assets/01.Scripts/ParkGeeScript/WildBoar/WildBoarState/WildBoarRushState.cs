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

    public override void Enter()
    {
        base.Enter();

        playerTrm = PlayerManager.Instance.PlayerTrm;
        //enemy.animatorCompo.SetBool(animBoolHash, false);

        enemy.FindPlayerEvt(() =>
        {
            rushStart = true;
            //enemy.animatorCompo.SetBool(animBoolHash, true);
        });
    }

    public override void Exit()
    {
        base.Exit();
        rushStart = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        // 돌진중이 아니라면 무시
        if (rushStart == false) return;

        // 범위
        float dist = Vector3.Distance(playerTrm.position, enemy.transform.position);

        if (dist <= enemy.attackDistance) wildBoar.Attack();

        // 낭떠러지라면 멈추기
        if (wildBoar.CheckFront() == false) return;

        // 벽에 박으면 Groggy상태로 전환
        canRush = wildBoar.IsGroundDetected();
        if (wildBoar.IsWallDetected() == true && canRush == true) Groggy();

        Vector2 dir = (playerTrm.position - enemy.transform.position).normalized;
        if (Mathf.Sign(dir.x) != Mathf.Sign(enemy.FacingDir)) enemy.Flip();

        if (wildBoar.moveAnima == true)
            enemy.SetVelocity(dir.x * enemy.moveSpeed, enemy.rigidbodyCompo.velocity.y);
    }

    private void Groggy()
    {
        //if (rushSkill == null)
            //rushSkill = wildBoar.Skills.GetSkillByEnum(WildBoarSkillEnum.Rush) as WildBoarRushSkillSO;

        //enemy.SetVelocity(0, rushSkill.rushSpeed.GetValue());
    }
}
