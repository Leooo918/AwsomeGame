using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal.Profiling.Memory.Experimental;
using System.Linq;



#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "SO/Item/ItemSet")]
public class ItemSetSO : ScriptableObject
{
    public List<ItemSO> itemset;

    public ItemSO GetItem(int id)
    {
        List<ItemSO> itemList = itemset.Where(item => item.id == id).ToList();

        if(itemList.Count == 0)
            return null;

        return itemList[0];
    }


    public void AddItem(ItemSO item)
    {
        if (itemset.Contains(item) == false)
            itemset.Add(item);

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    public ItemSO FindItem(int id)
    {
        for (int i = 0; i < itemset.Count; i++)
        {
            if (itemset[i].id == id)
            {
                return itemset[i];
            }
        }
        return null;
    }
}
