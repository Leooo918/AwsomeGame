using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public Dictionary<UIType, IManageableUI> UIDictionary;

    private void Awake()
    {
        UIDictionary = new Dictionary<UIType, IManageableUI>();

        foreach (UIType type in Enum.GetValues(typeof(UIType)))
        {
            string typeName = type.ToString();
            try
            {
                IManageableUI state = GameObject.Find(typeName).GetComponent<IManageableUI>();
                UIDictionary.Add(type, state);
            }
            catch (Exception ex)
            {
                Debug.LogError($"{typeName} is loading error!");
                Debug.LogError(ex);
            }
        }
    }

    public void ActiveUI(UIType type)
    {
        if(UIDictionary.TryGetValue(type, out IManageableUI ui))
        {
            ui.Active();
        }
    }

    public void DisableUI(UIType type)
    {
        if (UIDictionary.TryGetValue(type, out IManageableUI ui))
        {
            ui.Disable();
        }
    }
}

public enum UIType
{
    Option
}

public interface IManageableUI
{
    public void Active();
    public void Disable();
}
