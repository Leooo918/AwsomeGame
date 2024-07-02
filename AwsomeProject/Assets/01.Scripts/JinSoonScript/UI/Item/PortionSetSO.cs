using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/PortionSet")]
public class PortionSetSO : ScriptableObject
{
    [Header("Sequence is important!\n Portion that get same effects and diffrent point.\n You have to add portion that have bigger value first")]
    public List<PortionItemSO> portions = new List<PortionItemSO>();


    /// <summary>
    /// GC가 조금 많이 아파할거 같은 코드...
    /// </summary>
    /// <param name="effects">저기에 사용될 효과</param>
    /// <param name="portion">반환해줄 포션</param>
    /// <returns>만들수 있는 포션이 있나?</returns>
    public bool FindMakeablePortion(EffectInfo[] effects, Portion portionType, out PortionItemSO portion)
    {
        List<EffectInfo> infos = new List<EffectInfo>();

        //사용된 재료들의 효과들 중 겹치는 것들을 정리해서 infos에 넣어주
        for (int i = 0; i < effects.Length; i++)
        {
            bool isOverlap = false;
            for (int j = 0; j < infos.Count; j++)
            {
                if (effects[i].effect == infos[j].effect)
                {
                    isOverlap = true;
                    EffectInfo temp = infos[j];
                    temp.requirePoint += effects[i].requirePoint;
                    infos[j] = temp;
                    break;
                }
            }

            if (!isOverlap)
            {
                infos.Add(effects[i]);
            }
        }
        
        for (int i = 0; i < portions.Count; i++)
        {
            if (portions[i].CheckCanMakePortion(infos) && portions[i].portionType == portionType)
            {
                Debug.Log(portions[i].itemName);
                portion = CreateInstance("PortionItemSO") as PortionItemSO;
                portion.Init(portions[i]);
                return true;
            }
        }

        portion = null;
        return false;
    }
}
