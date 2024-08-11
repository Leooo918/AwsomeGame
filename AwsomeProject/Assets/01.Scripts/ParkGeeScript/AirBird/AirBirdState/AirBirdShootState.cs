using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBirdShootState : EnemyState<AirBirdEnum>
{
    private AirBird _airBird;
    private Transform _playerTrm;
    private AirBirdShootSkillSO _shootSkill;
    private bool _isShoot = false;

    public AirBirdShootState(Enemy<AirBirdEnum> enemy, EnemyStateMachine<AirBirdEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        _playerTrm = PlayerManager.Instance.PlayerTrm;
        _shootSkill = enemy.EntitySkillSO.GetSkillSO("Shoot") as AirBirdShootSkillSO;
        _airBird = enemy as AirBird;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        if(triggerCall)
        {
            enemyStateMachine.ChangeState(AirBirdEnum.Chase);
            return;
        }

        Vector2 playerDir = (_playerTrm.position - enemy.transform.position).normalized * _shootSkill.shootSpeed.GetValue();

        Feather feather = MonoBehaviour.Instantiate(_airBird.FeatherPf, enemy.transform.position, Quaternion.identity).GetComponent<Feather>();
        feather.Shoot(playerDir);

    }

    public override void Enter()
    {
        base.Enter();
        enemy.StopImmediately(true);
    }

    public override void Exit()
    {
        _isShoot = false;
        _airBird.SetAfterDelay(_shootSkill.skillAfterDelay.GetValue());
        base.Exit(); 
    }
}
