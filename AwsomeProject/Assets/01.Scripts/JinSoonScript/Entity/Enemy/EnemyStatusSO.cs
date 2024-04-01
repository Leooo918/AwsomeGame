using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
public class EnemyStatusSO<T> : StatusSO where T : Enum
{
    public Dictionary<StatEnum, Stat> statDic = new Dictionary<StatEnum, Stat>();
    public Dictionary<T, SkillSO> skillDic = new Dictionary<T, SkillSO>();

    [Header("TheseWillNotChange")]
    public float PatrolTime;
    public float PatrolDelay;
    public float DetectingDistance;

    [Header("Skills")]
    public List<SkillSO> skills;

    protected void OnEnable()
    {
        statDic = new Dictionary<StatEnum, Stat>();

        Type playerStatType = typeof(StatEnum);

        foreach (StatEnum statType in Enum.GetValues(typeof(StatEnum)))
        {
            string fieldName = statType.ToString();

            try
            {
                FieldInfo playerStatField = playerStatType.GetField(fieldName);
                Stat stat = playerStatField.GetValue(this) as Stat;

                statDic.Add(statType, stat);
            }
            catch (Exception ex)
            {
                Debug.LogError($"There are no stat field in player: {fieldName}, msg:{ex.Message}");
            }
        }

        foreach (T skillType in Enum.GetValues(typeof(T)))
        {
            foreach (var item in skills)
            {
                if (item.skillName == skillType.ToString())
                {
                    skillDic[skillType] = item;
                }
            }
        }
    }

    public Stat GetStatByEnum(StatEnum playerStat)
    {
        return statDic[playerStat];
    }

    public SkillSO GetSkillByEnum(T skillType)
    {
        return skillDic[skillType];
    }
}
