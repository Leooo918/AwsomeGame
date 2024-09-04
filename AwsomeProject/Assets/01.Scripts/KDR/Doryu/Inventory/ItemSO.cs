using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doryu.Inventory
{
    public enum ItemType
    { 
        Resource,
        Potion,
    }
    public enum ItemName
    {
        RedMushroom,
        Y_BounceMushroom,
        TrunkFruit,
        ArmoryMushroom,
        HornPigHorn,
        SpikeMushroom,
        StickyGrass,
        ShinyFruit,
        SlimeCore,
        YoungBirldWing
    }

    [CreateAssetMenu(menuName = "SO/Doryu/Item/Item")]
    public class ItemSO : ScriptableObject
    {
        [Header("ItemData")]
        public ItemType type;
        public int maxMergeAmount = 5;
        public ItemName itemName;
        [TextArea(3, 20)]
        public string itemExplain;

        [Space(20)]
        [Header("Sprite")]
        public Sprite dotImage;
        public Sprite image;

        [Space(20)]
        [Header("Prefab")]
        public Item prefab;

        [Space(20)]
        [Header("Gathering")]
        public float time;
    }
}
