using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        Dictionary<int, string> a = new Dictionary<int, string>();

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
                        effectType: Enum.Parse<EffectEnum>(dataArr[6]),
                        point: int.Parse(dataArr[7]),
                        gatheringTime: int.Parse(dataArr[8]));
                    break;
                case SOType.PortionItemSO:
                    EffectInfo[] effects = new EffectInfo[int.Parse(dataArr[6])];

                    for (int i = 0; i < int.Parse(dataArr[6]); i++)
                    {
                        effects[i].effect = Enum.Parse<EffectEnum>(dataArr[7 + i * 2]);
                        effects[i].requirePoint = int.Parse(dataArr[8 + i * 2]);
                        Debug.Log(effects[i].effect);
                        Debug.Log(effects[i].requirePoint);
                    }

                    CreateSO(
                        fileName: dataArr[0],
                        id: int.Parse(dataArr[1]),
                        itemName: dataArr[2],
                        maxCarryAmountPerSlot: int.Parse(dataArr[3]),
                        itemExplain: dataArr[4],
                        portionEffect: Enum.Parse<EffectEnum>(dataArr[5]),
                        requireEffects: effects);
                    Debug.Log("밍");
                    //portionType: Enum.Parse<Portion>(dataArr[5])
                    break;
            }
        }), this);                                                          //이거 건드리시고
    }

    private void CreateSO(string fileName, int id, string itemName, int maxCarryAmountPerSlot, string itemExplain, IngredientType ingredientType, EffectEnum effectType, int point, int gatheringTime)       //이거 인수 바꾸고 SO만드는 거도 바꿔
    {
        Debug.Log("밍");

        string filePath = $"Assets/07.SO/Items/NewIngredient/{fileName}.asset";                  //경로 조금 바꾸셈
        IngredientItemSO asset = AssetDatabase.LoadAssetAtPath<IngredientItemSO>(filePath);

        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<IngredientItemSO>();
            asset.id = id;
            asset.itemName = itemName;
            asset.maxCarryAmountPerSlot = maxCarryAmountPerSlot;
            asset.itemExplain = itemExplain;
            asset.ingredientType = ingredientType;
            asset.effectType = effectType;
            asset.effectPoint = point;
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
            asset.effectType = effectType;
            asset.effectPoint = point;
            asset.gatheringTime = gatheringTime;
            EditorUtility.SetDirty(asset);
        }
    }

    private void CreateSO(string fileName, int id, string itemName, int maxCarryAmountPerSlot, string itemExplain, EffectEnum portionEffect, EffectInfo[] requireEffects)
    {
        string filePath = $"Assets/07.SO/Items/NewPortion/{fileName}.asset";                  //경로 조금 바꾸셈
        PortionItemSO asset = AssetDatabase.LoadAssetAtPath<PortionItemSO>(filePath);

        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<PortionItemSO>();
            asset.id = id;
            asset.itemName = itemName;
            asset.maxCarryAmountPerSlot = maxCarryAmountPerSlot;
            asset.itemExplain = itemExplain;
            asset.effect = portionEffect;
            asset.reqireEffects = requireEffects.ToList();

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
            asset.effect = portionEffect;
            asset.reqireEffects = requireEffects.ToList();

            EditorUtility.SetDirty(asset);
        }
    }
    #endregion
}

public struct EffectPoint
{
    public EffectEnum effect;
    public int point;
}