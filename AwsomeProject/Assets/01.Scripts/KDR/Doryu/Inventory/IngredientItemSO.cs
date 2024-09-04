using Doryu.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doryu.Inventory
{
    [CreateAssetMenu(menuName = "SO/Doryu/Item/IngredientItem")]
    public class IngredientItemSO : ItemSO
    {
        [Space(20)]
        [Header("Gathering")]
        public float gatheringTime;
    }
}
