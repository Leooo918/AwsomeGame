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
            _blank[currentIndex].gameObject.SetActive(true);
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
            ClearEffectAtIndex(lastIndex);
            _activeIndexes.RemoveAt(_activeIndexes.Count - 1);
        }
    }

    public void ArmorOn(int a)
    {
        if (_activeIndexes.Count < _blank.Length)
        {
            int currentIndex = _activeIndexes.Count;
            _blank[currentIndex].gameObject.SetActive(true);
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
            ClearEffectAtIndex(lastIndex);
            _activeIndexes.RemoveAt(_activeIndexes.Count - 1);
        }
    }

    private void ClearEffectAtIndex(int index)
    {
        _blank[index].gameObject.SetActive(false);
        _blank[index].sprite = null;
        _border[index].sprite = null;
    }

    private void RemoveEffectAtIndex(int index)
    {
        _blank[index].gameObject.SetActive(false);
        _blank[index].sprite = null;
        _border[index].sprite = null;
    }
}

