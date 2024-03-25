using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "SO/Item/ItemSet")]
public class ItemSetSO : ScriptableObject
{
    public List<ItemSO> itemset;

    public void AddItem(ItemSO item)
    {
        if (itemset.Contains(item) == false)
            itemset.Add(item);

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif  

    }
}
