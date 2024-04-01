using System;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public StatusSO Status { get; private set; }
    public Entity owner { get; private set; }
    public Stat maxHp { get; private set; }
    public int curHp { get; private set; }

    //효과, 지속시간, 시작된 시간
    protected List<Tuple<Effect, float, float>> effects = new List<Tuple<Effect, float, float>>();

    public Action onHit;
    public Action<Vector2> onKnockBack;
    public Action<Vector2> onDie;

    public void Init(StatusSO status)
    {
        this.Status = status;
        maxHp = Status.MaxHealth;
        curHp = maxHp.GetValue();
    }

    #region HealthRegion

    public void TakeDamage(int damage, Vector2 knockPower, Entity dealer)
    {
        //방어력 계산해주셈
        //damage = owners.

        curHp -= damage;

        AfterHitFeedback(knockPower, true);
    }

    private void AfterHitFeedback(Vector2 knockPower, bool withFeedBack = true)
    {
        if (withFeedBack)
        {
            onHit?.Invoke();
            onKnockBack?.Invoke(knockPower);
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

    public void ReduceMaxHp(int amount)
    {
        amount = -Mathf.Clamp(amount, 0, maxHp.GetValue() - 1);
        maxHp.AddModifier(amount);
    }

    #endregion

    #region EffectRegion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(10, new Vector2(-3, 3), owner);
        }

        foreach (var effect in effects)
        {
            effect.Item1.UpdateEffort();

            if (effect.Item3 + effect.Item2 < Time.time)
            {
                effect.Item1.ExitEffort();
                effects.Remove(effect);
            }
        }
    }


    public virtual void GetEffort(Effect effect, float duration, bool isInfiniteEffect = false)
    {

        for (int i = 0; i < effects.Count; i++)
        {
            var item = effects[i];
            if (item.Item1 == effect && item.Item1.isInfiniteEffect == false)
            {
                float remainDuration = item.Item2;
                effects[i] = new Tuple<Effect, float, float>(item.Item1, remainDuration + duration, item.Item3);
                return;
            }
        }

        effect.Init(duration, isInfiniteEffect);
        effects.Add(new Tuple<Effect, float, float>(effect, duration, Time.time));
        effect.EnterEffort(owner);
    }


    #endregion
}
