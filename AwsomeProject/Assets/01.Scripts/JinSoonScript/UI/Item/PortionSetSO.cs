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
    /// GC�� ���� ���� �����Ұ� ���� �ڵ�...
    /// </summary>
    /// <param name="effects">���⿡ ���� ȿ��</param>
    /// <param name="portion">��ȯ���� ����</param>
    /// <returns>����� �ִ� ������ �ֳ�?</returns>
    public bool FindMakeablePortion(List<EffectInfo> effects, out PortionItemSO portion)
    {
        List<EffectInfo> infos = effects;

        //���� ������ ȿ���� �� ��ġ�� �͵��� �����ؼ� infos�� �־���
        //for (int i = 0; i < effects.Count; i++)
        //{
        //    bool isOverlap = false;
        //    for (int j = 0; j < infos.Count; j++)
        //    {
        //        if (effects[i].effect == infos[j].effect)
        //        {
        //            isOverlap = true;
        //            EffectInfo temp = infos[j];
        //            temp.requirePoint += effects[i].requirePoint;
        //            infos[j] = temp;
        //            break;
        //        }
        //    }

        //    if (!isOverlap)
        //    {
        //        infos.Add(effects[i]);
        //    }
        
        for (int i = 0; i < portions.Count; i++)
        {
            if (portions[i].CheckCanMakePortion(infos))
            {
                Debug.Log(portions[i].itemType);
                portion = ScriptableObject.Instantiate(portions[i]);// CreateInstance("PortionItemSO") as PortionItemSO;
                //portion.Init(portions[i]);
                return true;
            }
        }

        portion = null;
        return false;
    }
}
