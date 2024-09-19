using UnityEngine;

public class SlimeReturnState : EnemyState<SlimeStateEnum>
{
    private Slime slime;
    private SlimeJumpSkillSO jumpSkill;
    private bool isJumping = false;

    public SlimeReturnState(Enemy<SlimeStateEnum> enemy, EnemyStateMachine<SlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        slime = enemy as Slime;
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
