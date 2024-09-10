using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Health : MonoBehaviour, IDamageable
{
    public Entity owner { get; private set; }

    public Stat maxHp { get; private set; }
    public float weight { get; private set; }
    private float _curHp;
    public float curHp 
    { 
        get => _curHp; 
        private set
        {
            if (_curHp < value)
            {
                OnHeal?.Invoke();
            }
            else
            {
                OnHit?.Invoke();
            }
            _curHp = value;
        }
    }

    public HitData hitData { get; private set; }

    public Stat maxArmor { get; private set; }
    public float curArmor { get; private set; }
    public float lastAttackDamage { get; private set; }
    public bool isLastAttackCritical { get; private set; }

    public bool isInvincible { get; private set; }

    //ȿ��, ���ӽð�, ���۵� �ð�
    //protected List<Tuple<Effect, float, float>> effects = new List<Tuple<Effect, float, float>>();

    public Action OnHit;
    public Action OnHeal;
    public Action OnCrit;
    public Action<Vector2> OnKnockBack;
    public Action<Vector2> OnDie;


    private void Awake()
    {
        owner = GetComponent<Entity>();
        Init();
    }

    public void Init()
    {
        weight = owner.Stat.weight;
        maxHp = owner.Stat.maxHp;
        maxArmor = owner.Stat.maxHp;
        curHp = maxHp.GetValue();
        curArmor = maxArmor.GetValue();
        hitData = new HitData();
    }

    #region HealthRegion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="knockPower"></param>
    /// <param name="dealer"></param>
    public virtual void TakeDamage(int damage, Vector2 knockPower, Entity dealer)
    {
        if (owner.IsDead || isInvincible) return;

        //ũ��Ƽ�� ���
        if (dealer != null)
        {
            float criticalChance = dealer.Stat.criticalChance.GetValue() * 100;
            bool isCrit = Random.Range(0, 10001) <= criticalChance;

            if (isCrit)
            {
                OnCrit?.Invoke();
                damage = (int)(damage * (dealer.Stat.criticalDamage.GetValue() / 100));
            }
            hitData.isLastAttackCritical = isCrit;
        }

        //hitData�� �¾Ҵٶ�� ����
        hitData.lastAttackDamage = damage;
        hitData.lastAttackEntity = dealer;
        hitData.lastHitTime = Time.time;

        //hp���ҽ���
        curHp -= damage;
        curHp = Mathf.Clamp(curHp, 0, maxHp.GetValue());

        //���Է� ���� ���԰� 1�̸� �״��, 2�� 1/2�� ����
        knockPower /= weight;
        AfterHitFeedback(knockPower, true);
    }

    private void AfterHitFeedback(Vector2 knockPower, bool withFeedBack = true)
    {
        if (withFeedBack)
        {
            OnKnockBack?.Invoke(knockPower);
        }
        if (curHp <= 0)
            OnDie?.Invoke(knockPower);
    }

    public void GetHeal(int amount)
    {
        curHp += amount;
        curHp = Mathf.Clamp(curHp, 0, maxHp.GetValue());
    }

    public void GetArmor(int amount)
    {
        curArmor += amount;
        curArmor = Math.Clamp(curArmor, 0, maxArmor.GetValue());
    }

    public void LostArmor(int amount)
    {
        curArmor -= amount;
        curArmor = Math.Clamp(curArmor, 0, maxArmor.GetValue());
    }

    public void ReduceMaxHp(float amount)
    {
        amount = -Mathf.Clamp(amount, 0f, maxHp.GetValue() - 1);
        maxHp.AddModifier(amount);
    }

    public void EnableInvincibility()
    {
        isInvincible = true;
    }

    public void DisableInvincibility()
    {
        isInvincible = false;
    }

    #endregion

    #region EffectRegion

    private void Update()
    {
        //for (int i = 0; i < effects.Count; i++)
        //{
        //    Tuple<Effect, float, float> effect = effects[i];
        //    effect.Item1.UpdateEffort();

        //    if (effect.Item3 + effect.Item2 < Time.time)
        //    {
        //        effect.Item1.ExitEffort();
        //        effects.Remove(effect);
        //    }
        //}
    }


    //public virtual void GetEffort(Effect effect)
    //{
    //    for (int i = 0; i < effects.Count; i++)
    //    {
    //        var item = effects[i];
    //        if (item.Item1 == effect)
    //        {
    //            float remainDuration = item.Item2;
    //            effects[i] = new Tuple<Effect, float, float>(item.Item1, remainDuration + effect.duration, item.Item3);
    //            return;
    //        }
    //    }

    //    effects.Add(new Tuple<Effect, float, float>(effect, effect.duration, Time.time));
    //    effect.EnterEffort(owner);
    //}

    public void Rape(float time)
    {
        KingSlime slime = owner as KingSlime;
        if (slime)
        {
            slime.StateMachine.ChangeState(KingSlimeStateEnum.Vined);
            StartCoroutine(DelayRapeOff(slime, time));
        }
    }

    IEnumerator DelayRapeOff(KingSlime slime, float time)
    {
        yield return new WaitForSeconds(time);
        slime.CanStateChangeable = true;
        slime.StateMachine.ChangeState(KingSlimeStateEnum.Ready);
    }


    #endregion
}

public class HitData
{
    public float lastHitTime;
    public float lastAttackDamage;
    public bool isLastAttackCritical;
    public Entity lastAttackEntity;
}