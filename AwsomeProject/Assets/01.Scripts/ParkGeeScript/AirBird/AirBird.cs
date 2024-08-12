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
    public AirBirdSkill Skills { get; private set; }

    [HideInInspector] public bool moveAnima = false;
    [HideInInspector] public bool readyFlip = false;

    [SerializeField] private Transform hpBar;
    [SerializeField] private GameObject _featherPf;
    private Transform _playerTrm;
    private Transform pivot;

    public GameObject FeatherPf => _featherPf;
    private SkillSO _shootSkill;

    private float _skillReuseTime;

    public void SetAfterDelay(float time) => _skillReuseTime = Time.time + time;

    protected override void Awake()
    {
        base.Awake();
        _playerTrm = PlayerManager.Instance.PlayerTrm;
        Skills = gameObject.AddComponent<AirBirdSkill>();
        Skills.Init(EntitySkillSO);

        foreach (var item in EntitySkillSO.skills)
        {
            item.skill.SetOwner(this);
            Type type = item.skill.GetType();
            gameObject.AddComponent(type);
        }

        moveSpeed = Stat.moveSpeed.GetValue();
        detectingDistance = EnemyStat.detectingDistance.GetValue();

        pivot = hpBar.Find("Pivot");
        _shootSkill = Skills.GetSkillByEnum(AirBirdSkillEnum.Shoot);
        _skillReuseTime = Time.time;
    }

    private void OnEnable()
    {
        healthCompo.onHit += OnHit;
        healthCompo.onDie += OnDie;
    }

    private void OnDisable()
    {
        healthCompo.onHit -= OnHit;
        healthCompo.onDie -= OnDie;
    }

    private void Start()
    {
        StateMachine.Initialize(AirBirdEnum.Idle, this);
    }

    private void Update()
    {
        StateMachine.CurrentState.UpdateState();
    }

    public void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public override void Stun(float duration)
    {
        if (IsDead) return;
        stunDuration = duration;
        StateMachine.ChangeState(AirBirdEnum.Stun);
    }

    public override void Dead(Vector2 dir)
    {
    }

    public void TryAttack()
    {
        float dist = (_playerTrm.position - transform.position).magnitude;
        if (_shootSkill.attackDistance.GetValue() < dist || _skillReuseTime > Time.time) return;

        _shootSkill.skill.UseSkill();
    }


    private void OnHit()
    {
        HitEvent?.Invoke();
        StateMachine.ChangeState(AirBirdEnum.Shoot);
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
