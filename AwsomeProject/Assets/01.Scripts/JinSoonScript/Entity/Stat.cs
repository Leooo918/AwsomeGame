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
    /// ���� ������ ������ �� ����ؼ� ����
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
    /// �߰��ص� �����ڸ� ������
    /// </summary>
    /// <param name="value"></param>
    public void RemoveModifier(int value)
    {
        if (value != 0)
            modifiers.Remove(value);
    }

    /// <summary>
    /// �����ڸ� �߰�����
    /// </summary>
    /// <param name="value">�󸶳� ����������(���� ���̰� �ʹٸ� ������ �ָ� ��)</param>
    public void AddModifier(int value)
    {
        if (value != 0)
            modifiers.Add(value);
    }

    /// <summary>
    /// �����ڸ� ���̾ƴ϶� ������ �߰�����
    /// </summary>
    /// <param name="percentValue">������ ���̰� ����� �ø��� �������� ���ٰ���</param>
    public void AddModifierByPercent(int percentValue)
    {
        if (percentValue != 0)
            percentModifiers.Add(percentValue);
    }

    /// <summary>
    /// �����ڸ� ���̾ƴ϶� ������ �߰����ذ� ����
    /// </summary>
    /// <param name="percentValue"></param>
    public void RemoveModifierByPercent(int percentValue)
    {
        if (percentValue != 0)
            percentModifiers.Remove(percentValue);
    }

    /// <summary>
    /// �� ��� �����ڵ��� ����
    /// </summary>
    /// <param name="value"></param>
    public void SetDefaultValue(int value)
    {
        baseValue = value;
    }
}
