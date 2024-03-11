using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject canvas;

    private Vector3 goalPos;    

    private Sequence seq;

    private void Update()
    {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameObject ming = Instantiate(settingPanel) as GameObject;
            ming.transform.SetParent(canvas.transform, false);
            //seq.Append(transform.DOLocalMove(goalPos, 2).SetEase(Ease.Linear));
        }
    }
}
