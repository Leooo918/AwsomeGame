using UnityEngine;

public class PlayerStunState : PlayerState
{
    private float stunTime;

    public PlayerStunState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        stunTime = Time.time;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Debug.Log("±âÀý ¹Ö");
        if (Time.time - stunTime >= player.stunDuration)
        {
            stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
