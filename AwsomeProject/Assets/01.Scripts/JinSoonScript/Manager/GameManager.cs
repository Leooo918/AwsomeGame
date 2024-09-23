using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Image _fadeout;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _shopBuyPanel;
    [SerializeField] private TextMeshProUGUI _coinTxt;
    private float _fadeOutTime;
    private Sequence _fadeAwaySeq;
    private bool _gameOver = false;

    public float playStartTime = 0;
    public int killCnt = 0;
    public int gatherCnt = 0;
    public int coinCnt = 0;

    private void Start()
    {
        playStartTime = Time.time;
    }

    public void Restart()
    {
        _fadeAwaySeq = DOTween.Sequence();
        if (_gameOver == true)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            _fadeout.gameObject.SetActive(true);
            _fadeAwaySeq.Append(_fadeout.DOFade(1, _fadeOutTime))
                .AppendCallback(() => SceneManager.LoadScene(1));
        }
    }

    public void GameOver()
    {
        _fadeAwaySeq = DOTween.Sequence();

        _fadeout.gameObject.SetActive(true);
        _gameOver = true;
        _fadeAwaySeq.Append(_fadeout.DOFade(1, _fadeOutTime))
            .AppendCallback(() => _gameOverUI.SetActive(true));
    }

    public void GoToTitle()
    {
        _fadeAwaySeq = DOTween.Sequence();

        if (_gameOver == true)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            _fadeout.gameObject.SetActive(true);
            _fadeAwaySeq.Append(_fadeout.DOFade(1, _fadeOutTime))
                .AppendCallback(() => SceneManager.LoadScene(0));
        }
    }

    public void BuyItem()
    {
        _shopBuyPanel.SetActive(false);

    }
}
