using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBirdIdleState : EnemyState<AirBirdEnum>
{
    private Transform _playerTrm;
    private bool _isGoDown = false;

    private float _originHeight;
    private float _upDownSpeed = 2f;

    public AirBirdIdleState(Enemy<AirBirdEnum> enemy, EnemyStateMachine<AirBirdEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        _playerTrm = PlayerManager.Instance.PlayerTrm;
        _originHeight = enemy.transform.position.y;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        _isGoDown = !_isGoDown;
    }


    public override void UpdateState()
    {
        base.UpdateState();

        float dist = (_playerTrm.position - enemy.transform.position).magnitude;
        if(dist < enemy.EnemyStat.detectingDistance.GetValue())
            enemyStateMachine.ChangeState(AirBirdEnum.Chase);

        float y = _isGoDown ? -1 : 1;
        enemy.MovementCompo.SetVelocity(new Vector2(0, y * _upDownSpeed));
    }
}
