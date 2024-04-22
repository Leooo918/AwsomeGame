using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PortionItemSO))]
public class PortionItemSOEditor : Editor
{
    PortionItemSO portionItemSO;
    //Effect effect = null;

    SerializedProperty effect;

    private void OnEnable()
    {
        portionItemSO = target as PortionItemSO;
        effect = serializedObject.FindProperty("effect");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //EditorGUILayout.PropertyField(effect, new GUIContent("Script"));
    }
}
