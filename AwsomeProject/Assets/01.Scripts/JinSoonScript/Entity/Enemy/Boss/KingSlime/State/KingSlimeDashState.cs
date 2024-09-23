using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VHierarchy.Libs;

public class KingSlimeDashState : EnemyState<KingSlimeStateEnum>
{
    private KingSlime _kingSlime;
    private Vector2 _dashDir;

    private bool _isDashStarted = false;

    public KingSlimeDashState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        _kingSlime = enemy as KingSlime;
    }

    public override void Enter()
    {
        base.Enter();
        _kingSlime.StartCoroutine(DashCoroutine());
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_isDashStarted)
        {
            if (_kingSlime.IsWallDetected(out Collider2D collider)) // 쳐박았따? 왜 뭘 쳐박았는지는 안알려주지? 난 알고싶다!!!!! 즉시 오버로드
            {
                if(collider.TryGetComponent(out GrowingBush bush))
                {
                    bush.gameObject.Destroy();
                }
                else
                {
                    _kingSlime.FlipController(-_dashDir.x);
                }
                _kingSlime.KnockBack(-_dashDir * 2f);
                enemyStateMachine.ChangeState(KingSlimeStateEnum.Idle);
            }
        }
    }

    public override void Exit()
    {
        _isDashStarted = false;
        base.Exit();
    }

    private IEnumerator DashCoroutine()
    {
        //대충 뭐 _kingSlime.dashInfos 하나 뽑아와서 돌진 방향이랑 이것저것 다 정해주면 됨.
        //위치도 바꿔주고.
        //DOJump라는 개 사기 메서드가 있었잖아???
        DashInfo info = _kingSlime.dashInfos[Random.Range(0, _kingSlime.dashInfos.Length)];
        _dashDir = info.direction;
        _kingSlime.FlipController(-_dashDir.x);
        _kingSlime.transform.DOJump(info.dashStartPos.position, 5, 1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        _kingSlime.FlipController(_dashDir.x);
        yield return new WaitForSeconds(_kingSlime.beforeDashDelay);
        _kingSlime.MovementCompo.SetVelocity(_dashDir * _kingSlime.dashSpeed);
        _isDashStarted = true;
    }
}
