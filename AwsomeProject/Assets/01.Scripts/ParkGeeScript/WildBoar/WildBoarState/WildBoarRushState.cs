using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBoarRushState : EnemyState<WildBoarEnum>
{
    private float playerDir;
    private WildBoarRushSkillSO rushSkill;
    private WildBoar wildboar;

    public WildBoarRushState(Enemy enemy, EnemyStateMachine<WildBoarEnum> enemyStateMachine, string animBoolName) 
        : base(enemy, enemyStateMachine, animBoolName)
    {
        wildboar = enemy as WildBoar;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetVelocity(0, rushSkill.rushSpeed.GetValue());
        playerDir = (PlayerManager.Instance.PlayerTrm.position - enemy.transform.position).x;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        enemy.SetVelocity(playerDir * enemy.moveSpeed, enemy.rigidbodyCompo.velocity.y);

        if(enemy.IsGroundDetected() && enemy.rigidbodyCompo.velocity.y < 0)
        {
            enemy.entityAttack.SetCurrentAttackInfo(rushSkill.AttackInfo);
            enemy.entityAttack.Attack();

            enemyStateMachine.ChangeState(WildBoarEnum.Groggy);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
