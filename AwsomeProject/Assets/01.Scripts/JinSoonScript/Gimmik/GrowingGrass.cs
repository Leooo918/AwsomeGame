using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VineState
{
    Growing,
    Grown,
    Shrinking,
    Shrunk,
}

public class GrowingGrass : MonoBehaviour, IAffectable, IAnimationTriggerable
{
    private Animator _animator;

    private readonly int _growStartHash = Animator.StringToHash("GrowStart");
    private readonly int _growResetHash = Animator.StringToHash("GrowReset");

    private int _animationTriggerBit = 0;

    public VineState CurrentState { get; private set; } = VineState.Grown;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        CurrentState = VineState.Shrunk;
    }

    public void ApplyEffect()
    {
        StartCoroutine(IEGrowing());
    }

    private IEnumerator IEGrowing()
    {
        CurrentState = VineState.Growing;
        _animator.SetTrigger(_growStartHash);
        yield return new WaitUntil(() => IsTriggered(AnimationTriggerEnum.EndTrigger));
        CurrentState = VineState.Grown;
        RemoveTrigger(AnimationTriggerEnum.EndTrigger);
        yield return new WaitForSeconds(5f);
        CurrentState = VineState.Shrinking;
        _animator.SetTrigger(_growResetHash);
        yield return new WaitUntil(() => IsTriggered(AnimationTriggerEnum.EndTrigger));

        CurrentState = VineState.Shrunk;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            player.StateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    public void AnimationTrigger(AnimationTriggerEnum trigger)
    {
        _animationTriggerBit |= (int)trigger;
    }

    public bool IsTriggered(AnimationTriggerEnum trigger) => (_animationTriggerBit & (int)trigger) != 0;

    public void RemoveTrigger(AnimationTriggerEnum trigger)
    {
        _animationTriggerBit &= ~(int)trigger;
    }
}
