using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using DG.Tweening;
using System.Drawing;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private List<CinemachineVirtualCamera> _cameraSet;
    [SerializeField] private PlayerFollowObj _follow;            //플레이어 따라가는 뇨속

    private Vector2 _startingTrackedObjectOffset;
    private Tween _panCameraTween;

    private CinemachineVirtualCamera _currentCam;                //현재 카메라
    private CinemachineFramingTransposer _framingTransposer;     //카메라 움직여주는 놈
    private CinemachineConfiner2D _currentConfiner;              //카메라 범위 제한
    private CinemachineBasicMultiChannelPerlin _currentPerline;  //카메라 흔들어 주는 놈

    private float _shakeTime = 0;
    private float _originOrthorgraphicSize = 0;
    private Transform _playerRect;

    private Vector2[] _dirArr = new Vector2[4]
    { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    private void Awake()
    {
        if (_cameraSet.Count > 0)
            ChangeCam(_cameraSet[0]);

        _playerRect = PlayerManager.Instance.PlayerTrm;
    }

    public void ChangeCam(CinemachineVirtualCamera activeCam)
    {
        _cameraSet.ForEach(x => x.Priority = 5);

        activeCam.Priority = 10;
        _currentCam = activeCam;

        _currentConfiner =
            _currentCam.GetComponent<CinemachineConfiner2D>();
        _framingTransposer = _currentCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        _currentPerline = _currentCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _currentCam.Follow = _follow.transform;
        _startingTrackedObjectOffset = _framingTransposer.m_TrackedObjectOffset;
    }

    public void PanCameraOnContact(float panDistance, float panTime, PanDirection direction, bool panToStartingPos)
    {
        Vector3 endPos = Vector3.zero;

        if (!panToStartingPos)
        {
            endPos =
                _dirArr[(int)direction] * panDistance + _startingTrackedObjectOffset;
        }
        else
            endPos = _startingTrackedObjectOffset;


        if (_panCameraTween != null && _panCameraTween.IsActive())
            _panCameraTween.Kill();


        _panCameraTween = DOTween.To(
            () => _framingTransposer.m_TrackedObjectOffset,
            value => _framingTransposer.m_TrackedObjectOffset = value,
            endPos, panTime);
    }

    public void ChangeConfinder(PolygonCollider2D collider)
    {
        _currentConfiner.m_BoundingShape2D = collider;
    }

    /// <summary>
    /// ZoomIn to 'size'
    /// </summary>
    /// <param name="size"></param>
    public void ZoomIn(float size)
    {
        _originOrthorgraphicSize = _currentCam.m_Lens.OrthographicSize;
        DOTween.To(() => _currentCam.m_Lens.OrthographicSize, x => _currentCam.m_Lens.OrthographicSize = x, size, 0.5f);
        //_currentCam.m_Lens.OrthographicSize = size;
    }

    /// <summary>
    /// ZoomOut to origin orthographic
    /// </summary>
    public void ZoomOut()
    {
        DOTween.To(() => _currentCam.m_Lens.OrthographicSize, x => _currentCam.m_Lens.OrthographicSize = x, _originOrthorgraphicSize, 0.5f);
    }

    public void ChangeFollow(Transform toFollow)=>_currentCam.m_Follow = toFollow;

    public void ChangeFollowToPlayer()=>_currentCam.m_Follow = _playerRect;

    public void ShakeCam(float amplitude, float frequency, float time)
    {
        _currentPerline.m_AmplitudeGain = amplitude;
        _currentPerline.m_FrequencyGain = frequency;
        _shakeTime = time;

        StartCoroutine(DelayStopShake());
    }

    public void StartShakeCam(float amplitude, float frequency)
    {
        _currentPerline.m_AmplitudeGain = amplitude;
        _currentPerline.m_FrequencyGain = frequency;
    }

    public void StopShakeCam()
    {
        _currentPerline.m_AmplitudeGain = 0;
        _currentPerline.m_FrequencyGain = 0;
    }

    private IEnumerator DelayStopShake()
    {
        yield return new WaitForSeconds(_shakeTime);

        _currentPerline.m_AmplitudeGain = 0f;
        _currentPerline.m_FrequencyGain = 0f;
    }

}
