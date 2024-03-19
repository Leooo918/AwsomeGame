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

    //상태에 진입했을 때 실행할 함수
    public virtual void Enter()
    {
        player.animatorCompo.SetBool(_animBoolHash, true);
        _triggerCall = false; //애니메이션이 다 끝났을때 실행될 불리언 값
    }

    //상태를 나갈때 실행할 함수
    public virtual void Exit()
    {
        player.animatorCompo.SetBool(_animBoolHash, false);
    }

    //이 상태일 때 실행될 함수
    public virtual void UpdateState()
    {
        //player.animatorCompo.SetFloat(_yVelocityHash, _rigidbody.velocity.y);
    }

    
    public virtual void AnimationFinishTrigger()
    {
        _triggerCall = true;
    }
}
