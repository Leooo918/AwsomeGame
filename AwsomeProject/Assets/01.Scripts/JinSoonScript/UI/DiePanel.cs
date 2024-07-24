using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
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

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("นึ");
            Init(189, 25, 32, 1202, 0.5f);
            Open();
        }
    }

    public void Init(int timer, int killCnt, int gatherCnt, int coinCnt, float progress)
    {
        int minute = timer / 60;
        int second = timer % 60;
        _timer.SetText($"{minute}: {second}");
        _kill.SetText(killCnt.ToString());
        _gather.SetText(gatherCnt.ToString());
        _coin.SetText(coinCnt.ToString());

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
        Debug.Log(curProgress);
    }
}
