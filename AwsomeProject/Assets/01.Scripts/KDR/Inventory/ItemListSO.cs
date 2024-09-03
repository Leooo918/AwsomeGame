using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doryu.Inventory
{
    [CreateAssetMenu(menuName = "SO/Doryu/Item/ItemListSO")]
    public class ItemListSO : ScriptableObject
    {
        public ItemSO[] itemSOList;
    }
}
