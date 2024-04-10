using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.EditorCoroutines.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public enum SOType
{
    IngredientItemSO,
    RecipeSO,
    PortionItemSO
}

public class SpreadSheetLoader : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset VisualTreeAsset = default;
    [SerializeField]
    private ItemSetSO ItemSetSO = default;
    [SerializeField]
    private RecipeSetSO RecipeSetSO = default;

    private string _documentID = "1PTDfGympK6kVi6F9eIktiwhA0LkB7RqPhUiVqJsjVUI";
    private string spreadSheetNum = "0";

    private EnumField SOTypeEnum;
    private VisualElement _loadingIcon;
    private Label _statusLabel;

    private SOType soType = SOType.IngredientItemSO;

    [MenuItem("Tools/SpreadSheetLoader")]
    public static void OpenWindow()
    {
        SpreadSheetLoader wnd = GetWindow<SpreadSheetLoader>();
        wnd.titleContent = new GUIContent("SpreadSheetLoader");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        VisualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
            "Assets/09.Tools/Editor/SpreadSheetLoader.uxml");
        ItemSetSO = AssetDatabase.LoadAssetAtPath<ItemSetSO>(
            "Assets/07.So/Items/ItemSet.asset");
        RecipeSetSO = AssetDatabase.LoadAssetAtPath<RecipeSetSO>(
            "Assets/07.So/Recipe/RecipeSet.asset");

        VisualElement templateContainer = VisualTreeAsset.Instantiate();
        templateContainer.style.flexGrow = 1;

        root.Add(templateContainer);
        _statusLabel = root.Q<Label>("status-label"); //상태라벨 표시기
        _loadingIcon = root.Q<VisualElement>("loading-icon");
        SetUp();
    }

    private void SetUp()
    {
        TextField txtUrl = rootVisualElement.Q<TextField>("txt-url");
        txtUrl.RegisterCallback<ChangeEvent<string>>(HandleUrlChange);
        txtUrl.SetValueWithoutNotify(_documentID);

        TextField txtSheetNum = rootVisualElement.Q<TextField>("txt-sheet-num");
        txtSheetNum.RegisterCallback<ChangeEvent<string>>(HandleSpreadSheetNum);
        txtSheetNum.SetValueWithoutNotify(spreadSheetNum);

        Button loadBtn = rootVisualElement.Q<Button>("btn-load");
        loadBtn.RegisterCallback<ClickEvent>(HandleLoadBtn);

        SOTypeEnum = rootVisualElement.Q<EnumField>("so-type-enum");
        SOTypeEnum.RegisterCallback<ChangeEvent<SOType>>(evt =>
        {
            soType = evt.newValue;
        });
    }

    #region 건드리지 마요
    private IEnumerator GetDataFromSheet(string sheetID, Action<string[]> Process)
    {
        UnityWebRequest req = UnityWebRequest.Get(
         $"https://docs.google.com/spreadsheets/d/{_documentID}/export?format=tsv&gid={sheetID}");

        _statusLabel.text = "데이터 로딩중";
        _loadingIcon.RemoveFromClassList("off");

        yield return req.SendWebRequest();

        //404 error , 500 error
        if (req.result == UnityWebRequest.Result.ConnectionError || req.responseCode != 200)
        {
            Debug.LogError("Error : " + req.responseCode);
            yield break;
        }

        _loadingIcon.AddToClassList("off");

        string resText = req.downloadHandler.text;

        string[] lines = resText.Split("\n");

        int lineNumber = 1;
        try
        {
            for (lineNumber = 1; lineNumber < lines.Length; ++lineNumber)
            {
                string[] dataArr = lines[lineNumber].Split("\t"); //TSV로 뽑아왔으니
                Process?.Invoke(dataArr);
            }
        }
        catch (Exception e)
        {
            _statusLabel.text += $"\n {_documentID} 로딩 중 오류 발생";
            _statusLabel.text += $"\n {lineNumber} : 줄 오류 발생";
            _statusLabel.text += $"\n {e.Message}";
        }

        _statusLabel.text = $"\n 로드 완료! {lineNumber - 1} 개의 파일이 성공적으로 작성됨.";

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void HandleUrlChange(ChangeEvent<string> evt)
    {
        _documentID = evt.newValue;
    }

    private void HandleSpreadSheetNum(ChangeEvent<string> evt)
    {
        spreadSheetNum = evt.newValue;
    }
    #endregion


    #region 이거 건드리세요
    private void HandleLoadBtn(ClickEvent evt)
    {
        EditorCoroutineUtility.StartCoroutine(GetDataFromSheet(spreadSheetNum, (dataArr) =>
        {
            soType = (SOType)SOTypeEnum.value;

            switch (soType)
            {
                case SOType.IngredientItemSO:
                    CreateSO(
                        fileName: dataArr[0],
                        id: int.Parse(dataArr[1]),
                        itemName: dataArr[2],
                        maxCarryAmountPerSlot: int.Parse(dataArr[3]),
                        itemExplain: dataArr[4],
                        ingredientType: Enum.Parse<IngredientType>(dataArr[5]),
                        gatheringTime: int.Parse(dataArr[6]));
                    break;
                case SOType.PortionItemSO:
                    CreateSO(
                        fileName: dataArr[0],
                        id: int.Parse(dataArr[1]),
                        itemName: dataArr[2],
                        maxCarryAmountPerSlot: int.Parse(dataArr[3]),
                        itemExplain: dataArr[4],
                        portionType: Enum.Parse<Portion>(dataArr[5]),
                        usingTime: float.Parse(dataArr[6]),
                        duration: float.Parse(dataArr[7]),
                        isInfinite: bool.Parse(dataArr[8]));
                    break;
                case SOType.RecipeSO:

                    string[] s = dataArr[2].Split(',');
                    int[] numbers = new int[s.Length];
                    for (int i = 0; i < s.Length; i++)
                        numbers[i] = int.Parse(s[i]);

                    CreateSO(
                        fileName: dataArr[0],
                        id: int.Parse(dataArr[1]),
                        ingredientsId: numbers,
                        portionId: int.Parse(dataArr[3]));
                    break;
            }
        }), this);                                                          //이거 건드리시고
    }

    private void CreateSO(string fileName, int id, string itemName, int maxCarryAmountPerSlot, string itemExplain, IngredientType ingredientType, int gatheringTime)       //이거 인수 바꾸고 SO만드는 거도 바꿔
    {
        string filePath = $"Assets/07.SO/Items/Ingredient/{fileName}.asset";                  //경로 조금 바꾸셈
        IngredientItemSO asset = AssetDatabase.LoadAssetAtPath<IngredientItemSO>(filePath);

        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<IngredientItemSO>();
            asset.id = id;
            asset.itemName = itemName;
            asset.maxCarryAmountPerSlot = maxCarryAmountPerSlot;
            asset.itemExplain = itemExplain;
            asset.ingredientType = ingredientType;
            asset.gatheringTime = gatheringTime;

            string filename = AssetDatabase.GenerateUniqueAssetPath(filePath);
            AssetDatabase.CreateAsset(asset, filename);
            ItemSetSO.AddItem(asset);
        }
        else
        {
            asset.id = id;
            asset.itemName = itemName;
            asset.maxCarryAmountPerSlot = maxCarryAmountPerSlot;
            asset.itemExplain = itemExplain;
            asset.ingredientType = ingredientType;
            asset.gatheringTime = gatheringTime;
            EditorUtility.SetDirty(asset);
        }
    }

    private void CreateSO(string fileName, int id, string itemName, int maxCarryAmountPerSlot, string itemExplain, Portion portionType, float usingTime, float duration, bool isInfinite)
    {
        string filePath = $"Assets/07.SO/Items/Portion/{fileName}.asset";                  //경로 조금 바꾸셈
        PortionItemSO asset = AssetDatabase.LoadAssetAtPath<PortionItemSO>(filePath);

        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<PortionItemSO>();
            asset.id = id;
            asset.itemName = itemName;
            asset.maxCarryAmountPerSlot = maxCarryAmountPerSlot;
            asset.itemExplain = itemExplain;
            asset.portionType = portionType;
            asset.usingTime = usingTime;
            asset.duration = duration;
            asset.isInfinite = isInfinite;

            string effectPath = $"/01.Scripts/JinSoonScript/PortionEffect/{fileName}Effect.cs";

            string path = $"{Application.dataPath}{effectPath}";
            if (File.Exists(path) == false)
            {
                string code = string.Format(CodeFormat.EffectScriptFormat, $"{fileName}Effect");
                Debug.Log(code);
                File.WriteAllText($"{path}", code);
                Effect effect = AssetDatabase.LoadAssetAtPath($"Assets{effectPath}", typeof(Effect)) as Effect;
                asset.effect = effect;
            }
            else
            {
                Effect effect = AssetDatabase.LoadAssetAtPath($"Assets{effectPath}", typeof(Effect)) as Effect;
                asset.effect = effect;
            }

            string filename = AssetDatabase.GenerateUniqueAssetPath(filePath);
            AssetDatabase.CreateAsset(asset, filename);
            ItemSetSO.AddItem(asset);
        }
        else
        {
            asset.id = id;
            asset.itemName = itemName;
            asset.maxCarryAmountPerSlot = maxCarryAmountPerSlot;
            asset.itemExplain = itemExplain;
            asset.portionType = portionType;
            asset.usingTime = usingTime;
            asset.duration = duration;
            asset.isInfinite = isInfinite;

            string effectPath = $"/01.Scripts/JinSoonScript/PortionEffect/{fileName}Effect.cs";

            string path = $"{Application.dataPath}{effectPath}";
            if (File.Exists(path) == false)
            {
                string code = string.Format(CodeFormat.EffectScriptFormat, $"{fileName}Effect");
                Debug.Log(code);
                File.WriteAllText($"{path}", code);
                Effect effect = AssetDatabase.LoadAssetAtPath($"Assets{effectPath}", typeof(Effect)) as Effect;
                asset.effect = effect;
            }
            else
            {
                Effect effect = AssetDatabase.LoadAssetAtPath($"Assets{effectPath}", typeof(Effect)) as Effect;
                asset.effect = effect;
            }
        }

            EditorUtility.SetDirty(asset);
        }
    
    private void CreateSO(string fileName, int id, int[] ingredientsId, int portionId)
    {
        List<IngredientItemSO> items = new List<IngredientItemSO>();
        PortionItemSO portion = new PortionItemSO();

        for (int i = 0; i < ItemSetSO.itemset.Count; i++)
        {
            Debug.Log(ItemSetSO.itemset[i]);
            for (int j = 0; j < ingredientsId.Length; j++)
            {
                Debug.Log(ingredientsId[j]);
                if (ingredientsId[j] == ItemSetSO.itemset[i].id)
                    items.Add(ItemSetSO.itemset[i] as IngredientItemSO);
            }

            if (portionId == ItemSetSO.itemset[i].id)
                portion = ItemSetSO.itemset[i] as PortionItemSO;
        }

        string filePath = $"Assets/07.SO/Recipe/{fileName}.asset";                  //경로 조금 바꾸셈
        RecipeSO asset = AssetDatabase.LoadAssetAtPath<RecipeSO>(filePath);

        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<RecipeSO>();
            asset.id = id;
            asset.ingredients = items.ToArray();
            asset.portion = portion;

            string filename = AssetDatabase.GenerateUniqueAssetPath(filePath);
            AssetDatabase.CreateAsset(asset, filename);
            RecipeSetSO.AddItem(asset);
        }
        else
        {
            asset.id = id;
            asset.ingredients = items.ToArray();
            asset.portion = portion;
            EditorUtility.SetDirty(asset);
        }
    }
    #endregion
}