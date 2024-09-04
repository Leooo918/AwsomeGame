using System;
using UnityEngine;
using TMPro;

namespace Doryu.Inventory
{
    [Serializable]
    public class Item : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        private int _amount = 1;
        public int amount
        {
            get => _amount;
            set
            {
                _amount = value;
                _text.SetText("x" + _amount.ToString());
            }
        }

        public ItemSO itemSO;
        public bool isFull => _amount == itemSO.maxMergeAmount;

        private void Awake()
        {
            _text = transform.Find("AmountText").GetComponent<TextMeshProUGUI>();
        }

        public void SetSlot(Transform trm)
        {
            transform.SetParent(trm);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
        }
    }
}
