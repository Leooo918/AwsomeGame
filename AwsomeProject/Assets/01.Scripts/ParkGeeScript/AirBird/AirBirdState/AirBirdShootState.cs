using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBirdShootState : EnemyState<AirBirdEnum>
{
    private AirBird airBird;
    private AirBirdShootSkillSO shootSkill;
    private bool canShoot = false;
    private Transform playerTrm;
    private bool shootStart = false;

    public AirBirdShootState(Enemy<AirBirdEnum> enemy, EnemyStateMachine<AirBirdEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        airBird = enemy as AirBird;
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
            shootStart = true;
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
    }
}
