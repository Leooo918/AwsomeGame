using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBirdShootState : EnemyState<AirBirdEnum>
{
    private AirBird _airBird;
    private Transform _playerTrm;
    private AirBirdShootSkillSO _shootSkill;
    private bool _isShoot = false;

    private Vector2[] _offset = new Vector2[3] {new Vector2(-4,0), new Vector2(0,0), new Vector2(4,0)};

    public AirBirdShootState(Enemy<AirBirdEnum> enemy, EnemyStateMachine<AirBirdEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        _playerTrm = PlayerManager.Instance.PlayerTrm;
        _shootSkill = enemy.EntitySkillSO.GetSkillSO("Shoot") as AirBirdShootSkillSO;
        _airBird = enemy as AirBird;
    }

    public override void AnimationFinishTrigger()
    {
        if (triggerCall)
        {
            enemyStateMachine.ChangeState(AirBirdEnum.Chase);
            return;
        }

        for (int i = 0; i < 3; i++)
        {
            Vector2 playerDir = ((_playerTrm.position + (Vector3)_offset[i]) - enemy.transform.position).normalized * _shootSkill.shootSpeed.GetValue();

            Feather feather = MonoBehaviour.Instantiate(_airBird.FeatherPf, enemy.transform.position, Quaternion.identity).GetComponent<Feather>();
            feather.Shoot(playerDir);
        }

        base.AnimationFinishTrigger();
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
