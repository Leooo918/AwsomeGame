using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BookMark : MonoBehaviour
{
    public BookMark another;

    private Image image;
    [SerializeField] private Sprite enableSprite;
    [SerializeField] private Sprite disableSprite;
    [SerializeField] private GameObject _panel;

    private Button _button;

    private void Awake()
    {
        image = GetComponent<Image>();
        _button = GetComponent<Button>();

        _button.onClick.AddListener(Enable);
    }

    public void Enable()
    {
        if (_panel.activeSelf) return;

        _panel.SetActive(true);
        another.Disable();
        image.sprite = enableSprite;

    }

    public void Disable()
    {
        if (_panel.activeSelf == false) return;

        _panel.SetActive(false);
        image.sprite = disableSprite;
    }
}
