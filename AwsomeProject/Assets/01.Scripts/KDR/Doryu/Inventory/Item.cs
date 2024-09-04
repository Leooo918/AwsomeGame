using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;

namespace Doryu.Inventory
{
    [Serializable]
    public class Item : MonoBehaviour
    {
        public ItemSO itemSO;
        public int amount = 1;

        public void SetSlot(Transform trm)
        {
            transform.SetParent(trm);
            transform.localPosition = Vector3.zero;
        }
    }
}
