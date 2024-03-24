using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeSelect : MonoBehaviour
{
    [SerializeField] private RecipeParent recipeParent;

    public Transform curSelectedRecipe { get; private set; }
    public RecipeSO curRecipe { get; private set; }

    public void SelectRecipe(RecipeUI recipe)
    {
        if (recipe.recipe == curRecipe)
        {
            curSelectedRecipe = null;
            curRecipe = null;
            recipe.transform.SetParent(recipeParent.transform); 

            recipeParent.Resize();
            return;
        }

        if (curSelectedRecipe != null) 
        {
            curSelectedRecipe.transform.SetParent(recipeParent.transform);
            curSelectedRecipe = null;
        }

        curSelectedRecipe = recipe.transform;
        curRecipe = recipe.recipe;
        curSelectedRecipe.SetParent(transform);

        recipeParent.Resize();
    }


    public void MakePortion()
    {
        bool canMakePortion = RecipeManager.Instance.TryMakePosion(curRecipe.ingredients, out PortionItemSO portion);

        if (canMakePortion == false && portion == null)
        {
            Debug.Log("������ ���� ��");
            //������ ���ٰ� UI�پ��ָ� ��
            return;
        }

        if (canMakePortion == false && portion != null)
        {
            Debug.Log("��� ���� ��");
            //��� ���ٰ� UI�پ��ָ� ��
            return;
        }

        Item itemInstance = InventoryManager.Instance.MakeItemInstanceByItemSO(portion);
        bool inventoryNotFull = InventoryManager.Instance.PlayerInventory.TryInsertItem(itemInstance);

        if (inventoryNotFull == false)
        {
            //�κ� ���� ã�ٰ� �پ��ָ��
            Destroy(itemInstance.gameObject);
            return;
        }

        if(RecipeManager.Instance.IsEverUseRecipe(curRecipe) == false)
        {
            Debug.Log("ó���̿���");
        }
    }
}
