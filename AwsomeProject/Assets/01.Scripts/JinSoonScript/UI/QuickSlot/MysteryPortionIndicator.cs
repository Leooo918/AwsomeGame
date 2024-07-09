using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MysteryPortionIndicator : MonoBehaviour
{
    [SerializeField] private MysteryPortionInventory _inventory;
    [SerializeField] private Image _portionImage;

    private PortionItem _portion;
    private PortionItemSO _mysteryPortion;

    public void ChangePortionImage(Item item)
    {
        _portion = item as PortionItem;

        if(_portion != null )
        {
            _mysteryPortion = _portion.itemSO as PortionItemSO;
        }

        _portionImage.sprite = item.itemSO.dotImage;
    }
}
