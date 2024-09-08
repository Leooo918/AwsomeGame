using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBirdChaseState : EnemyState<AirBirdEnum>
{
    private AirBird _airBird;
    private Transform _playerTrm;

    private float _upDownSpeed = 2f;
    private bool _isGoDown = false;

    public AirBirdChaseState(Enemy<AirBirdEnum> enemy, EnemyStateMachine<AirBirdEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        _airBird = enemy as AirBird;
        _playerTrm = PlayerManager.Instance.PlayerTrm;
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
        float xDist = Mathf.Abs(_playerTrm.position.x - enemy.transform.position.x);
        if (dist > _airBird.runAwayDistance)
            enemyStateMachine.ChangeState(AirBirdEnum.Idle);

        float playerDir = Mathf.Sign((_playerTrm.position - enemy.transform.position).x);

        if (enemy.FacingDir != playerDir && xDist > 4)
            enemy.Flip();

        float x = enemy.FacingDir * enemy.moveSpeed;
        float y = _isGoDown ? -1 : 1;

        enemy.MovementCompo.SetVelocity(new Vector2(x, y * _upDownSpeed));
        _airBird.TryAttack();
    }
}
