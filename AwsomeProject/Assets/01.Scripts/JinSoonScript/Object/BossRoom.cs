using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.UI;


public class BossRoom : MonoBehaviour
{
    public Action BossRoomEnterEvent;
    public Action BossClearEvent;

    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private CameraControllerTrigger _enterCameraTrigger;

    [SerializeField] private CinemachineVirtualCamera _bossRoomCam;
    [SerializeField] private CinemachineVirtualCamera _bossWatchingCam;

    [SerializeField] private GameObject _bossObj;
    [SerializeField] private Slider _bossHpSlider;
    [SerializeField] private FallingRockSpawner _spawner;
    [SerializeField] private CameraBetweenToObj _cameraFollow;
    private IBoss _boss;
    private KingSlime _kingSlime;

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

            _boss = Instantiate(_bossObj, transform).GetComponent<IBoss>();
            _kingSlime = (_boss as KingSlime);

            _kingSlime.healthCompo.hpSlider = _bossHpSlider;
            _kingSlime.spanwer = _spawner;

            _boss._bossWatchingCam = _bossWatchingCam;
            _boss._bossRoomCam = _bossRoomCam;
            _boss.StartBoss();

            _cameraFollow._obj1 = PlayerManager.Instance.PlayerTrm;
            _cameraFollow._obj2 = _kingSlime.transform;
        }
    }

    private void EndBossRoom()
    {
        if (_boss != null)
        {
            BossClearEvent?.Invoke();
            _boss.EndBoss();
        }
    }
}

[Serializable]
public struct DetectingStruct
{
    public Transform position;
    public Vector3 size;
}