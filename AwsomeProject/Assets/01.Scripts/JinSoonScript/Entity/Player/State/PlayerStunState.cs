using UnityEngine;

public class PlayerStunState : PlayerState
{
    private float stunTime;

    public PlayerStunState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        player.CanStateChangeable = false;
        player.stunEffect.SetActive(true);
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
        player.stunEffect.SetActive(false);
        player.CanStateChangeable = true;
        base.Exit();
    }
}
