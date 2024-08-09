using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HasEffectManager : Singleton<HasEffectManager>
{
    [SerializeField] private Image[] _blank;
    [SerializeField] private Image[] _border;

    [SerializeField] private Sprite[] _borderSprites;
    [SerializeField] private Sprite _dashEffectSprite;
    [SerializeField] private Sprite _armorUpEffectSprite;

    private List<int> _activeIndexes = new List<int>();

    public void DashOn(int a)
    {
        if (_activeIndexes.Count < _blank.Length)
        {
            int currentIndex = _activeIndexes.Count;
            _blank[currentIndex].sprite = _dashEffectSprite;
            _border[currentIndex].sprite = _borderSprites[a];
            _activeIndexes.Add(currentIndex);
            Debug.Log(currentIndex);
        }
    }

    public void DashOff()
    {
        if (_activeIndexes.Count > 0)
        {
            int lastIndex = _activeIndexes[_activeIndexes.Count - 1];
            RemoveEffectAtIndex(lastIndex);
            _activeIndexes.RemoveAt(_activeIndexes.Count - 1);
        }
    }

    public void ArmorOn(int a)
    {
        if (_activeIndexes.Count < _blank.Length)
        {
            int currentIndex = _activeIndexes.Count;
            _blank[currentIndex].sprite = _armorUpEffectSprite;
            _border[currentIndex].sprite = _borderSprites[a];
            _activeIndexes.Add(currentIndex);
            Debug.Log(currentIndex);
        }
    }

    public void ArmorOff()
    {
        if (_activeIndexes.Count > 0)
        {
            int lastIndex = _activeIndexes[_activeIndexes.Count - 1];
            RemoveEffectAtIndex(lastIndex);
            _activeIndexes.RemoveAt(_activeIndexes.Count - 1);
        }
    }

    private void RemoveEffectAtIndex(int index)
    {
        _blank[index].sprite = null;
        _border[index].sprite = null;

        for (int i = index; i < _blank.Length - 1; i++)
        {
            _blank[i].sprite = _blank[i + 1].sprite;
            _border[i].sprite = _border[i + 1].sprite;
        }

        _blank[_blank.Length - 1].sprite = null;
        _border[_border.Length - 1].sprite = null;
    }
}

