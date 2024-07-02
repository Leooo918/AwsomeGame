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
                GameObject.Find($"{w.ToString()}Panel").GetComponent<IManageableUI>();
            //IManageableUI panel = _canvasTrm.GetComponent($"{w.ToString()}Panel") as IManageableUI;
            panelDictionary.Add(w, panel);
        }
    }

    public void Open(UIType target)
    {
        if (panelDictionary.TryGetValue(target, out IManageableUI panel))
        {
            panel.Open();
        }
    }

    public void Close(UIType target)
    {
        if(panelDictionary.TryGetValue(target, out IManageableUI panel))
        {
            panel.Close();
        }
    }
}
