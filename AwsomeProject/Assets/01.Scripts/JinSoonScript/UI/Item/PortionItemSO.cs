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

    //이 포션이 가질 진짜 효과
    public EffectEnum effect;
    //요걸 리스트로 한 이유는 C포션을 만들려면 무조건 A효과와 B효과의 조합만이 아닐 수도 있다고 생각해서
    public List<EffectInfo> reqireEffects = new List<EffectInfo>();

    //얘네는 Portion으로 만들때 지정해줄건데 아마 여기서 빠질 듯
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

        //for문을 돌면서 temp와 effects에 겹치는 부분에 temp에 effects의 point만큼 빼주고 effects를 없애시오
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

        //이제 effects에서 겹치는 만큼 뺐으니 0이상인게 있으면 이 포션은 만들지 못합니다!
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
