using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerThrowState : PlayerState
{
    private readonly int _xInputHash = Animator.StringToHash("XInput");
    private Transform _throwPosTrm;
    private Projectary _projectary;

    public PlayerThrowState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        _throwPosTrm = player.transform.Find("ThrowPos");
        _projectary = player.transform.Find("Projectary").GetComponent<Projectary>();
    }

    public override void Enter()
    {
        base.Enter();
        _projectary.gameObject.SetActive(true);
        player.PlayerInput.OnUseQuickSlot += HandleThrow;
        player.animatorCompo.SetInteger(_xInputHash, Mathf.CeilToInt(player.PlayerInput.XInput));
    }

    public override void UpdateState()
    {
        Vector3 dir = (player.PlayerInput.MousePosition - (Vector2)player.transform.position).normalized;

        _projectary.DrawLine(_throwPosTrm.position, dir * 30);


        float z = Vector3.Cross(dir, Vector2.up).z;
        if (z > 0 && player.FacingDir != 1)
            player.Flip();
        else if (z < 0 && player.FacingDir != -1)
            player.Flip();

        float xInput = player.PlayerInput.XInput;
        player.animatorCompo.SetInteger(_xInputHash, Mathf.CeilToInt(xInput));
        player.SetVelocity(xInput * 5, rigidbody.velocity.y, true);
    }

    public override void Exit()
    {
        _projectary.gameObject.SetActive(false);
        player.PlayerInput.OnUseQuickSlot -= HandleThrow;
        base.Exit();
    }

    private void HandleThrow()
    {
        Vector3 dir = (player.PlayerInput.MousePosition - (Vector2)player.transform.position).normalized;
        Rigidbody2D rigid = GameObject.Instantiate(player.testObject, _throwPosTrm.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        rigid.AddForce(dir * 30, ForceMode2D.Impulse);
        stateMachine.ChangeState(PlayerStateEnum.Idle);
    }
}
