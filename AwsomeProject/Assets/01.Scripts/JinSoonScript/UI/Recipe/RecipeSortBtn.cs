using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecipeSortBtn : MonoBehaviour, IPointerClickHandler
{
    private RecipeSortChanger sortChanger;
    private RectTransform rect;
    private TextMeshProUGUI txt;

    public bool isSelected = false;
    public bool isAscending = true;
    public SortMode sortMode;


    private void Awake()
    {
        sortChanger = GetComponentInParent<RecipeSortChanger>();
        rect = GetComponent<RectTransform>();
        txt = transform.Find("Text").GetComponent<TextMeshProUGUI>();

        if (isAscending)
            txt.SetText($"{sortMode} : asc");
        else
            txt.SetText($"{sortMode} : dsc");

        if(isSelected)
            rect.localScale = new Vector3(1f, 1.2f, 1f);
    }

    public void OnClick()
    {
        if (isSelected == true) isAscending = !isAscending;
        rect.localScale = new Vector3(1f, 1.2f, 1f);
        isSelected = true;

        sortChanger.ChangeSortMode(sortMode, isAscending);
        if (isAscending)
            txt.SetText($"{sortMode} : asc");
        else
            txt.SetText($"{sortMode} : dsc");
    }

    public void OnClickOtherSortModeBtn()
    {
        isSelected = false;
        isAscending = true;
        rect.localScale = Vector3.one; 
        txt.SetText($"{sortMode} : asc");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Å¬¸¯~");
        OnClick();
    }
}
