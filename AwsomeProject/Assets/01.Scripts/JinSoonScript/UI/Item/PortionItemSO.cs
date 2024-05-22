using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public List<EffectInfo> requireEffects = new List<EffectInfo>();

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
        EffectInfo[] temp = requireEffects.ToArray();

        //for���� ���鼭 temp�� effects�� ��ġ�� �κп� temp�� effects�� point��ŭ ���ְ� effects�� ���ֽÿ�
        for (int j = 0; j < effects.Count; j++)
        {
            for (int i = 0; i < temp.Length; i++)
            {
                Debug.Log(temp[i].effect + " " + effects[j].effect);
                if (temp[i].effect == effects[j].effect)
                {
                    temp[i].requirePoint -= effects[j].requirePoint;
                }

            }
        }

        //���� effects���� ��ġ�� ��ŭ ������ 0�̻��ΰ� ������ �� ������ ������ ���մϴ�!
        for (int i = 0; i < temp.Length; i++)
        {
            Debug.Log(temp[i].requirePoint);
            if (temp[i].requirePoint > 0) return false;
        }

        return true;
    }

    public void Init(PortionItemSO portion)
    {
        id = portion.id;
        itemName = portion.itemName;
        itemType = portion.itemType;
        maxCarryAmountPerSlot = portion.maxCarryAmountPerSlot;
        itemExplain = portion.itemExplain;

        dotImage = portion.dotImage;
        itemImage = portion.itemImage;
        prefab = portion.prefab;

        effect = portion.effect;
        requireEffects = portion.requireEffects;
        effecLv = portion.effecLv;
        duration = portion.duration;
        usingTime = portion.usingTime;
        isInfinite = portion.isInfinite;
    }
}


[Serializable]
public struct EffectInfo
{
    public EffectEnum effect;
    public int requirePoint;
}
