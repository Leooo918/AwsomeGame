using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionInventory : MonoBehaviour
{
    public BookMark enableBookMark;
    public BookMark disableBookMark;

    public Inventory portionInventory;

    private void OnEnable()
    {
        enableBookMark.clickEvent += OnClickBookMark;
        disableBookMark.clickEvent += OnClickBookMark;
    }

    private void OnDisable()
    {
        enableBookMark.clickEvent -= OnClickBookMark;
        disableBookMark.clickEvent -= OnClickBookMark;
    }

    private void OnClickBookMark(bool isEnableInventory)
    {

    }
}
