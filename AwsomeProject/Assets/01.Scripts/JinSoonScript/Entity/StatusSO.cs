using System.Collections;
using UnityEngine;



public abstract class StatusSO : ScriptableObject
{
    [Header("CommonSetting")]
    public Stat MaxHealth;
    public Stat Attack;
    public Stat AttackSpeed;
    public Stat CriticalPercent;
    public Stat MoveSpeed;
    public Stat Defence;

    protected Entity owner;

    public virtual void SetOwner(Enemy owner)
    {
        this.owner = owner;
    }

    public void IncreaseStatBy(int value, float time, Stat stat)
    {
        owner.StartCoroutine(DelayReturnOriginValue(value, time, stat));
    }

    private IEnumerator DelayReturnOriginValue(int value, float time, Stat stat)
    {
        stat.AddModifier(value);
        yield return new WaitForSeconds(time);
        stat.RemoveModifier(value);
    }


    public void CalculateDamage(ref int value)
    {
        Defence.GetValue();
        //방어력 연산을 이쪽에 써주면 됨

        if (Random.Range(0, 100) > CriticalPercent.GetValue()) value = (int)(value * 1.5f);
    }

}
