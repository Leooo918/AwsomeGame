using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PortionItemSO))]
public class CustomPortionItemSO : Editor
{
    private SerializedProperty _id;
    private SerializedProperty _itemName;
    private SerializedProperty _itemType;
    private SerializedProperty _maxCarryAmountPerSlot;
    private SerializedProperty _itemExplain;
    private SerializedProperty _dotImage;
    private SerializedProperty _itemImage;
    private SerializedProperty _prefab;
    private SerializedProperty _portionType;
    private SerializedProperty _effect;
    private SerializedProperty _reqireEffects;
    private SerializedProperty _effecLv;
    private SerializedProperty _duration;
    private SerializedProperty _usingTime;
    private SerializedProperty _isInfinite;

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
        _portionType = serializedObject.FindProperty("portionType");
        _effect = serializedObject.FindProperty("effect");
        _reqireEffects = serializedObject.FindProperty("reqireEffects");
        _effecLv = serializedObject.FindProperty("effecLv");
        _duration = serializedObject.FindProperty("duration");
        _usingTime = serializedObject.FindProperty("usingTime");
        _isInfinite = serializedObject.FindProperty("isInfinite");
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
                    EditorGUILayout.TextField("Name", _itemName.stringValue);
                }
                EditorGUILayout.EndHorizontal();

                //GUIStyle style = GUIStyle.none;
                EditorGUILayout.TextField("ItemExplain",_itemExplain.stringValue);
                EditorGUILayout.PropertyField(_prefab);
                EditorGUILayout.PropertyField(_itemType);
                EditorGUILayout.IntField("MaxCarryAmountPerSlot",_maxCarryAmountPerSlot.intValue);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(_portionType);
        EditorGUILayout.PropertyField(_effect);
        EditorGUILayout.PropertyField(_reqireEffects);
        EditorGUILayout.IntField("EffectLv",_effecLv.intValue);
        EditorGUILayout.FloatField("Duration", _duration.floatValue);
        EditorGUILayout.FloatField("UsingTime", _usingTime.floatValue);
        EditorGUILayout.Toggle("isInfinite", _isInfinite.boolValue);


        //끝날 때 해줄일 
        serializedObject.ApplyModifiedProperties();
    }
}
