using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotInserterSet : MonoBehaviour
{
    public readonly QuickSlotInserter[] inserter = new QuickSlotInserter[5];
    private RectTransform inserterSetRect;
    private Image[] inserterImages;

    public bool isEnable = false;
    public int slotNum = 0;

    private Sequence seq;

    private void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            inserter[i] = transform.GetChild(i).GetComponent<QuickSlotInserter>();
            inserter[i].Init(i);
        }

        inserterSetRect = GetComponent<RectTransform>();
        inserterImages = GetComponentsInChildren<Image>();
    }

    public void GoDisable(Vector2 peek, Vector2 origin, Color disableColor)
    {
        if (seq != null && seq.IsActive())
            seq.Kill();

        seq = DOTween.Sequence();

        seq.Append(inserterSetRect.DOAnchorPos(peek, 0.3f))
            .Join(inserterSetRect.DOScale(1.1f, 0.3f))
            .AppendCallback(() => transform.SetAsFirstSibling())
            .Append(inserterSetRect.DOAnchorPos(origin, 0.2f))
            .Join(inserterSetRect.DOScale(0.9f, 0.2f));

        foreach (var item in inserterImages)
            seq.Join(item.DOColor(disableColor, 0.5f));
    }

    public void GoEnable(Vector2 origin, Color enableColor)
    {
        if (seq != null && seq.IsActive())
            seq.Kill();

        seq = DOTween.Sequence();

        seq.Append(inserterSetRect.DOAnchorPos(origin, 0.5f))
           .Join(inserterSetRect.DOScale(1f, 0.5f));

        foreach (var item in inserterImages)
            seq.Join(item.DOColor(enableColor, 0.5f));
    }

    public void Init(QuickSlotItems items)
    {
        for (int i = 0; i < items.items.Length; i++)
        {
            if (items.items[i] == null) continue;
            Item item = InventoryManager.Instance.MakeItemInstanceByItemSO(items.items[i]);
            inserter[i].InsertItem(item);
        }
    }
}
