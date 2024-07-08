using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookMarkContainer : MonoBehaviour
{
    public BookMark drinkingPortionBookMark;
    public BookMark throwingPortionBookMark;

    public BookMark enableBookMark;
    public BookMark disableBookMark;

    public Inventory portionInventory;

    [SerializeField] private GameObject portionInventoryObj;
    [SerializeField] private GameObject recipeObj;

    private void OnEnable()
    {
        drinkingPortionBookMark.clickEvent += OnClickBookMark;
        throwingPortionBookMark.clickEvent += OnClickBookMark;

        enableBookMark.clickEvent += OnClickLeftBookMark;
        disableBookMark.clickEvent += OnClickLeftBookMark;
    }

    private void OnDisable()
    {
        drinkingPortionBookMark.clickEvent -= OnClickBookMark;
        throwingPortionBookMark.clickEvent -= OnClickBookMark;

        enableBookMark.clickEvent -= OnClickLeftBookMark;
        disableBookMark.clickEvent -= OnClickLeftBookMark;
    }
    private void OnClickBookMark(bool isEnableDrinkingPortion)
    {
        if(isEnableDrinkingPortion)
        {
            portionInventory.IndicateDrinkingPortion = true;
            portionInventory.IndicateThrowingPortion = false;
        }
        else
        {
            portionInventory.IndicateDrinkingPortion = false;
            portionInventory.IndicateThrowingPortion = true;
        }
    }

    private void OnClickLeftBookMark(bool isEnableInventory)
    {
        InventoryManager.Instance.EnbableIgredientsInventory(isEnableInventory);

        if (isEnableInventory)
        {
            recipeObj.SetActive(true);
            portionInventoryObj.SetActive(false);
        }
        else
        {
            recipeObj.SetActive(false);
            portionInventoryObj.SetActive(true);
        }
    }
}
