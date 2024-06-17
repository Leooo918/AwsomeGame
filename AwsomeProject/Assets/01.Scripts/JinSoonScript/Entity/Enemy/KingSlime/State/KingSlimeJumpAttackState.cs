using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeJumpAttackState : EnemyState<KingSlimeStateEnum>
{
    private KingSlime kingSlime;
    private GameObject jumpAttackWarning;

    private float originGravityScale;
    private float playerFollowingSpeed = 2f;
    private float playerFollowingTime = 2f;

    private bool isFindingPlayer = false;
    private bool isGoUp = false;
    private bool isGoDown = false;

    private int fallAnimBoolHash = Animator.StringToHash("Fall");
    private int landAnimBoolHash = Animator.StringToHash("Land");


    public KingSlimeJumpAttackState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        kingSlime = enemy as KingSlime;
        jumpAttackWarning = enemy.transform.Find("JumpAttackWarning").gameObject;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        if (isGoUp == false) isGoUp = true;
        else if (isFindingPlayer == false)
        {
            jumpAttackWarning.SetActive(true);
            isFindingPlayer = true;

            enemy.StartDelayCallBack(playerFollowingTime, () =>
            {
                enemy.animatorCompo.SetBool(fallAnimBoolHash, true);
                jumpAttackWarning.SetActive(false);
                isGoDown = true;
            });
        }
        else enemyStateMachine.ChangeState(KingSlimeStateEnum.Ready);
    }

    public override void Enter()
    {
        base.Enter();
        enemy.StopImmediately(true);
        originGravityScale = enemy.rigidbodyCompo.gravityScale;
        jumpAttackWarning.SetActive(false);
        enemy.animatorCompo.SetBool(fallAnimBoolHash, false);
        enemy.animatorCompo.SetBool(landAnimBoolHash, false);
        isGoUp = false;
        isFindingPlayer = false;
        isGoDown = false;
        enemy.rigidbodyCompo.gravityScale = 0f;
    }

    public override void Exit()
    {
        jumpAttackWarning.SetActive(false);
        enemy.rigidbodyCompo.gravityScale = originGravityScale;
        enemy.animatorCompo.SetBool(fallAnimBoolHash, false);
        enemy.animatorCompo.SetBool(landAnimBoolHash, false);
        isGoUp = false;
        isFindingPlayer = false;
        isGoDown = false;
        kingSlime.SetSkillAfterDelay();
        enemy.StopImmediately(true);
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (isGoDown)
        {
            rigidbody.gravityScale = originGravityScale * 2.5f;

            if (enemy.IsGroundDetected())
            {
                enemy.animatorCompo.SetBool(landAnimBoolHash, true);
                rigidbody.gravityScale = originGravityScale;
            }

            return;
        }

        //올라가는 중
        if (isFindingPlayer == true)
        {
            float playerDir = PlayerManager.Instance.PlayerTrm.position.x - enemy.transform.position.x;

            enemy.transform.position +=
                (Vector3.right * playerDir).normalized * playerFollowingSpeed * Time.deltaTime;
        }
        else if (isGoUp == true)
            enemy.transform.position += Vector3.up * 100f * Time.deltaTime;
    }
}
