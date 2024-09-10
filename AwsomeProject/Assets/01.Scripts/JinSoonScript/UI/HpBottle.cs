using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBottle : MonoBehaviour
{
    private Image fill;
    private RectTransform imageRect;
    private Sequence seq;

    private bool isBottleFull = true;
    private bool isBottleEmpty = false;

    public bool IsBottleEmpty => isBottleEmpty;

    private void Awake()
    {
        imageRect = transform.Find("HealthBottle").GetComponent<RectTransform>();
        fill = transform.Find("HealthBottle/Heart").GetComponent<Image>();

        isBottleFull = true;
        isBottleEmpty = false;
    }

    public void HpDown()
    {
        if (isBottleEmpty == true) return;

        if (isBottleFull)
        {
            if(seq != null && seq.active)
                seq.Complete();

            seq = DOTween.Sequence();

            seq.Append(DOTween.To(() => 1, x => fill.fillAmount = x, 0.5f, 0.1f))
                .Join(imageRect.DOAnchorPosY(15f, 0.05f))
                .Insert(0.05f, imageRect.DOAnchorPosY(0f, 0.05f));
            isBottleFull = false;
        }
        else
        {
            if (seq != null && seq.active)
                seq.Kill();

            seq = DOTween.Sequence();

            seq.Append(DOTween.To(() => 0.5f, x => fill.fillAmount = x, 0f, 0.1f))
                .Join(imageRect.DOAnchorPosY(15f, 0.05f))
                .Insert(0.05f, imageRect.DOAnchorPosY(0f, 0.05f));
            isBottleEmpty = true;
        }
    }
    public void HpUp()
    {
        if (isBottleFull == true) return;

        if (isBottleEmpty)
        {
            if(seq != null && seq.active)
                seq.Complete();

            seq = DOTween.Sequence();

            seq.Append(DOTween.To(() => 0, x => fill.fillAmount = x, 0.5f, 0.1f))
                .Join(imageRect.DOAnchorPosY(15f, 0.05f))
                .Insert(0.05f, imageRect.DOAnchorPosY(0f, 0.05f));
            isBottleEmpty = false;
        }
        else
        {
            if (seq != null && seq.active)
                seq.Kill();

            seq = DOTween.Sequence();

            seq.Append(DOTween.To(() => 0.5f, x => fill.fillAmount = x, 1f, 0.1f))
                .Join(imageRect.DOAnchorPosY(15f, 0.05f))
                .Insert(0.05f, imageRect.DOAnchorPosY(0f, 0.05f));
            isBottleFull = true;
        }
    }

    public void SetAsHalfHp()
    {
        fill.fillAmount = 0.5f;
    }
}
