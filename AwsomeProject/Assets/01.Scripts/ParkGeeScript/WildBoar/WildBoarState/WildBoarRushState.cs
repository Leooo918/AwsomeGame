using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBoarRushState : EnemyState<WildBoarEnum>
{
    private WildBoar _wildBoar;
    private WildBoarRushSkillSO _rushSkill;
    private bool _isRushing = false;
    private float _rushDir = 0;
    private float _rushSpeed;

    public WildBoarRushState(Enemy<WildBoarEnum> enemy, EnemyStateMachine<WildBoarEnum> enemyStateMachine, string animBoolName)
        : base(enemy, enemyStateMachine, animBoolName)
    {
        _wildBoar = enemy as WildBoar;
        _rushSkill = enemy.EntitySkillSO.GetSkillSO("Rush") as WildBoarRushSkillSO;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        if (!_isRushing)
        {
            //�뽬 ����
            _isRushing = true;
            _wildBoar.dashAttackCollider.SetActive(true);
        }
        else
        {
            //�뽬 ��
            enemy.MovementCompo.StopImmediately(false);
            enemyStateMachine.ChangeState(WildBoarEnum.Move);
        }
    }

    public override void Enter()
    {
        base.Enter();
        _isRushing = false;
        _rushDir = enemy.FacingDir;
        _rushSpeed = _rushSkill.rushSpeed.GetValue();
        enemy.MovementCompo.StopImmediately(false);
    }

    public override void Exit()
    {
        _wildBoar.dashAttackCollider.SetActive(false);
        enemy.MovementCompo.StopImmediately(false);
        _wildBoar.SetAttackDelay(_rushSkill.skillCoolTime.GetValue());
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_isRushing)
            enemy.MovementCompo.SetVelocity(new Vector2(_rushDir * _rushSpeed, 0));

        if (enemy.CheckFront() == false)
        {
            _isRushing = false;
            enemy.MovementCompo.StopImmediately(true);
            enemy.StartDelayCallBack(0.3f, () => enemyStateMachine.ChangeState(WildBoarEnum.Move));
        }
        
        if (enemy.IsWallDetected() == true)
        {
            Vector2 dir = new Vector2(-enemy.FacingDir * 5, 10);
            enemy.KnockBack(dir);
            enemy.Stun(2);
        }
    }
}
