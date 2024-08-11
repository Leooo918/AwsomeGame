using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBirdPatrolState : EnemyState<AirBirdEnum>
{
    private AirBird _airBird;
    private Transform _playerTrm;
    private bool _isGoDown = false;

    private float _upDownDelay = 0.1f;
    private float _upDownChangeTime;

    public AirBirdPatrolState(Enemy<AirBirdEnum> enemy, EnemyStateMachine<AirBirdEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        _airBird = enemy as AirBird;
        _playerTrm = PlayerManager.Instance.PlayerTrm;
    }

    public override void Enter()
    {
        base.Enter();
        _upDownChangeTime = Time.time + _upDownDelay;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        float playerDir = Mathf.Sign((_playerTrm.position - enemy.transform.position).x);
        
        if (enemy.FacingDir != playerDir)
            enemy.Flip();

        if(_upDownChangeTime < Time.time)
        {
            _upDownChangeTime = Time.time + _upDownDelay;
            _isGoDown = !_isGoDown;
        }

        float x = enemy.FacingDir * enemy.moveSpeed;
        float y = _isGoDown ? -1 : 1;

        enemy.SetVelocity(x,y);
        _airBird.TryAttack();
    }
}
