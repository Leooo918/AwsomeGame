using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBoarReadyState : EnemyState<WildBoarEnum>
{
    private WildBoar wildBoar;
    private WildBoarRushSkillSO rushSkill;
    private bool canRush = false;
    private Transform playerTrm;

    public WildBoarReadyState(Enemy<WildBoarEnum> enemy, EnemyStateMachine<WildBoarEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        wildBoar = enemy as WildBoar;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("Ready");
        playerTrm = PlayerManager.Instance.PlayerTrm;
        enemy.animatorCompo.SetBool(animBoolHash, false);

        enemy.FindPlayerEvt(() =>
        {
            enemy.animatorCompo.SetBool(animBoolHash, true);
        });
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        // ����
        float dist = Vector3.Distance(playerTrm.position, enemy.transform.position);

        if (dist <= enemy.attackDistance) wildBoar.Attack();

        //// ����������� ���߱�
        //if (wildBoar.CheckFront() == false) return;

        //// ���� ������ stun ��ȯ
        //canRush = wildBoar.IsGroundDetected();
        //if (wildBoar.IsWallDetected() == true)
        //    enemyStateMachine.ChangeState(WildBoarEnum.Stun);

        Vector2 dir = (playerTrm.position - enemy.transform.position).normalized;
        if (Mathf.Sign(dir.x) != Mathf.Sign(enemy.FacingDir)) enemy.Flip();

        if (wildBoar.moveAnima == true)
            enemy.SetVelocity(dir.x * enemy.moveSpeed, enemy.rigidbodyCompo.velocity.y);
    }
}
