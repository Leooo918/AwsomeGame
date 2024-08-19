using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        DiePanel diePanel = UIManager.Instance.GetUI(UIType.PlayerDie) as DiePanel;
        diePanel.Init((int)GameManager.Instance.playTime, GameManager.Instance.killCnt, GameManager.Instance.gatherCnt, 99, 0.6f);
        diePanel.Open();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
