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
    [SerializeField] private bool _isActive;

    private Button _button;

    private void Awake()
    {
        image = GetComponent<Image>();
        _button = GetComponent<Button>();

        _button.onClick.AddListener(Enable);
    }

    private void Start()
    {
        if (_isActive == false)
        {
            Disable();
        }
    }

    public void Enable()
    {
        _isActive = true;
        _panel.SetActive(_isActive);
        another.Disable();
        image.sprite = enableSprite;

    }

    public void Disable()
    {
        _isActive = false;
        _panel.SetActive(_isActive);
        image.sprite = disableSprite;
    }
}
