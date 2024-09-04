//using System;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//using Object = UnityEngine.Object;

//public class ItemImageInserter : EditorWindow
//{
//    private static int toolbarIdx = 0;
//    private static Vector2 editorScrollPosition;
//    private static Dictionary<ItemType, Vector2> scrollPosition =
//        new Dictionary<ItemType, Vector2>();
//    private static Dictionary<ItemType, Object> selectedItem =
//        new Dictionary<ItemType, Object>();

//    private Editor cachedEditor;
//    private Texture2D selectTexture;
//    private GUIStyle selectBoxStyle;
//    private string[] _toolbarNames;

//    private ItemSetSO itemSet;

//    [MenuItem("Tools/ItemManement")]
//    private static void OpenWindow()
//    {
//        ItemImageInserter window = GetWindow<ItemImageInserter>("Utility");
//        window.minSize = new Vector2(700, 500);
//        window.Show();
//    }

//    private void OnEnable()
//    {
//        SetupUtility();
//    }
//    private void OnDisable()
//    {
//        //자원의 해제
//        DestroyImmediate(selectTexture);
//        DestroyImmediate(cachedEditor);
//    }

//    private void SetupUtility()
//    {
//        selectTexture = new Texture2D(1, 1);
//        selectTexture.SetPixel(0, 0, new Color(0.31f, 0.40f, 0.50f));
//        selectTexture.Apply();

//        selectBoxStyle = new GUIStyle();
//        selectBoxStyle.normal.background = selectTexture;
//        selectTexture.hideFlags = HideFlags.DontSave;

//        _toolbarNames = Enum.GetNames(typeof(ItemType));

//        foreach (ItemType t in Enum.GetValues(typeof(ItemType)))
//        {
//            if (scrollPosition.ContainsKey(t) == false)
//            {
//                scrollPosition[t] = Vector2.zero;
//            }
//            if (selectedItem.ContainsKey(t) == false)
//            {
//                selectedItem[t] = null;
//            }
//        }

//        bool isChanged = false;

//        if (itemSet == null)
//        {
//            itemSet = AssetDatabase.LoadAssetAtPath<ItemSetSO>(
//                $"Assets/07.SO/Items/ItemSet.asset");
//            if (itemSet == null) //현재 해당 파일이 존재하지 않는다.
//            {
//                //이건 그냥 메모리상에만 만든거야.
//                itemSet = ScriptableObject.CreateInstance<ItemSetSO>();

//                string filename = AssetDatabase.GenerateUniqueAssetPath(
//                    $"Assets/07.SO/Items/ItemSet.asset");
//                AssetDatabase.CreateAsset(itemSet, filename);
//                Debug.Log($"itemSet created at {filename}");
//                isChanged = true;
//            }
//        }

//        if (isChanged)
//        {
//            AssetDatabase.SaveAssets();
//            AssetDatabase.Refresh();
//        }
//    }

//    private void OnGUI()
//    {
//        toolbarIdx = GUILayout.Toolbar(toolbarIdx, _toolbarNames);
//        EditorGUILayout.Space(5f);

//        DrawContent(toolbarIdx);
//    }

//    private void DrawContent(int toolbarIdx)
//    {
//        switch (toolbarIdx)
//        {
//            case 0:
//                DrawIngredient();
//                break;
//            case 1:
//                DrawPortion();
//                break;
//        }
//    }



//    private void DrawPortion()
//    {
//        EditorGUILayout.BeginHorizontal();
//        {
//            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(300f));
//            {
//                EditorGUILayout.LabelField("Portions");
//                EditorGUILayout.Space(3f);

