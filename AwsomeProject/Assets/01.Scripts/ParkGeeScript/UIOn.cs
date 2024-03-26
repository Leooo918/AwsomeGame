using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIOn : MonoBehaviour
{
    [SerializeField] private GameObject[] ui;

    private RectTransform[] initialPos;
    private RectTransform rectTransform;

    private Sequence seq;

    [SerializeField] private bool isESC = false;
    [SerializeField] private bool isInven = false;
    [SerializeField] private bool isSet = false;
    //[SerializeField] private bool isOnOff = false;

    Vector3 escPanelPos, invenScale;

    private void Awake()
    {
        ui[0].SetActive(false);
        ui[1].SetActive(false);
        escPanelPos = ui[0].transform.position;
        invenScale = ui[1].transform.localScale;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isESC = !isESC;
            if (isESC)
            {
                if (seq != null && seq.IsActive()) seq.Kill();
                seq = DOTween.Sequence();
                ShowUI(isESC, ui, 0);
                rectTransform.position = escPanelPos;
                seq.Append(rectTransform.DOMove(new Vector2(2000, 450), 1).SetEase(Ease.Linear));
            }
            else
            {
                ui[0].transform.position = escPanelPos;
                ui[0].SetActive(false);
            }

            if (isInven)
            {
                ui[0].SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isInven = !isInven;
            if(isInven)
            {
                ShowUI(isInven, ui, 1);
                if (seq != null && seq.IsActive()) seq.Kill();
                seq = DOTween.Sequence();
                rectTransform.sizeDelta = new Vector2(100, 50);
                seq.Append(rectTransform.DOSizeDelta(new Vector2(1800, 950), 1).SetEase(Ease.Linear));
            }
            else
            {
                ui[1].transform.localPosition = invenScale;
                ui[1].SetActive(false);
            }

            if (isESC)
            {
                ui[1].SetActive(false);
            }
        }
    }

    private void ShowUI(bool isState, GameObject[] ui, int arr)
    {
        isState = true;
        ui[arr].SetActive(true);
        rectTransform = ui[arr].GetComponent<RectTransform>();
    }

    public void SetPanelOn()
    {
        ShowUI(isSet, ui, 2);
        if (seq != null && seq.IsActive()) seq.Kill();
        seq = DOTween.Sequence();
        rectTransform.position = escPanelPos;
        seq.Append(rectTransform.DOMove(new Vector2(2000, 450), 1).SetEase(Ease.Linear));
    }

    public void Continue()
    {
        for (int i = 0; i < ui.Length; i++)
        {
            ui[i].SetActive(false);
        }
    }
}
