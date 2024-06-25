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

        // �������� �ƴ϶�� ����
        if (rushStart == false) return;

        // ����
        float dist = Vector3.Distance(playerTrm.position, enemy.transform.position);

        if (dist <= enemy.attackDistance) wildBoar.Attack();

        // ����������� ���߱�
        if (wildBoar.CheckFront() == false) return;

        // ���� ������ Groggy���·� ��ȯ
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
