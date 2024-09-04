using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Doryu.Inventory
{
    [Serializable]
    public class Item : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        private TextMeshProUGUI _textOutLine;
        private Image _image;
        private int _amount = 1;
        public int amount
        {
            get => _amount;
            set
            {
                _amount = value;
                _text.SetText("x" + _amount.ToString());
                _textOutLine.SetText("x" + _amount.ToString());
            }
        }
         
        public ItemSO _itemSO;
        public ItemSO itemSO
        {
            get => _itemSO;
            set
            {
                _itemSO = value;
                _image.sprite = _itemSO.dotImage;
            }
        }
        public bool isFull => _amount == itemSO.maxMergeAmount;

        public void Init()
        {
            _text = transform.Find("AmountText").GetComponent<TextMeshProUGUI>();
            _textOutLine = transform.Find("AmountTextOutLine").GetComponent<TextMeshProUGUI>();
            _image = GetComponent<Image>();
        }

        public void SetSlot(Transform trm)
        {
            transform.SetParent(trm);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one * 1.67f;
        }
    }
}
