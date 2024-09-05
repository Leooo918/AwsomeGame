using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPushState : PlayerState
{
    private readonly int _inputHash = Animator.StringToHash("Input");

    private Transform _pushObjectPosTrm;
    private Rigidbody2D _pushObjectRigid;
    public PlayerPushState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        _pushObjectPosTrm = player.transform.Find("PushObjectPos");
    }

    public override void Enter()
    {
        base.Enter();
        _pushObjectRigid = player.CurrentPushTrm.GetComponent<Rigidbody2D>();
        _pushObjectRigid.velocity = Vector3.zero;
        Vector3 offset = new Vector3((player.CurrentPushTrm.GetComponent<BoxCollider2D>().size.x / 2) * player.CurrentPushTrm.localScale.x, 0);
        float prevY = player.CurrentPushTrm.position.y;
        player.CurrentPushTrm.position = _pushObjectPosTrm.position + offset * player.FacingDir;
        player.CurrentPushTrm.position = new Vector3(player.CurrentPushTrm.position.x, prevY);

        player.PlayerInput.InteractPress += HandleInteract;
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void HandleInteract()
    {
        player.CurrentPushTrm = null;
        stateMachine.ChangeState(PlayerStateEnum.Idle);
    }

    public override void UpdateState()
    {
        float xInput = player.PlayerInput.XInput;
        _pushObjectRigid.velocity = new Vector2(xInput * 4, _pushObjectRigid.velocity.y);
        player.SetVelocity(xInput * 4, rigidbody.velocity.y, true);
        player.animatorCompo.SetInteger(_inputHash, Mathf.CeilToInt(xInput));
    }
}
