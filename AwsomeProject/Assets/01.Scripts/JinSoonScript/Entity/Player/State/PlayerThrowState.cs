using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerThrowState : PlayerState
{
    private readonly int _inputHash = Animator.StringToHash("Input");
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
        player.animatorCompo.SetInteger(_inputHash, Mathf.CeilToInt(player.PlayerInput.XInput));
    }

    public override void UpdateState()
    {
        InventorySlot slot = QuickSlotManager.Instance.GetSelectedPotionSlot();
        if (slot == null || slot.assignedItem == null || slot.assignedItem.itemSO is DrinkPotionItemSO)
            stateMachine.ChangeState(PlayerStateEnum.Idle);

        Vector3 dir = (player.PlayerInput.MousePosition - (Vector2)player.transform.position).normalized;

        _projectary.DrawLine(_throwPosTrm.position, dir * 30);


        float z = Vector3.Cross(dir, Vector2.up).z;
        if (z > 0 && player.FacingDir != 1)
            player.Flip();
        else if (z < 0 && player.FacingDir != -1)
            player.Flip();

        float xInput = player.PlayerInput.XInput;
        player.animatorCompo.SetInteger(_inputHash, Mathf.CeilToInt(xInput));
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
        ThrowPotion potion = GameObject.Instantiate(QuickSlotManager.Instance.throwPotion, _throwPosTrm.position, Quaternion.identity);
        potion.Init(QuickSlotManager.Instance.GetSelectedPotionSlot());
        potion.GetComponent<Rigidbody2D>().AddForce(dir * 30, ForceMode2D.Impulse);
        stateMachine.ChangeState(PlayerStateEnum.Idle);
    }
}
