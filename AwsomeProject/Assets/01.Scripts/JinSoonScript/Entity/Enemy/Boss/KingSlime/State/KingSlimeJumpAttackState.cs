using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeJumpAttackState : EnemyState<KingSlimeStateEnum>
{
    private Transform _enemyTrm;
    private Transform _targetTrm;
    private KingSlime _kingSlime;
    private GameObject _jumpAttackWarning;

    private int _fallAnimTriggerHash = Animator.StringToHash("Fall");
    private int _landAnimTriggerHash = Animator.StringToHash("Land");
    private int _jumpanimTriggerHash = Animator.StringToHash("Jump");

    private float _attackDelay = 1f;
    private float _fallSpeed = 70f;
    private float _jumpDelay = 2f;
    private float _jumpHeight = 20f;
    private float _jumpSpeed = 50f;
    private float _moveSpeed = 5f;
    private float _randomDelay;
    private float _originGravity;

    private float _targetHeight;
    private bool _isStartSkillEffect = false;
    private bool _readyJump = true;
    private bool _isJumped = false;
    private bool _isFalling = false;
    private bool _isFollowingPlayer = false;

    public KingSlimeJumpAttackState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        _kingSlime = enemy as KingSlime;
        _enemyTrm = enemy.transform;
        _jumpAttackWarning =
            enemy.transform.Find("JumpAttackWarning").gameObject;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        if(!_isStartSkillEffect)
        {
            CameraManager.Instance.StartShakeCam(3, 3);
            _isStartSkillEffect = true;
            return;
        }

        if(_readyJump)
        {
            CameraManager.Instance.StopShakeCam();
            enemy.animatorCompo.SetTrigger(_jumpanimTriggerHash);
            _targetHeight = _enemyTrm.position.y + _jumpHeight;
            _originGravity = enemy.rigidbodyCompo.gravityScale;
            enemy.rigidbodyCompo.gravityScale = 0f;
            _isJumped = true;
            _readyJump = false;
            return;
        }

        enemy.CanStateChangeable = true;
        enemyStateMachine.ChangeState(KingSlimeStateEnum.Ready);
    }

    public override void Enter()
    {
        base.Enter();

        //시작하고 jumpDelay후 올라감
        Debug.Log("밍");
        enemy.CanKnockback = false;
        _targetTrm = PlayerManager.Instance.PlayerTrm;
        enemy.StopImmediately(true);
        enemy.CanStateChangeable = false;
    }

    public override void Exit()
    {
        enemy.rigidbodyCompo.gravityScale = _originGravity;
        _isJumped = false;
        _isFalling = false;
        _readyJump = true;
        _isFollowingPlayer = false;
        _isStartSkillEffect = false;
        _kingSlime.SetSkillAfterDelay();
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Debug.Log(_isJumped);

        //점프
        if (_isJumped)
            JumpProcess();

        //따라다니기
        else if (_isFollowingPlayer)
            FollowProcess();

        else if (_isFalling)
            FallProcess();
    }

    private void FallProcess()
    {
        //땅이 감지 됬다면 이제 착지 애니메이션
        if(enemy.IsGroundDetected())
        {
            enemy.rigidbodyCompo.gravityScale = _originGravity;
            enemy.animatorCompo.SetTrigger(_landAnimTriggerHash);
            _isFalling = false;
        }

        _enemyTrm.position += Vector3.down * _fallSpeed * Time.deltaTime;
    }

    private void JumpProcess()
    {
        _enemyTrm.position +=
                Vector3.up * _jumpSpeed * Time.deltaTime;

        if (_targetHeight <= _enemyTrm.position.y)
        {
            _isJumped = false;
            enemy.StartDelayCallBack(0.5f,
                () =>
                {
                    _jumpAttackWarning.SetActive(true);
                    _isFollowingPlayer = true;
                    _isJumped = false;

                    _randomDelay = Random.Range(4f, 7f);
                    enemy.StartDelayCallBack(_randomDelay, DelayFallProcess);
                });
        }
    }

    private void FollowProcess()
    {
        float dir =
                _targetTrm.position.x - _enemyTrm.position.x;
        dir = Mathf.Clamp(dir, -1, 1);

        _enemyTrm.position +=
            Vector3.right * _moveSpeed * dir * Time.deltaTime;
    }

    private void DelayFallProcess()
    {
        enemy.StartDelayCallBack(_attackDelay,
            () =>
            {
                _jumpAttackWarning.SetActive(false);
                enemy.animatorCompo.SetTrigger(_fallAnimTriggerHash);
                _isFollowingPlayer = false;
                _isFalling = true;
            });
    }
}
