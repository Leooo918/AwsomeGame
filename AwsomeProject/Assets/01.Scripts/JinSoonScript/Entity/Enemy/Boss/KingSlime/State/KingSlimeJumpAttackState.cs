using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEngine;

public class KingSlimeJumpAttackState : EnemyState<KingSlimeStateEnum>
{
    private Transform _enemyTrm;
    private Transform _targetTrm;
    private KingSlime _kingSlime;
    private BossHealth _health;
    private GameObject _jumpAttackWarning;
    private bool _isPlayerHit = false;

    #region animHash

    private int _fallAnimTriggerHash = Animator.StringToHash("Fall");
    private int _landAnimTriggerHash = Animator.StringToHash("Land");
    private int _jumpanimTriggerHash = Animator.StringToHash("Jump");

    #endregion

    #region Variables

    private float _attackDelay = 1f;
    private float _fallSpeed = 60f;
    private float _jumpDelay = 2f;
    private float _jumpHeight = 20f;
    private float _jumpSpeed = 50f;
    private float _moveSpeed = 5f;

    private float _randomDelay;
    private float _originGravity;
    private float _targetHeight;
    private float _groundHeight;

    #endregion

    #region States

    private JumpAttackState _currentState;

    #endregion

    public KingSlimeJumpAttackState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        _targetTrm = PlayerManager.Instance.PlayerTrm;
        _kingSlime = enemy as KingSlime;
        _health = _kingSlime.healthCompo as BossHealth;
        _enemyTrm = enemy.transform;
        _jumpAttackWarning =
            enemy.transform.parent.Find("JumpAttackWarning").gameObject;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        switch (_currentState)
        {
            //공격 준비 -> 스킬 이펙트
            case JumpAttackState.ReadyJump:
                _currentState = JumpAttackState.SkillEffect;
                break;
            //스킬 이펙트가 나가고 공격
            case JumpAttackState.SkillEffect:
                SkillEffect();
                break;
            //착지
            case JumpAttackState.Falling:
                enemy.CanStateChangeable = true;
                enemyStateMachine.ChangeState(KingSlimeStateEnum.Ready);
                break;
        }
    }

    public override void Enter()
    {
        base.Enter();

        //시작하고 점프 준비로 넘어가서
        _currentState = JumpAttackState.ReadyJump;

        //넉백이 안되고, 상태 변환도 안되고, 멈춘상태에서
        enemy.CanKnockback = false;
        enemy.CanStateChangeable = false;
        enemy.StopImmediately(true);
    }

    public override void Exit()
    {
        enemy.rigidbodyCompo.gravityScale = _originGravity;
        _currentState = JumpAttackState.ReadyJump;
        _kingSlime.SetSkillAfterDelay();
        _isPlayerHit = false;
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        switch (_currentState)
        {
            //animationTrigger에서 Jumped 스테이트로 넘어오면 위로 올라가
            case JumpAttackState.Jumped:
                JumpProcess();
                break;
            case JumpAttackState.Falling:
                FallProcess();
                break;
        }
    }

    private void FallProcess()
    {
        //땅이 감지 됬다면 이제 착지 애니메이션
        if (enemy.IsGroundDetected())
        {
            Land();
        }
        else
        {
            _enemyTrm.position += Vector3.down * _fallSpeed * Time.deltaTime;
        }
    }

    private void JumpProcess()
    {
        _enemyTrm.position +=
                Vector3.up * _jumpSpeed * Time.deltaTime;

        if (_targetHeight <= _enemyTrm.position.y)
        {
            CameraManager.Instance.StartShakeCam(3, 3);
            _jumpAttackWarning.SetActive(true);
            _currentState = JumpAttackState.JumpDelay;

            _randomDelay = Random.Range(1.5f, 2.5f);
            enemy.StartDelayCallBack(_randomDelay, DelayFallProcess);
        }
    }

    private void DelayFallProcess()
    {
        enemy.StartDelayCallBack(_attackDelay,
            () =>
            {
                _jumpAttackWarning.SetActive(false);
                enemy.animatorCompo.SetTrigger(_fallAnimTriggerHash);
                _currentState = JumpAttackState.Falling;
            });
    }

    private void SkillEffect()
    {
        enemy.animatorCompo.SetTrigger(_jumpanimTriggerHash);

        _groundHeight = _enemyTrm.position.y;
        _targetHeight = _enemyTrm.position.y + _jumpHeight;
        _originGravity = enemy.rigidbodyCompo.gravityScale;
        enemy.rigidbodyCompo.gravityScale = 0f;

        _currentState = JumpAttackState.Jumped;
    }

    private void Land()
    {
        if (_health.currentPhase == 1)
        {
            _kingSlime.spanwer.StartRockFalling(3);
            enemy.StartDelayCallBack(0.3f, () => CameraManager.Instance.StartShakeCam(5, 5));
            enemy.StartDelayCallBack(4f, () => CameraManager.Instance.StopShakeCam());
        }

        enemy.rigidbodyCompo.gravityScale = _originGravity;
        enemy.animatorCompo.SetTrigger(_landAnimTriggerHash);
        CameraManager.Instance.ShakeCam(30, 30, 0.2f);

        if (PlayerManager.Instance.Player.IsGroundDetected() && _isPlayerHit == false)
        {
            PlayerManager.Instance.Player.Stun(2f);
            PlayerManager.Instance.Player.healthCompo.TakeDamage(1, Vector2.up * 4, enemy);
            _isPlayerHit = true;
        }
    }
}

public enum JumpAttackState
{
    ReadyJump,
    SkillEffect,
    Jumped,
    JumpDelay,
    Falling
}
