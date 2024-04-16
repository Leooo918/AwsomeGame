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
        //����׿� �ڵ���
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (isRecipeBarOpen == true)
                CloseRecipeBar();
            else
                OpenRecipeBar();
        }
    }

    /// <summary>
    /// ������ �޾Ƽ� ���� ������ �ְ� ������ ����� �ִ��� Ȯ������
    /// </summary>
    /// <param name="ingredients"></param>
    /// <param name="posionToMake"></param>
    /// <returns></returns>
    public bool TryMakePosion(IngredientItemSO[] ingredients, out PortionItemSO posionToMake)
    {
        for (int i = 0; i < curRecipe.Count; ++i)
        {
            //���� �� ������ �����Ǹ� ������ �ִ��� Ȯ������
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

            //������ id�� ������ ����Ʈ�� ä���ٰ���
            List<Tuple<int, int>> items = new List<Tuple<int, int>>();

            for (int k = 0; k < ingredients.Length; ++k)
            {
                bool itemExist = false;
                for (int j = 0; j < items.Count; ++j)
                {
                    //���� ����Ʈ�� �̹� �������� �ִٸ�
                    if (items[j].Item1 == ingredients[k].id)
                    {
                        //�տ� ����Ʈ�� ������ �߰����ٰ���
                        items[j] = new Tuple<int, int>(items[j].Item1, (items[j].Item2 + 1));
                        itemExist = true;
                        Debug.Log($"id = {items[j].Item1}, Amount = {items[j].Item2}");
                        continue;
                    }
                }

                if (itemExist)
                    continue;

                //��Ḧ items����Ʈ�� �־��� id�� ���� ������
                Debug.Log($"id = {ingredients[k].id}, Amount = {1}");
                items.Add(new Tuple<int, int>(ingredients[k].id, 1));
            }

            Inventory myInventory = InventoryManager.Instance.PlayerInventory;
            List<Item> ingredientItems = new List<Item>();
            bool isPossesIngredients = true;

            //���� �κ��丮�� ������ ���� �ִ��� Ȯ��
            for (int j = 0; j < items.Count; ++j)
            {
                isPossesIngredients = myInventory.GetItem(items[j].Item1, items[j].Item2, out Item ingredient);

                if (isPossesIngredients == false)
                {
                    posionToMake = curRecipe[i].portion;
                    return false;
                }

                //������ ���� �� ����Ʈ�� �־��ְ�
                ingredientItems.Add(ingredient);
            }

            //��ᰡ �� �����۵��� ����Ʈ�� ���鼭 ��������
            for (int j = 0; j < ingredientItems.Count; ++j)
            {
                ingredientItems[j].RemoveItem(items[j].Item2);
                Debug.Log($"���� : {ingredientItems[j].itemAmount} {items[j].Item2}");
            }


            posionToMake = curRecipe[i].portion;
            return true;
        }

        //���� ������ �ִ� �����ǰ� �ƴ϶�� null �ְ� return false����
        posionToMake = null;
        return false;
    }

    /// <summary>
    /// id�� ������ �߰� ����
    /// </summary>
    /// <param name="id">�������� id</param>
    public void AddRecipe(int id)
    {
        for (int j = 0; j < recipeSet.recipes.Count; j++)
        {
            if (recipeSet.recipes[j].id == id && curRecipe.Contains(recipeSet.recipes[j]) == false)
                curRecipe.Add(recipeSet.recipes[j]);
        }
    }


    /// <summary>
    /// RecipeSO�� �����Ǹ� �߰�����
    /// </summary>
    /// <param name="recipe"></param>
    public void AddRecipe(RecipeSO recipe)
    {
        if (curRecipe.Contains(recipe) == false)
            curRecipe.Add(recipe);

        Save();
    }


    /// <summary>
    /// �ʱ�ȭ ���ٶ� ���� ������ �ִ� Recipe�� �ʱ�ȭ����
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
    /// �����Ǹ� ����� �� �� �����Ǹ� ���� �� ���� �ִ��� Ȯ�����ִ� ����
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