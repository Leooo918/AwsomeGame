using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private TextMeshProUGUI _textOutLine;
    private Image _image;
    public int amount;
     
    private ItemSO _itemSO;
    public ItemSO itemSO
    {
        get => _itemSO;
        set
        {
            _itemSO = value;
            _image.sprite = _itemSO.dotImage;
        }
    }
    public bool isFull => amount == itemSO.maxMergeAmount;

    public void TextUpdate()
    {
        _text.SetText("x" + amount.ToString());
        _textOutLine.SetText("x" + amount.ToString());
    }

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
    public void SetSlot()
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one * 1.67f;
    }
}

