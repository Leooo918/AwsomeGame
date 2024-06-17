using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IngredientItemSO))]
public class CustomIngredientItemSO : Editor
{
    private SerializedProperty _id;
    private SerializedProperty _itemName;
    private SerializedProperty _itemType;
    private SerializedProperty _maxCarryAmountPerSlot;
    private SerializedProperty _itemExplain;
    private SerializedProperty _dotImage;
    private SerializedProperty _itemImage;
    private SerializedProperty _prefab;
    private SerializedProperty _ingredientType;
    private SerializedProperty _gatheringTime;
    private SerializedProperty _effectType;
    private SerializedProperty _effectPoint;

    private GUIStyle _textAreaStyle; //텍스트 랩 스타일을 지정하기 위해서 

    private void OnEnable()
    {
        GUIUtility.keyboardControl = 0;

        _id = serializedObject.FindProperty("id");
        _itemName = serializedObject.FindProperty("itemName");
        _itemType = serializedObject.FindProperty("itemType");
        _maxCarryAmountPerSlot = serializedObject.FindProperty("maxCarryAmountPerSlot");
        _itemExplain = serializedObject.FindProperty("itemExplain");
        _dotImage = serializedObject.FindProperty("dotImage");
        _itemImage = serializedObject.FindProperty("itemImage");
        _prefab = serializedObject.FindProperty("prefab");
        _ingredientType = serializedObject.FindProperty("ingredientType");
        _gatheringTime = serializedObject.FindProperty("gatheringTime");
        _effectType = serializedObject.FindProperty("effectType");
        _effectPoint = serializedObject.FindProperty("effectPoint");
    }

    private void StyleSetUp()
    {
        if (_textAreaStyle == null)
        {
            _textAreaStyle = new GUIStyle(EditorStyles.textArea);
            _textAreaStyle.wordWrap = true; //이것때문에 오버라이드 한다.
        }
    }

    public override void OnInspectorGUI()
    {
        StyleSetUp();
        //시작할때 해줄 일
        serializedObject.Update();

        EditorGUILayout.Space(10f);
        EditorGUILayout.BeginHorizontal("HelpBox");
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.LabelField("DotImage");
                _dotImage.objectReferenceValue = EditorGUILayout.ObjectField(GUIContent.none,
                    _dotImage.objectReferenceValue,
                    typeof(Sprite),
                    false,
                    GUILayout.Width(65));

                EditorGUILayout.LabelField("IllustImage");
                _itemImage.objectReferenceValue = EditorGUILayout.ObjectField(GUIContent.none,
                _itemImage.objectReferenceValue,
                typeof(Sprite),
                false,
                GUILayout.Width(65));

            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.IntField("id", _id.intValue);

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PropertyField(_itemName);
                }
                EditorGUILayout.EndHorizontal();

                //GUIStyle style = GUIStyle.none;
                EditorGUILayout.PropertyField(_itemExplain);
                EditorGUILayout.PropertyField(_prefab);
                EditorGUILayout.PropertyField(_itemType);
                EditorGUILayout.PropertyField(_maxCarryAmountPerSlot);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(_ingredientType);
        EditorGUILayout.IntField(_gatheringTime.intValue);
        EditorGUILayout.PropertyField(_effectType);
        EditorGUILayout.IntField(_effectPoint.intValue);


        //끝날 때 해줄일 
        serializedObject.ApplyModifiedProperties();
    }
}
