using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBoarMoveState : EnemyState<WildBoarEnum>
{
    private Transform _playerTrm;
    private WildBoar _wildBoar;

    public WildBoarMoveState(Enemy<WildBoarEnum> enemy, EnemyStateMachine<WildBoarEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        _playerTrm = PlayerManager.Instance.PlayerTrm;
        _wildBoar = enemy as WildBoar;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (Mathf.Sign(_playerTrm.position.x - enemy.transform.position.x) != Mathf.Sign(enemy.FacingDir))
            enemy.Flip();

        float moveDir = enemy.FacingDir * enemy.moveSpeed;
        enemy.MovementCompo.SetVelocity(new Vector2(moveDir, enemy.rigidbodyCompo.velocity.y));

        _wildBoar.TryAttack();
    }
}
