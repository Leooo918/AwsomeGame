using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DiePanel : MonoBehaviour, IManageableUI
{
    private RectTransform _rect;

    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private TextMeshProUGUI _kill;
    [SerializeField] private TextMeshProUGUI _gather;
    [SerializeField] private TextMeshProUGUI _coin;

    [SerializeField] private RectTransform _progressStart, _progressEnd;
    [SerializeField] private RectTransform _player;
    [SerializeField] private Transform _gatheredIngredientsContainer;

    private float _progress;
    private int _time;
    private int _killCnt;
    private int _gatherCnt;
    private int _coinCnt;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("นึ");
            Init(189, 25, 32, 1202, 0.5f);
            Open();
        }
    }

    public void Init(int timer, int killCnt, int gatherCnt, int coinCnt, float progress)
    {
        _time = timer;
        _killCnt = killCnt;
        _gatherCnt = gatherCnt;
        _coinCnt = coinCnt;
        _progress = progress;
    }

    public void Close()
    {

    }

    public void Open()
    {
        _rect.DOAnchorPosY(0f, 0.5f)
            .OnComplete(() => StartCoroutine(ProgressRoutine()));
    }

    private IEnumerator ProgressRoutine()
    {
        float curProgress = 0;
        _player.anchoredPosition = new Vector2(_progressStart.anchoredPosition.x, _player.anchoredPosition.y);
        yield return new WaitForSeconds(0.5f);

        while (curProgress < _progress)
        {
            float posX = Mathf.Lerp(_progressStart.anchoredPosition.x, _progressEnd.anchoredPosition.x, curProgress);

            Vector2 playerPosition = new Vector2(posX, _player.anchoredPosition.y);
            _player.anchoredPosition = playerPosition;

            curProgress += 0.005f;

            yield return null;
        }

        int time = 0;
        curProgress = 0;
        while(curProgress <= 1)
        {
            time = (int)Mathf.Lerp(0, _time, curProgress);
            int minute = time / 60;
            int second = time % 60;

            _timer.SetText($"{minute} : {second}");
            curProgress += 0.005f;
            yield return null;
        }

        int kill = 0;
        curProgress = 0;
        while (curProgress <= 1)
        {
            kill = (int)Mathf.Lerp(0, _killCnt, curProgress);
            _kill.SetText($"{kill}");
            curProgress += 0.005f;
            yield return null;
        }

        int gather = 0;
        curProgress = 0;
        while (curProgress <= 1)
        {
            gather = (int)Mathf.Lerp(0, gather, curProgress);
            _gather.SetText($"{gather}");
            curProgress += 0.005f;
            yield return null;
        }

        int coin = 0;
        curProgress = 0;
        while (curProgress <= 1)
        {
            coin = (int)Mathf.Lerp(0, _coinCnt, curProgress);
            _coin.SetText($"{coin}");
            curProgress += 0.005f;
            yield return null;
        }
    }

    public void Init()
    {

    }
}
