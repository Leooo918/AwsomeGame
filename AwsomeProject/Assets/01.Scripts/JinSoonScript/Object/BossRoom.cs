using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public enum BossEnum
{
    KingSlime
}


public class BossRoom : MonoBehaviour
{
    public Action BossRoomEnterEvent;
    public Action BossClearEvent;

    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private CameraControllerTrigger _enterCameraTrigger;

    [SerializeField] private CinemachineVirtualCamera _bossRoomCam;
    [SerializeField] private CinemachineVirtualCamera _bossWatchingCam;

    [SerializeField] private GameObject _boss;
    private IBoss boss;

    private bool _enterRoom = false;
    private bool _clearRoom = false;

    private void Awake()
    {
        _enterCameraTrigger.OnAfterSwapCameraToRight += EnterBossRoom;
    }

    private void EnterBossRoom(Player player)
    {
        if (_enterRoom == false)
        {
            //BossRoomEnterEvent?.Invoke();
            _enterRoom = true;

            boss = Instantiate(_boss, transform).GetComponent<IBoss>();
            boss._bossWatchingCam = _bossWatchingCam;
            boss._bossRoomCam = _bossRoomCam; 
            boss.StartBoss();
        }
    }

    private void EndBossRoom()
    {
        if (boss != null && boss.IsBossDead == true)
        {
            BossClearEvent?.Invoke();
            boss.EndBoss();
        }
    }
}

[Serializable]
public struct DetectingStruct
{
    public Transform position;
    public Vector3 size;
}