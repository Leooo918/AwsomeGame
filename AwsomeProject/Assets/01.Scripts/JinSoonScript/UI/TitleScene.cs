using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pressKeyTxt;
    [SerializeField] private RectTransform _logo;

    private Sequence seq;

    private void Start()
    {
        _logo.DOAnchorPosY(300f, 1f).SetEase(Ease.OutBack);
        _pressKeyTxt.DOFade(0, 1f).SetEase(Ease.Linear).SetLoops(999999, LoopType.Yoyo);
    }

    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            //¿œ¥‹ æ¿ ¿Ãµø§°§°
            SceneManager.LoadScene(1);
        }
    }
}
