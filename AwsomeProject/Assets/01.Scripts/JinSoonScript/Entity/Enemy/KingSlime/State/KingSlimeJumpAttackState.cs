using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeJumpAttackState : EnemyState<KingSlimeStateEnum>
{
    private Transform enemyTrm;
    private Transform targetTrm;
    private KingSlime kingSlime;
    private GameObject jumpAttackWarning;

    private int fallAnimTriggerHash = Animator.StringToHash("Fall");
    private int landAnimTriggerHash = Animator.StringToHash("Land");
    private int jumpanimTriggerHash = Animator.StringToHash("Jump");

    private float attackDelay = 1f;
    private float fallSpeed = 70f;
    private float jumpDelay = 2f;
    private float jumpHeight = 20f;
    private float jumpSpeed = 50f;
    private float moveSpeed = 5f;
    private float randomDelay;
    private float originGravity;

    private float targetHeight;
    private bool _isJumped = false;
    private bool _isFalling = false;
    private bool _isFollowingPlayer = false;

    public KingSlimeJumpAttackState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        kingSlime = enemy as KingSlime;
        enemyTrm = enemy.transform;
        targetTrm = PlayerManager.Instance.PlayerTrm;
        jumpAttackWarning =
            enemy.transform.Find("JumpAttackWarning").gameObject;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        enemyStateMachine.ChangeState(KingSlimeStateEnum.Ready);
    }

    public override void Enter()
    {
        base.Enter();

        //시작하고 jumpDelay후 올라감
        enemy.StopImmediately(true);
        enemy.StartDelayCallBack(jumpDelay,
            () =>
            {
                enemy.animatorCompo.SetTrigger(jumpanimTriggerHash);
                targetHeight = enemyTrm.position.y + jumpHeight;
                originGravity = enemy.rigidbodyCompo.gravityScale;
                enemy.rigidbodyCompo.gravityScale = 0f;
                _isJumped = true;
            });
    }

    public override void Exit()
    {
        enemy.rigidbodyCompo.gravityScale = originGravity;
        _isJumped = false;
        _isFalling = false;
        _isFollowingPlayer = false;
        kingSlime.SetSkillAfterDelay();
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        //점프
        if (_isJumped)
            JumpProcess();

        //따라다니기
        if (_isFollowingPlayer)
            FollowProcess();

        if (_isFalling)
            FallProcess();
    }

    private void FallProcess()
    {
        Debug.Log("밍밍밍");
        //땅이 감지 됬다면 이제 착지 애니메이션
        if(enemy.IsGroundDetected())
        {
            enemy.rigidbodyCompo.gravityScale = originGravity;
            enemy.animatorCompo.SetTrigger(landAnimTriggerHash);
            _isFalling = false;
        }

        enemyTrm.position += Vector3.down * fallSpeed * Time.deltaTime;
    }

    private void JumpProcess()
    {
        enemyTrm.position +=
                Vector3.up * jumpSpeed * Time.deltaTime;

        if (jumpHeight <= enemyTrm.position.y)
        {
            _isJumped = false;
            enemy.StartDelayCallBack(0.5f,
                () =>
                {
                    jumpAttackWarning.SetActive(true);
                    _isFollowingPlayer = true;
                    _isJumped = false;

                    randomDelay = Random.Range(4f, 7f);
                    enemy.StartDelayCallBack(randomDelay,
                        DelayFallProcess);
                });
        }
    }

    private void FollowProcess()
    {
        float dir =
                targetTrm.position.x - enemyTrm.position.x;
        dir = Mathf.Clamp(dir, -1, 1);

        enemyTrm.position +=
            Vector3.right * moveSpeed * dir * Time.deltaTime;
    }

    private void DelayFallProcess()
    {
        Debug.Log("밍;;;");
        enemy.StartDelayCallBack(attackDelay,
            () =>
            {
                jumpAttackWarning.SetActive(false);
                enemy.animatorCompo.SetTrigger(fallAnimTriggerHash);
                _isFollowingPlayer = false;
                _isFalling = true;
            });
    }
}
