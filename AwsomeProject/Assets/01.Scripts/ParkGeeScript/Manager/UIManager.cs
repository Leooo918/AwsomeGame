using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIType
{
    Shop,
    PotionCraft,
    Option
}

public class UIManager : Singleton<UIManager>
{
    public Dictionary<UIType, IManageableUI> panelDictionary;

    //[SerializeField] private Transform _canvasTrm;

    private void Awake()
    {
        panelDictionary = new Dictionary<UIType, IManageableUI>();
        foreach (UIType w in Enum.GetValues(typeof(UIType)))
        {
            IManageableUI panel =
                GameObject.Find($"{w.ToString()}Panel")?.GetComponent<IManageableUI>();

            if (panel != null)
                panelDictionary.Add(w, panel);
        }
    }

    public void Open(UIType target)
    {
        if (panelDictionary.TryGetValue(target, out IManageableUI panel))
        {
            panel.Open();
        }
        else
        {
            Debug.LogWarning($"{target.ToString()}Panel is not exist in this scene.\nBut you trying to open it");
        }
    }

    public void Close(UIType target)
    {
        if (panelDictionary.TryGetValue(target, out IManageableUI panel))
        {
            panel.Close();
        }
        else
        {
            Debug.LogWarning($"{target.ToString()}Panel is not exist in this scene.\nBut you trying to close it");
        }
    }
}