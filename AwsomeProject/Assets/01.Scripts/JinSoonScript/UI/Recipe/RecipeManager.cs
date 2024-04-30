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
    /// 재료들을 받아서 포션을 만들 수 있는지 확인하고 포션을 만들어서 out에 할당해주는
    /// </summary>
    /// <param name="ingredients"></param>
    /// <param name="posionToMake"></param>
    /// <returns></returns>
    public bool TryMakePosion(IngredientItemSO[] ingredients, out PortionItemSO posionToMake)
    {
        posionToMake = new PortionItemSO();
        return true;
    }

    /// <summary>
    /// id로 레시피 추가 해줘
    /// </summary>
    /// <param name="id">레시피의 id</param>
    public void AddRecipe()
    {

    }


    /// <summary>
    /// RecipeSO로 레시피를 추가해줘
    /// </summary>
    /// <param name="recipe"></param>
    public void AddRecipe(RecipeSO recipe)
    {
        //if (curRecipe.Contains(recipe) == false)
        //    curRecipe.Add(recipe);

        Save();
    }


    /// <summary>
    /// 초기화 해줄때 현재 가지고 있는 Recipe를 초기화해줘
    /// </summary>
    public void ResetRecipeData()
    {
        curRecipe.Clear();
        Save();
    }

    public void OpenRecipeBar()
    {
        if (seq.IsActive() == true)
            seq.Kill();

        //QuickSlotManager.Instance.EnableQuickSlot();
        seq = DOTween.Sequence();

        isRecipeBarOpen = true;

        seq.Append(sideBar.DOAnchorPosX(-50f, 0.5f))
            .Join(selectedRecipe.DOAnchorPosY(425f, 0.5f));
    }

    public void CloseRecipeBar()
    {
        if (seq.IsActive() == true)
            seq.Kill();

        //QuickSlotManager.Instance.DisableQuickSlot();
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
    }
}

public class RecipeSave
{
    public List<int> ids;

    public List<int> recipeOnceUsed;
}