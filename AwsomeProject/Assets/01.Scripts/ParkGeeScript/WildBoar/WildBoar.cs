using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WildBoarEnum
{
    Idle,
    Move,
    Rush,
    Groggy,
    Stun,
    Dead
}

public enum WildBoarSkillEnum
{
    Rush
}

public class WildBoar : Enemy<WildBoarEnum>
{
    //public WildBoarStatus wildBoarStatus { get; protected set; }
    public WildBoarSkill Skills { get; private set; }

    [SerializeField] private Transform _hpBar;
    public GameObject dashAttackCollider; 
    private Transform _playerTrm;
    private Transform _pivot;

    private SkillSO _rushSkill;

    private float _attackDelay;
    public void SetAttackDelay(float delay) => _attackDelay = Time.time + delay;


    protected override void Awake()
    {
        base.Awake();
        _playerTrm = PlayerManager.Instance.PlayerTrm;

        Skills = gameObject.AddComponent<WildBoarSkill>();
        Skills.Init(EntitySkillSO);

        foreach (var item in EntitySkillSO.skills)
        {
            item.skill.SetOwner(this);
        }

        moveSpeed = Stat.moveSpeed.GetValue();
        detectingDistance = EnemyStat.detectingDistance.GetValue();
        _rushSkill = Skills.GetSkillByEnum(WildBoarSkillEnum.Rush);

        _pivot = _hpBar.Find("Pivot");
    }

    private void OnEnable()
    {
        healthCompo.onKnockBack += KnockBack;
        healthCompo.onHit += OnHit;
        healthCompo.onDie += OnDie;
    }

    private void OnDisable()
    {
        healthCompo.onKnockBack -= KnockBack;
        healthCompo.onHit -= OnHit;
        healthCompo.onDie -= OnDie;
    }

    private void Start()
    {
        StateMachine.Initialize(WildBoarEnum.Idle, this);
        attackDistance = Skills.GetSkillByEnum(WildBoarSkillEnum.Rush).attackDistance.GetValue();
    }

    private void Update()
    {
        StateMachine.CurrentState.UpdateState();

        float hpPercentage = (float)healthCompo.curHp / healthCompo.maxHp.GetValue();
        _hpBar.localScale = new Vector3(FacingDir * _hpBar.localScale.x, _hpBar.localScale.y, _hpBar.localScale.z);
        _pivot.localScale = new Vector3(hpPercentage, 1, 1);
    }

    public void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public override void Stun(float duration)
    {
        if (IsDead) return;
        stunDuration = duration;
        StateMachine.ChangeState(WildBoarEnum.Stun);
    }

    public override void Dead(Vector2 dir) { }

    public void TryAttack()
    {
        float dist = (transform.position - _playerTrm.position).magnitude;

        if (Time.time > _attackDelay && dist < _rushSkill.attackDistance.GetValue())
        {
            _rushSkill.skill.UseSkill();
        }
        else if(dist < attackDistance)
        {
            Attack();
        }
    }

    public void Attack()
    {

    }

    private void OnHit()
    {
        HitEvent?.Invoke();
        StateMachine.ChangeState(WildBoarEnum.Idle);
    } 

    private void OnDie(Vector2 dir)
    {
        IsDead = true;
        for (int i = 0; i < EnemyStat.dropItems.Count; i++)
        {
            if (UnityEngine.Random.Range(0, 101) < EnemyStat.dropItems[i].appearChance)
            {
                DropItem dropItem = Instantiate(EnemyStat.dropItems[i].dropItemPf).GetComponent<DropItem>();
                dropItem.transform.position = transform.position + Vector3.up;
                dropItem.SpawnItem(dir);
            }
        }

        StateMachine.ChangeState(WildBoarEnum.Dead);
    }
}
