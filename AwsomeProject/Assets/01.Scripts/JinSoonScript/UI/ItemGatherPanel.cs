using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemGatherPanel : MonoBehaviour, IManageableUI
{
    private ItemSO _item;

    [SerializeField] private CanvasGroup _bgGroup;
    private RectTransform _bgRect;

    [SerializeField] private TextMeshProUGUI _nameTxt;
    [SerializeField] private TextMeshProUGUI _explainTxt;
    [SerializeField] private Image _itemImage;

    private Sequence _seq;
    private bool _isOpen = false;

    private void Update()
    {
        if (_isOpen && Keyboard.current.anyKey.wasPressedThisFrame)
            Close();
    }

    public void Close()
    {
        _isOpen = false;
        if (_seq != null && _seq.active)
            _seq.Kill();

        _seq = DOTween.Sequence();

        _seq.Join(_bgGroup.DOFade(0f, 0.5f))
            .Join(_bgRect.DOAnchorPosY(-100f, 0.5f));

        PlayerManager.Instance.EnablePlayerMovementInput();
        PlayerManager.Instance.EnablePlayerInventoryInput();
        _bgGroup.blocksRaycasts = false;
    }

    public void Open()
    {
        if (_seq != null && _seq.active)
            _seq.Kill();

        _seq = DOTween.Sequence();

        _seq.Join(_bgGroup.DOFade(1f, 0.5f))
            .Join(_bgRect.DOAnchorPosY(0f, 0.5f))
            .OnComplete(() => _isOpen = true);
        PlayerManager.Instance.DisablePlayerMovementInput();
        PlayerManager.Instance.DisablePlayerInventoryInput();
        _bgGroup.blocksRaycasts = true;
    }

    public void Init(ItemSO item)
    {
        _item = item;
        _nameTxt.SetText(_item.itemName);
        _explainTxt.SetText(_item.itemExplain);
        _itemImage.sprite = _item.itemImage;
    }

    public void Init()
    {
        _bgRect = _bgGroup.GetComponent<RectTransform>();

        _bgGroup.alpha = 0f;
        _bgRect.anchoredPosition = new Vector2(_bgRect.anchoredPosition.x, -100);
    }
}