//                Vector2 scroolPos = Vector2.zero;
//                scrollPosition[ItemType.Portion] = EditorGUILayout.BeginScrollView
//                    (scrollPosition[ItemType.Portion], false, true,
//                    GUIStyle.none, GUI.skin.verticalScrollbar, GUIStyle.none);
//                {
//                    foreach (ItemSO item in itemSet.itemset)
//                    {
//                        PortionItemSO portion = item as PortionItemSO;
//                        if (portion == false) continue;
//                        GUIStyle style = selectedItem[ItemType.Portion] == item ? selectBoxStyle : GUIStyle.none;
//                        EditorGUILayout.BeginHorizontal(style, GUILayout.Height(40f));
//                        {
//                            EditorGUILayout.LabelField(item.itemName,
//                                GUILayout.Width(240f), GUILayout.Height(40f));
//                        }
//                        EditorGUILayout.EndHorizontal();

//                        Rect lastRect = GUILayoutUtility.GetLastRect();
//                        if (Event.current.type == EventType.MouseDown
//                            && lastRect.Contains(Event.current.mousePosition))
//                        {
//                            editorScrollPosition = Vector2.zero;
//                            selectedItem[ItemType.Portion] = item;
//                            Event.current.Use();
//                        }

//                        if (item == null) break;
//                    }
//                }
//                EditorGUILayout.EndScrollView();
//            }
//            EditorGUILayout.EndVertical();

//            EditorGUILayout.BeginVertical();
//            {
//                if (selectedItem[ItemType.Portion] != null)
//                {
//                    editorScrollPosition = EditorGUILayout.BeginScrollView(editorScrollPosition);
//                    {
//                        EditorGUILayout.Space(2f);
//                        Editor.CreateCachedEditor(
//                            selectedItem[ItemType.Portion], null, ref cachedEditor);
//                        cachedEditor.OnInspectorGUI();
//                    }
//                    EditorGUILayout.EndScrollView();
//                }
//            }
//            EditorGUILayout.EndVertical();
//        }
//        EditorGUILayout.EndHorizontal();
//    }

//    private void DrawIngredient()
//    {
//        EditorGUILayout.BeginHorizontal();
//        {
//            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(300f));
//            {
//                EditorGUILayout.LabelField("Ingredients");
//                EditorGUILayout.Space(3f);

//                Vector2 scroolPos = Vector2.zero;
//                scrollPosition[ItemType.Ingredient] = EditorGUILayout.BeginScrollView
//                    (scrollPosition[ItemType.Ingredient], false, true,
//                    GUIStyle.none, GUI.skin.verticalScrollbar, GUIStyle.none);
//                {
//                    foreach (ItemSO item in itemSet.itemset)
//                    {
//                        IngredientItemSO portion = item as IngredientItemSO;
//                        if (portion == false) continue;
//                        GUIStyle style = selectedItem[ItemType.Ingredient] == item ? selectBoxStyle : GUIStyle.none;
//                        EditorGUILayout.BeginHorizontal(style, GUILayout.Height(40f));
//                        {
//                            EditorGUILayout.LabelField(item.itemName,
//                                GUILayout.Width(240f), GUILayout.Height(40f));
//                        }
//                        EditorGUILayout.EndHorizontal();

//                        Rect lastRect = GUILayoutUtility.GetLastRect();
//                        if (Event.current.type == EventType.MouseDown
//                            && lastRect.Contains(Event.current.mousePosition))
//                        {
//                            editorScrollPosition = Vector2.zero;
//                            selectedItem[ItemType.Ingredient] = item;
//                            Event.current.Use();
//                        }

//                        if (item == null) break;
//                    }
//                }
//                EditorGUILayout.EndScrollView();
//            }
//            EditorGUILayout.EndVertical();

//            EditorGUILayout.BeginVertical();
//            {
//                if (selectedItem[ItemType.Ingredient] != null)
//                {
//                    editorScrollPosition = EditorGUILayout.BeginScrollView(editorScrollPosition);
//                    {
//                        EditorGUILayout.Space(2f);
//                        Editor.CreateCachedEditor(selectedItem[ItemType.Ingredient], null, ref cachedEditor);
//                        cachedEditor.OnInspectorGUI();
//                    }
//                    EditorGUILayout.EndScrollView();
//                }
//            }
//            EditorGUILayout.EndVertical();
//        }
//        EditorGUILayout.EndHorizontal();
//    }
//}
