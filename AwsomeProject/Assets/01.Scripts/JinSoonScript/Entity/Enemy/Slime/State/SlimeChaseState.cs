using UnityEngine;

public class SlimeChaseState : EnemyState<SlimeStateEnum>
{
    private Slime _slime;
    private Transform _playerTrm;
    private SlimeJumpSkillSO _jumpSkill;
    private bool _canJump = false;

    public SlimeChaseState(Enemy<SlimeStateEnum> enemy, EnemyStateMachine<SlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        _slime = enemy as Slime;
        _playerTrm = PlayerManager.Instance.PlayerTrm;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        //�Ÿ��� �־����� �� ���ư�
        float dist = Vector3.Distance(_playerTrm.position, enemy.transform.position);

        if (dist > enemy.detectingDistance + 5)
        {
            enemy.MissPlayer();
            enemyStateMachine.ChangeState(SlimeStateEnum.Idle);
        }

        //���� �������� ������ ����!
        if (dist <= enemy.attackDistance) _slime.Attack();

        //�տ� ���� ���ٸ� ���Ѹ� ��
        if (_slime.CheckFront() == false) return;

        //���� ���θ��� ������ ����
        _canJump = _slime.IsGroundDetected();
        if (_slime.IsWallDetected() == true && _canJump == true) Jump();

        //��� �i�ư��� ���ְ�
        Vector2 dir = (_playerTrm.position - enemy.transform.position).normalized;
        if (Mathf.Sign(dir.x) != Mathf.Sign(enemy.FacingDir)) enemy.Flip();

        if (_slime.moveAnim == true)
            enemy.SetVelocity(dir.x * enemy.moveSpeed, enemy.rigidbodyCompo.velocity.y);
    }

    private void Jump()
    {
        if (_jumpSkill == null)
            _jumpSkill = _slime.Skills.GetSkillByEnum(SlimeSkillEnum.JumpAttack) as SlimeJumpSkillSO;

        enemy.SetVelocity(0, _jumpSkill.jumpPower.GetValue());
    }
}
