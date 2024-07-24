using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeMucusAttackState : EnemyState<KingSlimeStateEnum>
{
    private KingSlime _kingSlime;
    private GameObject _mucusPf;
    private Tween _moveTween;
    private Vector2 _targetPosition;
    private bool _isFired = false;
    private bool _isJumped = false;
    private int _doMucusAttackAnimTriggerHash = Animator.StringToHash("DoMucusAttack");

    public KingSlimeMucusAttackState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        _kingSlime = enemy as KingSlime;
        if (_kingSlime?.EntitySkillSO.GetSkillSO("KingMucusAttack") != null)
            _mucusPf = (_kingSlime?.EntitySkillSO.GetSkillSO("KingMucusAttack") as KingMucusAttackSkillSO).mucusPf;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        if (_isFired == true)
        {
            enemy.CanStateChangeable = true;
            enemy.StateMachine.ChangeState(KingSlimeStateEnum.Ready);
            return;
        }

        if (_isJumped == false)
        {
            Vector3 targetPosition = _kingSlime.GetJumpPos();
            Vector3 playerPos = PlayerManager.Instance.PlayerTrm.position;

            float dir = (playerPos - targetPosition).normalized.x;
            enemy.FlipController(dir);

            Vector2 targetDir = targetPosition - enemy.transform.position;
            _moveTween = enemy.transform.DOJump(targetPosition, 10, 1, 0.8f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    enemy.animatorCompo.SetTrigger(_doMucusAttackAnimTriggerHash);
                    _isJumped = true;
                });

            return;
        }

        Vector2 fireDirection = new Vector2(0, 1);
        Vector2 playerDir = PlayerManager.Instance.PlayerTrm.position - _kingSlime.transform.position;
        fireDirection.x = playerDir.x;
        fireDirection.y *= fireDirection.magnitude;

        fireDirection = fireDirection.normalized * playerDir.magnitude;

        KingSlimeMucus mucusInstance = MonoBehaviour.Instantiate(_mucusPf).GetComponent<KingSlimeMucus>();
        mucusInstance.transform.position = _kingSlime.transform.position;
        mucusInstance.Fire(fireDirection * 2);
        _isFired = true;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("นึ");
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
