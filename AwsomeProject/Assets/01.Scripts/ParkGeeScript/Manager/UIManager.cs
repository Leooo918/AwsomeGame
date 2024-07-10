using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public TextMeshProUGUI PressFAlram { get; private set; }

    private void Awake()
    {
        PressFAlram = GameObject.Find("PressFAlram").GetComponent<TextMeshProUGUI>();
        PressFAlram.gameObject.SetActive(false);

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
