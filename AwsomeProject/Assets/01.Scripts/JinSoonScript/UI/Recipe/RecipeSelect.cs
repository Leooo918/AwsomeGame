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
            Debug.Log("레시피 없음 밍");
            //레시피 없다고 UI뛰어주면 됨
            return;
        }

        if (canMakePortion == false && portion != null)
        {
            Debug.Log("재료 부족 밍");
            //재료 없다고 UI뛰어주면 됨
            return;
        }

        Item itemInstance = InventoryManager.Instance.MakeItemInstanceByItemSO(portion);
        bool inventoryNotFull = InventoryManager.Instance.PlayerInventory.TryInsertItem(itemInstance);

        if (inventoryNotFull == false)
        {
            //인벤 가득 찾다고 뛰어주면됨
            Destroy(itemInstance.gameObject);
            return;
        }

        if(RecipeManager.Instance.IsEverUseRecipe(curRecipe) == false)
        {
            Debug.Log("처음이에요");
        }
    }
}
