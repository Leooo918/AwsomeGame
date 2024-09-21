using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AirBirdEnum
{
    Idle,
    Patrol,
    Chase,
    Shoot,
    Stun,
    Dead
}

public enum AirBirdSkillEnum
{
    Shoot
}

public class AirBird : Enemy<AirBirdEnum>
{
    [HideInInspector] public bool moveAnima = false;
    [HideInInspector] public bool readyFlip = false;

    public Feather featherPrefab;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        healthCompo.OnKnockBack += KnockBack;
        healthCompo.OnHit += OnHit;
        healthCompo.OnDie += OnDie;
    }

    private void OnDisable()
    {
        healthCompo.OnKnockBack -= KnockBack;
        healthCompo.OnHit -= OnHit;
        healthCompo.OnDie -= OnDie;
    }

    private void Start()
    {
        StateMachine.Initialize(AirBirdEnum.Idle, this);
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.CurrentState.UpdateState();
    }

    public void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public override void Stun(float duration)
    {
        base.Stun(duration);
        if (IsDead) return;
        stunDuration = duration;
        StateMachine.ChangeState(AirBirdEnum.Stun);
    }

    public override void Stone(float duration)
    {
        base.Stone(duration);
        animatorCompo.speed = 0;
    }

    public override void Dead(Vector2 dir)
    {

    }

    private void OnHit()
    {
        HitEvent?.Invoke();
        StateMachine.ChangeState(AirBirdEnum.Idle);
    }

    private void OnDie(Vector2 dir)
    {
        for (int i = 0; i < EnemyStat.dropItems.Count; i++)
        {
            if (UnityEngine.Random.Range(0, 101) < EnemyStat.dropItems[i].appearChance)
            {
                DropItem dropItem = Instantiate(EnemyStat.dropItems[i].dropItemPf).GetComponent<DropItem>();
                dropItem.transform.position = transform.position + Vector3.up;
                dropItem.SpawnItem(dir);
            }
        }

        CanStateChangeable = true;
        StateMachine.ChangeState(AirBirdEnum.Dead);
        CanStateChangeable = false;
        IsDead = true;
    }
}
