using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeMucusAttackState : EnemyState<KingSlimeStateEnum>
{
    private KingSlime _kingSlime;
    private KingMucusAttackSkillSO _mucusAttackSO;
    private GameObject _mucusPf;
    private Tween _moveTween;
    private Vector2 _targetPosition;
    private bool _isFired = false;
    private bool _isJumped = false;
    private int _doMucusAttackAnimTriggerHash = Animator.StringToHash("DoMucusAttack");

    private int[] fireOffset = new int[3] { -5, 0, 5 };

    public KingSlimeMucusAttackState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        _kingSlime = enemy as KingSlime;
        if (_kingSlime?.EntitySkillSO.GetSkillSO("KingMucusAttack") != null)
        {
            _mucusAttackSO = _kingSlime?.EntitySkillSO.GetSkillSO("KingMucusAttack") as KingMucusAttackSkillSO;
            _mucusPf = _mucusAttackSO.mucusPf;
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        Vector3 targetPosition = _kingSlime.GetJumpPos();
        Vector3 playerPos = PlayerManager.Instance.PlayerTrm.position;
        Vector2 targetDir = targetPosition - enemy.transform.position;

        float dir = playerPos.x < targetPosition.x ? -1 : 1;

        if (_isFired == true)
        {
            enemy.CanStateChangeable = true;
            enemy.StateMachine.ChangeState(KingSlimeStateEnum.Ready);
            return;
        }

        if (_isJumped == false)
        {
            _moveTween = enemy.transform.DOJump(targetPosition, 10, 1, 0.8f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    enemy.animatorCompo.SetTrigger(_doMucusAttackAnimTriggerHash);
                    enemy.FlipController(dir);
                    _isJumped = true;
                });

            return;
        }



        Rigidbody2D mucusRb = _mucusPf.GetComponent<Rigidbody2D>();
        for (int i = 0; i < 3; i++)
        {
            Vector2 fireDirection;
            fireDirection.x = (PlayerManager.Instance.PlayerTrm.position - _kingSlime.transform.position).magnitude * -dir * 2;
            fireDirection.y = Mathf.Clamp(mucusRb.gravityScale * 2 + fireOffset[i], 1, 100);

            KingSlimeMucus mucusInstance = MonoBehaviour.Instantiate(_mucusPf).GetComponent<KingSlimeMucus>();
            mucusInstance.transform.position = _kingSlime.transform.position;
            mucusInstance.Fire(fireDirection, enemy);
        }

        _isFired = true;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.CanStateChangeable = false;
        enemy.CanKnockback = false;
    }

    public override void Exit()
    {
        _isFired = false;
        _isJumped = false;
        enemy.CanKnockback = true;
        _kingSlime.SetSkillAfterDelay();
        base.Exit();
    }
}
