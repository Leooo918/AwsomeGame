using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemGatherPanal : MonoBehaviour, IManageableUI
{
    private ItemSO _item;

    [SerializeField] private CanvasGroup _bgGroup;
    private RectTransform _bgRect;

    [SerializeField] private TextMeshProUGUI _nameTxt;
    [SerializeField] private TextMeshProUGUI _explainTxt;
    [SerializeField] private Image _itemImage;

    private Sequence _seq;

    private void Awake()
    {
        _bgRect = _bgGroup.GetComponent<RectTransform>();
    }

    public void Close()
    {
        if (_seq != null && _seq.active)
            _seq.Kill();

        _seq = DOTween.Sequence();

        _seq.Join(_bgGroup.DOFade(0f, 0.5f))
            .Join(_bgRect.DOAnchorPosY(-100f, 0.5f));
    }

    public void Open()
    {
        if (_seq != null && _seq.active)
            _seq.Kill();

        _seq = DOTween.Sequence();

        _seq.Join(_bgGroup.DOFade(1f, 0.5f))
            .Join(_bgRect.DOAnchorPosY(0f, 0.5f));
    }

    public void Init(ItemSO item)
    {
        _item = item;
        _nameTxt.SetText(_item.itemName);
        _explainTxt.SetText(_item.itemExplain);
        _itemImage.sprite = _item.itemImage;
    }
}
