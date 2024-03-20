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
    [SerializeField] private bool isState = false;

    Vector3 asd, ming;

    private void Awake()
    {
        ui[0].SetActive(false);
        ui[1].SetActive(false);
        asd = ui[0].transform.position;
        ming = ui[1].transform.localScale;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isESC = !isESC;
            if (isESC)
            {
                ShowUI(isESC, ui, 0);
                seq.Append(rectTransform.DOMove(new Vector2(2000, 450), 1).SetEase(Ease.OutBack));
            }
            else
            {
                ui[0].transform.position = asd;
                ui[0].SetActive(false);
            }

            
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isInven = !isInven;
            if(isInven)
            {
                ShowUI(isInven, ui, 1);
                seq.Append(rectTransform.DOSizeDelta(new Vector2(1800, 950), 1).SetEase(Ease.OutBack));
            }
            else
            {
                ui[1].transform.localPosition = ming;
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
}
