using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance;

    public RecipeSetSO recipeSet;
    public List<RecipeSO> curRecipe;// { get; private set; }
    public List<int> recipeEverUsed;

    [SerializeField] private RectTransform sideBar;
    [SerializeField] private RectTransform selectedRecipe;

    private Sequence seq;
    private bool isRecipeBarOpen = false;

    private string path = "";

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);

        Instance = this;

        path = Path.Combine(Application.dataPath, "SaveDatas\\OwnedRecipes.json");
        seq = DOTween.Sequence();
    }

    private void Update()
    {
        //디버그용 코드임
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (isRecipeBarOpen == true)
                CloseRecipeBar();
            else
                OpenRecipeBar();
        }
    }

    /// <summary>
    /// 재료들을 받아서 만들 포션을 주고 포션을 만들수 있는지 확인해줘
    /// </summary>
    /// <param name="ingredients"></param>
    /// <param name="posionToMake"></param>
    /// <returns></returns>
    public bool TryMakePosion(IngredientItemSO[] ingredients, out PortionItemSO posionToMake)
    {
        for (int i = 0; i < curRecipe.Count; ++i)
        {
            //현재 이 물약의 레시피를 가지고 있는지 확인해줘
            bool isThisRecipe = true;
            for (int j = 0; j < curRecipe[i].ingredients.Length; ++j)
            {
                if (ingredients.Length <= j || ingredients[j] != curRecipe[i].ingredients[j])
                {
                    isThisRecipe = false;
                    break;
                }
            }

            if (isThisRecipe == false) break;

            //아이템 id랑 개수를 리스트에 채워줄거임
            List<Tuple<int, int>> items = new List<Tuple<int, int>>();

            for (int k = 0; k < ingredients.Length; ++k)
            {
                bool itemExist = false;
                for (int j = 0; j < items.Count; ++j)
                {
                    //현재 리스트에 이미 아이템이 있다면
                    if (items[j].Item1 == ingredients[k].id)
                    {
                        //앞에 리스트에 개수에 추가해줄거임
                        items[j] = new Tuple<int, int>(items[j].Item1, (items[j].Item2 + 1));
                        itemExist = true;
                        Debug.Log($"id = {items[j].Item1}, Amount = {items[j].Item2}");
                        continue;
                    }
                }

                if (itemExist)
                    continue;

                //재료를 items리스트에 넣어줘 id랑 개수 순으로
                Debug.Log($"id = {ingredients[k].id}, Amount = {1}");
                items.Add(new Tuple<int, int>(ingredients[k].id, 1));
            }

            Inventory myInventory = InventoryManager.Instance.PlayerInventory;
            List<Item> ingredientItems = new List<Item>();
            bool isPossesIngredients = true;

            //현재 인벤토리에 재료들이 전부 있는지 확인
            for (int j = 0; j < items.Count; ++j)
            {
                isPossesIngredients = myInventory.GetItem(items[j].Item1, items[j].Item2, out Item ingredient);

                if (isPossesIngredients == false)
                {
                    posionToMake = curRecipe[i].portion;
                    return false;
                }

                //재료들은 전부 요 리스트에 넣어주고
                ingredientItems.Add(ingredient);
            }

            //재료가 될 아이템들을 리스트를 돌면서 제거해줘
            for (int j = 0; j < ingredientItems.Count; ++j)
            {
                ingredientItems[j].RemoveItem(items[j].Item2);
                Debug.Log($"제거 : {ingredientItems[j].itemAmount} {items[j].Item2}");
            }


            posionToMake = curRecipe[i].portion;
            return true;
        }

        //현재 가지고 있는 레시피가 아니라면 null 주고 return false해줘
        posionToMake = null;
        return false;
    }

    /// <summary>
    /// id로 레시피 추가 해줘
    /// </summary>
    /// <param name="id">레시피의 id</param>
    public void AddRecipe(int id)
    {
        for (int j = 0; j < recipeSet.recipes.Count; j++)
        {
            if (recipeSet.recipes[j].id == id && curRecipe.Contains(recipeSet.recipes[j]) == false)
                curRecipe.Add(recipeSet.recipes[j]);
        }
    }


    /// <summary>
    /// RecipeSO로 레시피를 추가해줘
    /// </summary>
    /// <param name="recipe"></param>
    public void AddRecipe(RecipeSO recipe)
    {
        if (curRecipe.Contains(recipe) == false)
            curRecipe.Add(recipe);

        Save();
    }


    /// <summary>
    /// 초기화 해줄때 현재 가지고 있는 Recipe를 초기화해줘
    /// </summary>
    public void ResetRecipeData()
    {
        curRecipe.Clear();
        recipeEverUsed.Clear();
        Save();
    }

    public void OpenRecipeBar()
    {
        if (seq.IsActive() == true)
            seq.Kill();

        QuickSlotManager.Instance.EnableQuickSlot();
        seq = DOTween.Sequence();

        isRecipeBarOpen = true;

        seq.Append(sideBar.DOAnchorPosX(-50f, 0.5f))
            .Join(selectedRecipe.DOAnchorPosY(425f, 0.5f));
    }

    public void CloseRecipeBar()
    {
        if (seq.IsActive() == true)
            seq.Kill();

        QuickSlotManager.Instance.DisableQuickSlot();
        seq = DOTween.Sequence();

        isRecipeBarOpen = false;

        seq.Append(sideBar.DOAnchorPosX(900f, 0.5f))
        .Join(selectedRecipe.DOAnchorPosY(660f, 0.5f));
    }

    /// <summary>
    /// 레시피를 사용할 때 이 레시피를 전에 쓴 적이 있는지 확인해주는 뇨속
    /// </summary>
    /// <param name="recipe"></param>
    /// <returns></returns>
    public bool IsEverUseRecipe(RecipeSO recipe)
    {
        if(recipeEverUsed.Contains(recipe.id) == true) return false;

        recipeEverUsed.Add(recipe.id);
        Save();
        return true;
    }

    public void Save()
    {
        RecipeSave saves = new RecipeSave();
        saves.ids = new List<int>();

        for (int i = 0; i < curRecipe.Count; i++)
        {
            saves.ids.Add(curRecipe[i].id);
        }

        saves.recipeOnceUsed = recipeEverUsed;

        string json = JsonUtility.ToJson(saves, true);
        File.WriteAllText(path, json);
    }

    public void Load()
    {
        string json = File.ReadAllText(path);
        RecipeSave saves = JsonUtility.FromJson<RecipeSave>(json);

        for (int i = 0; i < saves.ids.Count; i++)
        {
            for (int j = 0; j < recipeSet.recipes.Count; j++)
            {
                if (recipeSet.recipes[j].id == saves.ids[i] && curRecipe.Contains(recipeSet.recipes[j]) == false)
                    curRecipe.Add(recipeSet.recipes[j]);
            }
        }

        recipeEverUsed = saves.recipeOnceUsed;
    }
}

public class RecipeSave
{
    public List<int> ids;

    public List<int> recipeOnceUsed;
}