using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Window
{
    Shop
}

public class UIManager : Singleton<UIManager>
{
    public Dictionary<Window, IWindowPanel> panelDictionary;

    [SerializeField] private Transform _canvasTrm;

    private void Awake()
    {
        panelDictionary = new Dictionary<Window, IWindowPanel>();
        foreach (Window w in Enum.GetValues(typeof(Window)))
        {
            IWindowPanel panel = _canvasTrm.GetComponent($"{w.ToString()}Panel") as IWindowPanel;
            panelDictionary.Add(w, panel);
        }
    }

    public void Open(Window target)
    {
        if (panelDictionary.TryGetValue(target, out IWindowPanel panel))
        {
            panel.Open();
        }
    }

    public void Close(Window target)
    {
        if(panelDictionary.TryGetValue(target, out IWindowPanel panel))
        {
            panel.Close();
        }
    }
}
