using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class ShopPanel : MonoBehaviour, IWindowPanel
{
    [SerializeField] private float _onYPos;
    [SerializeField] private float _offYPos;
    [SerializeField] private RectTransform _rectTrm;

    [SerializeField] private ShopItemUI[] _shopItem;
    [SerializeField] private ShopItemTableSO _shopItemTables;

    private bool _isPlay;

    public void Open()
    {
        if (_isPlay) return;
        Time.timeScale = 0;
        SettingRandomItem();
        _isPlay = true;

        _rectTrm.DOAnchorPosY(_onYPos, 0.3f)
            .SetEase(Ease.Flash)
            .SetUpdate(true)
            .OnComplete(() => _isPlay = false);
    }
    
    private void SettingRandomItem()
    {
        ShopItemSO[] soArr = _shopItemTables.list
                           .Where(x => x.CheckSelect()).ToArray();

        if (soArr.Length < 9)
        {
            //Ȥ�ó� ��û�� �̽������� ��ǻ� �ʿ���� �ڵ�
            Debug.LogError("Error! : Must hav 9 item at least");
            return;
        }

        //ShopItemTableSO �ȿ� �ִ� SO���� �ߺ��� ���� �ʰ� �������� ������ 9���� ���÷� �̴´�.
        for (int i = 0; i < 9; i++)
        {
            int index = Random.Range(0, soArr.Length - i);

            _shopItem[i].SetItemData(soArr[index]);
            soArr[index] = soArr[soArr.Length - 1 - i];
        }
    }

    public void Close()
    {
        if (_isPlay) return;
        Time.timeScale = 1;
        _isPlay = true;

        _rectTrm.DOAnchorPosY(_offYPos, 0.3f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true)
            .OnComplete(() => _isPlay = false);
    }
}
