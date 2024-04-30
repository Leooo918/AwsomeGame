using System;
using System.Collections.Generic;
using UnityEngine;

public enum Portion
{
    PortionForThrow,
    PortionForMyself,
    Flask
}

[CreateAssetMenu(menuName = "SO/Item/Portion")]
public class PortionItemSO : ItemSO
{
    [Space(20)]
    public Portion portionType;

    //�� ������ ���� ��¥ ȿ��
    public EffectEnum effect;
    //��� ����Ʈ�� �� ������ C������ ������� ������ Aȿ���� Bȿ���� ���ո��� �ƴ� ���� �ִٰ� �����ؼ�
    public List<EffectInfo> reqireEffects = new List<EffectInfo>();

    //��״� Portion���� ���鶧 �������ٰǵ� �Ƹ� ���⼭ ���� ��
    public int effecLv;
    public float duration;
    public float usingTime = 0.5f;
    public bool isInfinite;


    private void OnEnable()
    {
        itemType = ItemType.Portion;
    }

    /// <summary>
    /// GetListAndCheckCanMakeThisPortionSO
    /// </summary>
    /// <param name="effects"></param>
    /// <returns></returns>
    public bool CheckCanMakePortion(List<EffectInfo> effects)
    {
        List<EffectInfo> temp = reqireEffects;

        //for���� ���鼭 temp�� effects�� ��ġ�� �κп� temp�� effects�� point��ŭ ���ְ� effects�� ���ֽÿ�
        for (int i = 0; i < temp.Count; i++)
        {
            for (int j = 0; j < effects.Count; j++)
            {
                if (temp[i].effect == effects[j].effect)
                {
                    EffectInfo info = temp[i];
                    info.requirePoint -= effects[j].requirePoint;
                    effects.RemoveAt(j);
                    temp[i] = info;
                }

            }
        }

        //���� effects���� ��ġ�� ��ŭ ������ 0�̻��ΰ� ������ �� ������ ������ ���մϴ�!
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i].requirePoint > 0) return false;
        }

        return true;
    }
}


[System.Serializable]
public struct EffectInfo
{
    public EffectEnum effect;
    public int requirePoint;
}
