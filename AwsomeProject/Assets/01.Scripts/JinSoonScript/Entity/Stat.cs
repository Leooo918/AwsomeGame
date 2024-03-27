using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private int baseValue;

    public List<int> modifiers = new List<int>();
    public List<int> percentModifiers = new List<int>();


    /// <summary>
    /// 값에 수정된 값까지 다 계산해서 들고옴
    /// </summary>
    /// <returns></returns> 
    public int GetValue()
    {
        int finalValue = baseValue;

        foreach (int value in modifiers)
            finalValue += value;

        foreach (int value in percentModifiers)
            finalValue += finalValue / value;

        return finalValue;
    }

    /// <summary>
    /// 추가해둔 수정자를 제거함
    /// </summary>
    /// <param name="value"></param>
    public void RemoveModifier(int value)
    {
        if (value != 0)
            modifiers.Remove(value);
    }

    /// <summary>
    /// 수정자를 추가해줌
    /// </summary>
    /// <param name="value">얼마나 수정해줄지(값을 줄이고 싶다면 음수를 주면 됨)</param>
    public void AddModifier(int value)
    {
        if (value != 0)
            modifiers.Add(value);
    }

    /// <summary>
    /// 수정자를 값이아니라 비율로 추가해줌
    /// </summary>
    /// <param name="percentValue">음수면 줄이고 양수면 늘리는 연산으로 해줄거임</param>
    public void AddModifierByPercent(int percentValue)
    {
        if (percentValue != 0)
            percentModifiers.Add(percentValue);
    }

    /// <summary>
    /// 수정자를 값이아니라 비율로 추가해준거 제거
    /// </summary>
    /// <param name="percentValue"></param>
    public void RemoveModifierByPercent(int percentValue)
    {
        if (percentValue != 0)
            percentModifiers.Remove(percentValue);
    }

    /// <summary>
    /// 걍 모든 수정자들을 없애
    /// </summary>
    /// <param name="value"></param>
    public void SetDefaultValue(int value)
    {
        baseValue = value;
    }
}
