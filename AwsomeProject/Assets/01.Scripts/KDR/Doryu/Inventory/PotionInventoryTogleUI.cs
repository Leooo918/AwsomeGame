using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionInventoryTogleUI : MonoBehaviour
{
    [SerializeField] private Button _throwModeBtn;
    [SerializeField] private Button _drinkModeBtn;
    [SerializeField] private GameObject _throwInven;
    [SerializeField] private GameObject _drinkInven;

    private GameObject _currentInven;

    private void Start()
    {
        _currentInven = _throwInven;
        _drinkInven.SetActive(false);
        _throwModeBtn.onClick.AddListener(() => ChangeInvenView(_throwInven));
        _drinkModeBtn.onClick.AddListener(() => ChangeInvenView(_drinkInven));
    }

    private void ChangeInvenView(GameObject inven)
    {
        _currentInven.SetActive(false);
        _currentInven = inven;
        _currentInven.SetActive(true);
    }
}
