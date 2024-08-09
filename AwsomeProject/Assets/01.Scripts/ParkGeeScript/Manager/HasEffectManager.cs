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
    [SerializeField] private Sprite _hardenEffectSprite;

    private List<int> _activeIndexes = new List<int>();

    public int count = 0;

    public void DashOn(int a)
    {
        if (count < _blank.Length)
        {
            _blank[count].sprite = _dashEffectSprite;
            _border[count].sprite = _borderSprites[a];
            _activeIndexes.Add(count);
            Debug.Log(count);
            count++;
        }
    }

    public void DashOff()
    {
        if (count > 0)
        {
            int lastIndex = _activeIndexes[_activeIndexes.Count - 1];
            _blank[lastIndex].sprite = null;
            _border[lastIndex].sprite = null;
            _activeIndexes.RemoveAt(_activeIndexes.Count - 1);
            count--;
        }
    }

    public void HardenOn()
    {
        if (count < _blank.Length)
        {
            _blank[count].sprite = _hardenEffectSprite;
            _activeIndexes.Add(count);
            count++;
        }
    }

    public void HardenOff()
    {
        if (count > 0)
        {
            int lastIndex = _activeIndexes[_activeIndexes.Count - 1];
            _blank[lastIndex].sprite = null;
            _activeIndexes.RemoveAt(_activeIndexes.Count - 1);
            count--;
        }
    }
}
