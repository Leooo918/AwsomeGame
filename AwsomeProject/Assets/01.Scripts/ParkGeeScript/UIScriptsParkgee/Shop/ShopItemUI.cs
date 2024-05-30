using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private ShopItemSO _shopItemSO;
    [SerializeField] private TextMeshProUGUI _nameText, _descriptionText;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Button _selectBtn;

    private void OnValidate()
    {
        if(_shopItemSO != null)
        {
            SelectItemVisual();
        }
    }

    private void Awake()
    {
        _selectBtn.onClick.AddListener(SelectItem);
    }

    public void SetItemData(ShopItemSO data)
    {
        _shopItemSO = data;
        SelectItemVisual();
    }

    private void SelectItemVisual()
    {
        if(_nameText != null)
            _nameText.text = _shopItemSO.itemName;
        if (_descriptionText != null)
            _descriptionText.text = _shopItemSO.itemMenual;
        if (_iconImage != null)
            _iconImage.sprite = _shopItemSO.itemImg;
        
    }

    private void SelectItem()
    {
        UIManager.Instance.Close(Window.Shop);
    }
}
