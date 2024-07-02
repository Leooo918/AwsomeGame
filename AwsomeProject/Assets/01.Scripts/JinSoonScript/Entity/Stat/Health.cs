using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable, IGetPortionEffect
{
    public Entity owner { get; private set; }

    public Stat maxHp { get; private set; }
    public float curHp { get; private set; }
    public float lastAttackDamage { get; private set; }
    public bool isLastAttackCritical { get; private set; }
    public bool isInvincible { get; private set; }

    //효과, 지속시간, 시작된 시간
    protected List<Tuple<Effect, float, float>> effects = new List<Tuple<Effect, float, float>>();

    public Action onHit;
    public Action<Vector2> onKnockBack;
    public Action<Vector2> onDie;


    private void Awake()
    {
        owner = GetComponent<Entity>();
        Init();
    }

    public void Init()
    {
        maxHp = owner.Stat.maxHp;
        curHp = maxHp.GetValue();
    }

    #region HealthRegion

    public void TakeDamage(int damage, Vector2 knockPower, Entity dealer)
    {
        if (owner.isDead || isInvincible) return;
        //방어력 계산, 크리티컬 확인
        //_damage = owners.

        isLastAttackCritical = false;
        lastAttackDamage = damage;
        curHp -= damage;
        curHp = Mathf.Clamp(curHp, 0, maxHp.GetValue());

        if (curHp <= 0) onDie?.Invoke(knockPower);
        AfterHitFeedback(knockPower, true);
    }

    private void AfterHitFeedback(Vector2 knockPower, bool withFeedBack = true)
    {
        if (withFeedBack)
        {
            onHit?.Invoke();
            //onKnockBack?.Invoke(knockPower);
        }
        if (curHp <= 0)
        {
            onDie?.Invoke(knockPower);
        }
    }

    public void GetHeal(int amount)
    {
        curHp += amount;
        curHp = Mathf.Clamp(curHp, 0, maxHp.GetValue());
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

        for (int i = 0; i < effects.Count; i++)
        {
            Tuple<Effect, float, float> effect = effects[i];
            effect.Item1.UpdateEffort();

            if (effect.Item3 + effect.Item2 < Time.time)
            {
                effect.Item1.ExitEffort();
                effects.Remove(effect);
            }
        }
    }


    public virtual void GetEffort(Effect effect)
    {
        for (int i = 0; i < effects.Count; i++)
        {
            var item = effects[i];
            if (item.Item1 == effect)
            {
                float remainDuration = item.Item2;
                effects[i] = new Tuple<Effect, float, float>(item.Item1, remainDuration + effect.duration, item.Item3);
                return;
            }
        }

        effects.Add(new Tuple<Effect, float, float>(effect, effect.duration, Time.time));
        effect.EnterEffort(owner);
    }

    public void Rape(float time)
    {
        KingSlime slime = owner as KingSlime;
        if (slime)
        {
            Debug.Log("밍밍");
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
