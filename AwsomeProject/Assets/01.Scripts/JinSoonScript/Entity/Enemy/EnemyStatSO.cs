using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyStat")]
public class EnemyStatSO : ScriptableObject
{
    public Stat patrolTime;
    public Stat patrolDelay;
    public Stat detectingDistance;

    public List<DropItemStruct> dropItems = new List<DropItemStruct>();

    protected Dictionary<EnemyStatType, Stat> statDic;

    protected virtual void OnEnable()
    {
        statDic = new Dictionary<EnemyStatType, Stat>();

        Type agentStatType = typeof(EnemyStatSO);

        foreach (EnemyStatType enumType in Enum.GetValues(typeof(EnemyStatType)))
        {
            try
            {
                string fieldName = LowerFirstChar(enumType.ToString());
                FieldInfo statField = agentStatType.GetField(fieldName);
                statDic.Add(enumType, statField.GetValue(this) as Stat);
            }
            catch (Exception ex)
            {
                Debug.LogError($"There are no stat - {enumType.ToString()}, {ex.Message}");
            }
        }
    }

    private string LowerFirstChar(string input) => $"{char.ToLower(input[0])}{input.Substring(1)}";
}

public enum EnemyStatType
{
    PatrolTime,
    PatrolDelay,
    DetectingDistance,
}
