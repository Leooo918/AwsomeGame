using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rigidbody;

    protected int _animBoolHash;
    protected readonly int _yVelocityHash = Animator.StringToHash("y_velocity");
    protected bool _triggerCall;

    public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        _animBoolHash = Animator.StringToHash(animBoolName);
        rigidbody = this.player.rigidbodyCompo;
    }

    //���¿� �������� �� ������ �Լ�
    public virtual void Enter()
    {
        player.animatorCompo.SetBool(_animBoolHash, true);
        _triggerCall = false; //�ִϸ��̼��� �� �������� ����� �Ҹ��� ��
    }

    //���¸� ������ ������ �Լ�
    public virtual void Exit()
    {
        player.animatorCompo.SetBool(_animBoolHash, false);
    }

    //�� ������ �� ����� �Լ�
    public virtual void UpdateState()
    {
        //player.animatorCompo.SetFloat(_yVelocityHash, _rigidbody.velocity.y);
    }

    
    public virtual void AnimationFinishTrigger()
    {
        _triggerCall = true;
    }
}
