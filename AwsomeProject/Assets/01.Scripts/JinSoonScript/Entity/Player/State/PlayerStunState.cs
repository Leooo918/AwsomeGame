using UnityEngine;

public class PlayerStunState : PlayerState
{
    private float stunTime;

    public PlayerStunState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        player.CanStateChangeable = false;
        player.StunEffect.SetActive(true);
        stunTime = Time.time;
    }

    public override void UpdateState()
    {
        base.UpdateState();


        if (Time.time - stunTime >= player.stunDuration)
        {
            player.CanStateChangeable = true;
            stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    public override void Exit()
    {
        player.StunEffect.SetActive(false);
        player.CanStateChangeable = true;
        base.Exit();
    }
}
